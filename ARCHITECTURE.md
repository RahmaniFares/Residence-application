# Infrastructure & API Layer Architecture Refactoring

## Overview

This document outlines the refactored architecture focusing on:
1. **IEntityTypeConfiguration** pattern for entity configurations in the Infrastructure layer
2. **Static MapEndpoints** methods for organized endpoint management in the API layer

---

## Infrastructure Layer - Entity Type Configuration

### Purpose
Using `IEntityTypeConfiguration<T>` provides several benefits:
- **Separation of Concerns**: Model configuration is isolated from DbContext
- **Reusability**: Configurations can be easily shared and tested
- **Maintainability**: Each entity has its own configuration class
- **Scalability**: Easy to add new entities with their own configurations

### Structure

#### ResidenceConfiguration.cs
```csharp
public class ResidenceConfiguration : IEntityTypeConfiguration<Residence>
{
    public void Configure(EntityTypeBuilder<Residence> builder)
    {
        // All model configuration for Residence entity
    }
}
```

**Configuration includes:**
- Primary key setup
- Property constraints (Required, MaxLength, etc.)
- Column types for SQL Server
- Default values (SQL functions)
- Table mapping and schema

#### ApplicationDbContext.cs
The DbContext now applies configurations cleanly:
```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);
    modelBuilder.ApplyConfiguration(new ResidenceConfiguration());
}
```

### Adding New Entity Configurations

To add configuration for a new entity:

1. Create entity in `residence.domain/Entities/YourEntity.cs`
2. Create configuration in `residence.infrastructure/Configurations/YourEntityConfiguration.cs`
   ```csharp
   public class YourEntityConfiguration : IEntityTypeConfiguration<YourEntity>
   {
       public void Configure(EntityTypeBuilder<YourEntity> builder)
       {
           // Configure your entity
       }
   }
   ```
3. Apply in `ApplicationDbContext.OnModelCreating()`:
   ```csharp
   modelBuilder.ApplyConfiguration(new YourEntityConfiguration());
   ```

---

## API Layer - Static MapEndpoints Pattern

### Purpose
Organizing endpoints using static methods provides:
- **Clean Program.cs**: No cluttered endpoint definitions
- **Maintainability**: Related endpoints grouped together
- **Scalability**: Each entity has its own endpoint file
- **Testability**: Static methods can be unit tested
- **Readability**: Clear separation of mapping logic and handlers

### Structure

#### ResidenceEndpoints.cs
```csharp
public static class ResidenceEndpoints
{
    public static void MapResidenceEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/residences")...;
        
        group.MapGetAll();
        group.MapGetById();
        group.MapCreate();
        group.MapUpdate();
        group.MapDelete();
    }
    
    private static void MapGetAll(this RouteGroupBuilder group) { ... }
    private static void MapGetById(this RouteGroupBuilder group) { ... }
    private static void MapCreate(this RouteGroupBuilder group) { ... }
    private static void MapUpdate(this RouteGroupBuilder group) { ... }
    private static void MapDelete(this RouteGroupBuilder group) { ... }
    
    // Endpoint handlers
    private static async Task<IResult> GetAllResidences(...) { ... }
    private static async Task<IResult> GetResidenceById(...) { ... }
    // ... more handlers
}
```

#### Program.cs Usage
```csharp
var app = builder.Build();
// ... middleware configuration

// Map all endpoints
app.MapResidenceEndpoints();

app.Run();
```

### Features of ResidenceEndpoints

1. **RouteGroupBuilder Extension Methods**
   - Each HTTP method has its own `Map*` method
   - Encapsulates related metadata (Summary, Description, etc.)

2. **Comprehensive Error Handling**
   - Input validation with descriptive error messages
   - Try-catch blocks for service exceptions
   - Proper HTTP status codes

3. **Documentation**
   - Summary and Description for each endpoint
   - Produces metadata for response types
   - OpenAPI support

4. **Validation Helpers**
   - `ValidateCreateDto()` - Validates new residence data
   - `ValidateUpdateDto()` - Validates update data
   - Reusable across create and update operations

### Endpoint Definitions

Each endpoint is mapped with:
- **Route**: Relative to group path
- **Handler**: Static method reference
- **Name**: For endpoint linking
- **OpenAPI**: Support for Swagger documentation
- **Summary/Description**: For documentation
- **Produces**: Response type metadata

