# Tariff Update with Automatic Rappel Detection

## Overview

This document describes the enhanced `UpdateTarifAsync` method that automatically detects and creates rappels when a tariff amount is updated and increased.

## Feature Description

When an administrator updates an existing tariff (especially increasing the amount), the system now automatically:

1. **Detects the amount change** - Identifies if the tariff amount has been increased
2. **Identifies pre-paid months** - Finds houses with payments that cover periods after the effective date
3. **Calculates the delta** - Computes the difference between old and new tariff amounts
4. **Creates rappel records** - Generates retroactive payment records for the difference
5. **Prevents duplicates** - Ensures no duplicate rappels are created for the same house

## Technical Implementation

### Modified Method: `UpdateTarifAsync`

**Location:** `residence.application\Services\TarifService.cs`

```csharp
public async Task<TarifDto> UpdateTarifAsync(Guid residenceId, Guid tarifId, 
    UpdateTarifDto dto, string userId)
```

**Key Changes:**

1. **Track old amount** - Stores the current tariff amount before update
2. **Detect amount change** - Checks if new amount is different and greater
3. **Call rappel detection** - Only triggers when amount increases
4. **Pass effective date** - Uses `EffectiveDate` from DTO or defaults to current UTC time

### Business Logic

#### 1. Amount Change Detection
```csharp
var oldAmount = tarif.Amount;
var amountChanged = dto.Amount.HasValue && dto.Amount != tarif.Amount;

// Only detect rappels if amount increased
if (amountChanged && dto.Amount > oldAmount)
{
    // Trigger rappel detection
}
```

#### 2. History Recording
History is recorded when:
- Amount changes (increased or decreased), OR
- Description changes

The history includes:
- Previous and new amounts
- Previous and new descriptions
- Effective date (from DTO or current UTC)
- Change reason
- Changed by (user ID)

#### 3. Rappel Detection Trigger

Calls `DetectAndCreateRappelsAsync` with:
- **Old tariff**: Contains the previous amount
- **New tariff**: The updated tariff entity
- **Effective date**: When the new rate becomes effective

```csharp
if (amountChanged && dto.Amount > oldAmount)
{
    var oldTarif = new Tarif { Amount = oldAmount, Currency = tarif.Currency };
    var effectiveDate = dto.EffectiveDate ?? DateTime.UtcNow;

    await DetectAndCreateRappelsAsync(residenceId, oldTarif, tarif, effectiveDate);
}
```

## Rappel Detection Algorithm

### Input Requirements

- **Residence ID**: Identifies the residence being updated
- **Old Tariff**: Previous tariff amount and currency
- **New Tariff**: Updated tariff amount and currency
- **Effective Date**: When new rate takes effect

### Processing Steps

#### Step 1: Retrieve Houses
```csharp
var houses = await _houseRepository.GetByResidenceWithDetailsAsync(residenceId);
```

Fetches all houses in the residence to check for pre-paid months.

#### Step 2: Find Pre-Paid Months for Each House
```csharp
var prePaidMonths = payments
    .Where(p => p.PeriodEnd >= effectiveDate && 
           p.Status == PaymentStatus.Paid)
    .ToList();
```

Filters payments where:
- **PeriodEnd >= effectiveDate**: Payment covers periods from the new rate's effective date onwards
- **Status = Paid**: Payment has been received (confirmed pre-payment)

#### Step 3: Calculate Affected Month Count
```csharp
var affectedMonthCount = 0;

foreach (var payment in prePaidMonths)
{
    var paymentStart = payment.PeriodStart < effectiveDate 
        ? effectiveDate 
        : payment.PeriodStart;

    var monthsInPayment = ((payment.PeriodEnd.Year - paymentStart.Year) * 12) + 
                         (payment.PeriodEnd.Month - paymentStart.Month) + 1;

    affectedMonthCount += monthsInPayment;
}
```

**Month Calculation Logic:**
- If payment starts before effective date, count from effective date
- If payment starts on or after effective date, count from payment start
- Formula: `((YearEnd - YearStart) * 12) + (MonthEnd - MonthStart) + 1`
- Includes both start and end months

**Examples:**

| Period | Calculation | Months |
|--------|-------------|--------|
| Jan 1 - Jan 31 | 0 * 12 + 0 + 1 | 1 |
| Jan 1 - Feb 28 | 0 * 12 + 1 + 1 | 2 |
| Jan 1 - Dec 31 | 0 * 12 + 11 + 1 | 12 |
| Jan 2024 - Jan 2025 | 1 * 12 + 0 + 1 | 13 |

#### Step 4: Calculate Delta (Difference)
```csharp
var delta = newTarif.Amount - oldTarif.Amount;
```

**Condition:** Only proceeds if `delta > 0` (tariff increased)

