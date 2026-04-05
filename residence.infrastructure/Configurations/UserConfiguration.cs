using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using residence.domain.Entities;
using residence.domain.Enums;

namespace residence.infrastructure.Configurations;

/// <summary>
/// Entity configuration for User
/// Configures the one-to-one relationship between User and Resident
/// FK is on User side (User.ResidentId)
/// </summary>
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Email)
            .IsRequired()
            .HasMaxLength(255);

        builder.HasIndex(e => e.Email)
            .IsUnique();

        builder.Property(e => e.PasswordHash)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(e => e.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.PhoneNumber)
            .HasMaxLength(20);

        builder.Property(e => e.AvatarUrl)
            .HasMaxLength(500);

        builder.Property(e => e.Role)
            .HasDefaultValue(UserRole.Resident);


        builder.Property(e => e.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(e => e.IsDeleted)
            .HasDefaultValue(false);

        //// One-to-One Relationship Configuration
        //// One User can have one Resident, one Resident can have one User
        //// The foreign key is on the User side (ResidentId)
        builder.HasOne(h => h.Resident)
            .WithOne()
            .HasForeignKey<User>(h => h.ResidentId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.ToTable("Users", "dbo");
    }
}
