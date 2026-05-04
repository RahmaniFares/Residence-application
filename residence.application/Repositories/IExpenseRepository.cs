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

    // KPI and Statistics methods
    /// <summary>
    /// Get all expenses for a residence (for aggregation)
    /// </summary>
    Task<IEnumerable<Expense>> GetAllByResidenceAsync(Guid residenceId);

    /// <summary>
    /// Get expenses grouped by month for a residence
    /// </summary>
    Task<Dictionary<(int Year, int Month), List<Expense>>> GetExpensesByMonthAsync(Guid residenceId);

    /// <summary>
    /// Get expenses grouped by type for a residence
    /// </summary>
    Task<Dictionary<ExpenseType, List<Expense>>> GetExpensesByTypeAsync(Guid residenceId);

    /// <summary>
    /// Get count of expenses for a residence
    /// </summary>
    Task<int> GetCountAsync(Guid residenceId);

    /// <summary>
    /// Get min expense amount for a residence
    /// </summary>
    Task<decimal> GetMinAmountAsync(Guid residenceId);

    /// <summary>
    /// Get max expense amount for a residence
    /// </summary>
    Task<decimal> GetMaxAmountAsync(Guid residenceId);

    /// <summary>
    /// Get earliest expense date for a residence
    /// </summary>
    Task<DateTime?> GetEarliestDateAsync(Guid residenceId);

    /// <summary>
    /// Get latest expense date for a residence
    /// </summary>
    Task<DateTime?> GetLatestDateAsync(Guid residenceId);
}
