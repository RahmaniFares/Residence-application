using Microsoft.EntityFrameworkCore;
using residence.domain.Entities;
using residence.application.Repositories;
using residence.infrastructure.Data;

namespace residence.infrastructure.Repositories;

/// <summary>
/// Repository implementation for PostComment entity
/// </summary>
public class PostCommentRepository : Repository<PostComment>, IPostCommentRepository
{
    public PostCommentRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<PostComment>> GetByPostAsync(Guid postId)
    {
        return await _dbSet
            .Where(pc => pc.PostId == postId && !pc.IsDeleted)
            .Include(pc => pc.Author)
            .OrderByDescending(pc => pc.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<PostComment>> GetByAuthorAsync(Guid authorId)
    {
        return await _dbSet
            .Where(pc => pc.AuthorId == authorId && !pc.IsDeleted)
            .Include(pc => pc.Post)
            .OrderByDescending(pc => pc.CreatedAt)
            .ToListAsync();
    }
}
