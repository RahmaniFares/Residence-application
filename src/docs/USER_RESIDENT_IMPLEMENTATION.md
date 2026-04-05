# User-Resident One-to-One Relationship - Implementation Summary

## ✅ Completed Adjustments

### Overview
The User-Resident relationship has been adjusted and fully documented. A User is one-to-one with a Resident, meaning:
- **One User** can have **one Resident profile**
- **One Resident** is associated with **one User account**
- The relationship is **optional** on the User side (nullable UserId)
- The relationship is **restricted on deletion** to maintain data integrity

---

## 📁 Files Modified

### 1. Domain Models

#### **residence.domain/Entities/User.cs**
```csharp
// Navigation property - One-to-One relationship with Resident
public Resident? Resident { get; set; }
```
**Changes:**
- ✅ Enhanced XML documentation
- ✅ Clarified one-to-one relationship in comments
- ✅ No structural changes (backward compatible)

---

#### **residence.domain/Entities/Resident.cs**
```csharp
// Foreign Key for one-to-one relationship
public Guid? UserId { get; set; }

// Navigation property
public User? User { get; set; }
```
**Changes:**
- ✅ Enhanced all property documentation
- ✅ Clarified `UserId` as Foreign Key
- ✅ Added detailed comments on relationships
- ✅ No structural changes

---

### 2. Database Configuration

#### **residence.infrastructure/Configurations/UserConfiguration.cs**
```csharp
// One-to-One Relationship Configuration
builder.HasOne(u => u.Resident)
    .WithOne(r => r.User)
    .HasForeignKey<Resident>(r => r.UserId)
    .OnDelete(DeleteBehavior.Restrict);
```
**Changes:**
- ✅ Improved documentation
- ✅ Added explicit relationship comments
- ✅ Clarified that FK is on Resident side
- ✅ Explained Restrict delete behavior

---

#### **residence.infrastructure/Configurations/ResidentConfiguration.cs**
```csharp
// One-to-One Relationship Configuration
builder.HasOne(r => r.User)
    .WithOne(u => u.Resident)
    .HasForeignKey<Resident>(r => r.UserId)
    .OnDelete(DeleteBehavior.Restrict);
```
**Changes:**
- ✅ Enhanced documentation
- ✅ Explicitly marked Resident as dependent entity
- ✅ Clarified all relationship characteristics
- ✅ Added comprehensive comments

---

### 3. Data Transfer Objects

#### **residence.application/DTOs/UserDto.cs** - Enhanced
```csharp
public record UserDto(
    Guid Id,
    string Email,
    string FirstName,
    string LastName,
    string PhoneNumber,
    UserRole Role,
    string? AvatarUrl,
    Guid? ResidentId,        // ← NEW FIELD
    DateTime CreatedAt,
    DateTime? UpdatedAt
);
```
**Changes:**
- ✅ Added `ResidentId?: Guid?` field
- ✅ Allows referencing the associated resident
- ✅ Fully backward compatible (nullable)

---

#### **residence.application/DTOs/UserWithResidentDto.cs** - NEW
```csharp
public record UserWithResidentDto(
    Guid Id,
    string Email,
    string FirstName,
    string LastName,
    string PhoneNumber,
    UserRole Role,
    string? AvatarUrl,
    ResidentDto? Resident,   // ← Complete resident object
    DateTime CreatedAt,
    DateTime? UpdatedAt
);
```
**Purpose:**
- ✅ For scenarios requiring full resident data
- ✅ Profile pages, detailed user views
- ✅ API endpoints that need complete information

---

## 📊 Relationship Diagram

```
┌─────────────────────────────────────────────────────────┐
│                      DATABASE SCHEMA                    │
└─────────────────────────────────────────────────────────┘

┌──────────────────┐              ┌────────────────────┐
│    Users Table   │              │  Residents Table   │
├──────────────────┤              ├────────────────────┤
│ Id (PK)          │◄─────────────│ Id (PK)            │
│ Email (UNIQUE)   │  One-to-One  │ UserId (FK)        │
│ PasswordHash     │              │ HouseId (FK)       │
│ FirstName        │              │ FirstName          │
│ LastName         │              │ LastName           │
│ PhoneNumber      │              │ Email              │
│ Role             │              │ PhoneNumber        │
│ AvatarUrl        │              │ Address            │
│ ResidenceId      │              │ BirthDate          │
│ CreatedAt        │              │ Status             │
│ UpdatedAt        │              │ MoveInDate         │
│ IsDeleted        │              │ MoveOutDate        │
└──────────────────┘              │ ResidenceId        │
                                  │ CreatedAt          │
                                  │ UpdatedAt          │
                                  │ IsDeleted          │
                                  └────────────────────┘

KEY: Resident.UserId → Users.Id (Restrict on Delete)
```

---

## 🔄 Entity Relationships

### User Side (Principal Entity)
```csharp
public User
{
    public Guid Id { get; set; }
    // ... other properties ...
    
    // Navigation to associated Resident
    public Resident? Resident { get; set; }
}
```

### Resident Side (Dependent Entity)
```csharp
public Resident
{
    public Guid Id { get; set; }
    public Guid? UserId { get; set; }  // Foreign Key
    // ... other properties ...
    
    // Navigation to associated User
    public User? User { get; set; }
}
```

---

## 🛠️ Usage Examples

### Example 1: Get User with Resident Info
```csharp
var user = await _context.Users
    .Include(u => u.Resident)
    .FirstOrDefaultAsync(u => u.Id == userId);

if (user?.Resident != null)
{
    // User is a resident
    var moveInDate = user.Resident.MoveInDate;
}
```

