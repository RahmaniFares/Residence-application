using residence.domain.Enums;

namespace residence.application.DTOs;

public record CreateHouseDto(
    Guid? ResidentId,
    string Block,
    string Unit,
    string? Floor = null
);


