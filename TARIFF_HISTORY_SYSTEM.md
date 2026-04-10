# Tariff (Tarif) History Tracking System

## Overview

This document describes the tariff history tracking system implemented in the Residence application. This system maintains a complete audit trail of all tariff changes in each residence, allowing administrators and managers to view the history of rate changes over time.

## Features

### 1. **Current Tariff Management**
- Each residence can have one active tariff at a time
- When a new tariff is created, the previous one is automatically marked as inactive
- Track effective dates and end dates for each tariff

### 2. **Automatic History Tracking**
- Every time a tariff's amount or description changes, a history entry is automatically created
- Records the previous and new values
- Tracks who made the change and when
- Optional reason for the change can be recorded

### 3. **Query Capabilities**
- Get current active tariff for a residence
- View all tariffs (current and historical) for a residence
- View complete change history for a specific tariff
- View all changes for a residence
- Filter changes by date range

## Data Model

### Entities

#### **Tarif**
```csharp
public class Tarif : BaseEntity
{
    public Guid ResidenceId { get; set; }              // Which residence this tariff belongs to
    public string Description { get; set; }            // Tariff description (e.g., "Monthly rent")
    public decimal Amount { get; set; }                // The tariff amount
    public string Currency { get; set; }               // Currency code (e.g., "USD", "EUR")
    public DateTime EffectiveDate { get; set; }        // When this tariff becomes effective
    public DateTime? EndDate { get; set; }             // When this tariff ends (null if current)
    public bool IsActive { get; set; }                 // Whether this is the active tariff
    public string? Notes { get; set; }                 // Additional notes

    // Navigation
    public Residence Residence { get; set; }
    public ICollection<TarifHistory> History { get; set; }
}
```

#### **TarifHistory**
```csharp
public class TarifHistory : BaseEntity
{
    public Guid TarifId { get; set; }                  // Reference to the tariff
    public Guid ResidenceId { get; set; }              // Which residence (for easy querying)
    public decimal PreviousAmount { get; set; }        // Previous amount value
    public decimal NewAmount { get; set; }             // New amount value
    public string PreviousDescription { get; set; }    // Previous description
    public string NewDescription { get; set; }         // New description
    public DateTime EffectiveDate { get; set; }        // When the change is effective
    public string ChangedBy { get; set; }              // User/system that made the change
    public string? ChangeReason { get; set; }          // Why the change was made
    public DateTime ChangedAt { get; set; }            // When the change was recorded

    // Navigation
    public Tarif Tarif { get; set; }
    public Residence Residence { get; set; }
}
```

## API Endpoints

### Create a New Tariff
```
POST /api/residences/{residenceId}/tarifs
Content-Type: application/json

{
  "description": "Monthly maintenance fee",
  "amount": 150.00,
  "currency": "USD",
  "effectiveDate": "2024-03-01T00:00:00Z",
  "notes": "Increased due to inflation"
}
```

**Response:** `201 Created`
```json
{
  "id": "guid",
  "residenceId": "guid",
  "description": "Monthly maintenance fee",
  "amount": 150.00,
  "currency": "USD",
  "effectiveDate": "2024-03-01T00:00:00Z",
  "endDate": null,
  "isActive": true,
  "notes": "Increased due to inflation",
  "createdAt": "2024-03-01T10:30:00Z",
  "updatedAt": null
}
```

### Get Current Active Tariff
```
GET /api/residences/{residenceId}/tarifs/current/active
```

**Response:** `200 OK`
```json
{
  "id": "guid",
  "residenceId": "guid",
  "description": "Monthly maintenance fee",
  "amount": 150.00,
  "currency": "USD",
  "effectiveDate": "2024-03-01T00:00:00Z",
  "endDate": null,
  "isActive": true,
  "notes": "Increased due to inflation",
  "createdAt": "2024-03-01T10:30:00Z",
  "updatedAt": null
}
```

### Get All Tariffs for a Residence
```
GET /api/residences/{residenceId}/tarifs
```

