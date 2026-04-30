using System;
using System.Collections.Generic;

namespace residence.application.DTOs
{
    /// <summary>
    /// Create employee DTO
    /// </summary>
    public class CreateEmployeeDto
    {
        /// <summary>
        /// Residence ID
        /// </summary>
        public Guid ResidenceId { get; set; }

        /// <summary>
        /// First name
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Last name
        /// </summary>
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Position/role (gardien, femme de ménage, etc.)
        /// </summary>
        public string Position { get; set; } = string.Empty;

        /// <summary>
        /// Email
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Phone number
        /// </summary>
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Hire date
        /// </summary>
        public DateTime HireDate { get; set; }

        /// <summary>
        /// Employment end date
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Notes
        /// </summary>
        public string? Notes { get; set; }
    }

    /// <summary>
    /// Update employee DTO
    /// </summary>
    public class UpdateEmployeeDto
    {
        /// <summary>
        /// First name
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Last name
        /// </summary>
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Position/role
        /// </summary>
        public string Position { get; set; } = string.Empty;

        /// <summary>
        /// Email
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Phone number
        /// </summary>
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Employment status
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Employment end date
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Notes
        /// </summary>
        public string? Notes { get; set; }
    }

    /// <summary>
    /// Employee response DTO
    /// </summary>
    public class EmployeeDto
    {
        /// <summary>
        /// Employee ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Residence ID
        /// </summary>
        public Guid ResidenceId { get; set; }

        /// <summary>
        /// First name
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Last name
        /// </summary>
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Full name
        /// </summary>
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// Position/role
        /// </summary>
        public string Position { get; set; } = string.Empty;

        /// <summary>
        /// Email
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Phone number
        /// </summary>
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Hire date
        /// </summary>
        public DateTime HireDate { get; set; }

        /// <summary>
        /// Employment end date
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Employment status
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Current salary amount
        /// </summary>
        public decimal? CurrentSalary { get; set; }

        /// <summary>
        /// Notes
        /// </summary>
        public string? Notes { get; set; }

        /// <summary>
        /// Creation date
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Last update date
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
    }

    /// <summary>
    /// Employee with salary history DTO
    /// </summary>
    public class EmployeeDetailDto
    {
        /// <summary>
        /// Employee ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Residence ID
        /// </summary>
        public Guid ResidenceId { get; set; }

        /// <summary>
        /// Full name
        /// </summary>
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// Position/role
        /// </summary>
        public string Position { get; set; } = string.Empty;

        /// <summary>
        /// Email
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Phone number
        /// </summary>
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Hire date
        /// </summary>
        public DateTime HireDate { get; set; }

        /// <summary>
        /// Current salary
        /// </summary>
        public CurrentEmployeeSalaryDto? CurrentSalary { get; set; }

        /// <summary>
        /// Salary history (recent first)
        /// </summary>
        public ICollection<EmployeeSalaryDto> SalaryHistory { get; set; } = new List<EmployeeSalaryDto>();

        /// <summary>
        /// Employee status
        /// </summary>
        public int Status { get; set; }
    }

    /// <summary>
    /// Employee salary history DTO
    /// </summary>
    public class EmployeeSalaryDto
    {
        /// <summary>
        /// Salary record ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Employee ID
        /// </summary>
        public Guid EmployeeId { get; set; }

        /// <summary>
        /// Salary amount
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Effective date
        /// </summary>
        public DateTime EffectiveDate { get; set; }

        /// <summary>
        /// End date
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Is current salary
        /// </summary>
        public bool IsCurrent { get; set; }

        /// <summary>
        /// Period display text (e.g., "Jan 15, 2024 - Present")
        /// </summary>
        public string PeriodDisplay { get; set; } = string.Empty;

        /// <summary>
        /// Reason for change
        /// </summary>
        public string? Reason { get; set; }

        /// <summary>
        /// Additional notes
        /// </summary>
        public string? Notes { get; set; }

        /// <summary>
        /// Created date
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }

    /// <summary>
    /// Current employee salary DTO (simplified)
    /// </summary>
    public class CurrentEmployeeSalaryDto
    {
        /// <summary>
        /// Salary record ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Salary amount
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Effective date
        /// </summary>
        public DateTime EffectiveDate { get; set; }

        /// <summary>
        /// Reason for current salary
        /// </summary>
        public string? Reason { get; set; }
    }

    /// <summary>
    /// Create/Update employee salary DTO
    /// </summary>
    public class CreateEmployeeSalaryDto
    {
        /// <summary>
        /// Employee ID
        /// </summary>
        public Guid EmployeeId { get; set; }

        /// <summary>
        /// Salary amount
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Effective date (when this salary takes effect)
        /// </summary>
        public DateTime EffectiveDate { get; set; }

        /// <summary>
        /// Reason for change
        /// </summary>
        public string? Reason { get; set; }

        /// <summary>
        /// Additional notes
        /// </summary>
        public string? Notes { get; set; }
    }

    /// <summary>
    /// Employee summary with salary DTO
    /// </summary>
    public class EmployeeSummaryDto
    {
        /// <summary>
        /// Employee ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Full name
        /// </summary>
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// Position
        /// </summary>
        public string Position { get; set; } = string.Empty;

        /// <summary>
        /// Current salary amount
        /// </summary>
        public decimal? CurrentSalary { get; set; }

        /// <summary>
        /// Salary effective date
        /// </summary>
        public DateTime? SalaryEffectiveDate { get; set; }

        /// <summary>
        /// Employment status
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// Total salary history count
        /// </summary>
        public int SalaryHistoryCount { get; set; }
    }
}
