using residence.domain.Enums;

namespace residence.application.DTOs;

public record UpdatePaymentDto(
    PaymentStatus Status,
    DateTime? PaymentDate = null,
    string? Notes = null
);
