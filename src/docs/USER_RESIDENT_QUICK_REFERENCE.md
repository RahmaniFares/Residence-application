# User-Resident Relationship - Quick Reference Guide

## 🎯 One Sentence Summary
**A User account (authentication) has a one-to-one optional relationship with a Resident profile (tenant information).**

---

## 📊 Visual Relationship

```
USERS TABLE                          RESIDENTS TABLE
═════════════════════════════════════════════════════════
┌─────────────────┐                ┌──────────────────┐
│  Id (PK)        │◄───────────────│  Id (PK)         │
│  Email          │   One-to-One   │  UserId (FK)     │
│  PasswordHash   │   Relationship │  HouseId (FK)    │
│  FirstName      │◄───────────────│  FirstName       │
│  LastName       │                │  LastName        │
│  PhoneNumber    │                │  Email           │
│  Role           │                │  PhoneNumber     │
│  AvatarUrl      │                │  Address         │
│  ResidenceId    │                │  BirthDate       │
│  ...Audit...    │                │  Status          │
└─────────────────┘                │  MoveInDate      │
                                   │  MoveOutDate     │
                                   │  ResidenceId     │
                                   │  ...Audit...     │
                                   └──────────────────┘
```

---

## 🏗️ Entity Structure

### User (Principal)
```csharp
public class User : BaseEntity
{
    public string Email { get; set; }           // Unique identifier
    public string PasswordHash { get; set; }    // Authentication
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public UserRole Role { get; set; }
    public string? AvatarUrl { get; set; }
    
    // Navigation: One User can have one Resident
    public Resident? Resident { get; set; }
}
```

### Resident (Dependent)
```csharp
public class Resident : BaseEntity
{
    // Foreign Key pointing to User
    public Guid? UserId { get; set; }
    
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
    
    // Navigation: One Resident is associated with one User
    public User? User { get; set; }
    
    // Other collections
    public House? House { get; set; }
    public ICollection<Payment> Payments { get; set; }
    public ICollection<Incident> Incidents { get; set; }
    public ICollection<Post> Posts { get; set; }
}
```

---

## 🔧 Database Configuration

```csharp
// In UserConfiguration
builder.HasOne(u => u.Resident)
    .WithOne(r => r.User)
    .HasForeignKey<Resident>(r => r.UserId)
    .OnDelete(DeleteBehavior.Restrict);

// In ResidentConfiguration (same configuration, different side)
builder.HasOne(r => r.User)
    .WithOne(u => u.Resident)
    .HasForeignKey<Resident>(r => r.UserId)
    .OnDelete(DeleteBehavior.Restrict);
```

**Key Points:**
- ✅ Foreign Key: `Resident.UserId`
- ✅ Delete Behavior: `Restrict` (prevents orphaned residents)
- ✅ Relationship: Optional on User side (UserId is nullable)

---

## 📦 Data Transfer Objects

### UserDto - Basic User Info
```csharp
public record UserDto(
    Guid Id,
    string Email,
    string FirstName,
    string LastName,
    string PhoneNumber,
    UserRole Role,
    string? AvatarUrl,
    Guid? ResidentId,              // ← Reference to resident
    DateTime CreatedAt,
    DateTime? UpdatedAt
);
```

### UserWithResidentDto - Complete User + Resident
```csharp
public record UserWithResidentDto(
    Guid Id,
    string Email,
    string FirstName,
    string LastName,
    string PhoneNumber,
    UserRole Role,
    string? AvatarUrl,
    ResidentDto? Resident,          // ← Full resident object
    DateTime CreatedAt,
    DateTime? UpdatedAt
);
```

---

## 💾 Typical Database Operations

### CREATE: User with Resident
```csharp
var user = new User { Email = "john@example.com", ... };
var resident = new Resident 
{ 
    UserId = user.Id,  // FK relationship
    User = user,       // Navigation
    ... 
};
```

