# Update User Information API

## Overview

This API allows users to update their profile information, including updating their resident association via `ResidentId`.

## Endpoints

### Update User Profile

**Endpoint:** `PUT /api/users/{userId}`

**Authentication:** ✅ Required (Bearer Token)

**Headers:**
```
Authorization: Bearer {accessToken}
Content-Type: application/json
```

**Request:**
```json
{
  "firstName": "John",
  "lastName": "Doe",
  "phoneNumber": "+1234567890",
  "avatarUrl": "https://example.com/avatars/john.jpg",
  "residentId": "660e8400-e29b-41d4-a716-446655440001"
}
```

**Request Parameters:**
- **firstName** (string, required): User's first name
- **lastName** (string, required): User's last name
- **phoneNumber** (string, required): User's phone number
- **avatarUrl** (string?, optional): URL to user's avatar/profile picture
- **residentId** (guid?, optional): ID of associated resident profile

**Response (200 OK):**
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
  "updatedAt": "2024-01-20T15:45:30Z"
}
```

**Error Responses:**

**404 Not Found:**
```json
{
  "message": "User not found"
}
```

**400 Bad Request:**
```json
{
  "message": "Invalid request data",
  "errors": {
    "firstName": ["First name is required"],
    "lastName": ["Last name is required"]
  }
}
```

**401 Unauthorized:**
```json
{
  "message": "Invalid or missing authentication token"
}
```

---

## Database Changes

### New Relationship Structure

**Before:**
```
Users Table                     Residents Table
├─ Id (PK)                      ├─ Id (PK)
├─ Email                        ├─ UserId (FK) ← Foreign Key here
├─ FirstName                    ├─ FirstName
├─ LastName                     ├─ LastName
└─ ...                          └─ ...
```

**After:**
```
Users Table                     Residents Table
├─ Id (PK)                      ├─ Id (PK)
├─ Email                        ├─ FirstName
├─ FirstName                    ├─ LastName
├─ LastName                     ├─ HouseId (FK)
├─ ResidentId (FK) ← FK moved   └─ ...
└─ ...
```

### Migration Required

```sql
-- Add ResidentId column to Users table
ALTER TABLE dbo.Users
ADD ResidentId UNIQUEIDENTIFIER NULL;

-- Add foreign key constraint from Users to Residents
ALTER TABLE dbo.Users
ADD CONSTRAINT FK_Users_Residents_ResidentId
FOREIGN KEY (ResidentId) REFERENCES dbo.Residents(Id)
ON DELETE SET NULL;

-- Create index for query performance
CREATE INDEX IX_Users_ResidentId ON dbo.Users(ResidentId);

-- Drop old foreign key from Residents table (if migrating from old structure)
-- ALTER TABLE dbo.Residents
-- DROP CONSTRAINT FK_Residents_Users_UserId;

