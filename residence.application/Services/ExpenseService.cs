using residence.application.DTOs;
using residence.application.Interfaces;
using residence.domain.Entities;
using residence.application.Repositories;

namespace residence.application.Services;

/// <summary>
/// Implementation of Expense service
/// </summary>
public class ExpenseService : IExpenseService
{
    private readonly IExpenseRepository _expenseRepository;
    private readonly IExpenseImageRepository _expenseImageRepository;

    public ExpenseService(IExpenseRepository expenseRepository, IExpenseImageRepository expenseImageRepository)
    {
        _expenseRepository = expenseRepository;
        _expenseImageRepository = expenseImageRepository;
    }

    public async Task<ExpenseDto> CreateExpenseAsync(Guid residenceId, CreateExpenseDto dto)
    {
        var expense = new Expense
        {
            Id = Guid.NewGuid(),
            ResidenceId = residenceId,
            Title = dto.Title,
            Type = (domain.Enums.ExpenseType)dto.Type,
            Amount = dto.Amount,
            ExpenseDate = dto.ExpenseDate,
            Description = dto.Description,
            CreatedAt = DateTime.UtcNow
        };

        var created = await _expenseRepository.AddAsync(expense);
        return MapToDto(created);
    }

    public async Task<ExpenseDto> GetExpenseByIdAsync(Guid id)
    {
        var expense = await _expenseRepository.GetWithImagesAsync(id);
        if (expense == null)
            throw new Exception("Expense not found");

        return MapToDto(expense);
    }

    public async Task<ExpenseDto> UpdateExpenseAsync(Guid id, UpdateExpenseDto dto)
    {
        var expense = await _expenseRepository.GetByIdAsync(id);
        if (expense == null)
            throw new Exception("Expense not found");

        expense.Title = dto.Title;
        expense.Type = (domain.Enums.ExpenseType)dto.Type;
        expense.Amount = dto.Amount;
        expense.ExpenseDate = dto.ExpenseDate;
        expense.Description = dto.Description;
        expense.UpdatedAt = DateTime.UtcNow;

        await _expenseRepository.UpdateAsync(expense);

        return await GetExpenseByIdAsync(id);
    }

    public async Task DeleteExpenseAsync(Guid id)
    {
        var expense = await _expenseRepository.GetByIdAsync(id);
        if (expense == null)
            throw new Exception("Expense not found");

        await _expenseRepository.DeleteAsync(id);
    }

    public async Task<PagedResultDto<ExpenseDto>> GetExpensesByResidenceAsync(Guid residenceId, PaginationDto pagination)
    {
        var expenses = await _expenseRepository.GetByResidenceAsync(residenceId);
        
        var total = expenses.Count();
        var items = expenses
            .OrderByDescending(d => d.ExpenseDate)
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .Select(e => MapToDto(e))
            .ToList();

        var totalPages = (int)Math.Ceiling(total / (double)pagination.PageSize);

        return new PagedResultDto<ExpenseDto>(items, total, pagination.PageNumber, pagination.PageSize, totalPages);
    }

    public async Task<ExpenseDto> AddImageToExpenseAsync(Guid expenseId, CreateExpenseImageDto dto)
    {
        var expense = await _expenseRepository.GetByIdAsync(expenseId);
        if (expense == null)
            throw new Exception("Expense not found");

        var image = new ExpenseImage
        {
            Id = Guid.NewGuid(),
            ResidenceId = expense.ResidenceId,
            ExpenseId = expenseId,
            ImageUrl = dto.ImageUrl,
            CreatedAt = DateTime.UtcNow
        };

        await _expenseImageRepository.AddAsync(image);
        
        return await GetExpenseByIdAsync(expenseId);
    }

    public async Task RemoveImageFromExpenseAsync(Guid imageId)
    {
        var image = await _expenseImageRepository.GetByIdAsync(imageId);
        if (image == null)
            throw new Exception("Image not found");

        await _expenseImageRepository.DeleteAsync(imageId);
    }

    private ExpenseDto MapToDto(Expense expense)
    {
        var imageUrls = expense.Images?.Select(i => i.ImageUrl).ToList() ?? new List<string>();

        return new ExpenseDto(
            expense.Id,
            expense.Title,
            (residence.application.DTOs.ExpenseType)expense.Type,
            expense.Amount,
            expense.ExpenseDate,
            expense.Description,
            imageUrls,
            expense.CreatedAt,
            expense.UpdatedAt
        );
    }

