using residence.application.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace residence.application.Interfaces
{
    /// <summary>
    /// Service interface for Donation business logic
    /// </summary>
    public interface IDonationService
    {
        /// <summary>
        /// Create a new donation
        /// </summary>
        Task<DonationDto> CreateDonationAsync(Guid houseId, CreateDonationDto dto);

        /// <summary>
        /// Get donation by ID
        /// </summary>
        Task<DonationDto> GetDonationByIdAsync(Guid id);

        /// <summary>
        /// Get all donations for a house
        /// </summary>
        Task<IEnumerable<DonationDto>> GetDonationsByHouseAsync(Guid houseId);

        /// <summary>
        /// Get all donations from a donor
        /// </summary>
        Task<IEnumerable<DonationDto>> GetDonationsByDonorAsync(Guid donorId);

        /// <summary>
        /// Get all donations
        /// </summary>
        Task<IEnumerable<DonationDto>> GetAllDonationsAsync();

        /// <summary>
        /// Update a donation
        /// </summary>
        Task<DonationDto> UpdateDonationAsync(Guid id, UpdateDonationDto dto);

        /// <summary>
        /// Delete a donation
        /// </summary>
        Task DeleteDonationAsync(Guid id);

        /// <summary>
        /// Get donations within a date range
        /// </summary>
        Task<IEnumerable<DonationDto>> GetDonationsByDateRangeAsync(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Get total donation amount for a house
        /// </summary>
        Task<decimal> GetTotalDonationsByHouseAsync(Guid houseId);

        /// <summary>
        /// Get donation details with related house and donor info
        /// </summary>
        Task<DonationDetailDto> GetDonationDetailsAsync(Guid id);
    }
}
