using Microsoft.EntityFrameworkCore;
using residence.domain.Entities;
using residence.domain.Enums;
using residence.application.Repositories;
using residence.infrastructure.Data;

namespace residence.infrastructure.Repositories;

/// <summary>
/// Repository implementation for Incident entity
/// </summary>
public class IncidentRepository : Repository<Incident>, IIncidentRepository
{
    public IncidentRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Incident>> GetByResidentAsync(Guid residentId)
    {
        return await _dbSet
            .Where(i => i.ResidentId == residentId && !i.IsDeleted)
            .Include(i => i.Resident)
            .Include(i => i.House)
            .Include(i => i.Comments)
            .OrderByDescending(i => i.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Incident>> GetByStatusAsync(Guid residenceId, IncidentStatus status)
    {
        return await _dbSet
            .Where(i => i.ResidenceId == residenceId && i.Status == status && !i.IsDeleted)
            .Include(i => i.Resident)
            .Include(i => i.House)
            .OrderByDescending(i => i.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Incident>> GetByPriorityAsync(Guid residenceId, IncidentPriority priority)
    {
        return await _dbSet
            .Where(i => i.ResidenceId == residenceId && i.Priority == priority && !i.IsDeleted)
            .Include(i => i.Resident)
            .Include(i => i.House)
            .OrderByDescending(i => i.CreatedAt)
            .ToListAsync();
    }

    public async Task<Incident?> GetWithCommentsAsync(Guid id)
    {
        return await _dbSet
            .Where(i => i.Id == id && !i.IsDeleted)
            .Include(i => i.Comments)
            .Include(i => i.Resident)
            .Include(i => i.House)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Incident>> GetOpenAsync(Guid residenceId)
    {
        return await _dbSet
            .Where(i => i.ResidenceId == residenceId && 
                   i.Status == IncidentStatus.Open && 
                   !i.IsDeleted)
            .Include(i => i.Resident)
            .Include(i => i.House)
            .OrderByDescending(i => i.Priority)
            .ToListAsync();
    }
}
