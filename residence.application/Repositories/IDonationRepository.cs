using residence.domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace residence.application.Repositories
{
    /// <summary>
    /// Repository interface for Donation entity
    /// </summary>
    public interface IDonationRepository
    {
        /// <summary>
        /// Get donation by ID
        /// </summary>
        Task<Donation?> GetByIdAsync(Guid id);

        /// <summary>
        /// Get all donations for a specific house
        /// </summary>
        Task<IEnumerable<Donation>> GetByHouseAsync(Guid houseId);

        /// <summary>
        /// Get all donations from a specific donor
        /// </summary>
        Task<IEnumerable<Donation>> GetByDonorAsync(Guid donorId);

        /// <summary>
        /// Get all donations
        /// </summary>
        Task<IEnumerable<Donation>> GetAllAsync();

        /// <summary>
        /// Add a new donation
        /// </summary>
        Task<Donation> AddAsync(Donation donation);

        /// <summary>
        /// Update an existing donation
        /// </summary>
        Task<Donation> UpdateAsync(Donation donation);

        /// <summary>
        /// Delete a donation
        /// </summary>
        Task DeleteAsync(Guid id);

        /// <summary>
        /// Get donations within a date range
        /// </summary>
        Task<IEnumerable<Donation>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Get total donation amount for a house
        /// </summary>
        Task<decimal> GetTotalByHouseAsync(Guid houseId);

        /// <summary>
        /// Get donations with house details
        /// </summary>
        Task<IEnumerable<Donation>> GetWithHouseDetailsAsync(Guid houseId);
    }
}
