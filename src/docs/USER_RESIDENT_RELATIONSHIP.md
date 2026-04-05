# User-Resident One-to-One Relationship Documentation

## Overview
This document describes the one-to-one relationship between the `User` and `Resident` entities in the Residence Application. A user can be a resident, and a resident must have a user account for authentication and system access.

## Entity Relationship Diagram

```
┌────────────────┐                    ┌──────────────────┐
│     User       │                    │    Resident      │
├────────────────┤        1──────1     ├──────────────────┤
│ Id (PK)        │◄──────────────────►│ Id (PK)          │
│ Email          │    One-to-One      │ UserId (FK)      │
│ PasswordHash   │                    │ HouseId (FK)     │
│ FirstName      │                    │ FirstName        │
│ LastName       │                    │ LastName         │
│ PhoneNumber    │                    │ Email            │
│ Role           │                    │ PhoneNumber      │
│ AvatarUrl      │                    │ Address          │
│ ResidenceId    │                    │ BirthDate        │
│ CreatedAt      │                    │ Status           │
│ UpdatedAt      │                    │ MoveInDate       │
│ IsDeleted      │                    │ MoveOutDate      │
│                │                    │ ResidenceId      │
│                │                    │ CreatedAt        │
│                │                    │ UpdatedAt        │
│                │                    │ IsDeleted        │
└────────────────┘                    └──────────────────┘
```

## Domain Model

### User Entity
**Location:** `residence.domain/Entities/User.cs`

```csharp
public class User : BaseEntity
{
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public UserRole Role { get; set; }
    public string? AvatarUrl { get; set; }
    
    // One-to-One relationship
    public Resident? Resident { get; set; }
}
```

**Properties:**
- **Email**: Unique identifier for authentication (indexed)
- **PasswordHash**: BCrypt hashed password for security
- **FirstName, LastName**: User's name
- **PhoneNumber**: Contact number
- **Role**: User role (Admin/Resident)
- **AvatarUrl**: Profile picture URL
- **ResidenceId**: Multi-tenancy support
- **Resident**: Navigation property to associated resident profile

### Resident Entity
**Location:** `residence.domain/Entities/Resident.cs`

```csharp
public class Resident : BaseEntity
{
    public Guid? UserId { get; set; }          // Foreign Key
    public Guid? HouseId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string? Address { get; set; }
    public DateOnly? BirthDate { get; set; }
    public ResidentStatus Status { get; set; }
    public DateTime MoveInDate { get; set; }
    public DateTime? MoveOutDate { get; set; }
    
    // One-to-One relationship
    public User? User { get; set; }
    
    // Other relationships
    public House? House { get; set; }
    public ICollection<Payment> Payments { get; set; }
    public ICollection<Incident> Incidents { get; set; }
    public ICollection<Post> Posts { get; set; }
}
```

**Properties:**
- **UserId**: Foreign Key referencing User (nullable for flexibility)
- **HouseId**: References the resident's house
- **FirstName, LastName**: Resident's name
- **Email**: Resident's email
- **PhoneNumber**: Contact number
- **Address**: Resident's address
- **BirthDate**: Date of birth
- **Status**: Resident status (Active/Inactive/Suspended)
- **MoveInDate**: When the resident moved in
- **MoveOutDate**: When the resident moved out (if applicable)
- **User**: Navigation property to the associated user account

## Database Configuration

### UserConfiguration
**Location:** `residence.infrastructure/Configurations/UserConfiguration.cs`

```csharp
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // ... property configurations ...
        
        // One-to-One Relationship Configuration
        builder.HasOne(u => u.Resident)
            .WithOne(r => r.User)
            .HasForeignKey<Resident>(r => r.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
```

**Key Points:**
- One User can have zero or one Resident (optional)
- The relationship is configured from the User side with `HasOne`
- Foreign Key is on the Resident side (`UserId`)
- `OnDelete(DeleteBehavior.Restrict)` prevents deleting a User if a Resident depends on it

### ResidentConfiguration
**Location:** `residence.infrastructure/Configurations/ResidentConfiguration.cs`

```csharp
public class ResidentConfiguration : IEntityTypeConfiguration<Resident>
{
    public void Configure(EntityTypeBuilder<Resident> builder)
    {
        // ... property configurations ...
        
        // One-to-One Relationship Configuration
        builder.HasOne(r => r.User)
            .WithOne(u => u.Resident)
            .HasForeignKey<Resident>(r => r.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
```

**Key Points:**
- Resident is the dependent entity (has the foreign key)
- `UserId` is nullable, allowing a resident without an active user account
- Relationship is explicitly configured with clear documentation

