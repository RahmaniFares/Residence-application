using Microsoft.EntityFrameworkCore;
using residence.domain.Entities;
using residence.application.Repositories;
using residence.infrastructure.Data;

namespace residence.infrastructure.Repositories;

/// <summary>
/// Repository implementation for IncidentComment entity
/// </summary>
public class IncidentCommentRepository : Repository<IncidentComment>, IIncidentCommentRepository
{
    public IncidentCommentRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<IncidentComment>> GetByIncidentAsync(Guid incidentId)
    {
        return await _dbSet
            .Where(ic => ic.IncidentId == incidentId && !ic.IsDeleted)
            .OrderByDescending(ic => ic.CreatedAt)
            .ToListAsync();
    }
}
