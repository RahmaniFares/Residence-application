using Microsoft.EntityFrameworkCore;
using residence.domain.Entities;
using residence.application.Repositories;
using residence.infrastructure.Data;

namespace residence.infrastructure.Repositories;

/// <summary>
/// Repository implementation for Tarif entity
/// </summary>
public class TarifRepository : Repository<Tarif>, ITarifRepository
{
    public TarifRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Tarif>> GetTarifsByResidenceAsync(Guid residenceId)
    {
        return await _dbSet
            .Where(t => t.ResidenceId == residenceId && !t.IsDeleted)
            .OrderByDescending(t => t.EffectiveDate)
            .ToListAsync();
    }

    public async Task<Tarif?> GetCurrentTarifAsync(Guid residenceId)
    {
        return await _dbSet
            .Where(t => t.ResidenceId == residenceId && t.IsActive && !t.IsDeleted)
            .OrderByDescending(t => t.EffectiveDate)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Tarif>> GetHistoricalTarifsByResidenceAsync(Guid residenceId)
    {
        return await _dbSet
            .Where(t => t.ResidenceId == residenceId && !t.IsActive && !t.IsDeleted)
            .OrderByDescending(t => t.EndDate)
            .ToListAsync();
    }
}
