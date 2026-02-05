# Supabase PostgreSQL Migration Guide - Code First

## Overview

This guide will help you set up Entity Framework Core migrations with your Supabase PostgreSQL database using the code-first approach.

---

## ? Prerequisites Completed

Your application is now configured with:
- ? Npgsql.EntityFrameworkCore.PostgreSQL package
- ? Supabase connection string configured in `appsettings.json`
- ? PostgreSQL column types in ResidenceConfiguration
- ? SQL Server packages removed

---

## ?? Current Supabase Configuration

**Connection String (from appsettings.json):**
```
Host=db.yuaioblifgkhdoxgzmtm.supabase.co;
Database=postgres;
Username=postgres;
Password=Graviter123;
SSL Mode=Require;
Trust Server Certificate=true
```

**Database Type:** PostgreSQL  
**Provider:** Supabase  
**Network Access:** ? Configured with SSL

---

## ?? Code-First Migration Steps

### Step 1: Add Initial Migration

Run this command from the project root:

```bash
dotnet ef migrations add InitialCreate --project residence.infrastructure --startup-project residence.api
```

**What this does:**
- Analyzes your DbContext and entity configurations
- Creates a migration file in `residence.infrastructure/Migrations/`
- Generates SQL script for PostgreSQL
- Generates rollback code

**Output:**
```
Build started...
Build succeeded.
Done. To undo this action, use 'ef migrations remove'
```

---

### Step 2: Update Database

Apply the migration to your Supabase database:

```bash
dotnet ef database update --project residence.infrastructure --startup-project residence.api
```

**What this does:**
- Executes the migration on your Supabase database
- Creates the `residences` table
- Creates all indexes and constraints
- Logs the migration history

**Output:**
```
Build started...
Build succeeded.
Applying migration '20240115103000_InitialCreate'.
Done.
```

---

## ?? Migration Files Structure

After running migrations, you'll have:

```
residence.infrastructure/
??? Migrations/
    ??? 20240115103000_InitialCreate.cs          (Migration logic)
    ??? 20240115103000_InitialCreate.Designer.cs (Metadata)
    ??? ApplicationDbContextModelSnapshot.cs     (Current model)
```

### Migration File Example

The migration creates PostgreSQL-specific SQL:

```csharp
public partial class InitialCreate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.EnsureSchema(name: "public");

        migrationBuilder.CreateTable(
            name: "residences",
            schema: "public",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", 
                        NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Name = table.Column<string>(type: "character varying(200)", 
                    maxLength: 200, nullable: false),
                Address = table.Column<string>(type: "character varying(500)", 
                    maxLength: 500, nullable: false),
                // ... more columns
                CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", 
                    nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", 
                    nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_residences_id", x => x.Id);
            });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "residences",
            schema: "public");
    }
}
```

---

## ?? Workflow for Adding New Features

### When You Want to Add a New Entity

1. **Create the Entity** in `residence.domain/Entities/`
2. **Create Configuration** in `residence.infrastructure/Configurations/`
3. **Add DbSet** to `ApplicationDbContext.cs`
4. **Create Migration:**
   ```bash
   dotnet ef migrations add AddNewEntity --project residence.infrastructure --startup-project residence.api
   ```
5. **Update Database:**
   ```bash
   dotnet ef database update --project residence.infrastructure --startup-project residence.api
   ```

### Example: Adding a New Entity (Owner)

**1. Create Entity:**
```csharp
// residence.domain/Entities/Owner.cs
public class Owner
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
```

**2. Create Configuration:**
```csharp
// residence.infrastructure/Configurations/OwnerConfiguration.cs
public class OwnerConfiguration : IEntityTypeConfiguration<Owner>
{
    public void Configure(EntityTypeBuilder<Owner> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Name).IsRequired().HasMaxLength(200);
        builder.Property(e => e.Email).IsRequired().HasMaxLength(255);
        builder.Property(e => e.PhoneNumber).HasMaxLength(20);
        builder.ToTable("owners", "public");
    }
}
```

**3. Add to DbContext:**
```csharp
public DbSet<Owner> Owners { get; set; }

protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);
    modelBuilder.ApplyConfiguration(new ResidenceConfiguration());
    modelBuilder.ApplyConfiguration(new OwnerConfiguration());  // Add this
}
```

**4. Create Migration:**
```bash
dotnet ef migrations add AddOwnerEntity --project residence.infrastructure --startup-project residence.api
```

**5. Update Database:**
```bash
dotnet ef database update --project residence.infrastructure --startup-project residence.api
```

---

## ?? Useful EF Core Commands

### View Applied Migrations

```bash
dotnet ef migrations list --project residence.infrastructure --startup-project residence.api
```

### Rollback Last Migration

```bash
dotnet ef migrations remove --project residence.infrastructure --startup-project residence.api
```

**Note:** Only works if the migration hasn't been applied to the database.

### Rollback Database to Previous Migration

```bash
dotnet ef database update MigrationName --project residence.infrastructure --startup-project residence.api
```

### Generate SQL Script

```bash
dotnet ef migrations script --project residence.infrastructure --startup-project residence.api -o migration.sql
```

