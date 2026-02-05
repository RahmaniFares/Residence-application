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
    /// User account in the system
    /// </summary>
    public class User : BaseEntity
    {
        /// <summary>
        /// Email address (unique per residence)
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Password hash (BCrypt)
        /// </summary>
        public string PasswordHash { get; set; } = string.Empty;

        /// <summary>
        /// First name
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Last name
        /// </summary>
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Phone number
        /// </summary>
        public string PhoneNumber { get; set; } = string.Empty;

        /// <summary>
        /// User role (Admin/Resident)
        /// </summary>
        public UserRole Role { get; set; } = UserRole.Resident;

        /// <summary>
        /// Avatar image URL
        /// </summary>
        public string? AvatarUrl { get; set; }

        // Navigation property
        public Resident? Resident { get; set; }
    }
}
