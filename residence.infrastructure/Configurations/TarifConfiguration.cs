using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using residence.domain.Entities;

namespace residence.infrastructure.Configurations;

/// <summary>
/// Entity configuration for Tarif
/// </summary>
public class TarifConfiguration : IEntityTypeConfiguration<Tarif>
{
    public void Configure(EntityTypeBuilder<Tarif> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Description)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(e => e.Amount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(e => e.Currency)
            .HasMaxLength(3)
            .HasDefaultValue("USD");

        builder.Property(e => e.EffectiveDate)
            .IsRequired();

        builder.Property(e => e.EndDate)
            .IsRequired(false);

        builder.Property(e => e.IsActive)
            .HasDefaultValue(true);

        builder.Property(e => e.Notes)
            .HasMaxLength(1000)
            .IsRequired(false);

        builder.Property(e => e.ResidenceId)
            .IsRequired();

        builder.Property(e => e.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(e => e.IsDeleted)
            .HasDefaultValue(false);

        // Relationships
        builder.HasOne(t => t.Residence)
            .WithMany(r => r.Tarifs)
            .HasForeignKey(t => t.ResidenceId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(t => t.History)
            .WithOne(h => h.Tarif)
            .HasForeignKey(h => h.TarifId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(t => new { t.ResidenceId, t.IsActive })
            .HasDatabaseName("IX_Tarif_ResidenceId_IsActive");

        builder.HasIndex(t => t.EffectiveDate)
            .HasDatabaseName("IX_Tarif_EffectiveDate");
    }
}
