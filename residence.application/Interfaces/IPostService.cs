using residence.application.DTOs;

namespace residence.application.Interfaces;

/// <summary>
/// Post service interface
/// </summary>
public interface IPostService
{
    Task<PostDto> CreatePostAsync(Guid residenceId, Guid authorId, CreatePostDto dto);
    Task<PostDto> GetPostByIdAsync(Guid id, Guid? currentUserId = null);
    Task<PostDto> UpdatePostAsync(Guid id, UpdatePostDto dto);
    Task DeletePostAsync(Guid id);
    Task<PagedResultDto<PostDto>> GetPostsByResidenceAsync(Guid residenceId, Guid? currentUserId, PaginationDto pagination);
    Task<PostDto> LikePostAsync(Guid postId, Guid userId);
    Task RemoveLikeAsync(Guid postId, Guid userId);
    Task<PostCommentDto> AddCommentAsync(Guid postId, Guid authorId, CreatePostCommentDto dto);
    Task RemoveCommentAsync(Guid commentId);
    Task<PagedResultDto<PostCommentDto>> GetCommentsAsync(Guid postId, PaginationDto pagination);
}
