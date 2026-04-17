namespace residence.application.DTOs;

public record PaymentLineDto(
    Guid Id,
    Guid PaymentId,
    int FromMonth,
    int FromYear,
    int ToMonth,
    int ToYear,
    decimal Tarif,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);
