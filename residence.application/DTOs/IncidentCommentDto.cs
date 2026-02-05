namespace residence.application.DTOs;

public record IncidentCommentDto(
    Guid Id,
    Guid IncidentId,
    Guid AuthorId,
    string Text,
    DateTime CreatedAt
);