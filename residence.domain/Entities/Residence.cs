using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace residence.domain.Entities
{
    /// <summary>
    /// Represents a residence/building/community
    /// </summary>
    public class Residence
    {
        /// <summary>
        /// Unique identifier (GUID)
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Name of the residence/community
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Physical address
        /// </summary>
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// City location
        /// </summary>
        public string City { get; set; } = string.Empty;

        /// <summary>
        /// State/Province
        /// </summary>
        public string State { get; set; } = string.Empty;

        /// <summary>
        /// Postal code
        /// </summary>
        public string ZipCode { get; set; } = string.Empty;

        /// <summary>
        /// Detailed description
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Creation timestamp (UTC)
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Last update timestamp (UTC)
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Soft delete flag
        /// </summary>
        public bool IsDeleted { get; set; } = false;

        // Navigation properties
        public ICollection<User> Users { get; set; } = new List<User>();
        public ICollection<House> Houses { get; set; } = new List<House>();
        public ICollection<Resident> Residents { get; set; } = new List<Resident>();
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
        public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
        public ICollection<Incident> Incidents { get; set; } = new List<Incident>();
        public ICollection<Post> Posts { get; set; } = new List<Post>();
        public ResidenceSettings Settings { get; set; } = null!;
    }

}