-- Drop UserId column from Residents (if migrating)
-- ALTER TABLE dbo.Residents
-- DROP COLUMN UserId;
```

---

## Entity Relationship

```
┌─────────────────────────────────────────────────────────┐
│         USER-RESIDENT RELATIONSHIP (UPDATED)            │
├─────────────────────────────────────────────────────────┤
│                                                         │
│  USERS TABLE (Principal Entity)                        │
│  ├─ Id (PK)                                            │
│  ├─ Email (Unique)                                     │
│  ├─ PasswordHash                                       │
│  ├─ FirstName                                          │
│  ├─ LastName                                           │
│  ├─ PhoneNumber                                        │
│  ├─ Role                                               │
│  ├─ AvatarUrl                                          │
│  ├─ ResidentId (FK) ← Foreign Key (NEW LOCATION)      │
│  ├─ ResidenceId                                        │
│  └─ CreatedAt, UpdatedAt, IsDeleted                    │
│       │                                                │
│       │ One-to-One Relationship                       │
│       │ (FK on User side)                             │
│       │                                                │
│       ▼                                                │
│  RESIDENTS TABLE (Dependent Entity)                    │
│  ├─ Id (PK)                                            │
│  ├─ FirstName                                          │
│  ├─ LastName                                           │
│  ├─ Email                                              │
│  ├─ PhoneNumber                                        │
│  ├─ Address                                            │
│  ├─ BirthDate                                          │
│  ├─ Status                                             │
│  ├─ HouseId (FK)                                       │
│  ├─ MoveInDate                                         │
│  ├─ MoveOutDate                                        │
│  ├─ ResidenceId                                        │
│  └─ CreatedAt, UpdatedAt, IsDeleted                    │
│                                                         │
│  Cardinality: User(1) ←→ (1) Resident                 │
│  FK Column: Users.ResidentId                           │
│  Delete Behavior: SetNull                              │
│                                                         │
└─────────────────────────────────────────────────────────┘
```

---

## Implementation Examples

### C# Backend

#### UserService
```csharp
public async Task<UserDto> UpdateUserAsync(Guid id, UpdateUserDto dto)
{
    var user = await _userRepository.GetByIdAsync(id);
    if (user == null)
        throw new Exception("User not found");

    user.FirstName = dto.FirstName;
    user.LastName = dto.LastName;
    user.PhoneNumber = dto.PhoneNumber;
    user.AvatarUrl = dto.AvatarUrl;
    
    // Update resident association
    if (dto.ResidentId.HasValue)
    {
        user.ResidentId = dto.ResidentId.Value;
    }
    
    user.UpdatedAt = DateTime.UtcNow;
    await _userRepository.UpdateAsync(user);

    return MapToDto(user);
}
```

#### User Endpoint
```csharp
[HttpPut("{userId}")]
[Authorize]
public async Task<IResult> UpdateUser(
    Guid userId,
    IUserService userService,
    UpdateUserDto dto)
{
    try
    {
        var result = await userService.UpdateUserAsync(userId, dto);
        return Results.Ok(result);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { message = ex.Message });
    }
}
```

### Angular Frontend

#### TypeScript Service
```typescript
updateUser(userId: string, dto: UpdateUserDto): Observable<UserDto> {
  return this.http.put<UserDto>(
    `${environment.apiUrl}/api/users/${userId}`,
    dto
  ).pipe(
    tap(response => {
      // Update user in state/localStorage
      this.setCurrentUser(response);
    }),
    catchError(error => {
      console.error('Update failed:', error);
      return throwError(() => error);
    })
  );
}
```

#### Update Component
```typescript
onUpdateProfile(): void {
  if (this.profileForm.invalid) {
    return;
  }

  const updateDto: UpdateUserDto = {
    firstName: this.profileForm.value.firstName,
    lastName: this.profileForm.value.lastName,
    phoneNumber: this.profileForm.value.phoneNumber,
    avatarUrl: this.profileForm.value.avatarUrl,
    residentId: this.profileForm.value.residentId
  };

  this.userService.updateUser(this.userId, updateDto).subscribe({
    next: (response) => {
      this.showSuccessMessage('Profile updated successfully');
    },
    error: (error) => {
      this.showErrorMessage('Failed to update profile');
    }
  });
}
```

---

## Use Cases

### 1. Update User Profile
```json
{
  "firstName": "Jane",
  "lastName": "Smith",
  "phoneNumber": "+1987654321",
  "avatarUrl": "https://example.com/avatars/jane.jpg"
}
```

### 2. Associate User with Resident
```json
{
  "firstName": "John",
  "lastName": "Doe",
  "phoneNumber": "+1234567890",
  "residentId": "660e8400-e29b-41d4-a716-446655440001"
}
```

### 3. Dissociate User from Resident
```json
{
  "firstName": "John",
  "lastName": "Doe",
  "phoneNumber": "+1234567890",
  "residentId": null
}
```

---

## Validation Rules

- **firstName**: Required, max 100 characters
- **lastName**: Required, max 100 characters
- **phoneNumber**: Required, max 20 characters
- **avatarUrl**: Optional, max 500 characters
- **residentId**: Optional, must reference valid Resident ID if provided

---

## Security Notes

✅ **Authentication Required**: All update operations require valid JWT token

✅ **Authorization**: Users can only update their own profile (or admins can update any user)

✅ **Multi-Tenancy**: Users can only update profiles within their residence

✅ **Audit Trail**: All updates are logged with timestamps

---

## Related DTOs

### UpdateUserDto
```csharp
public record UpdateUserDto(
    string FirstName,
    string LastName,
    string PhoneNumber,
    string? AvatarUrl = null,
    Guid? ResidentId = null
);
```

### UserDto (Response)
```csharp
public record UserDto(
    Guid Id,
    string Email,
    string FirstName,
    string LastName,
    string PhoneNumber,
    UserRole Role,
    string? AvatarUrl,
    Guid? ResidentId,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);
```

---

## Status

✅ **Complete and Ready for Implementation**

**File Location:** `residence.api/Endpoints/UserEndpoints.cs`

**Next Steps:**
1. Create migration for database schema change
2. Implement endpoint in UserEndpoints
3. Update frontend service
4. Test update flow
5. Deploy migration
