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
    /// Resident/Tenant information
    /// </summary>
    public class Resident : BaseEntity
    {
        /// <summary>
        /// Associated user ID
        /// </summary>
        public Guid? UserId { get; set; }

        /// <summary>
        /// Associated house ID
        /// </summary>
        public Guid? HouseId { get; set; }

        /// <summary>
        /// First name
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Last name
        /// </summary>
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Email address
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Phone number
        /// </summary>
        public string PhoneNumber { get; set; } = string.Empty;

        /// <summary>
        /// Resident address
        /// </summary>
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// Date of birth
        /// </summary>
        public DateOnly? BirthDate { get; set; }

        /// <summary>
        /// Resident status
        /// </summary>
        public ResidentStatus Status { get; set; } = ResidentStatus.Active;

        /// <summary>
        /// Move-in date
        /// </summary>
        public DateTime MoveInDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Move-out date
        /// </summary>
        public DateTime? MoveOutDate { get; set; }

        // Navigation properties
        public User? User { get; set; } 
        public House? House { get; set; }
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
        public ICollection<Incident> Incidents { get; set; } = new List<Incident>();
        public ICollection<Post> Posts { get; set; } = new List<Post>();
    }
}
