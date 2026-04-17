using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using residence.domain.Entities;

namespace residence.infrastructure.Configurations;

public class PaymentLineConfiguration : IEntityTypeConfiguration<PaymentLine>
{
    public void Configure(EntityTypeBuilder<PaymentLine> builder)
    {
        builder.HasKey(e => e.Id);

        builder.ToTable("PaymentLines", "dbo");

        builder.Property(e => e.Tarif)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.HasOne(pl => pl.Payment)
            .WithMany(p => p.Lines)
            .HasForeignKey(pl => pl.PaymentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(e => e.ResidenceId)
            .IsRequired();

        builder.Property(e => e.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(e => e.IsDeleted)
            .HasDefaultValue(false);
    }
}