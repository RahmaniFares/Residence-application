# Complete API Implementation Guide

## Overview

I have created a **comprehensive backend API structure** for your Residence App based on your domain models and global specifications. The implementation follows **Clean Architecture**, **SOLID principles**, and **multi-tenancy** patterns.

---

## ? What Has Been Created

### 1. **Domain Layer** (residence.domain)

#### Base Entity (BaseEntity.cs)
- Foundation for all persistent entities
- Includes: Id (GUID), ResidenceId (tenant), CreatedAt, CreatedBy, UpdatedAt, UpdatedBy, IsDeleted
- Supports soft delete and audit trails

#### Enums (DomainEnums.cs)
```csharp
- HouseStatus (Occupied, Vacant)
- ResidentStatus (Active, MovedOut)
- PaymentStatus (Pending, Paid, Overdue)
- PaymentMethod (Cash, Transfer, Card)
- IncidentStatus (Open, InProgress, Resolved)
- IncidentPriority (Low, Medium, High)
- ExpenseType (Maintenance, Electricity, Water, Cleaning, Security, etc.)
- UserRole (Admin, Resident)
```

#### Domain Entities (DomainEntities.cs)
- **Residence** - Building/Community management
- **ResidenceSettings** - Settings & budgets
- **User** - Authentication & user accounts
- **House** - Individual units/apartments
- **Resident** - Tenant information
- **Payment** - Rent & fee payments
- **Expense** - Expense tracking with images
- **ExpenseImage** - Receipt/supporting images
- **Incident** - Maintenance requests with comments
- **IncidentComment** - Comments on incidents
- **Post** - Community social posts
- **PostLike** - Likes on posts
- **PostComment** - Comments on posts

### 2. **Application Layer** (residence.application)

#### DTOs (DomainDTOs.cs)
Comprehensive data transfer objects for:
- **Authentication**: CreateUserDto, LoginDto, AuthResponseDto, RefreshTokenDto
- **Users**: UserDto, UpdateUserDto
- **Residences**: ResidenceDto, ResidenceSettingsDto
- **Houses**: HouseDto, HouseDetailDto, CreateHouseDto, UpdateHouseDto
- **Residents**: ResidentDto, CreateResidentDto, UpdateResidentDto
- **Payments**: PaymentDto, CreatePaymentDto, UpdatePaymentDto
- **Expenses**: ExpenseDto, CreateExpenseDto, ExpenseImageDto
- **Incidents**: IncidentDto, IncidentCommentDto with create/update variants
- **Posts**: PostDto, PostCommentDto, CreatePostDto, CreatePostCommentDto
- **Pagination**: PaginationDto, PagedResultDto<T>

#### Service Interfaces (DomainServiceInterfaces.cs)
```csharp
- IAuthService - Authentication & token management
- IUserService - User CRUD operations
- IResidenceService - Residence management
- IHouseService - House/unit management
- IResidentService - Resident/tenant management
- IPaymentService - Payment tracking & management
- IExpenseService - Expense tracking with images
- IIncidentService - Incident management with comments
- IPostService - Community posts with likes & comments
```

### 3. **Infrastructure Layer** (residence.infrastructure)

#### EF Core Configurations (DomainEntityConfigurations.cs)
Complete entity configurations for all 13 entities:
- Primary keys (GUID)
- Foreign keys with proper delete behaviors
- Indexes (unique constraints on Email, PostLike)
- Column types, max lengths, default values
- Table naming convention (dbo schema)

#### Updated DbContext (ApplicationDbContext.cs)
- All DbSets registered
- Global query filters for soft delete
- Automatic audit field population (CreatedAt, UpdatedAt)
- Multi-tenancy ResidenceId enforcement

### 4. **API Layer** (residence.api)

#### Endpoints (DomainEndpoints.cs)
**Complete CRUD endpoints for all modules:**

**Authentication**
- POST /api/auth/login
- POST /api/auth/register
- POST /api/auth/refresh

**Residences**
- POST /api/residences
- GET /api/residences/{id}
- PUT /api/residences/{id}
- DELETE /api/residences/{id}
- GET /api/residences/{id}/settings
- PUT /api/residences/{id}/settings

