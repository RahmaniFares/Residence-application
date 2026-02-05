using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using residence.domain.Entities;

namespace residence.infrastructure.Configurations;

/// <summary>
/// Entity configuration for ResidenceSettings
/// </summary>
public class ResidenceSettingsConfiguration : IEntityTypeConfiguration<ResidenceSettings>
{
    public void Configure(EntityTypeBuilder<ResidenceSettings> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.ResidenceName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.ResidencePlace)
            .IsRequired()
            .HasMaxLength(300);

        builder.Property(e => e.InitialBudget)
            .HasPrecision(18, 2)
            .HasDefaultValue(0m);

        builder.Property(e => e.ResidenceId)
            .IsRequired();

        builder.Property(e => e.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(e => e.IsDeleted)
            .HasDefaultValue(false);

        // Relationships
        builder.HasOne(s => s.Residence)
            .WithOne(r => r.Settings)
            .HasForeignKey<ResidenceSettings>(s => s.ResidenceId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasData(
            [
                new ResidenceSettings
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111112"),
                    ResidenceName = "Default Residence",
                    ResidencePlace = "Default Place",
                    InitialBudget = 1000m,
                    ResidenceId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    CreatedAt = new DateTime(2024, 1, 1),
                    IsDeleted = false
                }
            ]

            );
        builder.ToTable("ResidenceSettings", "dbo");
    }
}
