using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using residence.domain.Entities;
using residence.domain.Enums;

namespace residence.infrastructure.Configurations;

/// <summary>
/// Entity configuration for Resident
/// </summary>
public class ResidentConfiguration : IEntityTypeConfiguration<Resident>
{
    public void Configure(EntityTypeBuilder<Resident> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.Email)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(e => e.PhoneNumber)
            .HasMaxLength(20);

        builder.Property(e => e.Address)
            .HasMaxLength(500);

        builder.Property(e => e.Status)
            .HasDefaultValue(ResidentStatus.Active); 

        builder.Property(e => e.MoveInDate)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(e => e.ResidenceId)
            .IsRequired();

        builder.Property(e => e.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(e => e.IsDeleted)
            .HasDefaultValue(false);

        // Relationships
        builder.HasOne(r => r.User)
            .WithOne(u => u.Resident)
            .HasForeignKey<Resident>(r => r.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.House)
            .WithMany(h => h.Residents)
            .HasForeignKey(r => r.HouseId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(r => r.Payments)
            .WithOne(p => p.Resident)
            .HasForeignKey(p => p.ResidentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(r => r.Incidents)
            .WithOne(i => i.Resident)
            .HasForeignKey(i => i.ResidentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(r => r.Posts)
            .WithOne(p => p.Author)
            .HasForeignKey(p => p.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.ToTable("Residents", "dbo");
    }
}
