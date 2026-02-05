using residence.domain.Entities;

namespace residence.application.Repositories;

/// <summary>
/// Repository interface for PostComment entity
/// </summary>
public interface IPostCommentRepository : IRepository<PostComment>
{
    Task<IEnumerable<PostComment>> GetByPostAsync(Guid postId);
    Task<IEnumerable<PostComment>> GetByAuthorAsync(Guid authorId);
}
