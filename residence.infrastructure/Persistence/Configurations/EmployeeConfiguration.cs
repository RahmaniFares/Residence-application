using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using residence.domain.Entities;

namespace residence.infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Entity configuration for Employee
    /// </summary>
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            // Configure table
            builder.ToTable("Employees", "dbo");

            // Configure primary key
            builder.HasKey(e => e.Id);

            // Configure properties
            builder.Property(e => e.Id)
                .HasColumnName("Id")
                .ValueGeneratedNever();

            builder.Property(e => e.ResidenceId)
                .HasColumnName("ResidenceId")
                .IsRequired();

            builder.Property(e => e.FirstName)
                .HasColumnName("FirstName")
                .HasColumnType("nvarchar(100)")
                .IsRequired();

            builder.Property(e => e.LastName)
                .HasColumnName("LastName")
                .HasColumnType("nvarchar(100)")
                .IsRequired();

            builder.Property(e => e.Position)
                .HasColumnName("Position")
                .HasColumnType("nvarchar(100)")
                .IsRequired();

            builder.Property(e => e.Email)
                .HasColumnName("Email")
                .HasColumnType("nvarchar(256)")
                .IsRequired(false);

            builder.Property(e => e.PhoneNumber)
                .HasColumnName("PhoneNumber")
                .HasColumnType("nvarchar(20)")
                .IsRequired(false);

            builder.Property(e => e.HireDate)
                .HasColumnName("HireDate")
                .HasColumnType("datetime2")
                .IsRequired();

            builder.Property(e => e.EndDate)
                .HasColumnName("EndDate")
                .HasColumnType("datetime2")
                .IsRequired(false);

            builder.Property(e => e.Status)
                .HasColumnName("Status")
                .HasColumnType("int")
                .IsRequired();

            builder.Property(e => e.Notes)
                .HasColumnName("Notes")
                .HasColumnType("nvarchar(max)")
                .IsRequired(false);

            builder.Property(e => e.CreatedAt)
                .HasColumnName("CreatedAt")
                .HasColumnType("datetime2")
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(e => e.UpdatedAt)
                .HasColumnName("UpdatedAt")
                .HasColumnType("datetime2")
                .IsRequired(false);

            // Configure relationships
            builder.HasOne(e => e.Residence)
                .WithMany(r => r.Employees)
                .HasForeignKey(e => e.ResidenceId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(e => e.Salaries)
                .WithOne(s => s.Employee)
                .HasForeignKey(s => s.EmployeeId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Configure indexes
            builder.HasIndex(e => e.ResidenceId)
                .HasDatabaseName("IX_Employees_ResidenceId");

            builder.HasIndex(e => new { e.ResidenceId, e.Status })
                .HasDatabaseName("IX_Employees_ResidenceId_Status");

            builder.HasIndex(e => new { e.ResidenceId, e.Position })
                .HasDatabaseName("IX_Employees_ResidenceId_Position");

            builder.HasIndex(e => e.Email)
                .HasDatabaseName("IX_Employees_Email");

            // Add check constraints
            builder.HasCheckConstraint("CK_Employees_HireDate", "[HireDate] <= GETUTCDATE()");
        }
    }
}