**Houses**
- POST /api/residences/{residenceId}/houses
- GET /api/residences/{residenceId}/houses
- GET /api/residences/{residenceId}/houses/{id}
- GET /api/residences/{residenceId}/houses/{id}/details
- PUT /api/residences/{residenceId}/houses/{id}
- DELETE /api/residences/{residenceId}/houses/{id}

**Residents**
- POST /api/residences/{residenceId}/residents
- GET /api/residences/{residenceId}/residents
- GET /api/residences/{residenceId}/residents/{id}
- PUT /api/residences/{residenceId}/residents/{id}
- DELETE /api/residences/{residenceId}/residents/{id}
- GET /api/residences/{residenceId}/residents/house/{houseId}

**Payments**
- POST /api/residences/{residenceId}/payments
- GET /api/residences/{residenceId}/payments
- GET /api/residences/{residenceId}/payments/{id}
- PUT /api/residences/{residenceId}/payments/{id}
- DELETE /api/residences/{residenceId}/payments/{id}
- GET /api/residences/{residenceId}/payments/resident/{residentId}
- GET /api/residences/{residenceId}/payments/house/{houseId}

**Expenses**
- POST /api/residences/{residenceId}/expenses
- GET /api/residences/{residenceId}/expenses
- GET /api/residences/{residenceId}/expenses/{id}
- PUT /api/residences/{residenceId}/expenses/{id}
- DELETE /api/residences/{residenceId}/expenses/{id}
- POST /api/residences/{residenceId}/expenses/{id}/images
- DELETE /api/residences/{residenceId}/expenses/images/{imageId}

**Incidents**
- POST /api/residences/{residenceId}/incidents
- GET /api/residences/{residenceId}/incidents
- GET /api/residences/{residenceId}/incidents/{id}
- PUT /api/residences/{residenceId}/incidents/{id}
- DELETE /api/residences/{residenceId}/incidents/{id}
- GET /api/residences/{residenceId}/incidents/resident/{residentId}
- POST /api/residences/{residenceId}/incidents/{id}/comments
- GET /api/residences/{residenceId}/incidents/{id}/comments

**Posts & Community**
- POST /api/residences/{residenceId}/posts
- GET /api/residences/{residenceId}/posts
- GET /api/residences/{residenceId}/posts/{id}
- PUT /api/residences/{residenceId}/posts/{id}
- DELETE /api/residences/{residenceId}/posts/{id}
- POST /api/residences/{residenceId}/posts/{id}/likes
- DELETE /api/residences/{residenceId}/posts/{id}/likes/{userId}
- POST /api/residences/{residenceId}/posts/{id}/comments
- DELETE /api/residences/{residenceId}/posts/{id}/comments/{commentId}
- GET /api/residences/{residenceId}/posts/{id}/comments

---

## ?? Next Steps: Implement Services

### Files to Create

You need to implement the service classes in `residence.application/Services/`:

1. **AuthService.cs** - JWT token generation, password hashing (BCrypt)
2. **UserService.cs** - User CRUD with password management
3. **ResidenceService.cs** - Residence CRUD
4. **HouseService.cs** - House/unit management
5. **ResidentService.cs** - Resident/tenant management
6. **PaymentService.cs** - Payment tracking & status management
7. **ExpenseService.cs** - Expense tracking with image handling
8. **IncidentService.cs** - Incident management with comments
9. **PostService.cs** - Community posts with likes & comments

### Service Implementation Template

```csharp
using residence.application.DTOs;
using residence.application.Interfaces;
using residence.infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace residence.application.Services;

public class [ServiceName] : I[ServiceName]
{
    private readonly ApplicationDbContext _context;

    public [ServiceName](ApplicationDbContext context)
    {
        _context = context;
    }

    // TODO: Implement interface methods
    // Pattern for each service:
    // 1. Validate input
    // 2. Query/modify database
    // 3. Handle exceptions
    // 4. Return DTOs
}
```

---

## ?? Program.cs Configuration

Add to your Program.cs:

