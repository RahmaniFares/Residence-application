# Quick Reference: Rappel Detection Feature

## What Happens When You Create a New Tariff

```
1. New Tariff Created (with effective date)
   ↓
2. Old Tariff Deactivated
   ↓
3. [NEW] Rappel Detection Triggered
   ├─ Scan all houses in residence
   ├─ Find pre-paid payments covering future months
   ├─ Calculate: (newAmount - oldAmount) × prepaidMonths
   └─ Create Rappel records for affected houses
   ↓
4. Response returns created tariff
```

## API Endpoint

```
POST /api/residences/{residenceId}/tarifs

Request:
{
  "description": "Updated Rate",
  "amount": 150.00,
  "currency": "USD",
  "effectiveDate": "2024-02-01T00:00:00Z",
  "notes": "Optional notes"
}

Response: TarifDto (with automatic rappels created)
```

## Rappel Creation Logic

```
FOR each house in residence:
  prepaidMonths = find payments where:
    - PeriodEnd >= newTariff.EffectiveDate
    - Status = Paid

  IF prepaidMonths.any():
    delta = newTariff.Amount - oldTariff.Amount

    IF delta > 0:
      rappelAmount = delta × prepaidMonths.count()

      CREATE Rappel IF no existing unpaid rappel
```

## Example

**Setup:**
- Old Tariff: 100 USD
- New Tariff: 150 USD (effective 2024-02-01)
- House A paid: 500 USD (covers Jan-May 2024)

**Result:**
- Rappel Created: 50 × 4 months = 200 USD
  (Feb, Mar, Apr, May are prepaid with old tariff rate)

## Dependencies Added

```csharp
private readonly IHouseRepository _houseRepository;
private readonly IPaymentRepository _paymentRepository;
private readonly IRappelRepository _rappelRepository;
```

## DI Configuration Required

```csharp
services.AddScoped<IHouseRepository, HouseRepository>();
services.AddScoped<IPaymentRepository, PaymentRepository>();
services.AddScoped<IRappelRepository, RappelRepository>();
```

## Detection Criteria

✅ Pre-paid if:
- Payment end date >= tariff effective date
- Payment status = Paid
- House has associated payment

✅ Create Rappel if:
- New tariff amount > old tariff amount
- At least one pre-paid month exists
- No existing unpaid rappel for house

❌ Skip if:
- First tariff (no previous)
- Tariff decreased/same
- No pre-paid months
- Existing unpaid rappel

## Rappel Details

```json
{
  "houseId": "550e8400-e29b-41d4-a716-446655440000",
  "amount": 200.00,
  "status": "Unpaid",
  "notes": "Rappel créé suite à l'augmentation du tarif du 01/02/2024. 
            Ancien tarif: 100 USD, 
            Nouveau tarif: 150 USD. 
            Nombre de mois pré-payés affectés: 4"
}
```

## Testing

### Test Case 1: Basic Rappel Creation
1. Create old tariff: 100 USD
2. Pay house for 3 months: 300 USD
3. Create new tariff: 150 USD
4. Verify: Rappel created for 50 × 2 = 100 USD
   (only 2 months after effective date)

### Test Case 2: No Rappel (Decrease)
1. Old tariff: 150 USD
2. Create new tariff: 100 USD
3. Verify: No rappel created (delta ≤ 0)

### Test Case 3: Multiple Houses
1. House A: pre-paid 300 USD
2. House B: pre-paid 100 USD (before change)
3. Create new tariff
4. Verify: Rappel for A only

## Files Modified

- `residence.application\Services\TarifService.cs`
  - Added 3 dependencies
  - Modified constructor
  - Enhanced CreateTarifAsync
  - Added DetectAndCreateRappelsAsync

## Build Status

✅ **Successful** - No errors

## Backward Compatibility

✅ **Fully compatible**
- Existing API unchanged
- Only adds automatic functionality
- No breaking changes

## Documentation Files

1. **RAPPEL_DETECTION_FEATURE.md** - Complete feature guide
2. **RAPPEL_DETECTION_IMPLEMENTATION.md** - Implementation details
3. **This file** - Quick reference

## Common Scenarios

| Scenario | Outcome |
|----------|---------|
| First tariff created | No rappel (no old tariff) |
| Tariff increase + pre-payments | Rappel created ✓ |
| Tariff decrease + pre-payments | No rappel (delta ≤ 0) |
| Tariff increase + no pre-payments | No rappel (no coverage) |
| Multiple unpaid rappels | Skip creating duplicate |

## Questions?

See:
- **RAPPEL_DETECTION_FEATURE.md** for detailed documentation
- **RAPPEL_DETECTION_IMPLEMENTATION.md** for technical details
- **TarifService.cs** for source code

---

**Status:** ✅ Ready for Production
