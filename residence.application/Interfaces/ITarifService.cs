using residence.application.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace residence.application.Interfaces
{
    /// <summary>
    /// Service interface for tariff management
    /// </summary>
    public interface ITarifService
    {
        /// <summary>
        /// Create a new tariff
        /// </summary>
        Task<TarifDto> CreateTarifAsync(Guid residenceId, CreateTarifDto dto, string userId);

        /// <summary>
        /// Update an existing tariff
        /// </summary>
        Task<TarifDto> UpdateTarifAsync(Guid residenceId, Guid tarifId, UpdateTarifDto dto, string userId);

        /// <summary>
        /// Get tariff by ID
        /// </summary>
        Task<TarifDto?> GetTarifByIdAsync(Guid tarifId);

        /// <summary>
        /// Get all tariffs for a residence
        /// </summary>
        Task<IEnumerable<TarifDto>> GetTarifsByResidenceAsync(Guid residenceId);

        /// <summary>
        /// Get current active tariff for a residence
        /// </summary>
        Task<TarifDto?> GetCurrentTarifAsync(Guid residenceId);

        /// <summary>
        /// Get history of tariff changes
        /// </summary>
        Task<IEnumerable<TarifHistoryDto>> GetTarifHistoryAsync(Guid tarifId);

        /// <summary>
        /// Get all tariff changes for a residence
        /// </summary>
        Task<IEnumerable<TarifHistoryDto>> GetResidenceTarifHistoryAsync(Guid residenceId);

        /// <summary>
        /// Get tariff changes within a date range
        /// </summary>
        Task<IEnumerable<TarifHistoryDto>> GetTarifHistoryByDateRangeAsync(Guid residenceId, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Delete a tariff (soft delete)
        /// </summary>
        Task<bool> DeleteTarifAsync(Guid residenceId, Guid tarifId);
    }
}