**Response:** `200 OK`
```json
[
  {
    "id": "guid",
    "residenceId": "guid",
    "description": "Monthly maintenance fee",
    "amount": 150.00,
    "currency": "USD",
    "effectiveDate": "2024-03-01T00:00:00Z",
    "endDate": null,
    "isActive": true,
    "notes": "Increased due to inflation"
  },
  {
    "id": "guid",
    "residenceId": "guid",
    "description": "Monthly maintenance fee",
    "amount": 140.00,
    "currency": "USD",
    "effectiveDate": "2024-02-01T00:00:00Z",
    "endDate": "2024-02-29T23:59:59Z",
    "isActive": false,
    "notes": null
  }
]
```

### Get Tariff by ID
```
GET /api/residences/{residenceId}/tarifs/{tarifId}
```

**Response:** `200 OK`

### Update a Tariff
```
PUT /api/residences/{residenceId}/tarifs/{tarifId}
Content-Type: application/json

{
  "description": "Monthly maintenance fee - updated",
  "amount": 160.00,
  "currency": "USD",
  "notes": "Additional services included",
  "changeReason": "Service enhancement"
}
```

**Response:** `200 OK` - Updated tariff with history entry created

### Delete a Tariff (Soft Delete)
```
DELETE /api/residences/{residenceId}/tarifs/{tarifId}
```

**Response:** `204 No Content`

### Get History of a Specific Tariff
```
GET /api/residences/{residenceId}/tarifs/{tarifId}/history
```

**Response:** `200 OK`
```json
[
  {
    "id": "guid",
    "tarifId": "guid",
    "residenceId": "guid",
    "previousAmount": 140.00,
    "newAmount": 150.00,
    "previousDescription": "Monthly maintenance fee",
    "newDescription": "Monthly maintenance fee",
    "effectiveDate": "2024-03-01T00:00:00Z",
    "changedBy": "admin@example.com",
    "changeReason": "Increased due to inflation",
    "changedAt": "2024-02-28T15:00:00Z"
  }
]
```

### Get All Tariff Changes for a Residence
```
GET /api/residences/{residenceId}/tarifs/history/all
```

**Response:** `200 OK` - List of all history entries for the residence

### Get Tariff Changes by Date Range
```
GET /api/residences/{residenceId}/tarifs/history/range?startDate=2024-01-01T00:00:00Z&endDate=2024-03-31T23:59:59Z
```

**Response:** `200 OK` - History entries within the specified date range

## Usage Examples

### Example 1: Creating and Tracking Tariff Changes

1. **Initial Setup** (January 1, 2024)
   - Create tariff: $100/month for maintenance

2. **First Change** (March 1, 2024)
   - Change amount to $110/month (inflation adjustment)
   - Reason: "Annual cost adjustment"
   - System automatically:
     - Marks previous tariff as inactive
     - Sets end date to February 29, 2024
     - Creates a history entry

3. **Second Change** (June 1, 2024)
   - Change amount to $115/month + description update
   - Reason: "Service expansion"
   - New description: "Maintenance + additional services"

4. **Query History**
   - Get current active tariff → Returns $115/month version
   - Get all tariffs → Returns all 3 versions
   - Get all changes → Returns 2 history entries
   - Get changes by date → Filter for changes in Q2 2024

### Example 2: Auditing Tariff Changes

```csharp
// Get all changes for a specific tariff
var history = await tarifService.GetTarifHistoryAsync(tarifId);

foreach (var change in history)
{
    Console.WriteLine($"Date: {change.ChangedAt}");
    Console.WriteLine($"Changed by: {change.ChangedBy}");
    Console.WriteLine($"Amount: {change.PreviousAmount} → {change.NewAmount}");
    Console.WriteLine($"Reason: {change.ChangeReason}");
    Console.WriteLine("---");
}
```

## Database Indexes

The system includes optimized indexes for common queries:

