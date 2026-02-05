namespace residence.application.DTOs;

public record CreatePostCommentDto(
    Guid PostId,
    string Content
);

