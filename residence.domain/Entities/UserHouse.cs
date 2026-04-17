using residence.domain.Common;
using System;

namespace residence.domain.Entities
{
    /// <summary>
    /// Junction entity representing the relationship between User and House
    /// Allows multiple users to be associated with a house (e.g., multiple residents in same unit)
    /// </summary>
    public class UserHouse : BaseEntity
    {
        /// <summary>
        /// User ID
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// House ID
        /// </summary>
        public Guid HouseId { get; set; }

        /// <summary>
        /// Assignment date (when user was assigned to house)
        /// </summary>
        public DateTime AssignedDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Notes about the assignment
        /// </summary>
        public string? Notes { get; set; }

        // Navigation properties
        public User User { get; set; } = null!;
        public House House { get; set; } = null!;
    }
}
