using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using residence.domain.Entities;

namespace residence.infrastructure.Configurations;

/// <summary>
/// Entity configuration for IncidentComment
/// </summary>
public class IncidentCommentConfiguration : IEntityTypeConfiguration<IncidentComment>
{
    public void Configure(EntityTypeBuilder<IncidentComment> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Text)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(e => e.ResidenceId)
            .IsRequired();

        builder.Property(e => e.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(e => e.IsDeleted)
            .HasDefaultValue(false);

        // Relationships
        builder.HasOne(ic => ic.Incident)
            .WithMany(i => i.Comments)
            .HasForeignKey(ic => ic.IncidentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.ToTable("IncidentComments", "dbo");
    }
}
