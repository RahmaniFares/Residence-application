# Quick Reference Guide

## Infrastructure Layer - IEntityTypeConfiguration Pattern

### Creating a New Entity Configuration

**Step 1: Create the configuration class**
```csharp
// residence.infrastructure/Configurations/YourEntityConfiguration.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using residence.domain.Entities;

public class YourEntityConfiguration : IEntityTypeConfiguration<YourEntity>
{
    public void Configure(EntityTypeBuilder<YourEntity> builder)
    {
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnType("nvarchar(200)");
        
        // ... more properties
        
        builder.ToTable("YourEntities", "dbo");
    }
}
```

**Step 2: Apply in DbContext**
```csharp
// residence.infrastructure/Data/ApplicationDbContext.cs
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);
    modelBuilder.ApplyConfiguration(new YourEntityConfiguration());
}
```

### Key Methods in IEntityTypeConfiguration

| Method | Purpose | Example |
|--------|---------|---------|
| `HasKey()` | Set primary key | `builder.HasKey(e => e.Id)` |
| `Property()` | Configure property | `builder.Property(e => e.Name)` |
| `IsRequired()` | Make field required | `.IsRequired()` |
| `HasMaxLength()` | Set max length | `.HasMaxLength(200)` |
| `HasColumnType()` | Set SQL column type | `.HasColumnType("nvarchar(200)")` |
| `HasDefaultValueSql()` | Set default SQL | `.HasDefaultValueSql("GETUTCDATE()")` |
| `HasPrecision()` | Set decimal precision | `.HasPrecision(18, 2)` |
| `ToTable()` | Map to table name | `.ToTable("Residences", "dbo")` |

---

## API Layer - MapEndpoints Pattern

### Creating a New Entity Endpoints Class

**Step 1: Create the endpoints file**
```csharp
// residence.api/Endpoints/YourEntityEndpoints.cs
using residence.application.DTOs;
using residence.application.Interfaces;

public static class YourEntityEndpoints
{
    public static void MapYourEntityEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/yourentities")
            .WithTags("YourEntities")
            .WithName("YourEntities");

        group.MapGetAll();
        group.MapGetById();
        group.MapCreate();
        group.MapUpdate();
        group.MapDelete();
    }

    private static void MapGetAll(this RouteGroupBuilder group)
    {
        group.MapGet("/", GetAll)
            .WithName("GetAll")
            .WithOpenApi()
            .Produces<List<YourEntityDto>>(StatusCodes.Status200OK);
    }

    private static async Task<IResult> GetAll(IYourEntityService service)
    {
        try
        {
            var items = await service.GetAllAsync();
            return Results.Ok(items);
        }
        catch (Exception ex)
        {
            return Results.StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    // ... implement remaining methods
}
```

**Step 2: Register in Program.cs**
```csharp
var app = builder.Build();

// ... middleware configuration

// Map all endpoints
app.MapYourEntityEndpoints();

app.Run();
```

### Standard Endpoint Method Structure

```csharp
private static void MapGetById(this RouteGroupBuilder group)
{
    group.MapGet("/{id}", GetById)           // Route and handler
        .WithName("GetById")                 // Endpoint name
        .WithOpenApi()                       // Swagger support
        .WithSummary("Get by ID")            // Swagger summary
        .WithDescription("Retrieves...")     // Swagger description
        .Produces<YourEntityDto>(StatusCodes.Status200OK)  // Success response
        .Produces(StatusCodes.Status404NotFound);          // Error response
}

private static async Task<IResult> GetById(int id, IYourEntityService service)
{
    if (id <= 0)
        return Results.BadRequest("ID must be greater than 0");

    try
    {
        var item = await service.GetByIdAsync(id);
        return item is not null ? Results.Ok(item) : Results.NotFound();
    }
    catch (Exception ex)
    {
        return Results.StatusCode(StatusCodes.Status500InternalServerError);
    }
}
```

### Standard Response Codes

| Status | Use Case | Handler |
|--------|----------|---------|
| 200 OK | Successful GET/PUT | `Results.Ok(data)` |
| 201 Created | Successful POST | `Results.Created(uri, data)` |
| 204 No Content | Successful DELETE | `Results.NoContent()` |
| 400 Bad Request | Invalid input | `Results.BadRequest(message)` |
| 404 Not Found | Resource not found | `Results.NotFound()` |
| 500 Internal Server Error | Server error | `Results.StatusCode(500)` |

---

## Validation Pattern

