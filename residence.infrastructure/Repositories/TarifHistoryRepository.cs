using Microsoft.EntityFrameworkCore;
using residence.domain.Entities;
using residence.application.Repositories;
using residence.infrastructure.Data;

namespace residence.infrastructure.Repositories;

/// <summary>
/// Repository implementation for TarifHistory entity
/// </summary>
public class TarifHistoryRepository : Repository<TarifHistory>, ITarifHistoryRepository
{
    public TarifHistoryRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<TarifHistory>> GetHistoryByTarifIdAsync(Guid tarifId)
    {
        return await _dbSet
            .Where(h => h.TarifId == tarifId && !h.IsDeleted)
            .OrderByDescending(h => h.ChangedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<TarifHistory>> GetHistoryByResidenceIdAsync(Guid residenceId)
    {
        return await _dbSet
            .Where(h => h.ResidenceId == residenceId && !h.IsDeleted)
            .OrderByDescending(h => h.ChangedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<TarifHistory>> GetHistoryByDateRangeAsync(Guid residenceId, DateTime startDate, DateTime endDate)
    {
        return await _dbSet
            .Where(h => h.ResidenceId == residenceId && 
                   h.ChangedAt >= startDate && 
                   h.ChangedAt <= endDate && 
                   !h.IsDeleted)
            .OrderByDescending(h => h.ChangedAt)
            .ToListAsync();
    }
}
