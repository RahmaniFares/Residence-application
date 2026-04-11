namespace residence.application.DTOs
{
    /// <summary>
    /// DTO for Payment KPI and statistics
    /// </summary>
    public class PaymentKpiDto
    {
        /// <summary>
        /// Total amount of paid payments
        /// </summary>
        public decimal TotalPaidAmount { get; set; }

        /// <summary>
        /// Total number of paid payments
        /// </summary>
        public int TotalPaidCount { get; set; }

        /// <summary>
        /// Total amount of pending/unpaid payments from previous months until current
        /// </summary>
        public decimal TotalPendingAmount { get; set; }

        /// <summary>
        /// Total number of pending/unpaid payments
        /// </summary>
        public int TotalPendingCount { get; set; }

        /// <summary>
        /// Total amount of overdue payments (past the period end date)
        /// </summary>
        public decimal TotalOverdueAmount { get; set; }

        /// <summary>
        /// Total number of overdue payments
        /// </summary>
        public int TotalOverdueCount { get; set; }

        /// <summary>
        /// Payment collection rate (percentage)
        /// </summary>
        public decimal CollectionRate { get; set; }

        /// <summary>
        /// Total expected amount (paid + pending)
        /// </summary>
        public decimal TotalExpectedAmount { get; set; }

        /// <summary>
        /// Outstanding balance (pending + overdue)
        /// </summary>
        public decimal OutstandingBalance { get; set; }

        /// <summary>
        /// Average payment amount
        /// </summary>
        public decimal AveragePaymentAmount { get; set; }

        /// <summary>
        /// Period start date for the KPI calculation
        /// </summary>
        public DateTime? PeriodStartDate { get; set; }

        /// <summary>
        /// Period end date for the KPI calculation
        /// </summary>
        public DateTime? PeriodEndDate { get; set; }

        /// <summary>
        /// Residence ID
        /// </summary>
        public Guid ResidenceId { get; set; }

        /// <summary>
        /// Last updated timestamp
        /// </summary>
        public DateTime CalculatedAt { get; set; } = DateTime.UtcNow;
    }

    /// <summary>
    /// Monthly payment summary DTO
    /// </summary>
    public class MonthlyPaymentSummaryDto
    {
        /// <summary>
        /// Year of the summary
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// Month of the summary (1-12)
        /// </summary>
        public int Month { get; set; }

        /// <summary>
        /// Total amount expected for this month
        /// </summary>
        public decimal TotalExpected { get; set; }

        /// <summary>
        /// Total amount paid for this month
        /// </summary>
        public decimal TotalPaid { get; set; }

        /// <summary>
        /// Total amount pending for this month
        /// </summary>
        public decimal TotalPending { get; set; }

        /// <summary>
        /// Number of payments for this month
        /// </summary>
        public int TotalPayments { get; set; }

        /// <summary>
        /// Number of paid payments
        /// </summary>
        public int PaidCount { get; set; }

        /// <summary>
        /// Number of pending payments
        /// </summary>
        public int PendingCount { get; set; }

        /// <summary>
        /// Collection percentage for this month
        /// </summary>
        public decimal CollectionPercentage { get; set; }
    }

    /// <summary>
    /// Payment history trend DTO
    /// </summary>
    public class PaymentTrendDto
    {
        /// <summary>
        /// Date of the trend point
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Amount paid on this date
        /// </summary>
        public decimal AmountPaid { get; set; }

        /// <summary>
        /// Amount pending on this date
        /// </summary>
        public decimal AmountPending { get; set; }

        /// <summary>
        /// Cumulative paid amount up to this date
        /// </summary>
        public decimal CumulativePaid { get; set; }

        /// <summary>
        /// Collection rate on this date
        /// </summary>
        public decimal CollectionRate { get; set; }
    }
}
