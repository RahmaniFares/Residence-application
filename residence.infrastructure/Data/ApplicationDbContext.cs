using Microsoft.EntityFrameworkCore;
using residence.domain.Common;
using residence.domain.Entities;
using residence.infrastructure.Configurations;

namespace residence.infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    #region DbSets

    // Core
    public DbSet<Residence> Residences { get; set; }
    public DbSet<ResidenceSettings> ResidenceSettings { get; set; }
    public DbSet<User> Users { get; set; }

    // Property Management
    public DbSet<House> Houses { get; set; }
    public DbSet<Resident> Residents { get; set; }

    // Payments
    public DbSet<Payment> Payments { get; set; }

    // Expenses
    public DbSet<Expense> Expenses { get; set; }
    public DbSet<ExpenseImage> ExpenseImages { get; set; }

    // Incidents
    public DbSet<Incident> Incidents { get; set; }
    public DbSet<IncidentComment> IncidentComments { get; set; }

    // Posts & Community
    public DbSet<Post> Posts { get; set; }
    public DbSet<PostLike> PostLikes { get; set; }
    public DbSet<PostComment> PostComments { get; set; }

    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply all configurations
        modelBuilder.ApplyConfiguration(new ResidenceConfiguration());
        modelBuilder.ApplyConfiguration(new ResidenceSettingsConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new HouseConfiguration());
        modelBuilder.ApplyConfiguration(new ResidentConfiguration());
        modelBuilder.ApplyConfiguration(new PaymentConfiguration());
        modelBuilder.ApplyConfiguration(new ExpenseConfiguration());
        modelBuilder.ApplyConfiguration(new ExpenseImageConfiguration());
        modelBuilder.ApplyConfiguration(new IncidentConfiguration());
        modelBuilder.ApplyConfiguration(new IncidentCommentConfiguration());
        modelBuilder.ApplyConfiguration(new PostConfiguration());
        modelBuilder.ApplyConfiguration(new PostLikeConfiguration());
        modelBuilder.ApplyConfiguration(new PostCommentConfiguration());



        //// Global query filters for soft delete
        //foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        //{
        //    if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
        //    {
        //        var parameter = System.Linq.Expressions.Expression.Parameter(entityType.ClrType, "p");
        //        var property = System.Linq.Expressions.Expression.Property(parameter, nameof(BaseEntity.IsDeleted));
        //        var notDeleted = System.Linq.Expressions.Expression.Equal(property, System.Linq.Expressions.Expression.Constant(false));
        //        var lambda = System.Linq.Expressions.Expression.Lambda(notDeleted, parameter);

        //        modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
        //    }
        //}

        //// Multi-tenancy: Ensure ResidenceId isolation
        //foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        //{
        //    if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType) && entityType.ClrType != typeof(Residence))
        //    {
        //        var property = entityType.FindProperty(nameof(BaseEntity.ResidenceId));
        //        if (property != null)
        //        {
        //            // ResidenceId is required for all tenant-scoped entities
        //        }
        //    }
        //}
    }

    /// <summary>
    /// Override SaveChanges to automatically set audit fields
    /// </summary>
    public override int SaveChanges()
    {
        SetAuditFields();
        return base.SaveChanges();
    }

    /// <summary>
    /// Override SaveChangesAsync to automatically set audit fields
    /// </summary>
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        SetAuditFields();
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void SetAuditFields()
    {
        var entries = ChangeTracker.Entries<BaseEntity>();

        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    break;

                case EntityState.Modified:
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    break;
            }
        }
    }

    /// <summary>
    /// Seed database with initial data when creating
    /// </summary>

}


