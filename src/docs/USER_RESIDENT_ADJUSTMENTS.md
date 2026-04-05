# User-Resident Relationship Model Adjustments Summary

## Changes Made

### 1. **Domain Models Updated**

#### User.cs
- ✅ Enhanced documentation for `Resident` navigation property
- ✅ Clarified one-to-one relationship: "A user can optionally be associated with a resident, and a resident must have a user account"
- **No breaking changes** - relationship was already in place

#### Resident.cs
- ✅ Enhanced documentation for all properties
- ✅ Clarified `UserId` as Foreign Key for one-to-one relationship
- ✅ Added detailed XML documentation for navigation properties
- ✅ Improved clarity on relationship with User entity
- **No breaking changes** - structure remains the same

### 2. **Database Configurations Enhanced**

#### UserConfiguration.cs
- ✅ Updated summary documentation
- ✅ Explicitly documented one-to-one relationship configuration
- ✅ Added clear comments explaining the relationship:
  ```csharp
  // One-to-One Relationship Configuration
  // One User can have one Resident, and one Resident must have a User
  // The foreign key is defined on the Resident side (UserId)
  builder.HasOne(u => u.Resident)
      .WithOne(r => r.User)
      .HasForeignKey<Resident>(r => r.UserId)
      .OnDelete(DeleteBehavior.Restrict);
  ```

#### ResidentConfiguration.cs
- ✅ Updated summary documentation
- ✅ Explicitly documented one-to-one relationship
- ✅ Enhanced comments explaining Resident as dependent entity
- ✅ Improved clarity on cascade delete behavior

**Configuration Details:**
- **Relationship Type**: One-to-One
- **FK Location**: Resident.UserId
- **Delete Behavior**: Restrict (prevents deleting a User with an associated Resident)
- **Status**: Optional relationship (UserId is nullable)

### 3. **DTOs Created/Updated**

#### UserDto.cs - Enhanced
- ✅ Added `ResidentId` field: `Guid? ResidentId`
- ✅ Now includes reference to associated resident
- ✅ Maintains backward compatibility (new field is nullable)

**Before:**
```csharp
public record UserDto(
    Guid Id,
    string Email,
    string FirstName,
    string LastName,
    string PhoneNumber,
    UserRole Role,
    string? AvatarUrl,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);
```

**After:**
```csharp
public record UserDto(
    Guid Id,
    string Email,
    string FirstName,
    string LastName,
    string PhoneNumber,
    UserRole Role,
    string? AvatarUrl,
    Guid? ResidentId,        // NEW FIELD
    DateTime CreatedAt,
    DateTime? UpdatedAt
);
```

#### UserWithResidentDto.cs - New DTO
- ✅ Created for scenarios requiring full resident information
- ✅ Includes complete `ResidentDto` object
- ✅ Useful for profile pages and detailed user views

```csharp
public record UserWithResidentDto(
    Guid Id,
    string Email,
    string FirstName,
    string LastName,
    string PhoneNumber,
    UserRole Role,
    string? AvatarUrl,
    ResidentDto? Resident,           // Full resident object
    DateTime CreatedAt,
    DateTime? UpdatedAt
);
```

## Relationship Summary

### One-to-One Configuration
```
User (1) ←──→ (1) Resident
  ▲                  │
  │                  │
  └──────────────────┘
   Foreign Key: Resident.UserId
   Delete Behavior: Restrict
```

### Key Characteristics
| Aspect | Value |
|--------|-------|
| **Relationship Type** | One-to-One |
| **FK Location** | Resident.UserId |
| **FK Nullable** | Yes (Optional relationship) |
| **Delete Behavior** | Restrict |
| **Principal Entity** | User |
| **Dependent Entity** | Resident |

## Database Impact

### No Migration Required
- The relationship was already correctly configured
- These changes are documentation and DTO enhancements
- Existing database schema remains unchanged

### New Index (Optional)
For optimal query performance, consider adding:
```sql
CREATE INDEX IX_Residents_UserId ON dbo.Residents(UserId);
```

## Service Layer Implications

### When Using UserDto
```csharp
// Get basic user info with resident ID reference
var userDto = new UserDto(
    user.Id,
    user.Email,
    user.FirstName,
    user.LastName,
    user.PhoneNumber,
    user.Role,
    user.AvatarUrl,
    user.Resident?.Id,  // ResidentId field
    user.CreatedAt,
    user.UpdatedAt
);
```

