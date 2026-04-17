using System;

namespace residence.domain.Entities
{
    /// <summary>
    /// Monthly payment summary
    /// View Model for displaying aggregated payment data for a specific month
    /// Useful for reports and monthly reconciliation
    /// </summary>
    public class PaymentMonthSummary
    {
        /// <summary>
        /// House ID
        /// </summary>
        public Guid HouseId { get; set; }

        /// <summary>
        /// Year (YYYY)
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// Month (1-12)
        /// </summary>
        public int Month { get; set; }

        /// <summary>
        /// Tariff amount applied
        /// </summary>
        public decimal TarifAmount { get; set; }

        /// <summary>
        /// Total amount paid
        /// </summary>
        public decimal AmountPaid { get; set; }

        /// <summary>
        /// Payment date (when actually paid)
        /// </summary>
        public DateTime? PaymentDate { get; set; }

        /// <summary>
        /// Payment status
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// Was tariff changed during this month?
        /// </summary>
        public bool TarifChanged { get; set; }

        /// <summary>
        /// Previous tariff if changed
        /// </summary>
        public decimal? PreviousTarif { get; set; }

        /// <summary>
        /// Month name (e.g., "January")
        /// </summary>
        public string MonthName => new DateTime(Year, Month, 1).ToString("MMMM");

        /// <summary>
        /// Full date representation (e.g., "January 2024")
        /// </summary>
        public string DateRepresentation => $"{MonthName} {Year}";

        /// <summary>
        /// Check if payment is overdue (pending for more than 15 days)
        /// </summary>
        public bool IsOverdue => 
            string.Equals(Status, "Pending", StringComparison.OrdinalIgnoreCase) && 
            DateTime.UtcNow.Day > 15;

        /// <summary>
        /// Check if still pending
        /// </summary>
        public bool IsPending => 
            string.Equals(Status, "Pending", StringComparison.OrdinalIgnoreCase);

        /// <summary>
        /// Check if paid
        /// </summary>
        public bool IsPaid => 
            string.Equals(Status, "Paid", StringComparison.OrdinalIgnoreCase);

        /// <summary>
        /// Get tariff change description
        /// </summary>
        public string GetTarifChangeDescription() => 
            TarifChanged && PreviousTarif.HasValue 
                ? $"Changed from {PreviousTarif:C} to {TarifAmount:C}" 
                : "No change";
    }
}
