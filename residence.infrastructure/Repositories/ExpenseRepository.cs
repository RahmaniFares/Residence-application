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
}
