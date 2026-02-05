# SQL Server Quick Setup - 3 Simple Steps

## ? Everything is Configured for SQL Server

Your application is now **100% configured** for SQL Server LocalDB with code-first migrations.

---

## ?? Three Commands to Get Started

### Command 1: Create Migration
```bash
dotnet ef migrations add InitialCreate --project residence.infrastructure --startup-project residence.api
```

**What it does:**
- Analyzes your entities and configurations
- Generates SQL Server migration code
- Creates migration files
- No changes to database yet

### Command 2: Apply Migration
```bash
dotnet ef database update --project residence.infrastructure --startup-project residence.api
```

**What it does:**
- Creates `ResidenceDb` database
- Creates `dbo.Residences` table
- Applies all migrations
- Database is now ready

### Command 3: Run Application
```bash
dotnet run --project residence.api
```

**What it does:**
- Starts your API
- Listens on https://localhost:7000
- Ready to test

---

## ?? What Was Configured

| Item | Status |
|------|--------|
| SQL Server Provider | ? Installed |
| PostgreSQL Provider | ? Removed |
| Entity Configuration | ? SQL Server types |
| Service Configuration | ? UseSqlServer() |
| Connection String | ? LocalDB configured |
| Database | ? Ready to create |
| Build | ? Successful |

---

## ?? Connection Details

```
Server: (localdb)\mssqllocaldb
Database: ResidenceDb
Authentication: Windows (Trusted_Connection)
```

**In appsettings.json:**
```json
"DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ResidenceDb;Trusted_Connection=true;"
```

---

## ?? One-Minute Walkthrough

### 1. Open Terminal
```bash
cd "C:\Users\RAHMANI Fares\source\repos\Residence-app"
```

### 2. Build (Verify everything compiles)
```bash
dotnet build
```

Output: `Build succeeded.`

### 3. Create Migration (Generate migration code)
```bash
dotnet ef migrations add InitialCreate --project residence.infrastructure --startup-project residence.api
```

Output:
```
Build succeeded.
Done. To undo this action, use 'ef migrations remove'
```

### 4. Apply Migration (Create database and tables)
```bash
dotnet ef database update --project residence.infrastructure --startup-project residence.api
```

Output:
```
Build succeeded.
Applying migration '20240115103000_InitialCreate'.
Done.
```

### 5. Run Application (Start the API)
```bash
dotnet run --project residence.api
```

Output:
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:7000
```

### 6. Test (Open Swagger UI)
```
https://localhost:7000/swagger
```

---

## ? What Happens Behind the Scenes

### Migration Creation
```
Your C# Entities
        ?
ResidenceConfiguration (SQL Server types)
        ?
EF Core (Analyzes)
        ?
Migration Generated (SQL Server SQL)
        ?
20240115103000_InitialCreate.cs created
```

### Database Update
```
Migration File
        ?
SQL Server Provider
        ?
LocalDB Connection
        ?
Database Created (ResidenceDb)
        ?
Table Created (dbo.Residences)
        ?
Migration Logged (__EFMigrationsHistory)
```

### Runtime
```
HTTP Request
        ?
API Endpoint
        ?
Service
        ?
Repository
        ?
EF Core (DbSet queries)
        ?
SQL Server Provider (T-SQL)
        ?
LocalDB Database
        ?
Response
```

---

## ?? Generated SQL Server Schema

After running migrations, you'll have:

```sql
CREATE TABLE [dbo].[Residences] (
    [Id] int NOT NULL IDENTITY(1,1) PRIMARY KEY,
    [Name] nvarchar(200) NOT NULL,
    [Address] nvarchar(500) NOT NULL,
    [City] nvarchar(100) NOT NULL,
    [State] nvarchar(50) NOT NULL,
    [ZipCode] nvarchar(20) NOT NULL,
    [Description] nvarchar(1000),
    [NumberOfRooms] int NOT NULL,
    [Price] decimal(18,2) NOT NULL,
    [CreatedAt] datetime2 NOT NULL DEFAULT GETUTCDATE(),
    [UpdatedAt] datetime2 NOT NULL DEFAULT GETUTCDATE()
);
```

---

## ?? Verify Everything Works

### 1. Check Migrations Created
```bash
dotnet ef migrations list --project residence.infrastructure --startup-project residence.api
```

Should show: `InitialCreate`

### 2. Check Database Created
```bash
dotnet ef database info --project residence.infrastructure --startup-project residence.api
```

Should show: SQL Server provider and ResidenceDb database

### 3. Query the Table
Open SQL Server Management Studio:
```
Server: (localdb)\mssqllocaldb
Database: ResidenceDb
Table: dbo.Residences
```

Or run query:
```sql
SELECT * FROM [ResidenceDb].[dbo].[Residences];
```

Should return: Empty result (no data yet)

---

## ?? Adding New Entities Later

When you want to add new features:

### 1. Create Entity
```csharp
public class Property
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
```

### 2. Create Configuration
```csharp
public class PropertyConfiguration : IEntityTypeConfiguration<Property>
{
    public void Configure(EntityTypeBuilder<Property> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Name).IsRequired().HasMaxLength(200);
        builder.ToTable("Properties", "dbo");
    }
}
```

### 3. Add to DbContext
```csharp
public DbSet<Property> Properties { get; set; }

protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.ApplyConfiguration(new PropertyConfiguration());
}
```

### 4. Create & Apply Migration
```bash
dotnet ef migrations add AddPropertyEntity --project residence.infrastructure --startup-project residence.api
dotnet ef database update --project residence.infrastructure --startup-project residence.api
```

---

## ?? Tips & Tricks

### Tip 1: Always Build First
```bash
dotnet build
```

### Tip 2: Check Migration Before Applying
```bash
dotnet ef migrations script --project residence.infrastructure --startup-project residence.api -o migration.sql
# Review migration.sql before running database update
```

### Tip 3: Save Migration Scripts
```bash
dotnet ef migrations script --project residence.infrastructure --startup-project residence.api -o InitialCreate.sql
```

### Tip 4: View Pending Migrations
```bash
dotnet ef database info --project residence.infrastructure --startup-project residence.api
```

### Tip 5: Rollback if Needed
```bash
dotnet ef migrations remove --project residence.infrastructure --startup-project residence.api
```

---

## ?? Quick Troubleshooting

| Problem | Solution |
|---------|----------|
| Build fails | Run `dotnet restore` then `dotnet build` |
| Migration not found | Check project paths and startup project |
| Database not created | Run `dotnet ef database update` |
| Connection failed | Verify LocalDB is installed |
| EF tool missing | Run `dotnet tool install --global dotnet-ef` |

---

## ?? You're Ready!

Everything is configured. Just run these three commands:

```bash
# 1. Create migration
dotnet ef migrations add InitialCreate --project residence.infrastructure --startup-project residence.api

# 2. Apply to SQL Server
dotnet ef database update --project residence.infrastructure --startup-project residence.api

# 3. Run application
dotnet run --project residence.api
```

Then open: `https://localhost:7000/swagger`

**Done!** Your SQL Server database is ready. ??
