using residence.domain.Enums;

namespace residence.application.DTOs;

public record CreatePaymentDto(
    Guid HouseId,
    Guid ResidentId,
    decimal Amount,
    PaymentMethod Method,
    DateTime PeriodStart,
    DateTime PeriodEnd,
    DateTime? PaymentDate,
    string? ReceiptNumber = null,
    string? Notes = null,
    IEnumerable<CreatePaymentLineDto>? Lines = null
);


