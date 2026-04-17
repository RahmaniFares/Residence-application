namespace residence.application.DTOs;

public record CreatePaymentLineDto(
    int FromMonth,
    int FromYear,
    int ToMonth,
    int ToYear,
    decimal Tarif
);
