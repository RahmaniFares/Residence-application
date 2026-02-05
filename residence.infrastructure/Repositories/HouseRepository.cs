using Microsoft.EntityFrameworkCore;
using residence.domain.Entities;
using residence.domain.Enums;
using residence.application.Repositories;
using residence.infrastructure.Data;

namespace residence.infrastructure.Repositories;

/// <summary>
/// Repository implementation for House entity
/// </summary>
public class HouseRepository : Repository<House>, IHouseRepository
{
    public HouseRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<House>> GetByResidenceWithDetailsAsync(Guid residenceId)
    {
        return await _dbSet
            .Where(h => h.ResidenceId == residenceId && !h.IsDeleted)
            .Include(h => h.CurrentResident)
            .Include(h => h.Residents)
            .ToListAsync();
    }

    public async Task<House?> GetWithResidentsAsync(Guid id)
    {
        return await _dbSet
            .Where(h => h.Id == id && !h.IsDeleted)
            .Include(h => h.CurrentResident)
            .Include(h => h.Residents)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<House>> GetOccupiedAsync(Guid residenceId)
    {
        return await _dbSet
            .Where(h => h.ResidenceId == residenceId && h.Status == HouseStatus.Occupied && !h.IsDeleted)
            .Include(h => h.CurrentResident)
            .ToListAsync();
    }

    public async Task<IEnumerable<House>> GetVacantAsync(Guid residenceId)
    {
        return await _dbSet
            .Where(h => h.ResidenceId == residenceId && h.Status == HouseStatus.Vacant && !h.IsDeleted)
            .ToListAsync();
    }
}
