using residence.domain.Enums;

namespace residence.application.DTOs;


public record CreateResidenceDto(
    string Name,
    string Address,
    string City,
    string State,
    string ZipCode,
    string Description
);

