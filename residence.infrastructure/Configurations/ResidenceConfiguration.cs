using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using residence.domain.Entities;

namespace residence.infrastructure.Configurations;

/// <summary>
/// Entity configuration for Residence
/// </summary>
public class ResidenceConfiguration : IEntityTypeConfiguration<Residence>
{
    public void Configure(EntityTypeBuilder<Residence> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.Address)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(e => e.City)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.State)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.ZipCode)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(e => e.Description)
            .HasMaxLength(1000);

        builder.Property(e => e.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(e => e.IsDeleted)
            .HasDefaultValue(false);

        // Relationships
        builder.HasMany(r => r.Users)
            .WithOne()
            .HasForeignKey(u => u.ResidenceId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(r => r.Houses)
            .WithOne()
            .HasForeignKey(h => h.ResidenceId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(r => r.Residents)
            .WithOne()
            .HasForeignKey(r => r.ResidenceId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(r => r.Payments)
            .WithOne()
            .HasForeignKey(p => p.ResidenceId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(r => r.Expenses)
            .WithOne()
            .HasForeignKey(e => e.ResidenceId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(r => r.Incidents)
            .WithOne()
            .HasForeignKey(i => i.ResidenceId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(r => r.Posts)
            .WithOne()
            .HasForeignKey(p => p.ResidenceId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(r => r.Settings)
            .WithOne(s => s.Residence)
            .HasForeignKey<ResidenceSettings>(s => s.ResidenceId)
            .OnDelete(DeleteBehavior.NoAction);
        builder.HasData([
            new Residence
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Name = "Residence Mariem",
                Address = "123 Main St",
                City = "Anytown",
                State = "State",
                ZipCode = "12345",
                Description = "This is the default residence.",
                CreatedAt = new DateTime(2024, 1, 1),
                IsDeleted = false
            }
            ]
            );
        builder.ToTable("Residences", "dbo");
    }
}