    // KPI and Statistics Implementation
    public async Task<TotalExpenseKpiDto> GetTotalExpenseKpiAsync(Guid residenceId)
    {
        var total = await _expenseRepository.GetTotalAsync(residenceId);
        var count = await _expenseRepository.GetCountAsync(residenceId);
        var minAmount = await _expenseRepository.GetMinAmountAsync(residenceId);
        var maxAmount = await _expenseRepository.GetMaxAmountAsync(residenceId);
        var earliestDate = await _expenseRepository.GetEarliestDateAsync(residenceId);
        var latestDate = await _expenseRepository.GetLatestDateAsync(residenceId);

        var average = count > 0 ? total / count : 0;

        return new TotalExpenseKpiDto(
            total,
            count,
            average,
            maxAmount,
            minAmount,
            earliestDate,
            latestDate
        );
    }

    public async Task<MonthlyExpensesDto> GetMonthlyExpensesAsync(Guid residenceId)
    {
        var expensesByMonth = await _expenseRepository.GetExpensesByMonthAsync(residenceId);

        var monthlyData = new List<MonthlyExpenseDto>();
        decimal totalAmount = 0;
        int totalCount = 0;

        foreach (var monthGroup in expensesByMonth)
        {
            var (year, month) = monthGroup.Key;
            var expenses = monthGroup.Value;

            var monthTotal = expenses.Sum(e => e.Amount);
            var monthCount = expenses.Count;
            var monthAverage = monthCount > 0 ? monthTotal / monthCount : 0;

            // Get month name
            var monthName = new DateTime(year, month, 1).ToString("MMMM");

            monthlyData.Add(new MonthlyExpenseDto(
                year,
                month,
                monthName,
                monthTotal,
                monthCount,
                monthAverage
            ));

            totalAmount += monthTotal;
            totalCount += monthCount;
        }

        return new MonthlyExpensesDto(
            monthlyData,
            totalAmount,
            totalCount,
            monthlyData.Count
        );
    }

    public async Task<ExpenseStatsDto> GetExpenseStatsByTypeAsync(Guid residenceId)
    {
        var expensesByType = await _expenseRepository.GetExpensesByTypeAsync(residenceId);
        var totalAmount = await _expenseRepository.GetTotalAsync(residenceId);
        var totalCount = await _expenseRepository.GetCountAsync(residenceId);

        var statsData = new List<ExpenseTypeStatsDto>();

        foreach (var typeGroup in expensesByType)
        {
            var type = typeGroup.Key;
            var expenses = typeGroup.Value;

            var count = expenses.Count;
            var amount = expenses.Sum(e => e.Amount);
            var average = count > 0 ? amount / count : 0;
            var percentage = totalAmount > 0 ? (amount / totalAmount) * 100 : 0;

            statsData.Add(new ExpenseTypeStatsDto(
                (DTOs.ExpenseType)type,
                GetExpenseTypeName(type),
                count,
                amount,
                average,
                percentage
            ));
        }

        // Sort by amount descending to find highest and lowest
        var sorted = statsData.OrderByDescending(s => s.TotalAmount).ToList();
        var highest = sorted.FirstOrDefault() ?? new ExpenseTypeStatsDto(
            DTOs.ExpenseType.Other, "N/A", 0, 0, 0, 0);
        var lowest = sorted.LastOrDefault() ?? new ExpenseTypeStatsDto(
            DTOs.ExpenseType.Other, "N/A", 0, 0, 0, 0);

        return new ExpenseStatsDto(
            statsData.OrderBy(s => s.Type).ToList(),
            totalAmount,
            totalCount,
            highest,
            lowest
        );
    }

    private string GetExpenseTypeName(domain.Enums.ExpenseType type)
    {
        return type switch
        {
            domain.Enums.ExpenseType.Maintenance => "Maintenance",
            domain.Enums.ExpenseType.Electricity => "Electricity",
            domain.Enums.ExpenseType.Water => "Water",
            domain.Enums.ExpenseType.Cleaning => "Cleaning",
            domain.Enums.ExpenseType.Security => "Security",
            domain.Enums.ExpenseType.Gardening => "Gardening",
            domain.Enums.ExpenseType.Repairs => "Repairs",
            domain.Enums.ExpenseType.Equipment => "Equipment",
            domain.Enums.ExpenseType.Insurance => "Insurance",
            domain.Enums.ExpenseType.Taxes => "Taxes",
            domain.Enums.ExpenseType.Other => "Other",
            _ => "Unknown"
        };
    }
}
