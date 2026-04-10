# Quick Reference - Tariff System

## 📋 Backend - Quick API Reference

### Base URL
```
http://localhost:YOUR_PORT/api/residences/{residenceId}/tarifs
```

### Endpoints Cheat Sheet

| Method | Endpoint | Purpose | Auth |
|--------|----------|---------|------|
| `POST` | `/` | Create new tariff | Required |
| `GET` | `/{tarifId}` | Get specific tariff | Required |
| `GET` | `/` | Get all tariffs | Required |
| `GET` | `/current/active` | Get active tariff | Required |
| `PUT` | `/{tarifId}` | Update tariff | Required |
| `DELETE` | `/{tarifId}` | Delete tariff | Required |
| `GET` | `/{tarifId}/history` | Get tariff changes | Required |
| `GET` | `/history/all` | Get all residence changes | Required |
| `GET` | `/history/range` | Get changes by date | Required |

---

## 🚀 Angular Service - Quick Start

### 1. Installation
```bash
# Copy files to your Angular project
src/app/
├── models/tarif.model.ts
├── services/tarif/tarif.service.ts
└── components/
    ├── tarif-list/
    ├── tarif-create/
    └── tarif-history/
```

### 2. Import in Module
```typescript
import { HttpClientModule } from '@angular/common/http';
import { TarifService } from './services/tarif/tarif.service';

@NgModule({
  imports: [HttpClientModule],
  providers: [TarifService]
})
export class AppModule {}
```

### 3. Use in Component
```typescript
constructor(private tarifService: TarifService) {}

getCurrentTariff() {
  this.tarifService.getCurrentTarif(residenceId).subscribe(
    tariff => console.log(tariff)
  );
}
```

---

## 📝 API Request Examples

### Create Tariff
```bash
curl -X POST http://localhost:5000/api/residences/{residenceId}/tarifs \
  -H "Content-Type: application/json" \
  -d '{
    "description": "Monthly maintenance",
    "amount": 150.00,
    "currency": "USD",
    "effectiveDate": "2024-03-01T00:00:00Z",
    "notes": "Increased for new services"
  }'
```

### Get Current Tariff
```bash
curl http://localhost:5000/api/residences/{residenceId}/tarifs/current/active \
  -H "Authorization: Bearer YOUR_TOKEN"
```

### Update Tariff
```bash
curl -X PUT http://localhost:5000/api/residences/{residenceId}/tarifs/{tarifId} \
  -H "Content-Type: application/json" \
  -d '{
    "amount": 160.00,
    "changeReason": "Service enhancement"
  }'
```

### Get History
```bash
curl http://localhost:5000/api/residences/{residenceId}/tarifs/{tarifId}/history \
  -H "Authorization: Bearer YOUR_TOKEN"
```

### Get History by Date Range
```bash
curl "http://localhost:5000/api/residences/{residenceId}/tarifs/history/range?startDate=2024-01-01&endDate=2024-12-31" \
  -H "Authorization: Bearer YOUR_TOKEN"
```

---

## 💾 Database Schema Quick View

### Tarif Entity
```sql
CREATE TABLE Tarifs (
  Id UNIQUEIDENTIFIER PRIMARY KEY,
  ResidenceId UNIQUEIDENTIFIER NOT NULL,
  Description NVARCHAR(500),
  Amount DECIMAL(18,2),
  Currency NVARCHAR(3),
  EffectiveDate DATETIME2,
  EndDate DATETIME2 NULL,
  IsActive BIT,
  Notes NVARCHAR(1000) NULL,
  CreatedAt DATETIME2,
  UpdatedAt DATETIME2 NULL,
  IsDeleted BIT
)

CREATE INDEX IX_Tarif_ResidenceId_IsActive ON Tarifs(ResidenceId, IsActive)
CREATE INDEX IX_Tarif_EffectiveDate ON Tarifs(EffectiveDate)
```

### TarifHistory Entity
```sql
CREATE TABLE TarifHistories (
  Id UNIQUEIDENTIFIER PRIMARY KEY,
  TarifId UNIQUEIDENTIFIER NOT NULL,
  ResidenceId UNIQUEIDENTIFIER NOT NULL,
  PreviousAmount DECIMAL(18,2),
  NewAmount DECIMAL(18,2),
  PreviousDescription NVARCHAR(500),
  NewDescription NVARCHAR(500),
  EffectiveDate DATETIME2,
  ChangedBy NVARCHAR(256),
  ChangeReason NVARCHAR(1000) NULL,
  ChangedAt DATETIME2,
  CreatedAt DATETIME2,
  IsDeleted BIT
)

CREATE INDEX IX_TarifHistory_ResidenceId_ChangedAt ON TarifHistories(ResidenceId, ChangedAt)
CREATE INDEX IX_TarifHistory_TarifId ON TarifHistories(TarifId)
CREATE INDEX IX_TarifHistory_ChangedAt ON TarifHistories(ChangedAt)
```

---

## 🔧 Common Operations

### Backend - C#

#### Get Current Tariff
```csharp
var currentTarif = await tarifService.GetCurrentTarifAsync(residenceId);
if (currentTarif != null) {
    Console.WriteLine($"Current: {currentTarif.Amount} {currentTarif.Currency}");
}
```

#### Create Tariff
```csharp
var dto = new CreateTarifDto {
    Description = "Monthly fee",
    Amount = 150m,
    Currency = "USD",
    EffectiveDate = DateTime.UtcNow,
    Notes = "Q1 2024 adjustment"
};

var tarif = await tarifService.CreateTarifAsync(residenceId, dto, "admin@example.com");
```

