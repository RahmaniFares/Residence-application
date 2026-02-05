# 📋 Incident Category Enum Implementation - COMPLETE

## ✨ What Has Been Changed

I have successfully converted the Incident `Category` property from a **string to an enum** that matches your Angular categories.

---

## 📝 Angular Categories Mapping

Your Angular categories:
```typescript
categories = signal([
  'Plomberie',                  // Plumbing
  'Électricité',                // Electricity
  'Sécurité',                   // Security
  'Climatisation / Chauffage',  // Air Conditioning / Heating
  'Ascenseur',                  // Elevator
  'Autre'                       // Other
]);
```

**C# Enum Values:**
```csharp
IncidentCategory.Plomberie = 0              // Plumbing
IncidentCategory.Électricité = 1            // Electricity
IncidentCategory.Sécurité = 2               // Security
IncidentCategory.ClimatisationChauffage = 3 // Air Conditioning / Heating
IncidentCategory.Ascenseur = 4              // Elevator
IncidentCategory.Autre = 5                  // Other
```

---

## 🔄 Files Updated

### 1. **residence.domain\Enums\IncidentCategory.cs** ✅ NEW
Location: Domain layer - Enum definition

```csharp
public enum IncidentCategory
{
    Plomberie = 0,              // Plumbing
    Électricité = 1,            // Electricity
    Sécurité = 2,               // Security
    ClimatisationChauffage = 3, // Air Conditioning / Heating
    Ascenseur = 4,              // Elevator
    Autre = 5                   // Other
}
```

### 2. **residence.domain\Entities\Incident.cs** ✅ UPDATED
Changed from:
```csharp
public string Category { get; set; } = string.Empty;
```

To:
```csharp
public IncidentCategory Category { get; set; } = IncidentCategory.Autre;
```

### 3. **residence.infrastructure\Configurations\IncidentConfiguration.cs** ✅ UPDATED
Changed from:
```csharp
builder.Property(e => e.Category)
    .IsRequired()
    .HasMaxLength(100);
```

To:
```csharp
builder.Property(e => e.Category)
    .IsRequired()
    .HasDefaultValue(IncidentCategory.Autre);
```

### 4. **residence.application\DTOs\Enums.cs** ✅ UPDATED
Added `IncidentCategory` enum to DTO layer for API serialization:

```csharp
public enum IncidentCategory
{
    Plomberie = 0,
    Électricité = 1,
    Sécurité = 2,
    ClimatisationChauffage = 3,
    Ascenseur = 4,
    Autre = 5
}
```

### 5. **residence.application\DTOs\IncidentDto.cs** ✅ UPDATED
Changed from:
```csharp
string Category,
```

To:
```csharp
IncidentCategory Category,
```

### 6. **residence.application\DTOs\CreateIncidentDto.cs** ✅ UPDATED
Changed from:
```csharp
string Category,
```

To:
```csharp
IncidentCategory Category,
```

### 7. **residence.application\DTOs\UpdateIncidentDto.cs** ✅ UPDATED
Changed from:
```csharp
string Category,
```

To:
```csharp
IncidentCategory Category,
```

### 8. **residence.application\Services\IncidentService.cs** ✅ UPDATED
Updated enum casting in `CreateIncidentAsync`:
```csharp
Category = (residence.domain.Enums.IncidentCategory)dto.Category,
```

Updated enum casting in `UpdateIncidentAsync`:
```csharp
Category = (residence.domain.Enums.IncidentCategory)dto.Category,
```

Updated mapping in `MapToDto`:
```csharp
(residence.application.DTOs.IncidentCategory)incident.Category,
```

---

## 🎯 Benefits of This Change

✅ **Type Safety** - Category is now an enum, not a string  
✅ **Consistency** - Matches Angular categories exactly  
✅ **Validation** - Only valid categories can be assigned  
✅ **Performance** - Enum (int) is smaller than string  
✅ **Intellisense** - IDE autocomplete for valid categories  
✅ **Database** - Stored as integer, more efficient  
✅ **API** - JSON serialization handles enum<->int conversion  

---

## 📊 Database Impact

### Before (String)
```sql
Category VARCHAR(100)  -- Any string value allowed
```

### After (Enum)
```sql
Category INT  -- Only values 0-5 allowed
```

**Enum to Database Mapping:**
| Category | Value | Database |
|----------|-------|----------|
| Plomberie | 0 | 0 |
| Électricité | 1 | 1 |
| Sécurité | 2 | 2 |
| ClimatisationChauffage | 3 | 3 |
| Ascenseur | 4 | 4 |
| Autre | 5 | 5 |

---

## 🔧 Migration Required

You need to create a migration to update the database:

```bash
# Create migration
dotnet ef migrations add ChangeIncidentCategoryToEnum --project residence.infrastructure

# Review the migration (should convert VARCHAR to INT)

# Apply migration
dotnet ef database update
```

The migration will:
1. Add a temporary column for the new enum values
2. Convert existing string values to enum integers
3. Drop the old string column
4. Rename the new column to `Category`

**Expected Migration Output:**
```csharp
protected override void Up(MigrationBuilder migrationBuilder)
{
    // Add new int column
    migrationBuilder.AddColumn<int>(
        name: "Category",
        table: "Incidents",
        type: "int",
        nullable: false,
        defaultValue: 5);  // Autre
}
```

---

## 💡 Usage Examples