## Data Transfer Objects (DTOs)

### UserDto
**Location:** `residence.application/DTOs/UserDto.cs`

Used for basic user information transfers:

```csharp
public record UserDto(
    Guid Id,
    string Email,
    string FirstName,
    string LastName,
    string PhoneNumber,
    UserRole Role,
    string? AvatarUrl,
    Guid? ResidentId,        // Reference to associated resident
    DateTime CreatedAt,
    DateTime? UpdatedAt
);
```

### UserWithResidentDto
**Location:** `residence.application/DTOs/UserWithResidentDto.cs`

Used when you need complete user and resident information together:

```csharp
public record UserWithResidentDto(
    Guid Id,
    string Email,
    string FirstName,
    string LastName,
    string PhoneNumber,
    UserRole Role,
    string? AvatarUrl,
    ResidentDto? Resident,   // Full resident details
    DateTime CreatedAt,
    DateTime? UpdatedAt
);
```

### ResidentDto
**Location:** `residence.application/DTOs/ResidentDto.cs`

```csharp
public record ResidentDto(
    Guid Id,
    Guid? UserId,
    Guid? HouseId,
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    string? Address,
    DateOnly? BirthDate,
    ResidentStatus Status,
    DateTime MoveInDate,
    DateTime? MoveOutDate,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);
```

## Usage Scenarios

### Scenario 1: Get User with Associated Resident
```csharp
// In a Service
var userWithResident = await _userRepository.GetWithResidentAsync(userId);
return new UserWithResidentDto(
    userWithResident.Id,
    userWithResident.Email,
    userWithResident.FirstName,
    userWithResident.LastName,
    userWithResident.PhoneNumber,
    userWithResident.Role,
    userWithResident.AvatarUrl,
    userWithResident.Resident != null ? MapToResidentDto(userWithResident.Resident) : null,
    userWithResident.CreatedAt,
    userWithResident.UpdatedAt
);
```

### Scenario 2: Create a User with Resident Profile
```csharp
// User registration that creates both User and Resident
var user = new User
{
    Email = dto.Email,
    PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
    FirstName = dto.FirstName,
    LastName = dto.LastName,
    PhoneNumber = dto.PhoneNumber,
    Role = UserRole.Resident,
    ResidenceId = residenceId
};

var resident = new Resident
{
    User = user,  // This sets up the relationship
    UserId = user.Id,
    FirstName = dto.FirstName,
    LastName = dto.LastName,
    Email = dto.Email,
    PhoneNumber = dto.PhoneNumber,
    Status = ResidentStatus.Active,
    ResidenceId = residenceId
};

await _userRepository.AddAsync(user);
await _residentRepository.AddAsync(resident);
```

### Scenario 3: Update User Profile
```csharp
var user = await _userRepository.GetWithResidentAsync(userId);

user.FirstName = updateDto.FirstName;
user.LastName = updateDto.LastName;
user.PhoneNumber = updateDto.PhoneNumber;
user.AvatarUrl = updateDto.AvatarUrl;

// Update associated resident if exists
if (user.Resident != null)
{
    user.Resident.FirstName = updateDto.FirstName;
    user.Resident.LastName = updateDto.LastName;
    user.Resident.PhoneNumber = updateDto.PhoneNumber;
}

await _userRepository.UpdateAsync(user);
```

### Scenario 4: Get All Residents with User Information
```csharp
// Query to get residents with their user accounts
var residents = await _residentRepository
    .GetAll()
    .Include(r => r.User)
    .Where(r => r.ResidenceId == residenceId)
    .ToListAsync();

return residents.Select(r => new ResidentWithUserDto(
    r.Id,
    new UserDto(
        r.User.Id,
        r.User.Email,
        r.User.FirstName,
        r.User.LastName,
        r.User.PhoneNumber,
        r.User.Role,
        r.User.AvatarUrl,
        r.UserId,
        r.User.CreatedAt,
        r.User.UpdatedAt
    ),
    r.FirstName,
    r.LastName,
    r.Email,
    r.PhoneNumber,
    r.Address,
    r.BirthDate,
    r.Status,
    r.MoveInDate,
    r.MoveOutDate,
    r.CreatedAt,
    r.UpdatedAt
)).ToList();
```

## Key Design Decisions

### 1. **Optional Relationship**
- `UserId` is nullable (`Guid?`)
- Allows flexibility for future use cases
- A resident can exist without an active user account

### 2. **Foreign Key on Resident**
- Resident is the dependent entity
- Makes sense logically: residents depend on user accounts
- Simplifies cascade operations

