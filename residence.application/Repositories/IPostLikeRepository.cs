using residence.domain.Entities;

namespace residence.application.Repositories;

/// <summary>
/// Repository interface for PostLike entity
/// </summary>
public interface IPostLikeRepository : IRepository<PostLike>
{
    Task<PostLike?> GetByPostAndUserAsync(Guid postId, Guid userId);
    Task<IEnumerable<PostLike>> GetByPostAsync(Guid postId);
    Task<int> GetLikeCountAsync(Guid postId);
}
