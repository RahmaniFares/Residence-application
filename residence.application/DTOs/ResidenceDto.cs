using residence.domain.Enums;

namespace residence.application.DTOs;

public record ResidenceDto(
    Guid Id,
    string Name,
    string Address,
    string City,
    string State,
    string ZipCode,
    string Description,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);


