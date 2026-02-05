using residence.domain.Entities;
using residence.domain.Enums;

namespace residence.application.Repositories;

/// <summary>
/// Repository interface for Payment entity
/// </summary>
public interface IPaymentRepository : IRepository<Payment>
{
    Task<IEnumerable<Payment>> GetByResidentAsync(Guid residentId);
    Task<IEnumerable<Payment>> GetByHouseAsync(Guid houseId);
    Task<IEnumerable<Payment>> GetByStatusAsync(Guid residenceId, PaymentStatus status);
    Task<IEnumerable<Payment>> GetOverdueAsync(Guid residenceId);
    Task<decimal> GetTotalPaidAsync(Guid residentId);
    Task<decimal> GetTotalPendingAsync(Guid residenceId);
}
