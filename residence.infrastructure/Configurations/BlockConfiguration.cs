using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using residence.domain.Entities;

namespace residence.infrastructure.Configurations;

/// <summary>
/// Entity configuration for Block
/// </summary>
public class BlockConfiguration : IEntityTypeConfiguration<Block>
{
    public void Configure(EntityTypeBuilder<Block> builder)
    {
        builder.HasKey(b => b.Id);

        builder.Property(b => b.Name)
            .IsRequired()
            .HasMaxLength(1);

        builder.Property(b => b.Coefficient)
            .HasPrecision(5, 4)
            .IsRequired();

        builder.Property(b => b.ResidenceId)
            .IsRequired();

        builder.Property(b => b.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(b => b.IsDeleted)
            .HasDefaultValue(false);

        // Relationships
        builder.HasMany(b => b.Expenses)
            .WithOne(e => e.Block)
            .HasForeignKey(e => e.BlockId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.ToTable("Blocks", "dbo");
    }
}