Example:
```csharp
private static void MapGetById(this RouteGroupBuilder group)
{
    group.MapGet("/{id}", GetResidenceById)
        .WithName("GetResidenceById")
        .WithOpenApi()
        .WithSummary("Get residence by ID")
        .WithDescription("Retrieves a specific residence by its ID")
        .Produces<ResidenceDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);
}
```

### Adding New Endpoints

For a new entity (e.g., `Property`):

1. Create `residence.api/Endpoints/PropertyEndpoints.cs`
   ```csharp
   public static class PropertyEndpoints
   {
       public static void MapPropertyEndpoints(this WebApplication app)
       {
           var group = app.MapGroup("/api/properties").WithTags("Properties");
           // Map CRUD operations
       }
   }
   ```

2. Add to `Program.cs`:
   ```csharp
   app.MapResidenceEndpoints();
   app.MapPropertyEndpoints();  // Add new mapping
   ```

---

## File Structure

### Infrastructure Layer
```
residence.infrastructure/
??? Configurations/
?   ??? ResidenceConfiguration.cs      (IEntityTypeConfiguration<Residence>)
??? Data/
?   ??? ApplicationDbContext.cs        (Applies configurations)
??? Repositories/
?   ??? ResidenceRepository.cs
??? Extensions/
    ??? ServiceCollectionExtensions.cs
```

### API Layer
```
residence.api/
??? Endpoints/
?   ??? ResidenceEndpoints.cs          (Static MapEndpoints methods)
??? Program.cs                          (Clean, minimal configuration)
??? appsettings.json
```

---

## Benefits of This Architecture

### Infrastructure Layer with IEntityTypeConfiguration
? **Organization**: Entity configurations are separate from DbContext  
? **Reusability**: Configurations can be composed and shared  
? **Flexibility**: Easy to add fluent API configurations  
? **Testing**: Configurations can be tested independently  
? **Performance**: Column types are explicitly defined for SQL Server optimization  

### API Layer with MapEndpoints
? **Cleanliness**: Program.cs remains clean and readable  
? **Modularity**: Each entity endpoint set is self-contained  
? **Extensibility**: Easy to add new endpoint files  
? **Validation**: Centralized input validation logic  
? **Documentation**: Swagger metadata is part of endpoint definition  
? **Error Handling**: Comprehensive exception handling per endpoint  

---

## Database Column Types

The `ResidenceConfiguration` explicitly specifies SQL Server column types:

| Property | Column Type | Constraint |
|----------|-------------|-----------|
| Id | int | Primary Key |
| Name | nvarchar(200) | Required |
| Address | nvarchar(500) | Required |
| City | nvarchar(100) | Required |
| State | nvarchar(50) | Required |
| ZipCode | nvarchar(20) | Required |
| Description | nvarchar(1000) | Optional |
| NumberOfRooms | int | Required |
| Price | decimal(18,2) | Precision: 18,2 |
| CreatedAt | datetime2 | Default: GETUTCDATE() |
| UpdatedAt | datetime2 | Default: GETUTCDATE() |
| Table Name | Residences | Schema: dbo |

---

## Validation Strategy

### Input Validation
The `ResidenceEndpoints` class includes comprehensive validation:

```csharp
private static bool ValidateCreateDto(CreateResidenceDto dto, out string errorMessage)
{
    // Checks for:
    // - Required fields (Name, Address, City, State, ZipCode)
    // - Valid numeric values (NumberOfRooms > 0, Price > 0)
    // - Returns error message if validation fails
}
```

**Validated Fields:**
- Name - Required
- Address - Required
- City - Required
- State - Required
- ZipCode - Required
- NumberOfRooms - Must be > 0
- Price - Must be > 0

---

## Future Enhancements

1. **Add Data Annotations** to DTOs for automatic validation
2. **Implement FluentValidation** for complex business rules
3. **Add Logging** to endpoints and repository
4. **Create Unit Tests** for endpoint handlers
5. **Add API Versioning** (e.g., `/api/v1/residences`)
6. **Implement Pagination** in GetAll endpoint
7. **Add Filtering and Sorting** capabilities

---

## Summary

This refactored architecture provides:
- **Clean Separation**: Configuration logic separate from DbContext
- **Scalable Design**: Easy to add new entities and endpoints
- **Professional Structure**: Follows industry best practices
- **Maintainable Code**: Clear organization and responsibilities
- **Comprehensive Documentation**: Self-documenting endpoints with Swagger
