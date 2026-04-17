using Microsoft.EntityFrameworkCore;
using residence.domain.Entities;
using residence.application.Repositories;
using residence.infrastructure.Data;

namespace residence.infrastructure.Repositories;

/// <summary>
/// Repository implementation for UserHouse relationship management
/// </summary>
public class UserHouseRepository : Repository<UserHouse>, IUserHouseRepository
{
    public UserHouseRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<UserHouse>> GetUserHousesAsync(Guid userId)
    {
        return await _dbSet
            .Where(uh => uh.UserId == userId && !uh.IsDeleted)
            .Include(uh => uh.House)
            .Include(uh => uh.User)
            .OrderByDescending(uh => uh.AssignedDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<UserHouse>> GetHouseUsersAsync(Guid houseId)
    {
        return await _dbSet
            .Where(uh => uh.HouseId == houseId && !uh.IsDeleted)
            .Include(uh => uh.User)
            .Include(uh => uh.House)
            .OrderByDescending(uh => uh.AssignedDate)
            .ToListAsync();
    }

    public async Task<UserHouse?> GetUserHouseAsync(Guid userId, Guid houseId)
    {
        return await _dbSet
            .FirstOrDefaultAsync(uh => uh.UserId == userId && uh.HouseId == houseId && !uh.IsDeleted);
    }

    public async Task<bool> IsUserAssignedToHouseAsync(Guid userId, Guid houseId)
    {
        return await _dbSet
            .AnyAsync(uh => uh.UserId == userId && uh.HouseId == houseId && !uh.IsDeleted);
    }

    public async Task RemoveUserFromHouseAsync(Guid userId, Guid houseId)
    {
        var userHouse = await GetUserHouseAsync(userId, houseId);
        if (userHouse != null)
        {
            await DeleteAsync(userHouse.Id);
        }
    }

    public async Task<IEnumerable<UserHouse>> GetUserHousesByResidenceAsync(Guid residenceId)
    {
        return await _dbSet
            .Where(uh => uh.House.ResidenceId == residenceId && !uh.IsDeleted)
            .Include(uh => uh.User)
            .Include(uh => uh.House)
            .OrderByDescending(uh => uh.AssignedDate)
            .ToListAsync();
    }

    public async Task<int> CountUserHousesAsync(Guid userId)
    {
        return await _dbSet
            .Where(uh => uh.UserId == userId && !uh.IsDeleted)
            .CountAsync();
    }
}
