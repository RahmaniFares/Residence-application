using residence.domain.Entities;

namespace residence.application.Repositories;

/// <summary>
/// Repository interface for Resident entity
/// </summary>
public interface IResidentRepository : IRepository<Resident>
{
    Task<IEnumerable<Resident>> GetByHouseAsync(Guid houseId);
    Task<IEnumerable<Resident>> GetByResidenceWithDetailsAsync(Guid residenceId);
}