### See Generated SQL Before Applying

```bash
dotnet ef migrations script --project residence.infrastructure --startup-project residence.api --from 0 --to InitialCreate
```

---

## ??? PostgreSQL Specific Features

### Column Types Used

| .NET Type | PostgreSQL Type | EF Configuration |
|-----------|-----------------|------------------|
| string | character varying | HasColumnType("character varying(n)") |
| int | integer | Default |
| decimal | numeric(18,2) | HasColumnType("numeric(18,2)") |
| DateTime | timestamp with time zone | HasColumnType("timestamp with time zone") |
| bool | boolean | Default |
| long | bigint | Default |
| double | double precision | Default |

### Auto-Increment Columns

PostgreSQL uses SERIAL or IDENTITY for auto-increment:

```csharp
builder.Property(e => e.Id)
    .ValueGeneratedOnAdd();  // Auto-increment
```

EF Core automatically configures this for int primary keys.

### Default Values

PostgreSQL functions for defaults:

```csharp
// Current timestamp
.HasDefaultValueSql("CURRENT_TIMESTAMP")

// UUIDv4 (if needed)
.HasDefaultValueSql("gen_random_uuid()")
```

---

## ?? Running Your Application

### 1. Build the Solution

```bash
dotnet build
```

### 2. Ensure Migrations are Applied

```bash
dotnet ef database update --project residence.infrastructure --startup-project residence.api
```

### 3. Run the Application

```bash
dotnet run --project residence.api
```

### 4. Test the API

Navigate to: `https://localhost:7000/swagger`

---

## ?? Database Connection Verification

### Check Connection String

View in `residence.api/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=db.yuaioblifgkhdoxgzmtm.supabase.co;Database=postgres;Username=postgres;Password=Graviter123;SSL Mode=Require;Trust Server Certificate=true"
  }
}
```

### Verify Connection in Supabase Console

1. Go to https://app.supabase.com
2. Select your project
3. Go to "SQL Editor"
4. Run: `SELECT * FROM residences;`
5. You should see the table created

### Verify via EF Core

Add this to test connection:

```csharp
using (var context = new ApplicationDbContext(options))
{
    bool canConnect = await context.Database.CanConnectAsync();
    if (canConnect)
        Console.WriteLine("? Connected to Supabase!");
    else
        Console.WriteLine("? Failed to connect");
}
```

---

## ?? Viewing Your Database

### Using Supabase Console

1. Go to https://app.supabase.com/projects
2. Select your project (Residence-app)
3. Click "SQL Editor" or "Table Editor"
4. View your tables and data

### Using pgAdmin

1. Use connection details:
   - Host: db.yuaioblifgkhdoxgzmtm.supabase.co
   - Database: postgres
   - User: postgres
   - Password: Graviter123
   - SSL: Required

### Using DBeaver

1. Create new PostgreSQL connection
2. Enter same credentials as above
3. Connect and browse tables

---

## ?? Troubleshooting

### Issue: "No connection string found"

**Solution:**
```bash
# Verify appsettings.json has DefaultConnection
cat residence.api/appsettings.json
```

### Issue: "Authentication failed for user 'postgres'"

**Solution:**
- Check password in connection string (currently: `Graviter123`)
- Reset password in Supabase dashboard
- Update appsettings.json

### Issue: "SSL connection error"

**Solution:**
```
Ensure connection string has: SSL Mode=Require;Trust Server Certificate=true
```

### Issue: "Migration not applied"

**Solution:**
```bash
# Check migration status
dotnet ef migrations list --project residence.infrastructure --startup-project residence.api

# Check database status
dotnet ef database info --project residence.infrastructure --startup-project residence.api

# Force update
dotnet ef database update --project residence.infrastructure --startup-project residence.api --force
```

### Issue: "Table already exists"

**Solution:**
```bash
# Check if migration was already applied
dotnet ef migrations list --project residence.infrastructure

# The first migration should show Applied status
```

---

## ? Verification Checklist

- ? Npgsql package installed
- ? PostgreSQL column types configured
- ? Supabase connection string set
- ? Initial migration created
- ? Migration applied to database
- ? Table visible in Supabase
- ? Connection verified
- ? Application running
- ? API endpoints working

---

## ?? Next Steps

1. **Apply the initial migration:**
   ```bash
   dotnet ef database update --project residence.infrastructure --startup-project residence.api
   ```

2. **Test the API:**
   - Run `dotnet run --project residence.api`
   - Navigate to https://localhost:7000/swagger
   - Test endpoints

3. **Add new entities** following the pattern in this guide

4. **Monitor database** in Supabase console

---

## ?? Useful Resources

- **Supabase Docs:** https://supabase.com/docs
- **EF Core Migrations:** https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/
- **Npgsql EF Core:** https://www.npgsql.org/efcore/
- **PostgreSQL Types:** https://www.postgresql.org/docs/current/datatype.html

---

**Status:** ? Ready for Migrations  
**Next Command:** `dotnet ef migrations add InitialCreate --project residence.infrastructure --startup-project residence.api`
