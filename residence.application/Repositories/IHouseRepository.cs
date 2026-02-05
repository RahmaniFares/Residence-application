using residence.domain.Entities;

namespace residence.application.Repositories;

/// <summary>
/// Repository interface for House entity
/// </summary>
public interface IHouseRepository : IRepository<House>
{
    Task<IEnumerable<House>> GetByResidenceWithDetailsAsync(Guid residenceId);
    Task<House?> GetWithResidentsAsync(Guid id);
    Task<IEnumerable<House>> GetOccupiedAsync(Guid residenceId);
    Task<IEnumerable<House>> GetVacantAsync(Guid residenceId);
}
