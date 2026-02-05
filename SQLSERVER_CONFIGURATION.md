# ? SQL Server Configuration Complete

## ?? Successfully Reverted to SQL Server

Your Residence CRUD API has been **100% reconfigured** from PostgreSQL/Supabase back to **SQL Server LocalDB** with code-first migrations.

---

## ?? Configuration Summary

| Component | Before (PostgreSQL) | After (SQL Server) | Status |
|-----------|-------------------|-------------------|--------|
| Database | Supabase PostgreSQL | SQL Server LocalDB | ? Changed |
| Provider Package | Npgsql 8.0.0 | SqlServer 8.0.0 | ? Changed |
| Column Types | character varying | nvarchar | ? Changed |
| Timestamps | timestamp with timezone | datetime2 | ? Changed |
| Default Function | CURRENT_TIMESTAMP | GETUTCDATE() | ? Changed |
| Schema | public | dbo | ? Changed |
| Table Name | residences | Residences | ? Changed |
| Service Provider | UseNpgsql() | UseSqlServer() | ? Changed |
| Connection String | Supabase URL | LocalDB | ? Changed |
| Build Status | ? Success | ? Success | ? OK |

---

## ?? Files Modified

### 1. residence.infrastructure.csproj
```diff
- Microsoft.EntityFrameworkCore.SqlServer 8.0.0 (was there)
- Npgsql.EntityFrameworkCore.PostgreSQL 8.0.0
+ Microsoft.EntityFrameworkCore.SqlServer 8.0.0 (re-added)
```

### 2. ResidenceConfiguration.cs
```csharp
// String types
"character varying(200)" ? "nvarchar(200)"
"character varying(500)" ? "nvarchar(500)"
"character varying(100)" ? "nvarchar(100)"
"character varying(50)" ? "nvarchar(50)"
"character varying(20)" ? "nvarchar(20)"
"character varying(1000)" ? "nvarchar(1000)"

// Integer types
"integer" ? "int"

// Decimal types
"numeric(18,2)" ? "decimal(18,2)"

// DateTime types
"timestamp with time zone" ? "datetime2"

// Default functions
"CURRENT_TIMESTAMP" ? "GETUTCDATE()"

// Schema
"public" ? "dbo"

// Table name
"residences" ? "Residences"
```

### 3. ServiceCollectionExtensions.cs
```csharp
// Provider changed
UseNpgsql(connectionString) ? UseSqlServer(connectionString)
```

### 4. appsettings.json
```json
// Connection string
Before:
"Host=db.yuaioblifgkhdoxgzmtm.supabase.co;Database=postgres;Username=postgres;Password=Graviter123;SSL Mode=Require;Trust Server Certificate=true"

After:
"Server=(localdb)\\mssqllocaldb;Database=ResidenceDb;Trusted_Connection=true;"
```

---

## ? Verification

### Build Status
```bash
? Build successful - All projects compile without errors
```

### Configuration Files
```bash
? appsettings.json - SQL Server connection string configured
? ResidenceConfiguration.cs - SQL Server column types applied
? ServiceCollectionExtensions.cs - UseSqlServer() configured
? residence.infrastructure.csproj - SQL Server package installed
```

### Ready for Migrations
```bash
? Code-first ready - All entities defined
? Configurations ready - All mappings defined
? DbContext ready - ApplicationDbContext configured
? DI ready - Services registered
```

---

## ??? Project Structure

```
residence-app/
?
??? residence.api/
?   ??? appsettings.json ? SQL Server connection
?   ??? Program.cs ? Uses configuration
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
?   ??? Migrations/ (Empty - will be populated)
?   ??? residence.infrastructure.csproj ? SQL Server 8.0.0
?
??? residence.application/ (No changes)
??? residence.domain/ (No changes)
?
??? Documentation/
    ??? SQLSERVER_MIGRATION_GUIDE.md ? NEW
    ??? SQLSERVER_QUICK_START.md ? NEW
    ??? SQLSERVER_CONFIGURATION.md ? You are here
```

---

## ?? Connection Details

**Connection String in appsettings.json:**
```json
"Server=(localdb)\\mssqllocaldb;Database=ResidenceDb;Trusted_Connection=true;"
```

**Breakdown:**
- **Server:** `(localdb)\mssqllocaldb` - SQL Server LocalDB instance on your machine
- **Database:** `ResidenceDb` - Database will be created automatically
- **Authentication:** `Trusted_Connection=true` - Uses Windows authentication
- **Features:** Automatic creation, no external connection needed

---

## ?? Next Steps

### Step 1: Create Initial Migration
```bash
dotnet ef migrations add InitialCreate --project residence.infrastructure --startup-project residence.api
```

This generates migration files with SQL Server SQL.

