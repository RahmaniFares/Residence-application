using residence.application.DTOs;

namespace residence.application.Interfaces;

/// <summary>
/// Payment service interface
/// </summary>
public interface IPaymentService
{
    Task<PaymentDto> CreatePaymentAsync(Guid residenceId, CreatePaymentDto dto);
    Task<PaymentDto> GetPaymentByIdAsync(Guid id);
    Task<PaymentDto> UpdatePaymentAsync(Guid id, UpdatePaymentDto dto);
    Task DeletePaymentAsync(Guid id);
    Task<PagedResultDto<PaymentDto>> GetPaymentsByResidenceAsync(Guid residenceId, PaginationDto pagination);
    Task<PagedResultDto<PaymentDto>> GetPaymentsByResidentAsync(Guid residentId, PaginationDto pagination);
    Task<PagedResultDto<PaymentDto>> GetPaymentsByHouseAsync(Guid houseId, PaginationDto pagination);

    /// <summary>
    /// Get payment KPI/statistics for a residence
    /// </summary>
    Task<PaymentKpiDto> GetPaymentKpiAsync(Guid residenceId);

    /// <summary>
    /// Get payment KPI for a specific period
    /// </summary>
    Task<PaymentKpiDto> GetPaymentKpiByDateRangeAsync(Guid residenceId, DateTime startDate, DateTime endDate);

    /// <summary>
    /// Get monthly payment summaries
    /// </summary>
    Task<IEnumerable<MonthlyPaymentSummaryDto>> GetMonthlyPaymentSummaryAsync(Guid residenceId, int months = 12);

    /// <summary>
    /// Get payment trend data for charts
    /// </summary>
    Task<IEnumerable<PaymentTrendDto>> GetPaymentTrendAsync(Guid residenceId, DateTime startDate, DateTime endDate);
}
