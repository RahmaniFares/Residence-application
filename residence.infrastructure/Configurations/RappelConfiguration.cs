using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using residence.domain.Entities;
using residence.domain.Enums;

namespace residence.infrastructure.Configurations;

public class RappelConfiguration : IEntityTypeConfiguration<Rappel>
{
    public void Configure(EntityTypeBuilder<Rappel> builder)
    {
        builder.HasKey(e => e.Id);

        builder.ToTable("Rappels", "dbo");

        builder.Property(e => e.Amount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(e => e.Status)
            .HasDefaultValue(RappelStatus.Unpaid);

        builder.Property(e => e.Notes)
            .HasMaxLength(500);

        builder.Property(e => e.ResidenceId)
            .IsRequired();

        builder.Property(e => e.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(e => e.IsDeleted)
            .HasDefaultValue(false);

        builder.HasOne(r => r.House)
            .WithMany(h => h.Rappels)
            .HasForeignKey(r => r.HouseId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