### Example 2: Create User as Resident
```csharp
var user = new User
{
    Email = "john@example.com",
    PasswordHash = BCrypt.HashPassword("password"),
    FirstName = "John",
    LastName = "Doe",
    PhoneNumber = "+1234567890",
    Role = UserRole.Resident,
    ResidenceId = residenceId
};

var resident = new Resident
{
    UserId = user.Id,  // FK to User
    FirstName = "John",
    LastName = "Doe",
    Email = "john@example.com",
    PhoneNumber = "+1234567890",
    Status = ResidentStatus.Active,
    ResidenceId = residenceId
};

await _context.Users.AddAsync(user);
await _context.Residents.AddAsync(resident);
await _context.SaveChangesAsync();
```

### Example 3: Mapping to DTOs
```csharp
// Basic UserDto
var userDto = new UserDto(
    user.Id,
    user.Email,
    user.FirstName,
    user.LastName,
    user.PhoneNumber,
    user.Role,
    user.AvatarUrl,
    user.Resident?.Id,  // ResidentId
    user.CreatedAt,
    user.UpdatedAt
);

// Complete UserWithResidentDto
var userWithResidentDto = new UserWithResidentDto(
    user.Id,
    user.Email,
    user.FirstName,
    user.LastName,
    user.PhoneNumber,
    user.Role,
    user.AvatarUrl,
    user.Resident != null ? MapToResidentDto(user.Resident) : null,
    user.CreatedAt,
    user.UpdatedAt
);
```

---

## 📚 Documentation Files Created

### 1. **src/docs/USER_RESIDENT_RELATIONSHIP.md**
Comprehensive documentation including:
- Detailed entity structure
- Configuration explanation
- Usage scenarios
- Best practices
- Common queries
- Migration considerations
- API endpoint examples

### 2. **src/docs/USER_RESIDENT_ADJUSTMENTS.md**
Summary of all changes made:
- List of modified files
- Impact analysis
- DTO changes
- Database impact
- Backward compatibility notes
- Testing recommendations

---

## ✨ Key Features

### ✅ Strong Typing
```csharp
public record UserWithResidentDto(
    // ... other fields ...
    ResidentDto? Resident  // Strongly typed
);
```

### ✅ Null Safety
```csharp
// All relationship navigation properties are optional (nullable)
public Resident? Resident { get; set; }
public User? User { get; set; }
```

### ✅ Data Integrity
```csharp
// DeleteBehavior.Restrict prevents orphaning
.OnDelete(DeleteBehavior.Restrict)
```

### ✅ Multi-Tenancy Support
```csharp
// Both entities include ResidenceId for tenant isolation
public Guid ResidenceId { get; set; }
```

---

## 📋 Compilation Status

✅ **All files compile without errors**

Verified files:
- ✅ residence.domain/Entities/User.cs
- ✅ residence.domain/Entities/Resident.cs
- ✅ residence.infrastructure/Configurations/UserConfiguration.cs
- ✅ residence.infrastructure/Configurations/ResidentConfiguration.cs
- ✅ residence.application/DTOs/UserDto.cs
- ✅ residence.application/DTOs/UserWithResidentDto.cs

---

## 🚀 Next Steps

### In Your Services
```csharp
public class UserService : IUserService
{
    // Get user without resident details
    public async Task<UserDto> GetUserAsync(Guid userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        return MapToUserDto(user);
    }
    
    // Get user with resident details
    public async Task<UserWithResidentDto> GetUserWithResidentAsync(Guid userId)
    {
        var user = await _userRepository.GetWithResidentAsync(userId);
        return MapToUserWithResidentDto(user);
    }
}
```

### In Your Repositories
```csharp
public class UserRepository : IUserRepository
{
    public async Task<User?> GetWithResidentAsync(Guid id)
    {
        return await _dbSet
            .Include(u => u.Resident)
            .FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);
    }
}
```

### In Your Controllers/Endpoints
```csharp
[HttpGet("{userId}")]
public async Task<IResult> GetUser(Guid userId)
{
    var userDto = await _userService.GetUserAsync(userId);
    return Results.Ok(userDto);
}

[HttpGet("{userId}/with-resident")]
public async Task<IResult> GetUserWithResident(Guid userId)
{
    var userWithResidentDto = await _userService.GetUserWithResidentAsync(userId);
    return Results.Ok(userWithResidentDto);
}
```

---

## 🔐 Relationship Configuration Summary

| Property | Value |
|----------|-------|
| **Type** | One-to-One |
| **Principal Entity** | User |
| **Dependent Entity** | Resident |
| **Foreign Key** | Resident.UserId |
| **FK Nullable** | Yes |
| **Delete Behavior** | Restrict |
| **Cascade Updates** | Yes |
| **Lazy Loading** | Disabled (use .Include()) |

---

## 📝 Notes

- The relationship configuration was already correct in the codebase
- These changes enhance documentation and add support DTOs
- No database migrations are required
- All changes are backward compatible
- The relationship is optional (a User doesn't have to be a Resident)

---

## 🎯 Summary

✅ **User-Resident one-to-one relationship is fully configured and documented**

Models:
- ✅ User.cs - Enhanced with relationship documentation
- ✅ Resident.cs - Enhanced with FK and relationship documentation

Configuration:
- ✅ UserConfiguration - Clear relationship setup
- ✅ ResidentConfiguration - Clear dependent entity setup

DTOs:
- ✅ UserDto - Added ResidentId field
- ✅ UserWithResidentDto - New DTO for complete data

Documentation:
- ✅ USER_RESIDENT_RELATIONSHIP.md - Comprehensive guide
- ✅ USER_RESIDENT_ADJUSTMENTS.md - Summary of changes

Ready for implementation in services, repositories, and API endpoints!