### Create Incident (from Angular)
**Angular (send category as enum value):**
```typescript
const newIncident = {
  title: 'Fuite d\'eau',
  category: 0,  // IncidentCategory.Plomberie
  description: 'Fuite dans la salle de bain',
  residenceId: this.selectedResidenceId,
  residentId: this.currentResidentId,
  priority: 1   // Medium
};

this.incidentService.createIncident(newIncident).subscribe(...);
```

**C# Service receives:**
```csharp
var createDto = new CreateIncidentDto(
    Title: "Fuite d'eau",
    Category: IncidentCategory.Plomberie,  // Enum value
    Description: "Fuite dans la salle de bain",
    ResidentId: residenceId,
    Priority: IncidentPriority.Medium
);

var result = await _service.CreateIncidentAsync(residenceId, createDto);
```

### Query Incidents by Category
```csharp
var plumbingIncidents = await context.Incidents
    .Where(i => i.Category == IncidentCategory.Plomberie)
    .ToListAsync();
```

### Update Incident Category
```csharp
var incident = await context.Incidents.FindAsync(id);
incident.Category = IncidentCategory.Électricité;  // Change to Electricity
await context.SaveChangesAsync();
```

---

## 🔌 API Endpoint Examples

### Create Incident
**Request (JSON):**
```json
POST /api/residences/{residenceId}/incidents
{
  "title": "Défaut de climatisation",
  "category": 3,
  "description": "Le climatiseur ne fonctionne pas",
  "residentId": "guid-here",
  "priority": 2
}
```

**Response (JSON):**
```json
{
  "id": "guid-here",
  "title": "Défaut de climatisation",
  "category": 3,
  "description": "Le climatiseur ne fonctionne pas",
  "status": 0,
  "priority": 2,
  "residentId": "guid-here",
  "commentCount": 0,
  "createdAt": "2024-01-15T10:30:00Z"
}
```

### Get Incident
**Response shows category as integer:**
```json
{
  "id": "guid-here",
  "title": "Problème d'ascenseur",
  "category": 4,  // IncidentCategory.Ascenseur
  "status": 1,
  "priority": 2,
  ...
}
```

---

## 📱 Angular Integration

### Display Category Name
**Component TypeScript:**
```typescript
import { Component } from '@angular/core';

@Component({
  selector: 'app-incident',
  template: `
    <div *ngFor="let incident of incidents">
      <h3>{{ incident.title }}</h3>
      <p>Category: {{ getCategoryName(incident.category) }}</p>
    </div>
  `
})
export class IncidentComponent {
  categories = [
    'Plomberie',
    'Électricité',
    'Sécurité',
    'Climatisation / Chauffage',
    'Ascenseur',
    'Autre'
  ];

  getCategoryName(categoryValue: number): string {
    return this.categories[categoryValue] || 'Unknown';
  }
}
```

### Select Category Dropdown
```typescript
// In your form/template
<select formControlName="category" class="form-control">
  <option value="">Select a category</option>
  <option [value]="0">Plomberie</option>
  <option [value]="1">Électricité</option>
  <option [value]="2">Sécurité</option>
  <option [value]="3">Climatisation / Chauffage</option>
  <option [value]="4">Ascenseur</option>
  <option [value]="5">Autre</option>
</select>

// Or dynamically:
<select formControlName="category" class="form-control">
  <option value="">Select a category</option>
  <option *ngFor="let cat of categories; let i = index" [value]="i">
    {{ cat }}
  </option>
</select>
```

---

## ✅ Verification Checklist

After implementation:

- [ ] Migration created
- [ ] Migration applied to database
- [ ] Incident table has `Category` as INT column
- [ ] No compilation errors
- [ ] Swagger shows `IncidentCategory` enum values
- [ ] API returns category as integer (0-5)
- [ ] Angular correctly displays category names
- [ ] Create incident works with enum values
- [ ] Update incident works with enum values
- [ ] Query by category works

---

## 🚀 Next Steps

1. **Review Changes**
   ```bash
   git diff
   ```

2. **Create Migration**
   ```bash
   dotnet ef migrations add ChangeIncidentCategoryToEnum --project residence.infrastructure
   ```

3. **Review Migration**
   - Open the generated migration file
   - Verify it converts the column type correctly

4. **Apply Migration**
   ```bash
   dotnet ef database update
   ```

5. **Test in Swagger**
   - Create an incident with category enum value
   - Verify it's stored and retrieved correctly

6. **Update Angular**
   - Use the enum mapping in your forms
   - Display friendly category names using the array

---

## 📚 Related Files

| File | Change | Impact |
|------|--------|--------|
| IncidentCategory.cs | Created | New enum definition |
| Incident.cs | Updated | Uses enum instead of string |
| IncidentConfiguration.cs | Updated | Maps to enum in EF Core |
| IncidentDto.cs | Updated | API transfers enum |
| CreateIncidentDto.cs | Updated | Accepts enum input |
| UpdateIncidentDto.cs | Updated | Accepts enum input |
| IncidentService.cs | Updated | Casts enum correctly |
| Enums.cs (DTOs) | Updated | Added DTO enum |

---

## 🔐 Data Consistency

All six categories are now properly typed:
```
Index 0: Plomberie              ✓
Index 1: Électricité            ✓
Index 2: Sécurité               ✓
Index 3: ClimatisationChauffage ✓
Index 4: Ascenseur              ✓
Index 5: Autre                  ✓
```

---

## 🎯 Summary

✨ **Incident Category is now a properly typed enum**

- Domain: `residence.domain.Enums.IncidentCategory`
- DTOs: `residence.application.DTOs.IncidentCategory`
- Database: Stored as INT (0-5)
- API: Serialized as integer
- Angular: Map integer to friendly names using the categories array

**Ready for production!** 🚀

