# Migration Guide: User-Resident FK Relationship Restructure

## Overview

This guide documents the migration from having the Foreign Key on the Resident side to the User side.

**Before:**
- `Resident.UserId` (FK) → `User.Id`

**After:**
- `User.ResidentId` (FK) → `Resident.Id`

---

## Step 1: Create Migration

Run the following command in the Package Manager Console:

```powershell
Add-Migration MoveUserResidentForeignKeyToUserSide -Project residence.infrastructure
```

Or using .NET CLI:

```bash
dotnet ef migrations add MoveUserResidentForeignKeyToUserSide -p residence.infrastructure
```

---

## Step 2: Review Generated Migration

The migration file should be created at: `residence.infrastructure/Migrations/[timestamp]_MoveUserResidentForeignKeyToUserSide.cs`

It should contain something like:

```csharp
public partial class MoveUserResidentForeignKeyToUserSide : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        // Drop old foreign key from Residents
        migrationBuilder.DropForeignKey(
            name: "FK_Residents_Users_UserId",
            table: "Residents");

        // Drop old index
        migrationBuilder.DropIndex(
            name: "IX_Residents_UserId",
            table: "Residents");

        // Drop UserId column from Residents
        migrationBuilder.DropColumn(
            name: "UserId",
            table: "Residents");

        // Add ResidentId column to Users
        migrationBuilder.AddColumn<Guid>(
            name: "ResidentId",
            table: "Users",
            type: "uniqueidentifier",
            nullable: true);

        // Create index on ResidentId
        migrationBuilder.CreateIndex(
            name: "IX_Users_ResidentId",
            table: "Users",
            column: "ResidentId",
            unique: true);

        // Add new foreign key
        migrationBuilder.AddForeignKey(
            name: "FK_Users_Residents_ResidentId",
            table: "Users",
            column: "ResidentId",
            principalTable: "Residents",
            principalColumn: "Id",
            onDelete: ReferentialAction.SetNull);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        // Reverse the migration
        migrationBuilder.DropForeignKey(
            name: "FK_Users_Residents_ResidentId",
            table: "Users");

        migrationBuilder.DropIndex(
            name: "IX_Users_ResidentId",
            table: "Users");

        migrationBuilder.DropColumn(
            name: "ResidentId",
            table: "Users");

        migrationBuilder.AddColumn<Guid>(
            name: "UserId",
            table: "Residents",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.CreateIndex(
            name: "IX_Residents_UserId",
            table: "Residents",
            column: "UserId",
            unique: true);

        migrationBuilder.AddForeignKey(
            name: "FK_Residents_Users_UserId",
            table: "Residents",
            column: "UserId",
            principalTable: "Users",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);
    }
}
```

---

## Step 3: Apply Migration

Execute the migration to update the database:

```powershell
Update-Database -Project residence.infrastructure
```

Or using .NET CLI:

```bash
dotnet ef database update -p residence.infrastructure
```

---

## Step 4: Verify Migration

Check the database to confirm:

**Users Table Changes:**
```sql
-- Should have new ResidentId column
SELECT * FROM Users;

-- Should have index on ResidentId
SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID('Users');
```

**Residents Table Changes:**
```sql
-- Should NO LONGER have UserId column
SELECT * FROM Residents;

-- Old index should be gone
SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID('Residents');
```

---

## Step 5: Update Application Code

All code changes have already been made:

### ✅ Domain Models Updated
- **User.cs**: Added `ResidentId` property
- **Resident.cs**: Removed `UserId` property

### ✅ Configurations Updated
- **UserConfiguration**: FK on User side
- **ResidentConfiguration**: Updated relationship

### ✅ DTOs Updated
- **UpdateUserDto**: Added `ResidentId` parameter

### ✅ Services Updated
- **UserService**: Handles ResidentId updates

### ✅ Endpoints Created
- **UserEndpoints**: New endpoints for user management

### ✅ Program.cs Updated
- User endpoints mapped

---

## Step 6: Data Migration (If Needed)

If you have existing data, you may need to populate the `ResidentId` in Users:

```sql
-- Migrate data from Residents.UserId to Users.ResidentId
-- (Only if you have existing data in the old structure)

-- First, check for any issues
SELECT 
    r.Id,
    r.UserId,
    u.Id as UserRecordId
FROM Residents r
LEFT JOIN Users u ON r.UserId = u.Id;

-- If the above query shows data, manually update Users:
-- UPDATE Users u
-- SET u.ResidentId = (SELECT r.Id FROM Residents r WHERE r.UserId = u.Id)
-- WHERE EXISTS (SELECT 1 FROM Residents r WHERE r.UserId = u.Id);
```

---

## Step 7: Test the Changes

### Test 1: Update User with ResidentId

```csharp
// Test updating user with resident association
var updateDto = new UpdateUserDto(
    "John",
    "Doe",
    "+1234567890",
    "https://example.com/avatar.jpg",
    residentId: new Guid("660e8400-e29b-41d4-a716-446655440001")
);

var result = await userService.UpdateUserAsync(userId, updateDto);
Assert.IsNotNull(result.ResidentId);
```

