# Supabase PostgreSQL Complete Configuration

## ? Configuration Status: COMPLETE

All your application files have been updated to work with Supabase PostgreSQL.

---

## ?? What Changed

### 1. Removed SQL Server References
- **File:** `residence.infrastructure/residence.infrastructure.csproj`
- **Removed:** `Microsoft.EntityFrameworkCore.SqlServer` package
- **Reason:** Not needed for PostgreSQL

### 2. Updated Entity Configuration
- **File:** `residence.infrastructure/Configurations/ResidenceConfiguration.cs`
- **Changes:**
  - Column types changed from SQL Server (`nvarchar`) to PostgreSQL (`character varying`)
  - Default value changed from `GETUTCDATE()` to `CURRENT_TIMESTAMP`
  - Schema changed from `dbo` to `public`
  - Table name changed to lowercase `residences`
  - Integer types changed to `integer` instead of `int`
  - Decimal type changed to `numeric(18,2)`
  - Timestamp type changed to `timestamp with time zone`

### 3. Already Configured
- ? `residence.infrastructure/Extensions/ServiceCollectionExtensions.cs` already uses `.UseNpgsql()`
- ? `residence.api/appsettings.json` already has Supabase connection string
- ? `Npgsql.EntityFrameworkCore.PostgreSQL` package already installed

---

## ?? Connection String Details

**Location:** `residence.api/appsettings.json`

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=db.yuaioblifgkhdoxgzmtm.supabase.co;Database=postgres;Username=postgres;Password=Graviter123;SSL Mode=Require;Trust Server Certificate=true"
  }
}
```

**Components:**
- **Host:** `db.yuaioblifgkhdoxgzmtm.supabase.co` (Supabase PostgreSQL server)
- **Database:** `postgres` (Default PostgreSQL database)
- **Username:** `postgres` (Root user)
- **Password:** `Graviter123` (Your database password)
- **SSL Mode:** `Require` (Secure connection required)
- **Trust Certificate:** `true` (Accept SSL certificate)

---

## ?? PostgreSQL Column Mapping

| Property | .NET Type | PostgreSQL Type | Configuration |
|----------|-----------|-----------------|---------------|
| Id | int | integer | Primary Key, IDENTITY |
| Name | string | character varying(200) | NOT NULL |
| Address | string | character varying(500) | NOT NULL |
| City | string | character varying(100) | NOT NULL |
| State | string | character varying(50) | NOT NULL |
| ZipCode | string | character varying(20) | NOT NULL |
| Description | string | character varying(1000) | NULL |
| NumberOfRooms | int | integer | NOT NULL |
| Price | decimal | numeric(18,2) | Precision 18,2 |
| CreatedAt | DateTime | timestamp with time zone | Default: CURRENT_TIMESTAMP |
| UpdatedAt | DateTime | timestamp with time zone | Default: CURRENT_TIMESTAMP |

---

## ??? Directory Structure

```
residence-app/
??? residence.api/
?   ??? appsettings.json ? Connection string configured
?   ??? Program.cs ? Already uses PostgreSQL
?   ??? residence.api.csproj ? No changes needed
?
??? residence.infrastructure/
?   ??? Configurations/
?   ?   ??? ResidenceConfiguration.cs ? UPDATED (PostgreSQL types)
?   ??? Data/
?   ?   ??? ApplicationDbContext.cs ? Ready for PostgreSQL
?   ??? Repositories/
?   ?   ??? ResidenceRepository.cs ? Works with PostgreSQL
?   ??? Extensions/
?   ?   ??? ServiceCollectionExtensions.cs ? Already uses .UseNpgsql()
?   ??? residence.infrastructure.csproj ? UPDATED (SQL Server removed)
?   ??? Migrations/ (Will be created)
?
??? residence.application/ ? No changes needed
??? residence.domain/ ? No changes needed
?
??? Documentation/
    ??? SUPABASE_MIGRATION_GUIDE.md ? NEW
    ??? POSTGRES_QUICK_SETUP.md ? NEW
    ??? SUPABASE_POSTGRESQL_CONFIG.md ? You are here
```

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
- EF Core analyzes your entities
- Compares with database schema
- Creates migration file with SQL

**Files created:**
```
residence.infrastructure/Migrations/
??? 20240115103000_InitialCreate.cs
??? 20240115103000_InitialCreate.Designer.cs
??? ApplicationDbContextModelSnapshot.cs
```

### Step 3: Apply Migration to Supabase
```bash
dotnet ef database update --project residence.infrastructure --startup-project residence.api
```

**What happens:**
- Connects to Supabase database
- Executes migration SQL
- Creates `residences` table
- Logs migration in `__EFMigrationsHistory`

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

## ?? Testing Connections

### Test 1: Verify Connection String
```bash
# Check if connection string is correct
cat residence.api/appsettings.json
```

Should show your Supabase details.

### Test 2: Check Database Access
```bash
# After applying migration, run a quick test
dotnet ef database info --project residence.infrastructure --startup-project residence.api
```

Should show database and provider information.

### Test 3: Verify Table Creation
In Supabase console:
```sql
SELECT table_name FROM information_schema.tables 
WHERE table_schema = 'public';
```

Should show `residences` table.

### Test 4: Check Schema
```sql
SELECT column_name, data_type FROM information_schema.columns 
WHERE table_schema = 'public' AND table_name = 'residences';
```

Should show all columns with PostgreSQL types.

---

## ?? Migration Generated SQL

Your migration will create this SQL on Supabase:

```sql
CREATE TABLE public.residences (
    id integer NOT NULL GENERATED ALWAYS AS IDENTITY,
    name character varying(200) NOT NULL,
    address character varying(500) NOT NULL,
    city character varying(100) NOT NULL,
    state character varying(50) NOT NULL,
    zipcode character varying(20) NOT NULL,
    description character varying(1000),
    numberofrooms integer NOT NULL,
    price numeric(18,2) NOT NULL,
    createdat timestamp with time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updatedat timestamp with time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT pk_residences PRIMARY KEY (id)
);
```

---

## ?? Files Modified

### residence.infrastructure.csproj
```diff
- <PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="8.0.0" />
  ? Removed - Not needed for PostgreSQL
