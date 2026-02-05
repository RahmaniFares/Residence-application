namespace residence.domain.Common;

/// <summary>
/// Base entity for all persistent entities with multi-tenancy and audit support
/// </summary>
public abstract class BaseEntity
{
    /// <summary>
    /// Unique identifier (GUID)
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Tenant identifier - establishes multi-tenancy
    /// </summary>
    public Guid ResidenceId { get; set; }

    /// <summary>
    /// Creation timestamp (UTC)
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// User who created this entity
    /// </summary>
    public Guid? CreatedBy { get; set; }

    /// <summary>
    /// Last update timestamp (UTC)
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// User who last updated this entity
    /// </summary>
    public Guid? UpdatedBy { get; set; }

    /// <summary>
    /// Soft delete flag - marks as deleted without physical removal
    /// </summary>
    public bool IsDeleted { get; set; } = false;
}