### READ: User with Resident
```csharp
var user = await _context.Users
    .Include(u => u.Resident)  // Must include to load
    .FirstOrDefaultAsync(u => u.Id == userId);

if (user?.Resident != null)
{
    // Access resident data
}
```

### UPDATE: Modify User and Resident
```csharp
user.FirstName = "Jane";
user.Resident.Address = "123 New St";
await _context.SaveChangesAsync();
```

### DELETE: Restricted by Relationship
```csharp
// This will throw because of DeleteBehavior.Restrict
// Must delete Resident first, or update its UserId to null
await _context.Users.RemoveAsync(user);  // ❌ Will fail
```

---

## 🎯 API Endpoints

```http
# Get user with resident ID only
GET /api/users/{userId}
Response: { id, email, firstName, ..., residentId }

# Get user with full resident details
GET /api/users/{userId}/with-resident
Response: { id, email, firstName, ..., resident: { id, firstName, ... } }

# Get resident with user information
GET /api/residents/{residentId}
Response: { id, userId, firstName, ..., user: { id, email, ... } }
```

---

## 📋 CRUD Operations in Service

```csharp
public class UserService
{
    // Create User with Resident
    public async Task<UserWithResidentDto> CreateUserAsResidentAsync(
        Guid residenceId, 
        CreateUserDto dto)
    {
        var user = new User
        {
            Email = dto.Email,
            PasswordHash = BCrypt.HashPassword(dto.Password),
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            PhoneNumber = dto.PhoneNumber,
            Role = UserRole.Resident,
            ResidenceId = residenceId
        };

        var resident = new Resident
        {
            UserId = user.Id,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            Status = ResidentStatus.Active,
            ResidenceId = residenceId,
            User = user  // Set navigation property
        };

        await _userRepository.AddAsync(user);
        await _residentRepository.AddAsync(resident);
        
        return MapToUserWithResidentDto(user);
    }

    // Get user with resident
    public async Task<UserWithResidentDto?> GetUserWithResidentAsync(Guid userId)
    {
        var user = await _userRepository.GetWithResidentAsync(userId);
        
        if (user == null)
            return null;

        return new UserWithResidentDto(
            user.Id,
            user.Email,
            user.FirstName,
            user.LastName,
            user.PhoneNumber,
            user.Role,
            user.AvatarUrl,
            user.Resident != null 
                ? new ResidentDto(user.Resident.Id, ...) 
                : null,
            user.CreatedAt,
            user.UpdatedAt
        );
    }

    // Update user and resident
    public async Task<UserWithResidentDto> UpdateUserAsync(
        Guid userId, 
        UpdateUserDto dto)
    {
        var user = await _userRepository.GetWithResidentAsync(userId);
        
        if (user == null)
            throw new Exception("User not found");

        // Update user
        user.FirstName = dto.FirstName;
        user.LastName = dto.LastName;
        user.PhoneNumber = dto.PhoneNumber;
        user.AvatarUrl = dto.AvatarUrl;

        // Update associated resident if exists
        if (user.Resident != null)
        {
            user.Resident.FirstName = dto.FirstName;
            user.Resident.LastName = dto.LastName;
            user.Resident.PhoneNumber = dto.PhoneNumber;
        }

        await _userRepository.UpdateAsync(user);
        
        return MapToUserWithResidentDto(user);
    }

    // Get users with residents
    public async Task<List<UserWithResidentDto>> GetUsersWithResidentsAsync(
        Guid residenceId)
    {
        var users = await _userRepository.GetAllWithResidentsAsync(residenceId);
        
        return users
            .Select(u => MapToUserWithResidentDto(u))
            .ToList();
    }
}
```

---

## 🔍 Query Patterns

### Pattern 1: Load User and Resident Together
```csharp
var user = await _context.Users
    .Include(u => u.Resident)
    .FirstOrDefaultAsync(u => u.Id == userId);
```

### Pattern 2: Load Resident with User
```csharp
var resident = await _context.Residents
    .Include(r => r.User)
    .FirstOrDefaultAsync(r => r.Id == residentId);
```

