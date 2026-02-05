namespace residence.application.DTOs;



public record CreatePostDto(
    string Content,
    string? ImageUrl = null,
    string? GifUrl = null
);

