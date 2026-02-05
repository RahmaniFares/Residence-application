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
}
