using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using residence.domain.Entities;
using residence.domain.Enums;

namespace residence.infrastructure.Configurations;

/// <summary>
/// Entity configuration for Incident
/// </summary>
public class IncidentConfiguration : IEntityTypeConfiguration<Incident>
{
    public void Configure(EntityTypeBuilder<Incident> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.Category)
            .IsRequired()
            .HasDefaultValue(IncidentCategory.Autre); // IncidentCategory.Autre

        builder.Property(e => e.Description)
            .HasMaxLength(1000);

        builder.Property(e => e.Status)
            .HasDefaultValue(IncidentStatus.Open); // IncidentStatus.Open

        builder.Property(e => e.Priority)
            .HasDefaultValue(IncidentPriority.Medium); // IncidentPriority.Medium

        builder.Property(e => e.ResidenceId)
            .IsRequired();

        builder.Property(e => e.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(e => e.IsDeleted)
            .HasDefaultValue(false);

        // Relationships
        builder.HasOne(i => i.Resident)
            .WithMany(r => r.Incidents)
            .HasForeignKey(i => i.ResidentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(i => i.House)
            .WithMany(h => h.Incidents)
            .HasForeignKey(i => i.HouseId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(i => i.Comments)
            .WithOne(ic => ic.Incident)
            .HasForeignKey(ic => ic.IncidentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.ToTable("Incidents", "dbo");
    }
}
