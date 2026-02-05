using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using residence.domain.Entities;

namespace residence.infrastructure.Configurations;

/// <summary>
/// Entity configuration for ExpenseImage
/// </summary>
public class ExpenseImageConfiguration : IEntityTypeConfiguration<ExpenseImage>
{
    public void Configure(EntityTypeBuilder<ExpenseImage> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.ImageUrl)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(e => e.ResidenceId)
            .IsRequired();

        builder.Property(e => e.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(e => e.IsDeleted)
            .HasDefaultValue(false);

        // Relationships
        builder.HasOne(ei => ei.Expense)
            .WithMany(e => e.Images)
            .HasForeignKey(ei => ei.ExpenseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.ToTable("ExpenseImages", "dbo");
    }
}
