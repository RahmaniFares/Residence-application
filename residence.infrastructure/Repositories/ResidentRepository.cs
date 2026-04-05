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
            .ToListAsync();
    }

    public async Task<IEnumerable<Resident>> GetByResidenceWithDetailsAsync(Guid residenceId)
    {
        return await _dbSet
            .Where(r => r.ResidenceId == residenceId && !r.IsDeleted)
            .Include(r => r.House)
            .ToListAsync();
    }

    
}
