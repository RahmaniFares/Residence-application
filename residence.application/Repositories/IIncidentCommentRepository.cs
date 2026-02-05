using residence.domain.Entities;

namespace residence.application.Repositories;

/// <summary>
/// Repository interface for IncidentComment entity
/// </summary>
public interface IIncidentCommentRepository : IRepository<IncidentComment>
{
    Task<IEnumerable<IncidentComment>> GetByIncidentAsync(Guid incidentId);
}