#### Step 5: Check for Existing Unpaid Rappels
```csharp
var existingRappels = await _rappelRepository.GetByHouseAsync(house.Id);
var hasUnpaidRappel = existingRappels.Any(r => r.Status == RappelStatus.Unpaid);

if (!hasUnpaidRappel)
{
    // Create new rappel
}
```

**Duplicate Prevention:** Only creates a new rappel if no unpaid rappel exists for that house.

#### Step 6: Create Rappel Record
```csharp
var rappelAmount = delta * affectedMonthCount;

var rappel = new Rappel
{
    HouseId = house.Id,
    Amount = rappelAmount,
    Status = RappelStatus.Unpaid,
    Notes = $"Rappel créé suite à l'augmentation du tarif du {effectiveDate:dd/MM/yyyy}. " +
           $"Ancien tarif: {oldTarif.Amount} {oldTarif.Currency}, " +
           $"Nouveau tarif: {newTarif.Amount} {newTarif.Currency}. " +
           $"Nombre de mois pré-payés affectés: {affectedMonthCount}"
};

await _rappelRepository.AddAsync(rappel);
```

**Rappel Amount = Delta × Affected Months**

**Example:**
- Old tariff: $100
- New tariff: $120
- Delta: $20
- Affected pre-paid months: 3
- Rappel amount: $20 × 3 = $60

### Output

After processing all houses:
```csharp
await _rappelRepository.SaveChangesAsync();
```

All rappel records are persisted to the database.

## API Endpoint

### Update Tariff

```
PUT /api/residences/{residenceId}/tarifs/{tarifId}
```

**Request Body:**
```json
{
  "amount": 120.00,
  "effectiveDate": "2024-01-15T00:00:00Z",
  "description": "Updated tariff",
  "changeReason": "Annual adjustment",
  "notes": "New rate includes utilities"
}
```

**Response:**
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "residenceId": "7ea85f64-5717-4562-b3fc-2c963f66afa6",
  "description": "Updated tariff",
  "amount": 120.00,
  "currency": "USD",
  "effectiveDate": "2024-01-15T00:00:00Z",
  "isActive": true,
  "notes": "New rate includes utilities",
  "createdAt": "2024-01-10T10:00:00Z",
  "updatedAt": "2024-01-14T15:30:00Z"
}
```

## Scenarios

### Scenario 1: Simple Tariff Increase

**Setup:**
- Residence: "Main Building"
- House 101: Has paid 3 months (Jan-Mar) at $100/month
- Current tariff: $100
- Update: Increase to $120

**Result:**
- Delta: $20
- Affected months: 3
- Rappel created: $60 for House 101

### Scenario 2: Multiple Houses

**Setup:**
- House 101: 3 months pre-paid ($100 → $120)
- House 102: 2 months pre-paid ($100 → $120)
- House 103: No pre-payments

**Result:**
- House 101: Rappel $60
- House 102: Rappel $40
- House 103: No rappel

### Scenario 3: Duplicate Prevention

**Setup:**
- House 101: Has existing unpaid rappel from previous tariff increase
- New tariff update: Increase from $120 to $130

**Result:**
- No new rappel created (existing unpaid rappel prevents duplicate)
- Existing rappel remains: $60 (from previous increase)

### Scenario 4: No Change

**Setup:**
- Current tariff: $100
- Update: Set amount to same value $100

**Result:**
- No rappel created
- History not recorded (no actual change)
- Tariff updated with other fields only

### Scenario 5: Decrease (No Rappel)

**Setup:**
- Current tariff: $120
- Update: Decrease to $100

**Result:**
- History recorded: Previous $120, New $100
- No rappel created (delta is negative)
- Tariff updated successfully

## Integration with Frontend

### Angular Service Method

```typescript
updateTariff(residenceId: string, tarifId: string, dto: UpdateTarifDto): Observable<TarifDto> {
  return this.http.put<TarifDto>(
    `/api/residences/${residenceId}/tarifs/${tarifId}`,
    dto
  );
}
```

### Component Usage

```typescript
onUpdateTariff(residenceId: string, tarifId: string, formData: UpdateTarifDto) {
  this.tarifService.updateTariff(residenceId, tarifId, formData).subscribe({
    next: (updated) => {
      console.log('Tariff updated successfully');
      console.log('Rappels will be created automatically if amount increased');
      this.loadTariffs();
      this.loadRappels(); // Reload rappels to show new ones
    },
    error: (err) => {
      console.error('Failed to update tariff', err);
    }
  });
}
```

## Database Impact

### Tables Modified

1. **Tarif**
   - Amount updated
   - EffectiveDate updated
   - UpdatedAt timestamp

2. **TarifHistory**
   - New history record created

3. **Rappel**
   - New rappel records may be created (one per house with pre-paid months)

### Transaction Handling

All changes are saved atomically:
```csharp
await _tarifRepository.UpdateAsync(tarif);
await _tarifHistoryRepository.AddAsync(history);
await _rappelRepository.AddAsync(rappel);
await _rappelRepository.SaveChangesAsync(); // Commits all changes
```

## Error Handling

### Validation Errors

```csharp
// Tariff not found
throw new InvalidOperationException($"Tariff with ID {tarifId} not found.");

