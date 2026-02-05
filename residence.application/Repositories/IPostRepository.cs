using residence.domain.Entities;

namespace residence.application.Repositories;

/// <summary>
/// Repository interface for Post entity
/// </summary>
public interface IPostRepository : IRepository<Post>
{
    Task<Post?> GetWithDetailsAsync(Guid id);
    Task<IEnumerable<Post>> GetByAuthorAsync(Guid authorId);
    Task<Post?> GetWithLikesAndCommentsAsync(Guid id);
}
