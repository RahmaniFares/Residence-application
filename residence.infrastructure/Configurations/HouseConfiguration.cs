using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using residence.domain.Entities;
using residence.domain.Enums;

namespace residence.infrastructure.Configurations;

/// <summary>
/// Entity configuration for House
/// </summary>
public class HouseConfiguration : IEntityTypeConfiguration<House>
{
    public void Configure(EntityTypeBuilder<House> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Block)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.Unit)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.Floor)
            .HasMaxLength(50);

        builder.Property(e => e.Status)
            .HasDefaultValue(HouseStatus.Vacant); // HouseStatus.Vacant

        builder.Property(e => e.ResidenceId)
            .IsRequired();

        builder.Property(e => e.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(e => e.IsDeleted)
            .HasDefaultValue(false);

        // Relationships
        builder.HasOne(h => h.CurrentResident)
            .WithOne()
            .HasForeignKey<House>(h => h.CurrentResidentId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(h => h.Residents)
            .WithOne(r => r.House)
            .HasForeignKey(r => r.HouseId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(h => h.Payments)
            .WithOne(p => p.House)
            .HasForeignKey(p => p.HouseId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(h => h.Incidents)
            .WithOne(i => i.House)
            .HasForeignKey(i => i.HouseId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.ToTable("Houses", "dbo");
    }
}