- `IX_Tarif_ResidenceId_IsActive` - Quick lookup of current tariff
- `IX_Tarif_EffectiveDate` - Range queries by date
- `IX_TarifHistory_ResidenceId_ChangedAt` - Residence history queries
- `IX_TarifHistory_TarifId` - Tariff-specific history
- `IX_TarifHistory_ChangedAt` - Global history timeline

## Service Interface (ITarifService)

```csharp
public interface ITarifService
{
    Task<TarifDto> CreateTarifAsync(Guid residenceId, CreateTarifDto dto, string userId);
    Task<TarifDto> UpdateTarifAsync(Guid residenceId, Guid tarifId, UpdateTarifDto dto, string userId);
    Task<TarifDto?> GetTarifByIdAsync(Guid tarifId);
    Task<IEnumerable<TarifDto>> GetTarifsByResidenceAsync(Guid residenceId);
    Task<TarifDto?> GetCurrentTarifAsync(Guid residenceId);
    Task<IEnumerable<TarifHistoryDto>> GetTarifHistoryAsync(Guid tarifId);
    Task<IEnumerable<TarifHistoryDto>> GetResidenceTarifHistoryAsync(Guid residenceId);
    Task<IEnumerable<TarifHistoryDto>> GetTarifHistoryByDateRangeAsync(Guid residenceId, DateTime startDate, DateTime endDate);
    Task<bool> DeleteTarifAsync(Guid residenceId, Guid tarifId);
}
```

## Implementation Details

### Automatic History Recording
When a tariff is updated via `UpdateTarifAsync`:
1. The service checks if amount or description has changed
2. If changed, a new `TarifHistory` entry is created
3. The entry captures previous and new values
4. The user ID and optional reason are recorded
5. The timestamp is automatically set to UTC now

### Deactivation of Previous Tariffs
When creating a new tariff:
1. The system finds the current active tariff
2. Marks it as `IsActive = false`
3. Sets `EndDate` to the day before the new tariff's effective date
4. The new tariff is set as `IsActive = true`

### Soft Deletes
Both `Tarif` and `TarifHistory` support soft deletes:
- Records are not physically deleted from the database
- `IsDeleted` flag is set to true
- Queries automatically exclude deleted records

## Future Enhancements

1. **Notifications** - Notify residents of tariff changes
2. **Approval Workflow** - Require approval for tariff changes
3. **Bulk Updates** - Apply tariff changes to multiple residences
4. **Export** - Export tariff history to PDF/Excel
5. **Forecasting** - Project future costs based on historical tariff trends
6. **Comparisons** - Compare tariffs across residences

## Migration Notes

The database migration has been included in the project. When you next run:
```
dotnet ef database update
```

It will automatically create the `Tarifs` and `TarifHistories` tables with proper indexes and relationships.

## Files Added/Modified

### New Files Created
- `residence.domain\Entities\Tarif.cs`
- `residence.domain\Entities\TarifHistory.cs`
- `residence.application\DTOs\TarifDto.cs`
- `residence.application\Repositories\ITarifRepository.cs`
- `residence.application\Interfaces\ITarifService.cs`
- `residence.application\Services\TarifService.cs`
- `residence.api\Endpoints\TarifEndpoints.cs`
- `residence.infrastructure\Repositories\TarifRepository.cs`
- `residence.infrastructure\Repositories\TarifHistoryRepository.cs`
- `residence.infrastructure\Configurations\TarifConfiguration.cs`
- `residence.infrastructure\Configurations\TarifHistoryConfiguration.cs`

### Modified Files
- `residence.domain\Entities\Residence.cs` - Added navigation properties
- `residence.api\Program.cs` - Registered new endpoints
- `residence.application\Extensions\ServiceCollectionExtensions.cs` - Registered service
- `residence.infrastructure\Extensions\ServiceCollectionExtensions.cs` - Registered repositories
- `residence.infrastructure\Data\ApplicationDbContext.cs` - Added DbSets and configurations