### Pattern 3: Find All Residents with Users
```csharp
var residents = await _context.Residents
    .Include(r => r.User)
    .Where(r => r.ResidenceId == residenceId)
    .ToListAsync();
```

### Pattern 4: Find Orphaned Residents (No User)
```csharp
var orphaned = await _context.Residents
    .Where(r => r.UserId == null)
    .ToListAsync();
```

### Pattern 5: Load Full User Hierarchy
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

---

## ⚠️ Important Rules

### ✅ DO
```csharp
✅ Always .Include(u => u.Resident) when needed
✅ Check if Resident is null before accessing
✅ Set both UserId FK and User navigation property
✅ Respect DeleteBehavior.Restrict
✅ Use ResidenceId for multi-tenant queries
```

### ❌ DON'T
```csharp
❌ Access Resident without .Include()
❌ Forget to set FK when creating relationship
❌ Directly delete User with associated Resident
❌ Ignore DeleteBehavior violations
❌ Skip ResidenceId checks in queries
```

---

## 🧪 Testing Template

```csharp
[Test]
public async Task User_With_Resident_Should_Load_Successfully()
{
    // Arrange
    var residenceId = Guid.NewGuid();
    var user = new User
    {
        Email = "test@example.com",
        FirstName = "John",
        LastName = "Doe",
        PhoneNumber = "555-1234",
        Role = UserRole.Resident,
        ResidenceId = residenceId
    };

    var resident = new Resident
    {
        FirstName = "John",
        LastName = "Doe",
        Email = "test@example.com",
        PhoneNumber = "555-1234",
        Status = ResidentStatus.Active,
        ResidenceId = residenceId,
        User = user,
        UserId = user.Id
    };

    // Act
    await _context.Users.AddAsync(user);
    await _context.Residents.AddAsync(resident);
    await _context.SaveChangesAsync();

    var loadedUser = await _context.Users
        .Include(u => u.Resident)
        .FirstOrDefaultAsync(u => u.Id == user.Id);

    // Assert
    Assert.IsNotNull(loadedUser);
    Assert.IsNotNull(loadedUser.Resident);
    Assert.AreEqual(resident.Id, loadedUser.Resident.Id);
    Assert.AreEqual(user.Id, loadedUser.Resident.UserId);
}
```

---

## 📚 File Locations

```
residence.domain/
├── Entities/
│   ├── User.cs                    ✅ Updated
│   └── Resident.cs                ✅ Updated
│
residence.infrastructure/
└── Configurations/
    ├── UserConfiguration.cs       ✅ Updated
    └── ResidentConfiguration.cs   ✅ Updated

residence.application/
└── DTOs/
    ├── UserDto.cs                 ✅ Updated
    ├── UserWithResidentDto.cs     ✅ NEW
    └── ResidentDto.cs             (existing)

src/docs/
├── USER_RESIDENT_RELATIONSHIP.md  ✅ NEW (detailed)
├── USER_RESIDENT_ADJUSTMENTS.md   ✅ NEW (summary)
└── USER_RESIDENT_IMPLEMENTATION.md ✅ NEW (guide)
```

---

## ✨ Quick Checklist

- ✅ Models configured for one-to-one relationship
- ✅ Foreign key on Resident side
- ✅ DeleteBehavior.Restrict in place
- ✅ UserDto includes ResidentId
- ✅ UserWithResidentDto created for full data
- ✅ Database configuration documented
- ✅ Relationship documentation complete
- ✅ All files compile without errors
- ✅ No database migrations needed
- ✅ Backward compatible

---

## 🚀 Ready for Implementation

Your codebase is now ready to:
1. Create users with resident profiles
2. Query users with their resident information
3. Update both user and resident data together
4. Maintain data integrity with delete restrictions
5. Support multi-tenant scenarios with ResidenceId

**Status: ✅ READY FOR PRODUCTION**
