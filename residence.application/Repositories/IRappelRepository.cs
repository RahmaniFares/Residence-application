using residence.domain.Entities;
using residence.domain.Enums;

namespace residence.application.Repositories;

public interface IRappelRepository : IRepository<Rappel>
{
    Task<IEnumerable<Rappel>> GetByHouseAsync(Guid houseId);
    Task<IEnumerable<Rappel>> GetByStatusAsync(Guid residenceId, RappelStatus status);
}
