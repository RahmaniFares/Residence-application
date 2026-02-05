using residence.domain.Entities;

namespace residence.application.Repositories;

/// <summary>
/// Repository interface for User entity
/// </summary>
public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
    Task<IEnumerable<User>> GetByResidenceWithDetailsAsync(Guid residenceId);
}
