using residence.domain.Common;
using System;

namespace residence.domain.Entities
{
    /// <summary>
    /// Donation/Contribution entity (Encaissement in French)
    /// Represents a donation or contribution received for a house
    /// </summary>
    public class Donation : BaseEntity
    {
        /// <summary>
        /// Associated house ID (Foreign Key)
        /// </summary>
        public Guid? HouseId { get; set; }

        /// <summary>
        /// Donor/Contributor ID (Foreign Key) - Optional
        /// </summary>
        public Guid? DonorId { get; set; }

        /// <summary>
        /// Donation amount
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Date of donation
        /// </summary>
        public DateTime DonationDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Description or purpose of donation
        /// </summary>
        public string? Description { get; set; }

        // Navigation properties
        /// <summary>
        /// Associated house
        /// </summary>
        public House? House { get; set; }

        /// <summary>
        /// Donor/Contributor (Resident)
        /// </summary>
        public Resident? Donor { get; set; }
    }
}
