using residence.domain.Entities;
using residence.domain.Enums;

namespace residence.application.Repositories;

/// <summary>
/// Repository interface for Expense entity
/// </summary>
public interface IExpenseRepository : IRepository<Expense>
{
    Task<IEnumerable<Expense>> GetByTypeAsync(Guid residenceId, ExpenseType type);
    Task<IEnumerable<Expense>> GetByDateRangeAsync(Guid residenceId, DateTime startDate, DateTime endDate);
    Task<Expense?> GetWithImagesAsync(Guid id);
    Task<decimal> GetTotalAsync(Guid residenceId);
    Task<decimal> GetTotalByTypeAsync(Guid residenceId, ExpenseType type);
}
