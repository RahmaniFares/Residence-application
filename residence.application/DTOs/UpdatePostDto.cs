namespace residence.application.DTOs;

public record UpdatePostDto(
    string Content,
    string? ImageUrl = null,
    string? GifUrl = null
);