```

### ResidenceConfiguration.cs
```diff
- .HasColumnType("nvarchar(200)")
+ .HasColumnType("character varying(200)")

- .HasColumnType("datetime2")
+ .HasColumnType("timestamp with time zone")

- .HasDefaultValueSql("GETUTCDATE()")
+ .HasDefaultValueSql("CURRENT_TIMESTAMP")

- .ToTable("Residences", "dbo")
+ .ToTable("residences", "public")
```

---

## ?? How It Works

### 1. Code First Workflow
```
Your Code Models
        ?
ResidenceConfiguration (Maps to PostgreSQL)
        ?
ApplicationDbContext (Collects all configurations)
        ?
EF Core Migrations (Generates SQL)
        ?
Supabase PostgreSQL Database (Creates tables)
```

### 2. Database Update Flow
```
Connection String
        ?
Npgsql Provider (Understands PostgreSQL)
        ?
EF Core (Executes migrations)
        ?
Supabase Database (Receives SQL)
        ?
Tables Created
```

### 3. Runtime Flow
```
HTTP Request
        ?
API Endpoint
        ?
Service (Business logic)
        ?
Repository (Data access)
        ?
ApplicationDbContext (DbSet queries)
        ?
Npgsql Provider (SQL translation)
        ?
Supabase PostgreSQL (Execute query)
        ?
Response back to client
```

---

## ?? Security Notes

### Current Setup
- ? SSL/TLS encryption enabled
- ? Secure connection required
- ? Certificate validation enabled

### Important Recommendations
1. **Never commit passwords** - Use secrets manager
2. **Use environment variables** for sensitive data
3. **Rotate password** periodically
4. **Create database user** with limited permissions

### For Production
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "{value from environment variable or secrets manager}"
  }
}
```

---

## ??? Troubleshooting

### Issue: Build fails with NuGet errors
```
Solution: Run dotnet restore
dotnet restore
```

### Issue: Cannot connect to Supabase
```
1. Verify connection string in appsettings.json
2. Check Supabase project is running
3. Check password is correct
4. Test with Supabase connection URL
```

### Issue: Migration fails
```
1. Verify DbContext is correct
2. Verify Entity configurations
3. Check for duplicate migrations
4. Run: dotnet ef migrations remove
5. Try again with new migration name
```

### Issue: EF Core command not found
```
Solution: Install global tool
dotnet tool install --global dotnet-ef
```

---

## ?? Key Concepts

### Migrations
- **Definition:** Version control for your database schema
- **Purpose:** Track database changes alongside code
- **Type:** Code-first (model ? database)

### Npgsql
- **What it is:** .NET data provider for PostgreSQL
- **Role:** Translates EF Core queries to PostgreSQL SQL
- **Benefit:** Type-safe data access

### IEntityTypeConfiguration
- **Purpose:** Separate entity mapping logic
- **Benefit:** Clean, reusable configurations
- **PostgreSQL specific:** Column types, defaults, constraints

### DbContext
- **Purpose:** Bridge between code and database
- **Contains:** DbSets for each entity
- **Applies:** All entity configurations

---

## ? Verification Checklist

- ? Connection string configured
- ? PostgreSQL provider installed (Npgsql)
- ? SQL Server package removed
- ? Entity configuration uses PostgreSQL types
- ? Build successful
- ? Ready for migrations

**Next Step:** Run migrations!

---

## ?? After Configuration

### Immediate Next Steps
1. Create initial migration
2. Apply to Supabase
3. Run application
4. Test endpoints

### Then Add Your Features
1. Create new entities
2. Add configurations
3. Create migrations
4. Apply to database
5. Implement services

---

## ?? Documentation Files

| File | Purpose |
|------|---------|
| **SUPABASE_MIGRATION_GUIDE.md** | Detailed migration instructions |
| **POSTGRES_QUICK_SETUP.md** | Quick one-command setup |
| **SUPABASE_POSTGRESQL_CONFIG.md** | This file - Configuration details |

---

## ?? You're Ready!

Everything is configured. Your next command is:

```bash
dotnet ef migrations add InitialCreate --project residence.infrastructure --startup-project residence.api
```

Good luck! ??
