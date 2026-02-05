namespace residence.application.DTOs;

public record CreateIncidentDto(
    string Title,
    IncidentCategory Category,
    string Description,
    Guid ResidentId,
    Guid? HouseId = null,
    IncidentPriority Priority = IncidentPriority.Medium
);

