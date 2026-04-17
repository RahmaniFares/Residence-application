namespace residence.application.DTOs;

public record UpdatePaymentLineDto(
    Guid? Id,
    int FromMonth,
    int FromYear,
    int ToMonth,
    int ToYear,
    decimal Tarif
);