using Microsoft.EntityFrameworkCore;
using residence.domain.Entities;
using residence.domain.Enums;
using residence.application.Repositories;
using residence.infrastructure.Data;

namespace residence.infrastructure.Repositories;

/// <summary>
/// Repository implementation for Expense entity
/// </summary>
public class ExpenseRepository : Repository<Expense>, IExpenseRepository
{
    public ExpenseRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Expense>> GetByTypeAsync(Guid residenceId, ExpenseType type)
    {
        return await _dbSet
            .Where(e => e.ResidenceId == residenceId && e.Type == type && !e.IsDeleted)
            .Include(e => e.Images)
            .OrderByDescending(e => e.ExpenseDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Expense>> GetByDateRangeAsync(Guid residenceId, DateTime startDate, DateTime endDate)
    {
        return await _dbSet
            .Where(e => e.ResidenceId == residenceId && 
                   e.ExpenseDate >= startDate && 
                   e.ExpenseDate <= endDate && 
                   !e.IsDeleted)
            .Include(e => e.Images)
            .OrderByDescending(e => e.ExpenseDate)
            .ToListAsync();
    }

    public async Task<Expense?> GetWithImagesAsync(Guid id)
    {
        return await _dbSet
            .Where(e => e.Id == id && !e.IsDeleted)
            .Include(e => e.Images)
            .FirstOrDefaultAsync();
    }

    public async Task<decimal> GetTotalAsync(Guid residenceId)
    {
        return await _dbSet
            .Where(e => e.ResidenceId == residenceId && !e.IsDeleted)
            .SumAsync(e => e.Amount);
    }

    public async Task<decimal> GetTotalByTypeAsync(Guid residenceId, ExpenseType type)
    {
        return await _dbSet
            .Where(e => e.ResidenceId == residenceId && e.Type == type && !e.IsDeleted)
            .SumAsync(e => e.Amount);
    }

    public async Task<IEnumerable<Expense>> GetAllByResidenceAsync(Guid residenceId)
    {
        return await _dbSet
            .Where(e => e.ResidenceId == residenceId && !e.IsDeleted)
            .Include(e => e.Images)
            .OrderByDescending(e => e.ExpenseDate)
            .ToListAsync();
    }

    public async Task<Dictionary<(int Year, int Month), List<Expense>>> GetExpensesByMonthAsync(Guid residenceId)
    {
        var expenses = await GetAllByResidenceAsync(residenceId);
        return expenses
            .GroupBy(e => (e.ExpenseDate.Year, e.ExpenseDate.Month))
            .OrderBy(g => g.Key.Year)
            .ThenBy(g => g.Key.Month)
            .ToDictionary(g => g.Key, g => g.ToList());
    }

    public async Task<Dictionary<ExpenseType, List<Expense>>> GetExpensesByTypeAsync(Guid residenceId)
    {
        var expenses = await GetAllByResidenceAsync(residenceId);
        return expenses
            .GroupBy(e => e.Type)
            .ToDictionary(g => g.Key, g => g.ToList());
    }

    public async Task<int> GetCountAsync(Guid residenceId)
    {
        return await _dbSet
            .Where(e => e.ResidenceId == residenceId && !e.IsDeleted)
            .CountAsync();
    }

    public async Task<decimal> GetMinAmountAsync(Guid residenceId)
    {
        var expenses = await _dbSet
            .Where(e => e.ResidenceId == residenceId && !e.IsDeleted)
            .ToListAsync();

        return expenses.Any() ? expenses.Min(e => e.Amount) : 0;
    }

    public async Task<decimal> GetMaxAmountAsync(Guid residenceId)
    {
        return await _dbSet
            .Where(e => e.ResidenceId == residenceId && !e.IsDeleted)
            .MaxAsync(e => (decimal?)e.Amount) ?? 0;
    }

    public async Task<DateTime?> GetEarliestDateAsync(Guid residenceId)
    {
        return await _dbSet
            .Where(e => e.ResidenceId == residenceId && !e.IsDeleted)
            .OrderBy(e => e.ExpenseDate)
            .Select(e => e.ExpenseDate)
            .FirstOrDefaultAsync();
    }

    public async Task<DateTime?> GetLatestDateAsync(Guid residenceId)
    {
        return await _dbSet
            .Where(e => e.ResidenceId == residenceId && !e.IsDeleted)
            .OrderByDescending(e => e.ExpenseDate)
            .Select(e => e.ExpenseDate)
            .FirstOrDefaultAsync();
    }
}
