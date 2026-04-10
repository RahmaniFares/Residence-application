using System;

namespace residence.application.DTOs
{
    /// <summary>
    /// DTO for creating a tariff
    /// </summary>
    public class CreateTarifDto
    {
        /// <summary>
        /// Description of the tariff
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Amount per unit
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Currency code
        /// </summary>
        public string Currency { get; set; } = "USD";

        /// <summary>
        /// Effective date
        /// </summary>
        public DateTime EffectiveDate { get; set; }

        /// <summary>
        /// Additional notes
        /// </summary>
        public string? Notes { get; set; }
    }

    /// <summary>
    /// DTO for updating a tariff
    /// </summary>
    public class UpdateTarifDto
    {
        /// <summary>
        /// Description of the tariff
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Amount per unit
        /// </summary>
        public decimal? Amount { get; set; }

        /// <summary>
        /// Currency code
        /// </summary>
        public string? Currency { get; set; }

        /// <summary>
        /// Additional notes
        /// </summary>
        public string? Notes { get; set; }

        /// <summary>
        /// Reason for the change (for audit trail)
        /// </summary>
        public string? ChangeReason { get; set; }
    }

    /// <summary>
    /// DTO for tariff response
    /// </summary>
    public class TarifDto
    {
        /// <summary>
        /// Tariff ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Residence ID
        /// </summary>
        public Guid ResidenceId { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Amount
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Currency code
        /// </summary>
        public string Currency { get; set; } = "USD";

        /// <summary>
        /// Effective date
        /// </summary>
        public DateTime EffectiveDate { get; set; }

        /// <summary>
        /// End date
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Is active
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Additional notes
        /// </summary>
        public string? Notes { get; set; }

        /// <summary>
        /// Created at
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Updated at
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
    }

    /// <summary>
    /// DTO for tariff history
    /// </summary>
    public class TarifHistoryDto
    {
        /// <summary>
        /// History ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Tarif ID
        /// </summary>
        public Guid TarifId { get; set; }

        /// <summary>
        /// Residence ID
        /// </summary>
        public Guid ResidenceId { get; set; }

        /// <summary>
        /// Previous amount
        /// </summary>
        public decimal PreviousAmount { get; set; }

        /// <summary>
        /// New amount
        /// </summary>
        public decimal NewAmount { get; set; }

        /// <summary>
        /// Previous description
        /// </summary>
        public string PreviousDescription { get; set; } = string.Empty;

        /// <summary>
        /// New description
        /// </summary>
        public string NewDescription { get; set; } = string.Empty;

        /// <summary>
        /// Effective date
        /// </summary>
        public DateTime EffectiveDate { get; set; }

        /// <summary>
        /// Changed by
        /// </summary>
        public string ChangedBy { get; set; } = string.Empty;

        /// <summary>
        /// Change reason
        /// </summary>
        public string? ChangeReason { get; set; }

        /// <summary>
        /// Changed at
        /// </summary>
        public DateTime ChangedAt { get; set; }
    }
}