### Test 2: Load User with Resident

```csharp
// Test loading user with related resident
var user = await _context.Users
    .Include(u => u.Resident)
    .FirstOrDefaultAsync(u => u.Id == userId);

Assert.IsNotNull(user);
Assert.IsNotNull(user.Resident);
```

### Test 3: API Endpoint

```bash
# Test the update user endpoint
curl -X PUT "https://localhost:5001/api/users/{userId}" \
  -H "Authorization: Bearer {token}" \
  -H "Content-Type: application/json" \
  -d '{
    "firstName": "Jane",
    "lastName": "Smith",
    "phoneNumber": "+1987654321",
    "avatarUrl": "https://example.com/avatar2.jpg",
    "residentId": "660e8400-e29b-41d4-a716-446655440001"
  }'
```

---

## Troubleshooting

### Issue: Foreign Key Constraint Violation

**Problem:** Cannot apply migration due to FK constraint violation

**Solution:**
1. Check for orphaned records (Users with non-existent ResidentIds)
2. Manually clean up data
3. Run migration again

```sql
-- Find orphaned relationships
SELECT u.Id, u.ResidentId
FROM Users u
LEFT JOIN Residents r ON u.ResidentId = r.Id
WHERE r.Id IS NULL AND u.ResidentId IS NOT NULL;

-- Remove orphaned data
DELETE FROM Users 
WHERE ResidentId IS NOT NULL 
AND ResidentId NOT IN (SELECT Id FROM Residents);
```

### Issue: Rollback Migration

If you need to rollback:

```powershell
# Using Package Manager Console
Update-Database -Migration [PreviousMigrationName] -Project residence.infrastructure

# Using .NET CLI
dotnet ef database update [PreviousMigrationName] -p residence.infrastructure
```

---

## Code Changes Summary

### Files Modified

| File | Change |
|------|--------|
| `residence.domain/Entities/User.cs` | Added `ResidentId` property |
| `residence.domain/Entities/Resident.cs` | Removed `UserId` property |
| `residence.infrastructure/Configurations/UserConfiguration.cs` | Updated relationship config |
| `residence.infrastructure/Configurations/ResidentConfiguration.cs` | Updated relationship config |
| `residence.application/DTOs/UpdateUserDto.cs` | Added `ResidentId` parameter |
| `residence.application/Services/UserService.cs` | Handle ResidentId updates |
| `residence.api/Endpoints/UserEndpoints.cs` | New file - User endpoints |
| `residence.api/Program.cs` | Added UserEndpoints mapping |

### Files Created

| File | Purpose |
|------|---------|
| `src/docs/UPDATE_USER_API.md` | API documentation |

---

## Verification Checklist

- [ ] Migration created successfully
- [ ] Migration applied to database
- [ ] Users table has `ResidentId` column
- [ ] Residents table no longer has `UserId` column
- [ ] Foreign key constraint exists on Users.ResidentId
- [ ] Index created on Users.ResidentId
- [ ] Code compiles without errors
- [ ] Tests pass
- [ ] API endpoints working
- [ ] User update with ResidentId works
- [ ] Load user with resident works
- [ ] No orphaned data in database

---

## Production Deployment

### Pre-Deployment Checklist

- [ ] Backup database
- [ ] Test migration on staging environment
- [ ] Review rollback plan
- [ ] Notify team of database changes
- [ ] Schedule maintenance window if needed

### Deployment Steps

1. Create backup of production database
2. Deploy new code version
3. Run migration: `dotnet ef database update`
4. Monitor logs for errors
5. Run smoke tests
6. Monitor application health

### Post-Deployment

- [ ] Verify all users can log in
- [ ] Verify user profile updates work
- [ ] Check database integrity
- [ ] Monitor for any errors
- [ ] Document any issues

---

## Rollback Plan

If the migration causes issues:

1. Stop the application
2. Rollback the migration:
   ```bash
   dotnet ef database update [PreviousMigrationName]
   ```
3. Revert code to previous version
4. Restart application
5. Investigate root cause
6. Fix and redeploy

---

## Performance Impact

**Positive:**
- ✅ Queries loading Users with Resident will be slightly faster (FK on accessed side)
- ✅ Index on Users.ResidentId improves lookup performance

**Negative:**
- ❌ None significant

---

## Related Documentation

- `src/docs/UPDATE_USER_API.md` - API endpoint documentation
- `src/docs/USER_RESIDENT_RELATIONSHIP.md` - Relationship documentation
- `src/docs/AUTHENTICATION.md` - Authentication with user system

---

## Timeline

| Step | Estimated Time |
|------|----------------|
| Create Migration | 5 minutes |
| Review Migration | 5 minutes |
| Apply Migration | 2-5 minutes |
| Update Code | Already done ✅ |
| Test Changes | 15-30 minutes |
| Deploy | 10-20 minutes |
| **Total** | **45-75 minutes** |

---

## Support

For questions or issues:
1. Check troubleshooting section above
2. Review migration file
3. Check database schema
4. Review entity configurations
5. Check application logs

---

**Status:** Ready for deployment ✅
**Last Updated:** 2024
