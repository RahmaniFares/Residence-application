using residence.domain.Enums;

namespace residence.application.DTOs;

public record CreatePaymentDto(
    Guid HouseId,
    Guid ResidentId,
    decimal Amount,
    PaymentMethod Method,
    DateTime PeriodStart,
    DateTime PeriodEnd,
    string? Notes = null
);


