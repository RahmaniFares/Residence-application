using residence.domain.Common;

namespace residence.domain.Entities;

/// <summary>
/// Represents a specific period and tariff covered by a payment
/// </summary>
public class PaymentLine : BaseEntity
{
    public Guid PaymentId { get; set; }
    public int FromMonth { get; set; }
    public int FromYear { get; set; }
    public int ToMonth { get; set; }
    public int ToYear { get; set; }
    public decimal Tarif { get; set; }

    public Payment Payment { get; set; } = null!;
}
