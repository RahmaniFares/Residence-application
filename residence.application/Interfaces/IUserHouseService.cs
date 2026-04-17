using residence.application.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace residence.application.Interfaces
{
    /// <summary>
    /// Service interface for User-House relationship management
    /// </summary>
    public interface IUserHouseService
    {
        /// <summary>
        /// Assign a user to a house
        /// </summary>
        Task<UserHouseDto> AssignUserToHouseAsync(Guid residenceId, CreateUserHouseDto dto);

        /// <summary>
        /// Remove user from house
        /// </summary>
        Task<bool> RemoveUserFromHouseAsync(Guid residenceId, Guid userId, Guid houseId);

        /// <summary>
        /// Get all houses for a user
        /// </summary>
        Task<UserHousesResponseDto> GetUserHousesAsync(Guid userId);

        /// <summary>
        /// Get all users assigned to a house
        /// </summary>
        Task<IEnumerable<UserHouseDto>> GetHouseUsersAsync(Guid houseId);

        /// <summary>
        /// Update user-house relationship
        /// </summary>
        Task<UserHouseDto> UpdateUserHouseAsync(Guid residenceId, Guid userId, Guid houseId, UpdateUserHouseDto dto);

        /// <summary>
        /// Check if user is assigned to house
        /// </summary>
        Task<bool> IsUserAssignedToHouseAsync(Guid userId, Guid houseId);

        /// <summary>
        /// Get user-house relationship details
        /// </summary>
        Task<UserHouseDto?> GetUserHouseDetailsAsync(Guid userId, Guid houseId);
    }
}
