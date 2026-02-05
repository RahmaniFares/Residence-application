using residence.domain.Entities;

namespace residence.application.Repositories;

/// <summary>
/// Repository interface for ExpenseImage entity
/// </summary>
public interface IExpenseImageRepository : IRepository<ExpenseImage>
{
    Task<IEnumerable<ExpenseImage>> GetByExpenseAsync(Guid expenseId);
}
