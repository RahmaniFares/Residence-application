using residence.domain.Enums;

namespace residence.application.DTOs;

public record UpdatePaymentDto(
    PaymentStatus Status,
    DateTime? PaymentDate = null,
    string? Notes = null,
    DateTime? PeriodStart = null,
    DateTime? PeriodEnd = null,
    decimal? Amount = null,
    IEnumerable<UpdatePaymentLineDto>? Lines = null
);
