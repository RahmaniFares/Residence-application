# PostgreSQL Migration Quick Setup

## ?? One-Command Setup (Recommended)

Run these commands in order from your project root directory:

### Step 1: Build the Project
```bash
dotnet build
```

### Step 2: Create Initial Migration
```bash
dotnet ef migrations add InitialCreate --project residence.infrastructure --startup-project residence.api
```

### Step 3: Apply Migration to Supabase
```bash
dotnet ef database update --project residence.infrastructure --startup-project residence.api
```

### Step 4: Run the Application
```bash
dotnet run --project residence.api
```

### Step 5: Test in Swagger
Open: `https://localhost:7000/swagger`

---

## ? What Each Command Does

### Build
- Compiles all projects
- Checks for errors
- Prepares for EF Core tooling

### Migrations Add
- Analyzes your entities and configurations
- Compares with database schema
- Creates migration files in `Migrations/` folder
- Generates C# code representing database changes

### Database Update
- **Connects to Supabase** using connection string
- **Creates tables** based on your entities
- **Adds columns** with proper types (PostgreSQL specific)
- **Sets up indexes** and constraints
- **Logs migration history** in `__EFMigrationsHistory` table

### Run Application
- Starts ASP.NET Core server
- Listens on https://localhost:7000
- Connects to Supabase database

---

## ?? Verify Migration Success

After running the database update command, verify:

### 1. Check Migrations folder
```
residence.infrastructure/Migrations/
??? 20240115103000_InitialCreate.cs
??? 20240115103000_InitialCreate.Designer.cs
??? ApplicationDbContextModelSnapshot.cs
```

### 2. Check Supabase Console
```
https://app.supabase.com/projects ? SQL Editor
SELECT * FROM residences;
```

You should see the table with these columns:
- id (integer)
- name (character varying)
- address (character varying)
- city (character varying)
- state (character varying)
- zipcode (character varying)
- description (character varying)
- numberofrooms (integer)
- price (numeric)
- createdat (timestamp with time zone)
- updatedat (timestamp with time zone)

### 3. Check EF Migrations History
```sql
SELECT * FROM "__EFMigrationsHistory";
```

Should show your "InitialCreate" migration.

---

## ?? Adding New Features (After Initial Setup)

When you want to add a new entity:

### Example: Add "User" Entity

1. **Create the entity:**
   ```csharp
   // residence.domain/Entities/User.cs
   public class User
   {
       public int Id { get; set; }
       public string Email { get; set; } = string.Empty;
       public string FullName { get; set; } = string.Empty;
   }
   ```

2. **Create configuration:**
   ```csharp
   // residence.infrastructure/Configurations/UserConfiguration.cs
   public class UserConfiguration : IEntityTypeConfiguration<User>
   {
       public void Configure(EntityTypeBuilder<User> builder)
       {
           builder.HasKey(e => e.Id);
           builder.Property(e => e.Email).IsRequired().HasMaxLength(255);
           builder.Property(e => e.FullName).IsRequired().HasMaxLength(200);
           builder.ToTable("users", "public");
       }
   }
   ```

3. **Add to DbContext:**
   ```csharp
   public DbSet<User> Users { get; set; }

   protected override void OnModelCreating(ModelBuilder modelBuilder)
   {
       base.OnModelCreating(modelBuilder);
       modelBuilder.ApplyConfiguration(new ResidenceConfiguration());
       modelBuilder.ApplyConfiguration(new UserConfiguration());  // NEW
   }
   ```

4. **Create migration:**
   ```bash
   dotnet ef migrations add AddUserEntity --project residence.infrastructure --startup-project residence.api
   ```

5. **Apply migration:**
   ```bash
   dotnet ef database update --project residence.infrastructure --startup-project residence.api
   ```

---

## ?? Configuration Breakdown

### ResidenceConfiguration.cs
Defines how the Residence entity maps to PostgreSQL:

```csharp
// PostgreSQL column type for strings
.HasColumnType("character varying(200)")

// PostgreSQL integer type
.HasColumnType("integer")

// PostgreSQL numeric type with precision
.HasColumnType("numeric(18,2)")

// PostgreSQL timestamp with timezone
.HasColumnType("timestamp with time zone")

// PostgreSQL default function
.HasDefaultValueSql("CURRENT_TIMESTAMP")

// Table name in public schema
.ToTable("residences", "public")
```

### ApplicationDbContext.cs
Applies all configurations:

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);
    modelBuilder.ApplyConfiguration(new ResidenceConfiguration());
    // Add more configurations here as needed
}
```

### ServiceCollectionExtensions.cs
Registers DbContext with PostgreSQL provider:

```csharp
services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));  // PostgreSQL provider
```

---

## ?? Common Issues & Solutions

### Issue: "Cannot connect to Supabase"
```
Error: "Connection refused" or "Unable to connect"
```

**Solution:**
1. Verify connection string in `appsettings.json`
2. Check internet connection
3. Verify Supabase project is running
4. Check firewall/proxy settings

### Issue: "Migration already exists"
```
Error: "Migration 'InitialCreate' already exists"
```

**Solution:**
- Use a different migration name:
  ```bash
  dotnet ef migrations add InitialCreateV2
  ```

### Issue: "Table already exists"
```
Error: "Relation residences already exists"
```

**Solution:**
- Migration was already applied
- Use `dotnet ef migrations list` to see applied migrations

### Issue: "SSL certificate validation failed"
```
Error: "SSL/TLS error"
```

**Solution:**
- Connection string already has: `SSL Mode=Require;Trust Server Certificate=true`
- This should work with Supabase

---

## ?? Database Schema Created

The migration creates this PostgreSQL schema:

```sql
CREATE TABLE public.residences (
    id integer PRIMARY KEY GENERATED ALWAYS AS IDENTITY,
    name character varying(200) NOT NULL,
    address character varying(500) NOT NULL,
    city character varying(100) NOT NULL,
    state character varying(50) NOT NULL,
    zipcode character varying(20) NOT NULL,
    description character varying(1000),
    numberofrooms integer NOT NULL,
    price numeric(18,2) NOT NULL,
    createdat timestamp with time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updatedat timestamp with time zone NOT NULL DEFAULT CURRENT_TIMESTAMP
);
```

---

## ? What You Get

After completing the setup:

? PostgreSQL database in Supabase  
? Residences table with proper schema  
? Auto-incrementing ID  
? Default timestamps  
? Migration history tracking  
? Type-safe C# code  
? Ready for data operations  

---

## ?? Related Files

- **Configuration:** `residence.infrastructure/Configurations/ResidenceConfiguration.cs`
- **DbContext:** `residence.infrastructure/Data/ApplicationDbContext.cs`
- **Connection:** `residence.api/appsettings.json`
- **Detailed Guide:** `SUPABASE_MIGRATION_GUIDE.md`

---

## ?? Tips

1. **Always apply migrations** after creating them
   ```bash
   dotnet ef database update
   ```

2. **Save migration scripts** for documentation:
   ```bash
   dotnet ef migrations script -o migration.sql
   ```

3. **Test migrations locally first** if possible

4. **Never delete migration files** - keep version history

5. **Commit migrations to version control**

---

## ?? Next Command

Ready to create your database?

```bash
dotnet ef migrations add InitialCreate --project residence.infrastructure --startup-project residence.api
```

Then:

```bash
dotnet ef database update --project residence.infrastructure --startup-project residence.api
```

Then run:

```bash
dotnet run --project residence.api
```

Done! ??
