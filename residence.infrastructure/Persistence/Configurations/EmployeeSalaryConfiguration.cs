using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using residence.domain.Entities;

namespace residence.infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Entity configuration for EmployeeSalary
    /// </summary>
    public class EmployeeSalaryConfiguration : IEntityTypeConfiguration<EmployeeSalary>
    {
        public void Configure(EntityTypeBuilder<EmployeeSalary> builder)
        {
            // Configure table
            builder.ToTable("EmployeeSalaries", "dbo");

            // Configure primary key
            builder.HasKey(es => es.Id);

            // Configure properties
            builder.Property(es => es.Id)
                .HasColumnName("Id")
                .ValueGeneratedNever();

            builder.Property(es => es.EmployeeId)
                .HasColumnName("EmployeeId")
                .IsRequired();

            builder.Property(es => es.Amount)
                .HasColumnName("Amount")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(es => es.EffectiveDate)
                .HasColumnName("EffectiveDate")
                .HasColumnType("datetime2")
                .IsRequired();

            builder.Property(es => es.EndDate)
                .HasColumnName("EndDate")
                .HasColumnType("datetime2")
                .IsRequired(false);

            builder.Property(es => es.IsCurrent)
                .HasColumnName("IsCurrent")
                .HasColumnType("bit")
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(es => es.Reason)
                .HasColumnName("Reason")
                .HasColumnType("nvarchar(255)")
                .IsRequired(false);

            builder.Property(es => es.Notes)
                .HasColumnName("Notes")
                .HasColumnType("nvarchar(max)")
                .IsRequired(false);

            builder.Property(es => es.CreatedAt)
                .HasColumnName("CreatedAt")
                .HasColumnType("datetime2")
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(es => es.UpdatedAt)
                .HasColumnName("UpdatedAt")
                .HasColumnType("datetime2")
                .IsRequired(false);

            // Configure relationships
            builder.HasOne(es => es.Employee)
                .WithMany(e => e.Salaries)
                .HasForeignKey(es => es.EmployeeId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Configure indexes
            builder.HasIndex(es => es.EmployeeId)
                .HasDatabaseName("IX_EmployeeSalaries_EmployeeId");

            builder.HasIndex(es => es.IsCurrent)
                .HasDatabaseName("IX_EmployeeSalaries_IsCurrent");

            builder.HasIndex(es => new { es.EmployeeId, es.IsCurrent })
                .HasDatabaseName("IX_EmployeeSalaries_EmployeeId_IsCurrent");

            builder.HasIndex(es => new { es.EmployeeId, es.EffectiveDate })
                .HasDatabaseName("IX_EmployeeSalaries_EmployeeId_EffectiveDate");

            builder.HasIndex(es => es.EffectiveDate)
                .HasDatabaseName("IX_EmployeeSalaries_EffectiveDate");

            // Add check constraints
            builder.HasCheckConstraint("CK_EmployeeSalaries_Amount_Positive", "[Amount] > 0");
            builder.HasCheckConstraint("CK_EmployeeSalaries_DateRange", "[EndDate] IS NULL OR [EndDate] >= [EffectiveDate]");
        }
    }
}
