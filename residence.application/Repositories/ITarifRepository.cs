using residence.domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace residence.application.Repositories
{
    /// <summary>
    /// Repository interface for Tarif operations
    /// </summary>
    public interface ITarifRepository : IRepository<Tarif>
    {
        /// <summary>
        /// Get all tariffs for a residence
        /// </summary>
        Task<IEnumerable<Tarif>> GetTarifsByResidenceAsync(Guid residenceId);

        /// <summary>
        /// Get current active tariff for a residence
        /// </summary>
        Task<Tarif?> GetCurrentTarifAsync(Guid residenceId);

        /// <summary>
        /// Get historical tariffs for a residence
        /// </summary>
        Task<IEnumerable<Tarif>> GetHistoricalTarifsByResidenceAsync(Guid residenceId);
    }

    /// <summary>
    /// Repository interface for TarifHistory operations
    /// </summary>
    public interface ITarifHistoryRepository : IRepository<TarifHistory>
    {
        /// <summary>
        /// Get history for a specific tariff
        /// </summary>
        Task<IEnumerable<TarifHistory>> GetHistoryByTarifIdAsync(Guid tarifId);

        /// <summary>
        /// Get history for a residence
        /// </summary>
        Task<IEnumerable<TarifHistory>> GetHistoryByResidenceIdAsync(Guid residenceId);

        /// <summary>
        /// Get all changes within a date range
        /// </summary>
        Task<IEnumerable<TarifHistory>> GetHistoryByDateRangeAsync(Guid residenceId, DateTime startDate, DateTime endDate);
    }
}
