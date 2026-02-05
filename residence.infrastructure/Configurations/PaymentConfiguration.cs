using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using residence.domain.Entities;
using residence.domain.Enums;

namespace residence.infrastructure.Configurations;

/// <summary>
/// Entity configuration for Payment
/// </summary>
public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Amount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(e => e.Method)
            .HasDefaultValue(PaymentMethod.Transfer); // PaymentMethod.Transfer

        builder.Property(e => e.Status)
            .HasDefaultValue(PaymentStatus.Pending); // PaymentStatus.Pending

        builder.Property(e => e.PeriodStart)
            .IsRequired();

        builder.Property(e => e.PeriodEnd)
            .IsRequired();

        builder.Property(e => e.Notes)
            .HasMaxLength(500);

        builder.Property(e => e.ResidenceId)
            .IsRequired();

        builder.Property(e => e.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(e => e.IsDeleted)
            .HasDefaultValue(false);

        // Relationships
        builder.HasOne(p => p.House)
            .WithMany(h => h.Payments)
            .HasForeignKey(p => p.HouseId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.Resident)
            .WithMany(r => r.Payments)
            .HasForeignKey(p => p.ResidentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.ToTable("Payments", "dbo");
    }
}
