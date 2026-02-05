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
}