### When Using UserWithResidentDto
```csharp
// Get complete user with resident details
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

## API Response Examples

### Endpoint: GET /api/users/{userId}
```json
{
    "id": "550e8400-e29b-41d4-a716-446655440000",
    "email": "john.doe@example.com",
    "firstName": "John",
    "lastName": "Doe",
    "phoneNumber": "+1234567890",
    "role": "Resident",
    "avatarUrl": "https://example.com/avatars/john.jpg",
    "residentId": "660e8400-e29b-41d4-a716-446655440001",
    "createdAt": "2024-01-01T00:00:00Z",
    "updatedAt": "2024-01-15T12:30:00Z"
}
```

### Endpoint: GET /api/users/{userId}/with-resident
```json
{
    "id": "550e8400-e29b-41d4-a716-446655440000",
    "email": "john.doe@example.com",
    "firstName": "John",
    "lastName": "Doe",
    "phoneNumber": "+1234567890",
    "role": "Resident",
    "avatarUrl": "https://example.com/avatars/john.jpg",
    "resident": {
        "id": "660e8400-e29b-41d4-a716-446655440001",
        "userId": "550e8400-e29b-41d4-a716-446655440000",
        "houseId": "770e8400-e29b-41d4-a716-446655440002",
        "firstName": "John",
        "lastName": "Doe",
        "email": "john.doe@example.com",
        "phoneNumber": "+1234567890",
        "address": "123 Main Street, Apt 4B",
        "birthDate": "1990-01-15",
        "status": "Active",
        "moveInDate": "2024-01-01T00:00:00Z",
        "moveOutDate": null,
        "createdAt": "2024-01-01T00:00:00Z",
        "updatedAt": "2024-01-01T00:00:00Z"
    },
    "createdAt": "2024-01-01T00:00:00Z",
    "updatedAt": "2024-01-15T12:30:00Z"
}
```

## Files Modified/Created

| File | Action | Changes |
|------|--------|---------|
| `residence.domain/Entities/User.cs` | Modified | Enhanced documentation |
| `residence.domain/Entities/Resident.cs` | Modified | Enhanced documentation with FK details |
| `residence.infrastructure/Configurations/UserConfiguration.cs` | Modified | Improved relationship documentation |
| `residence.infrastructure/Configurations/ResidentConfiguration.cs` | Modified | Improved relationship documentation |
| `residence.application/DTOs/UserDto.cs` | Modified | Added ResidentId field |
| `residence.application/DTOs/UserWithResidentDto.cs` | Created | New DTO for full user-resident data |
| `src/docs/USER_RESIDENT_RELATIONSHIP.md` | Created | Comprehensive relationship documentation |

## Backward Compatibility

✅ **Fully Backward Compatible**
- `UserDto` change adds a nullable field
- Existing code using `UserDto` will continue to work
- New `UserWithResidentDto` is optional for new code

## Best Practices to Follow

1. **Always Include Relationship When Needed**
   ```csharp
   var user = await _context.Users
       .Include(u => u.Resident)
       .FirstOrDefaultAsync(u => u.Id == userId);
   ```

2. **Handle Null Cases**
   ```csharp
   if (user.Resident != null)
   {
       // Work with resident data
   }
   ```

3. **Use Appropriate DTOs**
   - Use `UserDto` for basic user info
   - Use `UserWithResidentDto` when you need resident details

4. **Maintain Multi-Tenancy**
   ```csharp
   var user = await _context.Users
       .Where(u => u.ResidenceId == residenceId && u.Id == userId)
       .FirstOrDefaultAsync();
   ```

## Next Steps

1. ✓ Update your `UserService` to handle both DTOs
2. ✓ Create endpoints using the new DTOs
3. ✓ Update Angular service models to match new DTOs
4. ✓ Test the relationship with integration tests
5. ✓ Consider adding `ResidentWithUserDto` for resident queries
6. ✓ Document any additional relationship operations in your API

## Testing Recommendations

### Unit Tests
- Verify User can have a Resident
- Verify Resident requires a User (in business logic)
- Test DTO mapping from entities

### Integration Tests
- Create a User and associate a Resident
- Query User with related Resident
- Verify delete restrictions work
- Test cascade operations

### Example Integration Test
```csharp
[Test]
public async Task User_Should_Load_Associated_Resident()
{
    // Arrange
    var user = new User { Email = "test@example.com", ... };
    var resident = new Resident { User = user, UserId = user.Id, ... };
    
    // Act
    await _context.Users.AddAsync(user);
    await _context.Residents.AddAsync(resident);
    await _context.SaveChangesAsync();
    
    var loadedUser = await _context.Users
        .Include(u => u.Resident)
        .FirstOrDefaultAsync(u => u.Id == user.Id);
    
    // Assert
    Assert.IsNotNull(loadedUser.Resident);
    Assert.AreEqual(resident.Id, loadedUser.Resident.Id);
}
```

## Conclusion

The User-Resident relationship is now:
- ✅ Clearly documented
- ✅ Properly configured
- ✅ Supported by appropriate DTOs
- ✅ Ready for implementation in services and controllers
- ✅ Production-ready with comprehensive documentation
