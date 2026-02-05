using Microsoft.EntityFrameworkCore;
using residence.domain.Entities;
using residence.application.Repositories;
using residence.infrastructure.Data;

namespace residence.infrastructure.Repositories;

/// <summary>
/// Repository implementation for ExpenseImage entity
/// </summary>
public class ExpenseImageRepository : Repository<ExpenseImage>, IExpenseImageRepository
{
    public ExpenseImageRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<ExpenseImage>> GetByExpenseAsync(Guid expenseId)
    {
        return await _dbSet
            .Where(ei => ei.ExpenseId == expenseId && !ei.IsDeleted)
            .ToListAsync();
    }
}
