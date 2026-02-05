using Microsoft.EntityFrameworkCore;
using residence.domain.Entities;
using residence.domain.Enums;
using residence.application.Repositories;
using residence.infrastructure.Data;

namespace residence.infrastructure.Repositories;

/// <summary>
/// Repository implementation for Resident entity
/// </summary>
public class ResidentRepository : Repository<Resident>, IResidentRepository
{
    public ResidentRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Resident>> GetByHouseAsync(Guid houseId)
    {
        return await _dbSet
            .Where(r => r.HouseId == houseId && !r.IsDeleted)
            .Include(r => r.User)
            .ToListAsync();
    }

    public async Task<IEnumerable<Resident>> GetByResidenceWithDetailsAsync(Guid residenceId)
    {
        return await _dbSet
            .Where(r => r.ResidenceId == residenceId && !r.IsDeleted)
            .Include(r => r.User)
            .Include(r => r.House)
            .ToListAsync();
    }

    public async Task<Resident?> GetWithUserAsync(Guid id)
    {
        return await _dbSet
            .Where(r => r.Id == id && !r.IsDeleted)
            .Include(r => r.User)
            .FirstOrDefaultAsync();
    }

    public async Task<Resident?> GetByUserIdAsync(Guid userId)
    {
        return await _dbSet
            .Where(r => r.UserId == userId && !r.IsDeleted)
            .Include(r => r.User)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Resident>> GetActiveAsync(Guid residenceId)
    {
        return await _dbSet
            .Where(r => r.ResidenceId == residenceId && r.Status == ResidentStatus.Active && !r.IsDeleted)
            .Include(r => r.User)
            .Include(r => r.House)
            .ToListAsync();
    }
}
