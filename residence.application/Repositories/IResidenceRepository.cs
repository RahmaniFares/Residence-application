using residence.domain.Entities;

namespace residence.application.Repositories;

public interface IResidenceRepository
{
    Task<IEnumerable<Residence>> GetAllAsync();
    Task<Residence?> GetByIdAsync(Guid id);
    Task<Residence> CreateAsync(Residence residence);
    Task<Residence> UpdateAsync(Residence residence);
    Task<bool> DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
}

