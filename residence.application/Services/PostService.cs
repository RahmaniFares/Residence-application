using residence.application.DTOs;
using residence.application.Interfaces;
using residence.domain.Entities;
using residence.application.Repositories;

namespace residence.application.Services;

/// <summary>
/// Implementation of Post service
/// </summary>
public class PostService : IPostService
{
    private readonly IPostRepository _postRepository;
    private readonly IPostLikeRepository _postLikeRepository;
    private readonly IPostCommentRepository _postCommentRepository;

    public PostService(
        IPostRepository postRepository,
        IPostLikeRepository postLikeRepository,
        IPostCommentRepository postCommentRepository)
    {
        _postRepository = postRepository;
        _postLikeRepository = postLikeRepository;
        _postCommentRepository = postCommentRepository;
    }

    public async Task<PostDto> CreatePostAsync(Guid residenceId, Guid authorId, CreatePostDto dto)
    {
        var post = new Post
        {
            Id = Guid.NewGuid(),
            ResidenceId = residenceId,
            AuthorId = authorId,
            Content = dto.Content,
            ImageUrl = dto.ImageUrl,
            GifUrl = dto.GifUrl,
            CreatedAt = DateTime.UtcNow
        };

        var created = await _postRepository.AddAsync(post);
        return MapToDto(created, false);
    }

    public async Task<PostDto> GetPostByIdAsync(Guid id, Guid? currentUserId = null)
    {
        var post = await _postRepository.GetWithLikesAndCommentsAsync(id);
        if (post == null)
            throw new Exception("Post not found");

        var isLikedByCurrentUser = currentUserId.HasValue && 
            post.Likes.Any(l => l.UserId == currentUserId.Value);

        return MapToDto(post, isLikedByCurrentUser);
    }

    public async Task<PostDto> UpdatePostAsync(Guid id, UpdatePostDto dto)
    {
        var post = await _postRepository.GetByIdAsync(id);
        if (post == null)
            throw new Exception("Post not found");

        post.Content = dto.Content;
        post.ImageUrl = dto.ImageUrl;
        post.GifUrl = dto.GifUrl;
        post.UpdatedAt = DateTime.UtcNow;

        await _postRepository.UpdateAsync(post);

        return await GetPostByIdAsync(id);
    }

    public async Task DeletePostAsync(Guid id)
    {
        var post = await _postRepository.GetByIdAsync(id);
        if (post == null)
            throw new Exception("Post not found");

        await _postRepository.DeleteAsync(id);
    }

    public async Task<PagedResultDto<PostDto>> GetPostsByResidenceAsync(Guid residenceId, Guid? currentUserId, PaginationDto pagination)
    {
        var posts = await _postRepository.GetByResidenceAsync(residenceId);
        
        var total = posts.Count();
        var items = posts
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .Select(p => {
                var isLiked = currentUserId.HasValue && p.Likes?.Any(l => l.UserId == currentUserId.Value) == true;
                return MapToDto(p, isLiked);
            })
            .ToList();

        var totalPages = (int)Math.Ceiling(total / (double)pagination.PageSize);

        return new PagedResultDto<PostDto>(items, total, pagination.PageNumber, pagination.PageSize, totalPages);
    }

    public async Task<PostDto> LikePostAsync(Guid postId, Guid userId)
    {
        var post = await _postRepository.GetByIdAsync(postId);
        if (post == null)
            throw new Exception("Post not found");

        var existingLike = await _postLikeRepository.GetByPostAndUserAsync(postId, userId);
        if (existingLike != null)
            throw new Exception("Post already liked by this user");

        var like = new PostLike
        {
            Id = Guid.NewGuid(),
            ResidenceId = post.ResidenceId,
            PostId = postId,
            UserId = userId,
            CreatedAt = DateTime.UtcNow
        };

        await _postLikeRepository.AddAsync(like);

        return await GetPostByIdAsync(postId, userId);
    }

    public async Task RemoveLikeAsync(Guid postId, Guid userId)
    {
        var like = await _postLikeRepository.GetByPostAndUserAsync(postId, userId);
        if (like == null)
            throw new Exception("Like not found");

        await _postLikeRepository.DeleteAsync(like.Id);
    }

    public async Task<PostCommentDto> AddCommentAsync(Guid postId, Guid authorId, CreatePostCommentDto dto)
    {
        var post = await _postRepository.GetByIdAsync(postId);
        if (post == null)
            throw new Exception("Post not found");

        var comment = new PostComment
        {
            Id = Guid.NewGuid(),
            ResidenceId = post.ResidenceId,
            PostId = postId,
            AuthorId = authorId,
            Content = dto.Content,
            CreatedAt = DateTime.UtcNow
        };

        var created = await _postCommentRepository.AddAsync(comment);
        return MapCommentToDto(created, "Unknown");
    }

    public async Task RemoveCommentAsync(Guid commentId)
    {
        var comment = await _postCommentRepository.GetByIdAsync(commentId);
        if (comment == null)
            throw new Exception("Comment not found");

        await _postCommentRepository.DeleteAsync(commentId);
    }

    public async Task<PagedResultDto<PostCommentDto>> GetCommentsAsync(Guid postId, PaginationDto pagination)
    {
        var comments = await _postCommentRepository.GetByPostAsync(postId);
        
        var total = comments.Count();
        var items = comments
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .Select(c => MapCommentToDto(c, c.Author?.FirstName ?? "Unknown"))
            .ToList();

        var totalPages = (int)Math.Ceiling(total / (double)pagination.PageSize);

        return new PagedResultDto<PostCommentDto>(items, total, pagination.PageNumber, pagination.PageSize, totalPages);
    }

    private PostDto MapToDto(Post post, bool isLikedByCurrentUser)
    {
        var authorName = post.Author?.FirstName ?? "Unknown";

        return new PostDto(
            post.Id,
            post.AuthorId,
            authorName,
            post.Content,
            post.ImageUrl,
            post.GifUrl,
            post.Likes?.Count ?? 0,
            post.Comments?.Count ?? 0,
            isLikedByCurrentUser,
            post.CreatedAt,
            post.UpdatedAt
        );
    }

    private PostCommentDto MapCommentToDto(PostComment comment, string authorName)
    {
        return new PostCommentDto(
            comment.Id,
            comment.PostId,
            comment.AuthorId,
            authorName,
            comment.Content,
            comment.CreatedAt
        );
    }
}
