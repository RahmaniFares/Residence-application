# SQL Server Code-First Migration Guide

## ? Configuration Complete: SQL Server Ready

Your application has been successfully reconfigured to use **SQL Server** with code-first migrations and Entity Framework Core.

---

## ?? What Changed

### 1. Database Provider
- **Was:** PostgreSQL (Supabase)
- **Now:** SQL Server (LocalDB)
- ? Switched successfully

### 2. NuGet Packages
- **Removed:** `Npgsql.EntityFrameworkCore.PostgreSQL`
- **Added:** `Microsoft.EntityFrameworkCore.SqlServer` (v8.0.0)
- ? Updated successfully

### 3. Entity Configuration
- **File:** `residence.infrastructure/Configurations/ResidenceConfiguration.cs`
- **Changes:**
  - Column types: `character varying` ? `nvarchar`
  - Timestamp: `timestamp with time zone` ? `datetime2`
  - Default function: `CURRENT_TIMESTAMP` ? `GETUTCDATE()`
  - Schema: `public` ? `dbo`
  - Table name: `residences` ? `Residences`
  - Integer type: `integer` ? `int`
  - Numeric type: `numeric(18,2)` ? `decimal(18,2)`
- ? Updated successfully

### 4. Service Configuration
- **File:** `residence.infrastructure/Extensions/ServiceCollectionExtensions.cs`
- **Change:** `.UseNpgsql()` ? `.UseSqlServer()`
- ? Updated successfully

### 5. Connection String
- **File:** `residence.api/appsettings.json`
- **Was:** Supabase PostgreSQL connection
- **Now:** SQL Server LocalDB connection
- ? Updated successfully

---

## ?? Current SQL Server Configuration

**Connection String:**
```
Server=(localdb)\\mssqllocaldb;Database=ResidenceDb;Trusted_Connection=true;
```

**Components:**
- **Server:** `(localdb)\mssqllocaldb` - Local SQL Server instance
- **Database:** `ResidenceDb` - Database name
- **Authentication:** `Trusted_Connection=true` - Windows Authentication
- **Default:** Automatically creates database if it doesn't exist

---

## ?? SQL Server Column Mapping

| Property | .NET Type | SQL Server Type | Configuration |
|----------|-----------|-----------------|---------------|
| Id | int | int | Primary Key, IDENTITY(1,1) |
| Name | string | nvarchar(200) | NOT NULL |
| Address | string | nvarchar(500) | NOT NULL |
| City | string | nvarchar(100) | NOT NULL |
| State | string | nvarchar(50) | NOT NULL |
| ZipCode | string | nvarchar(20) | NOT NULL |
| Description | string | nvarchar(1000) | NULL |
| NumberOfRooms | int | int | NOT NULL |
| Price | decimal | decimal(18,2) | Precision 18,2 |
| CreatedAt | DateTime | datetime2 | Default: GETUTCDATE() |
| UpdatedAt | DateTime | datetime2 | Default: GETUTCDATE() |

---

## ?? Getting Started: Step by Step

### Step 1: Verify Build
```bash
cd "C:\Users\RAHMANI Fares\source\repos\Residence-app"
dotnet build
```

**Expected Output:**
```
Build succeeded.
```

### Step 2: Create Initial Migration
```bash
dotnet ef migrations add InitialCreate --project residence.infrastructure --startup-project residence.api
```

**What happens:**
- EF Core scans your entities
- Generates migration files for SQL Server
- Creates migration directory structure
- Generates migration code with SQL Server SQL

**Files created:**
```
residence.infrastructure/Migrations/
??? 20240115103000_InitialCreate.cs
??? 20240115103000_InitialCreate.Designer.cs
??? ApplicationDbContextModelSnapshot.cs
```

### Step 3: Apply Migration to SQL Server
```bash
dotnet ef database update --project residence.infrastructure --startup-project residence.api
```

