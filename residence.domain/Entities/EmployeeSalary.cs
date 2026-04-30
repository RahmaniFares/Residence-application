using residence.domain.Common;
using System;

namespace residence.domain.Entities
{
    /// <summary>
    /// Employee salary history entity
    /// </summary>
    public class EmployeeSalary : BaseEntity
    {
        /// <summary>
        /// Employee ID (Foreign Key)
        /// </summary>
        public Guid EmployeeId { get; set; }

        /// <summary>
        /// Salary amount
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Effective date for this salary
        /// </summary>
        public DateTime EffectiveDate { get; set; }

        /// <summary>
        /// End date for this salary (nullable if still active)
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Is this the current salary
        /// </summary>
        public bool IsCurrent { get; set; } = false;

        /// <summary>
        /// Reason for salary change
        /// </summary>
        public string? Reason { get; set; }

        /// <summary>
        /// Additional notes
        /// </summary>
        public string? Notes { get; set; }

        // Navigation property
        /// <summary>
        /// Employee associated with this salary
        /// </summary>
        public Employee? Employee { get; set; }

        /// <summary>
        /// Get display text for period
        /// </summary>
        public string GetPeriodDisplay()
        {
            var startText = EffectiveDate.ToString("MMM dd, yyyy");
            if (EndDate.HasValue)
            {
                return $"{startText} - {EndDate.Value.ToString("MMM dd, yyyy")}";
            }
            return $"{startText} - Present";
        }
    }
}
