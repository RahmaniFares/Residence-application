using residence.domain.Common;
using System;
using System.Collections.Generic;

namespace residence.domain.Entities
{
    /// <summary>
    /// Employee entity representing residence staff (gardien, femme de ménage, etc.)
    /// </summary>
    public class Employee : BaseEntity
    {
        /// <summary>
        /// Residence ID (Foreign Key)
        /// </summary>
        public Guid ResidenceId { get; set; }

        /// <summary>
        /// Employee first name
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Employee last name
        /// </summary>
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Employee position/role (gardien, femme de ménage, etc.)
        /// </summary>
        public string Position { get; set; } = string.Empty;

        /// <summary>
        /// Employee email
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Employee phone number
        /// </summary>
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Hire date
        /// </summary>
        public DateTime HireDate { get; set; }

        /// <summary>
        /// Employment end date (nullable if still employed)
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Employee status (Active, Inactive, On Leave, etc.)
        /// </summary>
        public EmployeeStatus Status { get; set; } = EmployeeStatus.Active;

        /// <summary>
        /// Additional notes about the employee
        /// </summary>
        public string? Notes { get; set; }

        // Navigation properties
        /// <summary>
        /// Residence the employee works for
        /// </summary>
        public Residence? Residence { get; set; }

        /// <summary>
        /// Salary history for this employee
        /// </summary>
        public ICollection<EmployeeSalary> Salaries { get; set; } = new List<EmployeeSalary>();

        /// <summary>
        /// Get full name
        /// </summary>
        public string GetFullName() => $"{FirstName} {LastName}".Trim();
    }

    /// <summary>
    /// Employee status enumeration
    /// </summary>
    public enum EmployeeStatus
    {
        /// <summary>
        /// Currently employed
        /// </summary>
        Active = 0,

        /// <summary>
        /// On temporary leave
        /// </summary>
        OnLeave = 1,

        /// <summary>
        /// Suspended
        /// </summary>
        Suspended = 2,

        /// <summary>
        /// No longer employed
        /// </summary>
        Inactive = 3
    }
}
