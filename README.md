# Residence CRUD API - Clean Architecture

## Architecture Overview

This project implements a clean architecture with the following layers:

### 1. **Domain Layer** (`residence.domain`)
- Contains core business entities and interfaces
- `Entities/Residence.cs` - The Residence entity model
- `Interfaces/IResidenceRepository.cs` - Repository pattern interface

### 2. **Application Layer** (`residence.application`)
- Business logic and use case handling
- `DTOs/` - Data Transfer Objects (CreateResidenceDto, UpdateResidenceDto, ResidenceDto)
- `Interfaces/IResidenceService.cs` - Service interface
- `Services/ResidenceService.cs` - Business logic implementation
- `Extensions/ServiceCollectionExtensions.cs` - Dependency injection setup

### 3. **Infrastructure Layer** (`residence.infrastructure`)
- Data access and external dependencies
- `Data/ApplicationDbContext.cs` - Entity Framework DbContext
- `Repositories/ResidenceRepository.cs` - Data access implementation
- `Extensions/ServiceCollectionExtensions.cs` - Infrastructure DI setup

### 4. **API Layer** (`residence.api`)
- HTTP endpoints using Minimal APIs
- Swagger/OpenAPI documentation
- CORS configuration

## Database Setup

### Connection String
The connection string in `appsettings.json` uses LocalDB:
```
Server=(localdb)\\mssqllocaldb;Database=ResidenceDb;Trusted_Connection=true;
```

### Create Database
The database is automatically created on application startup using `EnsureCreated()`.

For migrations (if needed):
```bash
dotnet ef migrations add InitialCreate --project residence.infrastructure
dotnet ef database update --project residence.infrastructure
```

## Running the Application

1. **Build the solution**
   ```bash
   dotnet build
   ```

2. **Run the API**
   ```bash
   dotnet run --project residence.api
   ```

3. **Access Swagger UI**
   - Navigate to: `https://localhost:7000/swagger`

## API Endpoints

All endpoints are prefixed with `/api/residences`

### GET - Get All Residences
```
GET /api/residences
Response: 200 OK - List<ResidenceDto>
```

### GET - Get Residence by ID
```
GET /api/residences/{id}
Response: 
  - 200 OK - ResidenceDto (if found)
  - 404 Not Found (if not found)
```

### POST - Create Residence
```
POST /api/residences
Content-Type: application/json

{
  "name": "Downtown Apartment",
  "address": "123 Main St",
  "city": "New York",
  "state": "NY",
  "zipCode": "10001",
  "numberOfRooms": 2,
  "price": 250000,
  "description": "Modern apartment in the city center"
}

Response: 201 Created - ResidenceDto
Location: /api/residences/{id}
```

### PUT - Update Residence
```
PUT /api/residences/{id}
Content-Type: application/json

{
  "name": "Updated Name",
  "address": "456 Park Ave",
  "city": "Boston",
  "state": "MA",
  "zipCode": "02101",
  "numberOfRooms": 3,
  "price": 350000,
  "description": "Updated description"
}

Response: 
  - 200 OK - ResidenceDto (if updated)
  - 404 Not Found (if not found)
```

### DELETE - Delete Residence
```
DELETE /api/residences/{id}
Response: 
  - 204 No Content (if deleted)
  - 404 Not Found (if not found)
```

## Project Structure

```
residence-app/
??? residence.api/
?   ??? Program.cs (Minimal API endpoints & configuration)
?   ??? appsettings.json
?   ??? residence.api.csproj
??? residence.application/
?   ??? DTOs/
?   ??? Interfaces/
?   ??? Services/
?   ??? Extensions/
?   ??? residence.application.csproj
??? residence.domain/
?   ??? Entities/
?   ??? Interfaces/
?   ??? residence.domain.csproj
??? residence.infrastructure/
    ??? Data/
    ??? Repositories/
    ??? Extensions/
    ??? residence.infrastructure.csproj
```

## Technologies Used

- **.NET 8**
- **Entity Framework Core 8.0**
- **SQL Server (LocalDB)**
- **Minimal APIs**
- **Swagger/OpenAPI**
- **CORS**

## Design Patterns Used

- **Clean Architecture** - Separation of concerns across layers
- **Repository Pattern** - Data access abstraction
- **Dependency Injection** - Loose coupling between layers
- **DTO Pattern** - Data transfer between layers
- **Service Layer Pattern** - Business logic encapsulation

## Adding New Features

When adding new features:

1. Create the entity in `residence.domain/Entities/`
2. Add DbSet to `ApplicationDbContext`
3. Create repository interface in `residence.domain/Interfaces/`
4. Implement repository in `residence.infrastructure/Repositories/`
5. Create DTOs in `residence.application/DTOs/`
6. Create service interface and implementation in `residence.application/`
7. Add endpoints in `residence.api/Program.cs`
8. Register services in DI extensions

This ensures clean separation of concerns and maintainability.
