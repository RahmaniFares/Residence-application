using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using residence.domain.Entities;

namespace residence.infrastructure.Configurations;

/// <summary>
/// Entity configuration for UserHouse relationship
/// </summary>
public class UserHouseConfiguration : IEntityTypeConfiguration<UserHouse>
{
    public void Configure(EntityTypeBuilder<UserHouse> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.UserId)
            .IsRequired();

        builder.Property(e => e.HouseId)
            .IsRequired();

        builder.Property(e => e.AssignedDate)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(e => e.Notes)
            .HasMaxLength(500)
            .IsRequired(false);

        builder.Property(e => e.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(e => e.IsDeleted)
            .HasDefaultValue(false);

        // Relationships
        builder.HasOne(uh => uh.User)
            .WithMany(u => u.UserHouses)
            .HasForeignKey(uh => uh.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(uh => uh.House)
            .WithMany(h => h.UserHouses)
            .HasForeignKey(uh => uh.HouseId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes for performance
        builder.HasIndex(uh => uh.UserId)
            .HasDatabaseName("IX_UserHouse_UserId");

        builder.HasIndex(uh => uh.HouseId)
            .HasDatabaseName("IX_UserHouse_HouseId");

        builder.HasIndex(uh => new { uh.UserId, uh.HouseId })
            .HasDatabaseName("IX_UserHouse_UserId_HouseId")
            .IsUnique(false);
    }
}
