using residence.domain.Enums;

namespace residence.application.DTOs;

public record RappelDto(
    Guid Id,
    Guid HouseId,
    decimal Amount,
    RappelStatus Status,
    DateTime? PaymentDate,
    string? Notes,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);

public record CreateRappelDto(
    Guid HouseId,
    decimal Amount,
    string? Notes = null
);

public record UpdateRappelDto(
    RappelStatus Status,
    DateTime? PaymentDate = null,
    string? Notes = null
);
