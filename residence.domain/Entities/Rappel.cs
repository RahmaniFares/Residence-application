using residence.domain.Common;
using residence.domain.Enums;
using System;

namespace residence.domain.Entities
{
    /// <summary>
    /// Represents a retroactive payment / backpay amount required
    /// when a tariff is increased and the house has already paid in advance.
    /// </summary>
    public class Rappel : BaseEntity
    {
        public Guid HouseId { get; set; }

        public decimal Amount { get; set; }

        public RappelStatus Status { get; set; } = RappelStatus.Unpaid;

        /// <summary>
        /// Date when this rappel was actually paid
        /// </summary>
        public DateTime? PaymentDate { get; set; }

        /// <summary>
        /// Reason or notes for this rappel
        /// </summary>
        public string? Notes { get; set; }

        // Navigation properties
        public House House { get; set; } = null!;
    }
}
