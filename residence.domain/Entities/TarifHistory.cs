using residence.domain.Common;
using System;

namespace residence.domain.Entities
{
    /// <summary>
    /// Audit trail for tariff changes
    /// </summary>
    public class TarifHistory : BaseEntity
    {
        /// <summary>
        /// Tarif ID
        /// </summary>
        public Guid TarifId { get; set; }

        /// <summary>
        /// Residence ID
        /// </summary>
        public Guid ResidenceId { get; set; }

        /// <summary>
        /// Previous amount before the change
        /// </summary>
        public decimal PreviousAmount { get; set; }

        /// <summary>
        /// New amount after the change
        /// </summary>
        public decimal NewAmount { get; set; }

        /// <summary>
        /// Previous description
        /// </summary>
        public string PreviousDescription { get; set; } = string.Empty;

        /// <summary>
        /// New description
        /// </summary>
        public string NewDescription { get; set; } = string.Empty;

        /// <summary>
        /// Date when the change became/will become effective
        /// </summary>
        public DateTime EffectiveDate { get; set; }

        /// <summary>
        /// User or system that made this change
        /// </summary>
        public string ChangedBy { get; set; } = string.Empty;

        /// <summary>
        /// Reason for the change
        /// </summary>
        public string? ChangeReason { get; set; }

        /// <summary>
        /// Timestamp of when the change was recorded
        /// </summary>
        public DateTime ChangedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public Tarif Tarif { get; set; } = null!;
        public Residence Residence { get; set; } = null!;
    }
}
