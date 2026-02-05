namespace residence.application.DTOs;

public record UpdateIncidentDto(
    string Title,
    IncidentCategory Category,
    string Description,
    IncidentStatus Status = IncidentStatus.Open,
    IncidentPriority Priority = IncidentPriority.Medium
);

