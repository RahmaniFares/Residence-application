using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using residence.domain.Entities;

namespace residence.infrastructure.Configurations;

/// <summary>
/// Entity configuration for TarifHistory
/// </summary>
public class TarifHistoryConfiguration : IEntityTypeConfiguration<TarifHistory>
{
    public void Configure(EntityTypeBuilder<TarifHistory> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.PreviousAmount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(e => e.NewAmount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(e => e.PreviousDescription)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(e => e.NewDescription)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(e => e.EffectiveDate)
            .IsRequired();

        builder.Property(e => e.ChangedBy)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(e => e.ChangeReason)
            .HasMaxLength(1000)
            .IsRequired(false);

        builder.Property(e => e.ChangedAt)
            .HasDefaultValueSql("GETUTCDATE()")
            .IsRequired();

        builder.Property(e => e.TarifId)
            .IsRequired();

        builder.Property(e => e.ResidenceId)
            .IsRequired();

        builder.Property(e => e.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(e => e.IsDeleted)
            .HasDefaultValue(false);

        // Relationships
        builder.HasOne(h => h.Tarif)
            .WithMany(t => t.History)
            .HasForeignKey(h => h.TarifId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(h => h.Residence)
            .WithMany(r => r.TarifHistories)
            .HasForeignKey(h => h.ResidenceId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(h => new { h.ResidenceId, h.ChangedAt })
            .HasDatabaseName("IX_TarifHistory_ResidenceId_ChangedAt");

        builder.HasIndex(h => h.TarifId)
            .HasDatabaseName("IX_TarifHistory_TarifId");

        builder.HasIndex(h => h.ChangedAt)
            .HasDatabaseName("IX_TarifHistory_ChangedAt");
    }
}
