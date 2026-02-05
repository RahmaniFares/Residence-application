namespace residence.application.DTOs;

public record PostCommentDto(
    Guid Id,
    Guid PostId,
    Guid AuthorId,
    string AuthorName,
    string Content,
    DateTime CreatedAt
);

