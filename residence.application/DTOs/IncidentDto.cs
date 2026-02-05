using residence.application.DTOs;

namespace residence.application.DTOs;

public record IncidentDto(
    Guid Id,
    string Title,
    IncidentCategory Category,
    string Description,
    IncidentStatus Status,
    IncidentPriority Priority,
    Guid ResidentId,
    Guid? HouseId,
    int CommentCount,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);