// Tariff belongs to different residence
throw new InvalidOperationException("Tariff does not belong to the specified residence.");
```

### Expected Behaviors

| Condition | Behavior |
|-----------|----------|
| Amount not changed | No rappel detection, history not recorded |
| Amount decreased | History recorded, no rappel created |
| Amount increased | History recorded, rappels created for affected houses |
| No pre-paid months | No rappels created (no affected months) |
| Existing unpaid rappel | New rappel not created (duplicate prevention) |
| Repository error | Exception propagated, transaction rolled back |

## Testing Recommendations

### Unit Tests

1. **Test amount change detection**
   - Verify `amountChanged` flag works correctly
   - Test with various amount values

2. **Test rappel detection trigger**
   - Verify called only when amount increases
   - Verify not called when amount decreases

3. **Test history recording**
   - Verify history created when amount changes
   - Verify history not created when nothing changes

4. **Test effective date handling**
   - Verify DTO effective date is used
   - Verify defaults to UTC now when not provided

### Integration Tests

1. **Test with multiple houses and payments**
   - Create residence with multiple houses
   - Add various pre-paid payments
   - Update tariff and verify rappels

2. **Test duplicate prevention**
   - Create first tariff update (generates rappel)
   - Create second tariff update (no duplicate)
   - Verify only one rappel exists

3. **Test edge cases**
   - Payment exactly on effective date
   - Payment spanning effective date
   - Multiple overlapping payments

## Best Practices

1. **Always provide EffectiveDate**
   - Specify when the new rate takes effect
   - Prevents ambiguity with system UTC time

2. **Include ChangeReason**
   - Document why tariff was updated
   - Helps with audit trail

3. **Verify pre-paid amounts before update**
   - Check how many houses will be affected
   - Consider communication to residents

4. **Monitor rappel creation**
   - Review rappels created by system
   - Ensure amounts are correct
   - Communicate outstanding amounts to residents

## Performance Considerations

- **House enumeration**: O(n) where n = number of houses
- **Payment filtering**: O(m) where m = number of payments per house
- **Rappel lookup**: O(k) where k = number of rappels per house
- **Database calls**: 
  - 1 tariff update
  - 1 history insert
  - n house queries
  - n × m payment queries
  - n × k rappel queries
  - n rappel inserts (worst case)

**Optimization Tips:**
- Use batch queries where possible
- Consider caching house/payment data if called frequently
- Index Payment.PeriodEnd and Payment.Status for faster filtering

## Related Features

- **CreateTarifAsync**: Also triggers rappel detection when new tariff created
- **UpdateTarifHistoryAsync**: Allows manual adjustment of history records
- **RappelEndpoints**: Manage rappel records created by this feature
- **GetRappelsByHouse**: View all rappels for a specific house

## Migration Notes

If updating from previous version without rappel detection:

1. Existing tariff updates won't have auto-generated rappels
2. Manual rappel creation may be needed for past updates
3. Consider data consistency review
4. Document any manual adjustments in change reason

## Support & Troubleshooting

### Rappels not being created

**Check:**
1. Amount actually increased (`dto.Amount > oldAmount`)
2. EffectiveDate is in the past or near future
3. Houses exist in the residence
4. Payments marked as "Paid" status
5. Payment PeriodEnd >= EffectiveDate

**Debug:**
```csharp
// Add logging to identify issue
Console.WriteLine($"Amount changed: {amountChanged}");
Console.WriteLine($"Delta: {newTarif.Amount - oldTarif.Amount}");
Console.WriteLine($"Houses found: {houses.Count()}");
Console.WriteLine($"Pre-paid months: {prePaidMonths.Count()}");
```

### Duplicate rappels

**Check:**
1. Verify duplicate prevention logic is working
2. Query database for existing unpaid rappels
3. Check RappelStatus enum values

**Solution:**
- Manually mark unwanted rappels as "Paid"
- Or delete through RappelEndpoints DELETE method

## Future Enhancements

1. **Configurable triggering conditions**
   - Allow disabling auto-rappel detection
   - Threshold for minimum increase amount

2. **Rollback mechanism**
   - Delete rappels if tariff update is rolled back
   - Audit trail of rappel generation

3. **Notification system**
   - Email residents about new rappels
   - SMS alerts for outstanding amounts

4. **Payment plan creation**
   - Auto-create payment plan for rappels
   - Define installment schedule

## Summary

The enhanced `UpdateTarifAsync` method provides automatic rappel detection when tariff amounts are updated, ensuring accurate and timely billing for rate increases on pre-paid services. The duplicate prevention mechanism prevents billing errors, while detailed audit trails support compliance and transparency.
