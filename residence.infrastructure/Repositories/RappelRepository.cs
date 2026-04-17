using Microsoft.EntityFrameworkCore;
using residence.domain.Entities;
using residence.domain.Enums;
using residence.application.Repositories;
using residence.infrastructure.Data;

namespace residence.infrastructure.Repositories;

public class RappelRepository : Repository<Rappel>, IRappelRepository
{
    public RappelRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Rappel>> GetByHouseAsync(Guid houseId)
    {
        return await _dbSet
            .Where(r => r.HouseId == houseId && !r.IsDeleted)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Rappel>> GetByStatusAsync(Guid residenceId, RappelStatus status)
    {
        return await _dbSet
            .Where(r => r.ResidenceId == residenceId && r.Status == status && !r.IsDeleted)
            .ToListAsync();
    }
}