### 3. **DeleteBehavior.Restrict**
- Prevents accidental deletion of users with resident profiles
- Requires explicit handling of the relationship before deletion
- Maintains data integrity

### 4. **Shared Data**
- Both User and Resident have FirstName, LastName, Email, PhoneNumber
- This is intentional to allow:
  - User info to be used for authentication
  - Resident info to be tenant-specific
  - Independent updates if needed

## Migration Considerations

If migrating existing data, ensure:

1. **Data Consistency**: Every resident should have a corresponding user
2. **Foreign Key Constraints**: UserId must reference valid User records
3. **Email Uniqueness**: User emails must be unique within a residence
4. **Status Alignment**: Verify resident status is appropriate with user role

### Example Migration Script
```sql
-- Ensure data integrity
ALTER TABLE dbo.Residents
ADD CONSTRAINT FK_Residents_Users
FOREIGN KEY (UserId) REFERENCES dbo.Users(Id)
ON DELETE RESTRICT;

-- Create index on foreign key for query performance
CREATE INDEX IX_Residents_UserId ON dbo.Residents(UserId);
```

## API Endpoints

### Get User with Resident Information
```
GET /api/users/{userId}
Response:
{
    "id": "guid",
    "email": "user@example.com",
    "firstName": "John",
    "lastName": "Doe",
    "phoneNumber": "+1234567890",
    "role": "Resident",
    "avatarUrl": "https://...",
    "residentId": "guid",
    "createdAt": "2024-01-01T00:00:00Z",
    "updatedAt": "2024-01-15T12:30:00Z"
}
```

### Get User with Full Resident Details
```
GET /api/users/{userId}/with-resident
Response:
{
    "id": "guid",
    "email": "user@example.com",
    "firstName": "John",
    "lastName": "Doe",
    "phoneNumber": "+1234567890",
    "role": "Resident",
    "avatarUrl": "https://...",
    "resident": {
        "id": "guid",
        "userId": "guid",
        "houseId": "guid",
        "firstName": "John",
        "lastName": "Doe",
        "email": "user@example.com",
        "phoneNumber": "+1234567890",
        "address": "123 Main St",
        "birthDate": "1990-01-15",
        "status": "Active",
        "moveInDate": "2024-01-01T00:00:00Z",
        "moveOutDate": null,
        "createdAt": "2024-01-01T00:00:00Z",
        "updatedAt": "2024-01-01T00:00:00Z"
    },
    "createdAt": "2024-01-01T00:00:00Z",
    "updatedAt": "2024-01-01T00:00:00Z"
}
```

## Best Practices

✅ **Always Load Relationship**: Use `.Include(u => u.Resident)` when querying users that need resident info

✅ **Validate Relationship**: Ensure UserId is set when creating a resident

✅ **Handle Null Cases**: Always check if `Resident` is null before accessing its properties

✅ **Use Appropriate DTOs**: 
- Use `UserDto` for basic user info
- Use `UserWithResidentDto` when you need resident details

✅ **Soft Deletes**: Respect the `IsDeleted` flag when querying

✅ **Multi-Tenancy**: Always include `ResidenceId` in queries to maintain tenant isolation

✅ **Audit Trail**: Leverage `CreatedBy` and `UpdatedBy` to track changes

## Common Queries

### Get User with All Related Data
```csharp
var user = await _context.Users
    .Include(u => u.Resident)
        .ThenInclude(r => r.House)
    .Include(u => u.Resident)
        .ThenInclude(r => r.Payments)
    .Include(u => u.Resident)
        .ThenInclude(r => r.Incidents)
    .FirstOrDefaultAsync(u => u.Id == userId);
```

### Get All Residents with Their Users
```csharp
var residents = await _context.Residents
    .Include(r => r.User)
    .Include(r => r.House)
    .Where(r => r.ResidenceId == residenceId && !r.IsDeleted)
    .ToListAsync();
```

### Find Residents Without User Accounts
```csharp
var orphanedResidents = await _context.Residents
    .Where(r => r.UserId == null && r.ResidenceId == residenceId)
    .ToListAsync();
```

## Summary

The User-Resident one-to-one relationship provides:
- **Authentication**: Users have credentials (email, password)
- **Profiling**: Residents have detailed tenant information
- **Flexibility**: Allows different future use cases
- **Integrity**: DeleteBehavior.Restrict maintains data consistency
- **Multi-tenancy**: Both entities support multi-tenant isolation

This design cleanly separates authentication/authorization concerns (User) from tenant-specific information (Resident).
