using residence.domain.Common;
using residence.domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace residence.domain.Entities
{
    /// <summary>
    /// Payment record
    /// </summary>
    public class Payment : BaseEntity
    {
        /// <summary>
        /// House ID
        /// </summary>
        public Guid HouseId { get; set; }

        /// <summary>
        /// Resident ID
        /// </summary>
        public Guid ResidentId { get; set; }

        /// <summary>
        /// Payment amount
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Payment method
        /// </summary>
        public PaymentMethod Method { get; set; } = PaymentMethod.Transfer;

        /// <summary>
        /// Period start date
        /// </summary>
        public DateTime PeriodStart { get; set; }

        /// <summary>
        /// Period end date
        /// </summary>
        public DateTime PeriodEnd { get; set; }

        /// <summary>
        /// Payment date (when actually paid)
        /// </summary>
        public DateTime? PaymentDate { get; set; }

        /// <summary>
        /// Payment status
        /// </summary>
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

        /// <summary>
        /// Additional notes
        /// </summary>
        public string? Notes { get; set; }

        // Navigation properties
        public House House { get; set; } = null!;
        public Resident Resident { get; set; } = null!;
    }
}
