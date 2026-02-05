using residence.domain.Enums;

namespace residence.application.DTOs;

public record HouseDetailDto(
    Guid Id,
    string Block,
    string Unit,
    string? Floor,
    HouseStatus Status,
    Guid? CurrentResidentId,
    ResidentDto? CurrentResident,
    int TotalResidents,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);