### Step 2: Apply Migration to SQL Server
```bash
dotnet ef database update --project residence.infrastructure --startup-project residence.api
```

This creates the database and tables in LocalDB.

### Step 3: Run Application
```bash
dotnet run --project residence.api
```

Application runs on https://localhost:7000

### Step 4: Test in Swagger
```
https://localhost:7000/swagger
```

All endpoints ready to test.

---

## ?? Generated SQL Server Schema

Your migration will create this schema:

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

### Adding New Entities

**Example: Add "Owner" Entity**

1. Create entity in `residence.domain/Entities/Owner.cs`
2. Create config in `residence.infrastructure/Configurations/OwnerConfiguration.cs`
3. Add `public DbSet<Owner> Owners { get; set; }` to DbContext
4. Run: `dotnet ef migrations add AddOwnerEntity --project residence.infrastructure --startup-project residence.api`
5. Run: `dotnet ef database update --project residence.infrastructure --startup-project residence.api`

---

## ?? Useful Commands

### View All Migrations
```bash
dotnet ef migrations list --project residence.infrastructure --startup-project residence.api
```

### Generate SQL Script (without applying)
```bash
dotnet ef migrations script --project residence.infrastructure --startup-project residence.api -o migration.sql
```

### Check Database Info
```bash
dotnet ef database info --project residence.infrastructure --startup-project residence.api
```

### Rollback Last Migration (before applied to DB)
```bash
dotnet ef migrations remove --project residence.infrastructure --startup-project residence.api
```

---

## ? Key SQL Server Features Used

| Feature | Purpose | Example |
|---------|---------|---------|
| **IDENTITY** | Auto-incrementing ID | `[Id] int NOT NULL IDENTITY(1,1)` |
| **nvarchar** | Unicode string support | `[Name] nvarchar(200)` |
| **decimal** | Precise decimal numbers | `[Price] decimal(18,2)` |
| **datetime2** | High-precision timestamps | `[CreatedAt] datetime2` |
| **GETUTCDATE()** | UTC timestamp function | `DEFAULT (GETUTCDATE())` |
| **dbo schema** | Default database schema | `[dbo].[Residences]` |

---

## ?? What You Can Do Now

? **Immediate:**
- Create migrations for database schema
- Apply migrations to LocalDB
- Run the application
- Test API endpoints in Swagger

? **Short Term:**
- Add new entities following code-first pattern
- Create migrations for new features
- Test your models before applying

? **Long Term:**
- Develop your complete API
- Deploy to production SQL Server
- Maintain version control with migrations

---

## ?? Troubleshooting

### Build Issues
```bash
dotnet restore
dotnet clean
dotnet build
```

### Migration Issues
```bash
# Check if EF tool is installed
dotnet tool list -g | grep dotnet-ef

# If missing, install it
dotnet tool install --global dotnet-ef
```

### Database Issues
```bash
# Verify connection
dotnet ef database info --project residence.infrastructure --startup-project residence.api

# Restart LocalDB
# Open Services (Windows) and restart "SQL Server (LocalDB)"
```

---

## ?? Checklist: Before You Start

- ? SQL Server package installed (8.0.0)
- ? PostgreSQL package removed
- ? Entity configuration updated (SQL Server types)
- ? Service configuration updated (UseSqlServer)
- ? Connection string updated (LocalDB)
- ? Build successful
- ? All files reviewed
- ? Ready to create migration

---

## ?? You're All Set!

Everything is configured and ready for code-first migrations with SQL Server.

### Your First Command Should Be:
```bash
dotnet ef migrations add InitialCreate --project residence.infrastructure --startup-project residence.api
```

Then:
```bash
dotnet ef database update --project residence.infrastructure --startup-project residence.api
```

Then:
```bash
dotnet run --project residence.api
```

**Enjoy your SQL Server-powered API!** ??

---

## ?? Quick Reference

| Task | Command |
|------|---------|
| Build | `dotnet build` |
| Create Migration | `dotnet ef migrations add InitialCreate --project residence.infrastructure --startup-project residence.api` |
| Apply Migration | `dotnet ef database update --project residence.infrastructure --startup-project residence.api` |
| Run App | `dotnet run --project residence.api` |
| View Migrations | `dotnet ef migrations list --project residence.infrastructure --startup-project residence.api` |
| Generate SQL | `dotnet ef migrations script --project residence.infrastructure --startup-project residence.api -o migration.sql` |
| Check DB Info | `dotnet ef database info --project residence.infrastructure --startup-project residence.api` |

---

**Status:** ? SQL SERVER CONFIGURATION COMPLETE  
**Build:** ? SUCCESSFUL  
**Ready for:** ? CODE-FIRST MIGRATIONS  
**Next Step:** Create initial migration
