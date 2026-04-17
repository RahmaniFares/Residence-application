using residence.domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace residence.application.Repositories
{
    /// <summary>
    /// Repository interface for UserHouse relationship management
    /// </summary>
    public interface IUserHouseRepository : IRepository<UserHouse>
    {
        /// <summary>
        /// Get all houses for a specific user
        /// </summary>
        Task<IEnumerable<UserHouse>> GetUserHousesAsync(Guid userId);

        /// <summary>
        /// Get all users for a specific house
        /// </summary>
        Task<IEnumerable<UserHouse>> GetHouseUsersAsync(Guid houseId);

        /// <summary>
        /// Get specific user-house relationship
        /// </summary>
        Task<UserHouse?> GetUserHouseAsync(Guid userId, Guid houseId);

        /// <summary>
        /// Check if user is assigned to house
        /// </summary>
        Task<bool> IsUserAssignedToHouseAsync(Guid userId, Guid houseId);

        /// <summary>
        /// Remove user from house
        /// </summary>
        Task RemoveUserFromHouseAsync(Guid userId, Guid houseId);

        /// <summary>
        /// Get all houses for users in a residence
        /// </summary>
        Task<IEnumerable<UserHouse>> GetUserHousesByResidenceAsync(Guid residenceId);

        /// <summary>
        /// Count houses for a user
        /// </summary>
        Task<int> CountUserHousesAsync(Guid userId);
    }
}
