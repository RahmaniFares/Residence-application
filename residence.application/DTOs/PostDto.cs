namespace residence.application.DTOs;

public record PostDto(
    Guid Id,
    Guid AuthorId,
    string AuthorName,
    string Content,
    string? ImageUrl,
    string? GifUrl,
    int LikeCount,
    int CommentCount,
    bool IsLikedByCurrentUser,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);

