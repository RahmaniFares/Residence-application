using residence.application.DTOs;

namespace residence.application.Interfaces;

/// <summary>
/// Expense service interface
/// </summary>
public interface IExpenseService
{
    Task<ExpenseDto> CreateExpenseAsync(Guid residenceId, CreateExpenseDto dto);
    Task<ExpenseDto> GetExpenseByIdAsync(Guid id);
    Task<ExpenseDto> UpdateExpenseAsync(Guid id, UpdateExpenseDto dto);
    Task DeleteExpenseAsync(Guid id);
    Task<PagedResultDto<ExpenseDto>> GetExpensesByResidenceAsync(Guid residenceId, PaginationDto pagination);
    Task<ExpenseDto> AddImageToExpenseAsync(Guid expenseId, CreateExpenseImageDto dto);
    Task RemoveImageFromExpenseAsync(Guid imageId);

    // KPI and Statistics methods
    /// <summary>
    /// Get total expense KPI for a residence
    /// </summary>
    Task<TotalExpenseKpiDto> GetTotalExpenseKpiAsync(Guid residenceId);

    /// <summary>
    /// Get monthly expense breakdown for a residence
    /// </summary>
    Task<MonthlyExpensesDto> GetMonthlyExpensesAsync(Guid residenceId);

    /// <summary>
    /// Get expense statistics by type for a residence
    /// </summary>
    Task<ExpenseStatsDto> GetExpenseStatsByTypeAsync(Guid residenceId);
}
