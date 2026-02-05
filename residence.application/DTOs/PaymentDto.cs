using residence.domain.Enums;

namespace residence.application.DTOs;

public record PaymentDto(
    Guid Id,
    Guid HouseId,
    Guid ResidentId,
    decimal Amount,
    PaymentMethod Method,
    DateTime PeriodStart,
    DateTime PeriodEnd,
    DateTime? PaymentDate,
    PaymentStatus Status,
    string? Notes,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);
