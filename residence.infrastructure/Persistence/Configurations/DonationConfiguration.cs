using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using residence.domain.Entities;
using System;

namespace residence.infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Entity configuration for Donation
    /// </summary>
    public class DonationConfiguration : IEntityTypeConfiguration<Donation>
    {
        public void Configure(EntityTypeBuilder<Donation> builder)
        {
            // Configure table name and schema
            builder.ToTable("Donations", "dbo");

            // Configure primary key
            builder.HasKey(d => d.Id);

            // Configure properties
            builder.Property(d => d.Id)
                .HasColumnName("Id")
                .ValueGeneratedNever();

            builder.Property(d => d.HouseId)
                .HasColumnName("HouseId")
                .IsRequired(false);

            builder.Property(d => d.DonorId)
                .HasColumnName("DonorId")
                .IsRequired(false);

            builder.Property(d => d.Amount)
                .HasColumnName("Amount")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(d => d.DonationDate)
                .HasColumnName("DonationDate")
                .HasColumnType("datetime2")
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(d => d.Description)
                .HasColumnName("Description")
                .HasColumnType("nvarchar(max)")
                .IsRequired(false);

            builder.Property(d => d.CreatedAt)
                .HasColumnName("CreatedAt")
                .HasColumnType("datetime2")
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(d => d.UpdatedAt)
                .HasColumnName("UpdatedAt")
                .HasColumnType("datetime2")
                .IsRequired(false);

            // Configure relationships
            builder.HasOne(d => d.House)
                .WithMany(h => h.Donations)
                .HasForeignKey(d => d.HouseId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(d => d.Donor)
                .WithMany(r => r.DonationsAsContributor)
                .HasForeignKey(d => d.DonorId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            // Configure indexes
            builder.HasIndex(d => d.HouseId)
                .HasDatabaseName("IX_Donations_HouseId");

            builder.HasIndex(d => d.DonorId)
                .HasDatabaseName("IX_Donations_DonorId");

            builder.HasIndex(d => d.DonationDate)
                .HasDatabaseName("IX_Donations_DonationDate");

            builder.HasIndex(d => new { d.HouseId, d.DonationDate })
                .HasDatabaseName("IX_Donations_HouseId_DonationDate");

            // Add check constraint for Amount > 0
            builder.HasCheckConstraint("CK_Donations_Amount_Positive", "[Amount] > 0");

            // Configure timestamp behavior
            builder.Property(d => d.CreatedAt)
                .Metadata.SetAfterSaveBehavior(Microsoft.EntityFrameworkCore.Metadata.PropertySaveBehavior.Ignore);
        }
    }
}