#### Update with History
```csharp
var updateDto = new UpdateTarifDto {
    Amount = 160m,
    ChangeReason = "Service improvement"
};

var updated = await tarifService.UpdateTarifAsync(
    residenceId, 
    tarifId, 
    updateDto, 
    "admin@example.com"
);

// History is automatically created
```

#### Get History
```csharp
var history = await tarifService.GetTarifHistoryAsync(tarifId);
foreach (var entry in history) {
    Console.WriteLine($"{entry.ChangedAt}: {entry.PreviousAmount} → {entry.NewAmount}");
    Console.WriteLine($"Reason: {entry.ChangeReason}");
}
```

### Frontend - Angular

#### Create Tariff Form
```typescript
const tarifForm = new FormGroup({
  description: new FormControl('', Validators.required),
  amount: new FormControl('', [Validators.required, Validators.min(0)]),
  currency: new FormControl('USD'),
  effectiveDate: new FormControl(new Date()),
  notes: new FormControl('')
});

this.tarifService.createTarif(residenceId, tarifForm.value).subscribe(
  tariff => console.log('Created:', tariff),
  error => console.error('Error:', error)
);
```

#### Display Current Tariff
```typescript
this.currentTariff$ = this.tarifService.getCurrentTarif(residenceId).pipe(
  startWith(null),
  shareReplay(1)
);

// In template:
// <div *ngIf="currentTariff$ | async as tariff">
//   Amount: {{ tariff.amount }}
// </div>
```

#### Show History
```typescript
this.history$ = this.tarifService.getTarifHistory(residenceId, tarifId).pipe(
  startWith([]),
  shareReplay(1)
);

// In template:
// <div *ngFor="let item of history$ | async">
//   {{ item.changedAt | date }} - {{ item.previousAmount }} → {{ item.newAmount }}
// </div>
```

---

## 🐛 Troubleshooting

### Issue: "No active tariff found" Error
**Solution:** Create a new tariff or check if the tariff's `IsActive` property is set to true

### Issue: History not appearing
**Solution:** Make sure changes are made via the service (not direct DB updates) so history is recorded

### Issue: CORS Error on Frontend
**Solution:** Ensure CORS is configured in `Program.cs`:
```csharp
builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll", builder => {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});
```

### Issue: Unauthorized Error (401)
**Solution:** Include JWT token in requests:
```typescript
const headers = new HttpHeaders({
  'Authorization': `Bearer ${token}`
});
```

---

## 📊 Response Examples

### Create Tariff Response (201 Created)
```json
{
  "id": "550e8400-e29b-41d4-a716-446655440000",
  "residenceId": "660e8400-e29b-41d4-a716-446655440000",
  "description": "Monthly maintenance",
  "amount": 150.00,
  "currency": "USD",
  "effectiveDate": "2024-03-01T00:00:00Z",
  "endDate": null,
  "isActive": true,
  "notes": "Increased for new services",
  "createdAt": "2024-03-01T10:30:00Z",
  "updatedAt": null
}
```

### Get History Response (200 OK)
```json
[
  {
    "id": "770e8400-e29b-41d4-a716-446655440000",
    "tarifId": "550e8400-e29b-41d4-a716-446655440000",
    "residenceId": "660e8400-e29b-41d4-a716-446655440000",
    "previousAmount": 140.00,
    "newAmount": 150.00,
    "previousDescription": "Monthly maintenance",
    "newDescription": "Monthly maintenance",
    "effectiveDate": "2024-03-01T00:00:00Z",
    "changedBy": "admin@example.com",
    "changeReason": "Increased due to inflation",
    "changedAt": "2024-02-28T15:00:00Z"
  }
]
```

### Error Response (400 Bad Request)
```json
{
  "message": "Residence with ID {residenceId} not found",
  "statusCode": 400,
  "details": "Invalid residence identifier"
}
```

---

## 🔐 Security Considerations

✅ Always validate tariff amounts are positive  
✅ Log all tariff changes for audit purposes  
✅ Require authentication for all endpoints  
✅ Validate effective dates are not in the past  
✅ Soft delete instead of hard delete for audit trail  
✅ Use HTTPS in production  
✅ Implement rate limiting on API endpoints  

---

## 📈 Performance Tips

1. **Use `shareReplay()`** - Prevent multiple HTTP calls
2. **Implement pagination** - For large history lists
3. **Use database indexes** - Already configured
4. **Cache current tariff** - In service or store
5. **Lazy load history** - Load only when needed
6. **Use date range filters** - Instead of loading all history

---

## 🔄 Workflow Example

```
1. Admin creates residence
   ↓
2. Admin creates initial tariff (150 USD/month)
   ↓
3. System creates Tarif record with IsActive=true
   ↓
4. Admin updates tariff to 160 USD (inflation)
   ↓
5. System creates TarifHistory record
   ↓
6. System marks old Tarif as IsActive=false
   ↓
7. Angular shows history timeline
   ↓
8. Residents can see tariff changes
```

---

## 📞 Support Resources

- **Backend Docs:** `TARIFF_HISTORY_SYSTEM.md`
- **Frontend Docs:** `ANGULAR_TARIF_SERVICE.md`
- **Implementation Guide:** `IMPLEMENTATION_SUMMARY.md`
- **API Documentation:** Swagger at `/swagger`

---

Last Updated: 2024
Status: ✅ Production Ready
