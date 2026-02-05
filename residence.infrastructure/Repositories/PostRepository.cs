using Microsoft.EntityFrameworkCore;
using residence.domain.Entities;
using residence.application.Repositories;
using residence.infrastructure.Data;

namespace residence.infrastructure.Repositories;

/// <summary>
/// Repository implementation for Post entity
/// </summary>
public class PostRepository : Repository<Post>, IPostRepository
{
    public PostRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Post?> GetWithDetailsAsync(Guid id)
    {
        return await _dbSet
            .Where(p => p.Id == id && !p.IsDeleted)
            .Include(p => p.Author)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Post>> GetByAuthorAsync(Guid authorId)
    {
        return await _dbSet
            .Where(p => p.AuthorId == authorId && !p.IsDeleted)
            .Include(p => p.Author)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<Post?> GetWithLikesAndCommentsAsync(Guid id)
    {
        return await _dbSet
            .Where(p => p.Id == id && !p.IsDeleted)
            .Include(p => p.Author)
            .Include(p => p.Likes)
            .Include(p => p.Comments)
                .ThenInclude(c => c.Author)
            .FirstOrDefaultAsync();
    }
}
