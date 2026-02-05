using residence.domain.Enums;

namespace residence.application.DTOs;

public record ResidenceSettingsDto(
    Guid Id,
    string ResidenceName,
    string ResidencePlace,
    decimal InitialBudget
);
