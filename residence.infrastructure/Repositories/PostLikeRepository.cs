using Microsoft.EntityFrameworkCore;
using residence.domain.Entities;
using residence.application.Repositories;
using residence.infrastructure.Data;

namespace residence.infrastructure.Repositories;

/// <summary>
/// Repository implementation for PostLike entity
/// </summary>
public class PostLikeRepository : Repository<PostLike>, IPostLikeRepository
{
    public PostLikeRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<PostLike?> GetByPostAndUserAsync(Guid postId, Guid userId)
    {
        return await _dbSet
            .FirstOrDefaultAsync(pl => pl.PostId == postId && pl.UserId == userId && !pl.IsDeleted);
    }

    public async Task<IEnumerable<PostLike>> GetByPostAsync(Guid postId)
    {
        return await _dbSet
            .Where(pl => pl.PostId == postId && !pl.IsDeleted)
            .ToListAsync();
    }

    public async Task<int> GetLikeCountAsync(Guid postId)
    {
        return await _dbSet
            .CountAsync(pl => pl.PostId == postId && !pl.IsDeleted);
    }
}