**What happens:**
- Connects to LocalDB SQL Server
- Creates `ResidenceDb` database (if doesn't exist)
- Creates `dbo.Residences` table
- Applies all migrations
- Logs migration history

### Step 4: Run Application
```bash
dotnet run --project residence.api
```

**Expected Output:**
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:7000
      Now listening on: http://localhost:5000
```

### Step 5: Test API
Navigate to: `https://localhost:7000/swagger`

---

## ?? Migration Generated SQL

Your migration will create this SQL Server schema:

```sql
CREATE TABLE [dbo].[Residences] (
    [Id] int NOT NULL IDENTITY(1,1),
    [Name] nvarchar(200) NOT NULL,
    [Address] nvarchar(500) NOT NULL,
    [City] nvarchar(100) NOT NULL,
    [State] nvarchar(50) NOT NULL,
    [ZipCode] nvarchar(20) NOT NULL,
    [Description] nvarchar(1000),
    [NumberOfRooms] int NOT NULL,
    [Price] decimal(18,2) NOT NULL,
    [CreatedAt] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
    [UpdatedAt] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
    CONSTRAINT [PK_Residences] PRIMARY KEY CLUSTERED ([Id])
);
```

---

## ?? Code-First Workflow

### When You Add a New Entity

1. **Create Entity** (Domain layer)
   ```csharp
   public class User
   {
       public int Id { get; set; }
       public string Email { get; set; } = string.Empty;
   }
   ```

2. **Create Configuration** (Infrastructure layer)
   ```csharp
   public class UserConfiguration : IEntityTypeConfiguration<User>
   {
       public void Configure(EntityTypeBuilder<User> builder)
       {
           builder.HasKey(e => e.Id);
           builder.Property(e => e.Email).IsRequired().HasMaxLength(255).HasColumnType("nvarchar(255)");
           builder.ToTable("Users", "dbo");
       }
   }
   ```

3. **Add to DbContext**
   ```csharp
   public DbSet<User> Users { get; set; }

   protected override void OnModelCreating(ModelBuilder modelBuilder)
   {
       base.OnModelCreating(modelBuilder);
       modelBuilder.ApplyConfiguration(new ResidenceConfiguration());
       modelBuilder.ApplyConfiguration(new UserConfiguration());  // NEW
   }
   ```

4. **Create Migration**
   ```bash
   dotnet ef migrations add AddUserEntity --project residence.infrastructure --startup-project residence.api
   ```

5. **Apply Migration**
   ```bash
   dotnet ef database update --project residence.infrastructure --startup-project residence.api
   ```

---

## ?? Testing Connection

### Test 1: Verify Connection String
```bash
cat residence.api/appsettings.json
```

Should show SQL Server LocalDB connection.

### Test 2: Check Database Info
```bash
dotnet ef database info --project residence.infrastructure --startup-project residence.api
```

Should show SQL Server provider and database information.

### Test 3: View in SQL Server Management Studio

1. Open SQL Server Management Studio
2. Connect to: `(localdb)\mssqllocaldb`
3. Navigate to Databases ? ResidenceDb ? Tables
4. You should see `dbo.Residences` table

### Test 4: Query Table
```sql
SELECT * FROM [ResidenceDb].[dbo].[Residences];
```

Should return empty result (no data yet).

---

## ??? Useful EF Core Commands

### View Applied Migrations
```bash
dotnet ef migrations list --project residence.infrastructure --startup-project residence.api
```

### Remove Last Migration (if not applied to DB)
```bash
dotnet ef migrations remove --project residence.infrastructure --startup-project residence.api
```

### Generate SQL Script Without Applying
```bash
dotnet ef migrations script --project residence.infrastructure --startup-project residence.api -o migration.sql
```

### Rollback to Previous Migration
```bash
dotnet ef database update PreviousMigrationName --project residence.infrastructure --startup-project residence.api
```

### Check Database State
```bash
dotnet ef database info --project residence.infrastructure --startup-project residence.api
```

---

## ?? SQL Server Specific Features Used

### IDENTITY (Auto-increment)
```sql
[Id] int NOT NULL IDENTITY(1,1)
```

### GETUTCDATE() Function
```sql
DEFAULT (GETUTCDATE())
```

### CLUSTERED Index
```sql
CONSTRAINT [PK_Residences] PRIMARY KEY CLUSTERED ([Id])
```

### nvarchar for Unicode Support
```sql
[Name] nvarchar(200) NOT NULL
```

### decimal with Precision
```sql
[Price] decimal(18,2) NOT NULL
```

### datetime2 for Accurate Timestamps
```sql
[CreatedAt] datetime2 NOT NULL DEFAULT (GETUTCDATE())
```

---

## ?? Project Structure

```
residence-app/
??? residence.api/
?   ??? appsettings.json ? SQL Server connection
?   ??? Program.cs
?   ??? residence.api.csproj
?
??? residence.infrastructure/
?   ??? Configurations/
?   ?   ??? ResidenceConfiguration.cs ? SQL Server types
?   ??? Data/
?   ?   ??? ApplicationDbContext.cs ? Ready for SQL Server
?   ??? Repositories/
?   ?   ??? ResidenceRepository.cs
?   ??? Extensions/
?   ?   ??? ServiceCollectionExtensions.cs ? UseSqlServer()
?   ??? Migrations/ ? Will be created
?   ??? residence.infrastructure.csproj ? SQL Server package
?
??? residence.application/
??? residence.domain/
```

---

## ? Quick Commands Reference

```bash
# Build
dotnet build

# Create migration
dotnet ef migrations add InitialCreate --project residence.infrastructure --startup-project residence.api

# Apply migration
dotnet ef database update --project residence.infrastructure --startup-project residence.api

# Run app
dotnet run --project residence.api

# View migrations
dotnet ef migrations list --project residence.infrastructure --startup-project residence.api

# Generate SQL script
dotnet ef migrations script --project residence.infrastructure --startup-project residence.api -o migration.sql
```

---

## ?? Troubleshooting

### Issue: LocalDB not found
**Solution:** Install SQL Server LocalDB or use different connection string

### Issue: Database already exists
**Solution:** Use existing database or delete and recreate

### Issue: "Cannot find schema dbo"
**Solution:** Migration will create it automatically on first run

### Issue: EF Core tool not found
**Solution:**
```bash
dotnet tool install --global dotnet-ef
```

### Issue: Build fails
**Solution:**
```bash
dotnet restore
dotnet clean
dotnet build
```

---

## ? Verification Checklist

- ? SQL Server package installed (v8.0.0)
- ? PostgreSQL package removed
- ? Entity configuration uses SQL Server types
- ? ServiceCollectionExtensions uses `.UseSqlServer()`
- ? Connection string set to LocalDB
- ? Build successful
- ? Ready for migrations

---

## ?? Next Steps

1. **Create Initial Migration:**
   ```bash
   dotnet ef migrations add InitialCreate --project residence.infrastructure --startup-project residence.api
   ```

2. **Apply to SQL Server:**
   ```bash
   dotnet ef database update --project residence.infrastructure --startup-project residence.api
   ```

3. **Run Application:**
   ```bash
   dotnet run --project residence.api
   ```

4. **Test in Swagger:**
   ```
   https://localhost:7000/swagger
   ```

---

## ?? SQL Server Resources

- **SQL Server Express:** https://www.microsoft.com/en-us/sql-server/sql-server-downloads
- **LocalDB:** https://learn.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb
- **SQL Server Management Studio:** https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms
- **EF Core SQL Server:** https://learn.microsoft.com/en-us/ef/core/providers/sql-server/

---

**Status:** ? Ready for SQL Server Migrations  
**Next Command:** `dotnet ef migrations add InitialCreate --project residence.infrastructure --startup-project residence.api`

Build: ? SUCCESS  
Configuration: ? COMPLETE  
Ready: ? YES
