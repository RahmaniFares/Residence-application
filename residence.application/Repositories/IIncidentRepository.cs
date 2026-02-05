using residence.domain.Entities;
using residence.domain.Enums;

namespace residence.application.Repositories;

/// <summary>
/// Repository interface for Incident entity
/// </summary>
public interface IIncidentRepository : IRepository<Incident>
{
    Task<IEnumerable<Incident>> GetByResidentAsync(Guid residentId);
    Task<IEnumerable<Incident>> GetByStatusAsync(Guid residenceId, IncidentStatus status);
    Task<IEnumerable<Incident>> GetByPriorityAsync(Guid residenceId, IncidentPriority priority);
    Task<Incident?> GetWithCommentsAsync(Guid id);
    Task<IEnumerable<Incident>> GetOpenAsync(Guid residenceId);
}
