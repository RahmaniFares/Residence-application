using residence.domain.Enums;

namespace residence.application.DTOs;

public record UpdateHouseDto(
    Guid? ResidentId,
    string Block,
    string Unit,
    string? Floor = null,
    HouseStatus Status = HouseStatus.Vacant
);