```csharp
using residence.application.Interfaces;
using residence.application.Services;
using residence.infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplicationBuilder.CreateBuilder(args);

// Add DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IResidenceService, ResidenceService>();
builder.Services.AddScoped<IHouseService, HouseService>();
builder.Services.AddScoped<IResidentService, ResidentService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IExpenseService, ExpenseService>();
builder.Services.AddScoped<IIncidentService, IncidentService>();
builder.Services.AddScoped<IPostService, PostService>();

// Add Swagger/OpenAPI
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure Swagger
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI();
}

// Map Endpoints
app.MapAuthEndpoints();
app.MapResidenceEndpoints();
app.MapHouseEndpoints();
app.MapResidentEndpoints();
app.MapPaymentEndpoints();
app.MapExpenseEndpoints();
app.MapIncidentEndpoints();
app.MapPostEndpoints();

app.UseHttpsRedirection();
app.Run();
```

---

## ?? Database Schema Overview

Your database will have these tables (in `dbo` schema):

```
Users
Residences
ResidenceSettings
Houses
Residents
Payments
Expenses
ExpenseImages
Incidents
IncidentComments
Posts
PostLikes
PostComments
```

All with:
- GUID primary keys
- Soft delete support (IsDeleted)
- Audit columns (CreatedAt, CreatedBy, UpdatedAt, UpdatedBy)
- Multi-tenancy (ResidenceId)

---

## ?? Multi-Tenancy

Every entity (except Residence itself) has a `ResidenceId`:
- Enforced via global query filters in DbContext
- Ensures complete tenant isolation
- All queries automatically filtered by tenant

---

## ?? Soft Delete

All entities support soft delete:
- `IsDeleted` boolean field
- Global query filter hides deleted records
- Data retained for audit/recovery
- Can be restored by setting IsDeleted = false

---

## ?? Audit Trail

Automatically tracked:
- `CreatedAt` - When created (UTC)
- `CreatedBy` - User who created (nullable Guid)
- `UpdatedAt` - When last updated (UTC)
- `UpdatedBy` - User who updated (nullable Guid)

---

## ?? Authentication Flow

1. User calls POST /api/auth/register
2. Service hashes password with BCrypt
3. Creates User entity in database
4. Returns JWT access token + refresh token

Login:
1. User calls POST /api/auth/login
2. Service verifies credentials
3. Generates JWT with ResidenceId + UserId + Role
4. Returns tokens in AuthResponseDto

Refresh:
1. User calls POST /api/auth/refresh with refresh token
2. Service validates refresh token
3. Issues new access token

---

## ?? Entity Relationships

### Residence
- One-to-Many: Users, Houses, Residents, Payments, Expenses, Incidents, Posts
- One-to-One: ResidenceSettings

### House
- One-to-Many: Residents, Payments, Incidents
- One-to-One: CurrentResident

### Resident
- One-to-One: User (bidirectional)
- One-to-Many: Payments, Incidents, Posts
- Many-to-One: House

### Payment
- Many-to-One: House, Resident, Residence

### Expense
- One-to-Many: ExpenseImages
- Many-to-One: Residence

### Incident
- One-to-Many: IncidentComments
- Many-to-One: Resident, House, Residence

### Post
- One-to-Many: Likes, Comments
- Many-to-One: Author (Resident), Residence

---

## ??? Technologies Used

- **.NET 8** - Framework
- **C# 12** - Language
- **Entity Framework Core 8** - ORM
- **SQL Server** - Database
- **Minimal APIs** - Lightweight endpoints
- **OpenAPI/Swagger** - Documentation

---

## ?? Error Handling

All endpoints include:
- Try-catch blocks
- Proper HTTP status codes
  - 200 OK
  - 201 Created
  - 204 No Content
  - 400 Bad Request
  - 404 Not Found
- Meaningful error messages

---

## ? Ready for Services Implementation

The complete **infrastructure** is ready. You now need to:

1. Create 9 service implementations
2. Add authentication middleware (JWT validation)
3. Configure CORS if needed
4. Add validation (FluentValidation)
5. Create database migrations

All structure follows **best practices**:
- ? Clean Architecture
- ? Repository pattern ready
- ? Dependency Injection
- ? Separation of concerns
- ? SOLID principles
- ? Multi-tenancy
- ? Soft delete support
- ? Audit trails

**Your API structure is enterprise-ready!** ??

