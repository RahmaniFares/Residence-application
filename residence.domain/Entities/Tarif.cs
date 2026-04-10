using residence.domain.Common;
using System;
using System.Collections.Generic;

namespace residence.domain.Entities
{
    /// <summary>
    /// Represents a tariff/rate for a residence
    /// </summary>
    public class Tarif : BaseEntity
    {
        /// <summary>
        /// Residence ID
        /// </summary>
        public Guid ResidenceId { get; set; }

        /// <summary>
        /// Description of the tariff
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Amount per unit (e.g., per month, per apartment)
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Currency code (e.g., USD, EUR)
        /// </summary>
        public string Currency { get; set; } = "USD";

        /// <summary>
        /// Effective date when this tariff becomes active
        /// </summary>
        public DateTime EffectiveDate { get; set; }

        /// <summary>
        /// End date when this tariff is no longer active (null if current)
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Whether this tariff is currently active
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Additional notes about the tariff
        /// </summary>
        public string? Notes { get; set; }

        // Navigation properties
        public Residence Residence { get; set; } = null!;
        public ICollection<TarifHistory> History { get; set; } = new List<TarifHistory>();
    }
}