### Validation Methods
```csharp
private static bool ValidateDto(YourEntityDto dto, out string errorMessage)
{
    errorMessage = string.Empty;

    if (string.IsNullOrWhiteSpace(dto.Name))
    {
        errorMessage = "Name is required";
        return false;
    }

    if (dto.Value <= 0)
    {
        errorMessage = "Value must be greater than 0";
        return false;
    }

    return true;
}
```

### Using Validation in Endpoint
```csharp
private static async Task<IResult> Create(CreateYourEntityDto createDto, IYourEntityService service)
{
    if (!ValidateDto(createDto, out var errorMessage))
        return Results.BadRequest(errorMessage);

    try
    {
        var item = await service.CreateAsync(createDto);
        return Results.Created($"/api/yourentities/{item.Id}", item);
    }
    catch (Exception ex)
    {
        return Results.BadRequest($"Failed to create: {ex.Message}");
    }
}
```

---

## Configuration Best Practices

### 1. Always Use Schema
```csharp
builder.ToTable("YourEntities", "dbo");  // Good
builder.ToTable("YourEntities");         // Avoid
```

### 2. Specify Column Types
```csharp
.HasColumnType("nvarchar(200)")  // Good - explicit SQL Server type
.HasMaxLength(200)               // Avoid - relies on default mapping
```

### 3. Set Precision for Decimals
```csharp
builder.Property(e => e.Price)
    .HasColumnType("decimal(18,2)")
    .HasPrecision(18, 2);
```

### 4. Use SQL Functions for Timestamps
```csharp
builder.Property(e => e.CreatedAt)
    .HasDefaultValueSql("GETUTCDATE()");  // SQL Server
```

---

## Endpoint Best Practices

### 1. Always Add OpenAPI Support
```csharp
.WithOpenApi()
.WithSummary("Clear description")
.WithDescription("Detailed description")
```

### 2. Always Validate Input
```csharp
if (id <= 0)
    return Results.BadRequest("ID must be greater than 0");
```

### 3. Always Handle Exceptions
```csharp
try
{
    // business logic
}
catch (Exception ex)
{
    return Results.StatusCode(StatusCodes.Status500InternalServerError);
}
```

### 4. Always Specify Response Types
```csharp
.Produces<YourEntityDto>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status404NotFound)
.Produces(StatusCodes.Status400BadRequest)
```

---

## Testing the Endpoints

### Using curl

```bash
# GET all
curl https://localhost:7000/api/residences

# GET by ID
curl https://localhost:7000/api/residences/1

# CREATE
curl -X POST https://localhost:7000/api/residences \
  -H "Content-Type: application/json" \
  -d '{"name":"Home","address":"123 St","city":"NYC","state":"NY","zipCode":"10001","numberOfRooms":3,"price":300000,"description":"Nice home"}'

# UPDATE
curl -X PUT https://localhost:7000/api/residences/1 \
  -H "Content-Type: application/json" \
  -d '{"name":"Updated","address":"123 St","city":"NYC","state":"NY","zipCode":"10001","numberOfRooms":4,"price":350000,"description":"Updated"}'

# DELETE
curl -X DELETE https://localhost:7000/api/residences/1
```

### Using Swagger UI
Navigate to: `https://localhost:7000/swagger`

---

## Common Issues & Solutions

### Issue: Configuration not applied to DbContext
**Solution**: Ensure `modelBuilder.ApplyConfiguration()` is called in `OnModelCreating()`

### Issue: Endpoint not showing in Swagger
**Solution**: Ensure `.WithOpenApi()` is added to the endpoint mapping

### Issue: Database column types not matching
**Solution**: Explicitly set `.HasColumnType()` in configuration

### Issue: Validation not working
**Solution**: Call validation method before service call and return error if false

---

## File Checklist for New Entity

- [ ] Create Entity in `residence.domain/Entities/`
- [ ] Create IEntityTypeConfiguration in `residence.infrastructure/Configurations/`
- [ ] Apply configuration in `ApplicationDbContext.OnModelCreating()`
- [ ] Create DTOs in `residence.application/DTOs/`
- [ ] Create IRepository in `residence.domain/Interfaces/`
- [ ] Implement Repository in `residence.infrastructure/Repositories/`
- [ ] Create IService in `residence.application/Interfaces/`
- [ ] Implement Service in `residence.application/Services/`
- [ ] Create Endpoints in `residence.api/Endpoints/`
- [ ] Map Endpoints in `Program.cs`
- [ ] Test all endpoints in Swagger UI
