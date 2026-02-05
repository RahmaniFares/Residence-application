namespace residence.application.DTOs;

public record CreateIncidentCommentDto(
    Guid IncidentId,
    string Text
);

