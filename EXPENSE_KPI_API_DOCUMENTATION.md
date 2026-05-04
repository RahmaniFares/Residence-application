# 📊 Expense KPI & Statistics API Documentation

## Overview

The Expense KPI & Statistics API provides comprehensive analytics endpoints for tracking and analyzing expense data across residences. These endpoints enable dashboard creation, trend analysis, and financial reporting.

---

## 🎯 Endpoints

### 1. **Total Expense KPI**
**Get aggregate expense metrics for a residence**

**Endpoint:** `GET /api/residences/{residenceId}/expenses/kpi/total`

**Description:** Returns total expense amount, count, averages, and date ranges.

**Response:**
```json
{
  "totalAmount": 15662.68,
  "totalExpenseCount": 179,
  "averageExpense": 87.49,
  "maxExpense": 7000.00,
  "minExpense": 0.00,
  "earliestExpenseDate": "2025-08-03T00:00:00Z",
  "latestExpenseDate": "2026-04-21T00:00:00Z"
}
```

**Response Fields:**
| Field | Type | Description |
|-------|------|-------------|
| `totalAmount` | decimal | Sum of all expenses |
| `totalExpenseCount` | int | Total number of expenses |
| `averageExpense` | decimal | Average expense amount |
| `maxExpense` | decimal | Highest individual expense |
| `minExpense` | decimal | Lowest individual expense |
| `earliestExpenseDate` | DateTime | Date of first expense |
| `latestExpenseDate` | DateTime | Date of last expense |

**Use Cases:**
- Dashboard KPI cards
- Quick financial overview
- Budget planning reference

---

### 2. **Monthly Expense Breakdown**
**Get monthly expense summaries for trend analysis**

**Endpoint:** `GET /api/residences/{residenceId}/expenses/kpi/monthly`

**Description:** Returns expenses grouped by month with monthly totals and averages.

**Response:**
```json
{
  "data": [
	{
	  "year": 2025,
	  "month": 8,
	  "monthName": "August",
	  "totalAmount": 1258.34,
	  "expenseCount": 12,
	  "averageExpense": 104.86
	},
	{
	  "year": 2025,
	  "month": 9,
	  "monthName": "September",
	  "totalAmount": 1456.78,
	  "expenseCount": 15,
	  "averageExpense": 97.12
	}
  ],
  "totalAmount": 15662.68,
  "totalExpenseCount": 179,
  "monthsWithData": 9
}
```

**Response Fields:**
| Field | Type | Description |
|-------|------|-------------|
| `data` | List | Array of monthly breakdowns |
| `data[].year` | int | Calendar year |
| `data[].month` | int | Month number (1-12) |
| `data[].monthName` | string | Month name (e.g., "August") |
| `data[].totalAmount` | decimal | Total expenses for the month |
| `data[].expenseCount` | int | Number of expenses in month |
| `data[].averageExpense` | decimal | Average expense for the month |
| `totalAmount` | decimal | Grand total across all months |
| `totalExpenseCount` | int | Total expenses across all months |
| `monthsWithData` | int | Number of months with data |

**Use Cases:**
- Monthly trend charts
- Seasonal analysis
- Budget variance reports
- Cash flow projections

---

### 3. **Expense Statistics by Type**
**Get expense breakdown by category**

**Endpoint:** `GET /api/residences/{residenceId}/expenses/kpi/by-type`

**Description:** Returns expenses grouped by type with totals, counts, and percentages.

**Response:**
```json
{
  "data": [
	{
	  "type": 3,
	  "typeName": "Cleaning",
	  "count": 42,
	  "totalAmount": 652.79,
	  "averageAmount": 15.54,
	  "percentageOfTotal": 4.17
	},
	{
	  "type": 6,
	  "typeName": "Repairs",
	  "count": 79,
	  "totalAmount": 8023.98,
	  "averageAmount": 101.57,
	  "percentageOfTotal": 51.23
	}
  ],
  "totalAmount": 15662.68,
  "totalExpenseCount": 179,
  "highestCategory": {
	"type": 6,
	"typeName": "Repairs",
	"count": 79,
	"totalAmount": 8023.98,
	"averageAmount": 101.57,
	"percentageOfTotal": 51.23
  },
  "lowestCategory": {
	"type": 2,
	"typeName": "Water",
	"count": 1,
	"totalAmount": 122.20,
	"averageAmount": 122.20,
	"percentageOfTotal": 0.78
  }
}
```

**Response Fields:**
| Field | Type | Description |
|-------|------|-------------|
| `data` | List | Array of type statistics |
| `data[].type` | int | Expense type enum value |
| `data[].typeName` | string | Expense type name |
| `data[].count` | int | Number of expenses of this type |
| `data[].totalAmount` | decimal | Total amount for this type |
| `data[].averageAmount` | decimal | Average expense for this type |
| `data[].percentageOfTotal` | decimal | Percentage of total expenses |
| `totalAmount` | decimal | Grand total |
| `totalExpenseCount` | int | Total expenses |
| `highestCategory` | Object | Category with highest spending |
| `lowestCategory` | Object | Category with lowest spending |

**Expense Types:**
```csharp
0  = Maintenance
1  = Electricity
2  = Water
3  = Cleaning
4  = Security
5  = Gardening
6  = Repairs
7  = Equipment
8  = Insurance
9  = Taxes
10 = Other
```

**Use Cases:**
- Pie charts / donut charts
- Budget allocation analysis
- Top spending categories
- Category-based forecasting

---

## 📈 Integration Examples

### Angular Service

```typescript
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ExpenseKpiService {
  private apiUrl = '/api/residences';

  constructor(private http: HttpClient) {}

  getTotalKpi(residenceId: string): Observable<any> {
	return this.http.get(
	  `${this.apiUrl}/${residenceId}/expenses/kpi/total`
	);
  }

  getMonthlyExpenses(residenceId: string): Observable<any> {
	return this.http.get(
	  `${this.apiUrl}/${residenceId}/expenses/kpi/monthly`
	);
  }

  getExpenseStatsByType(residenceId: string): Observable<any> {
	return this.http.get(
	  `${this.apiUrl}/${residenceId}/expenses/kpi/by-type`
	);
  }
}
```

### Angular Component - Dashboard

```typescript
import { Component, OnInit } from '@angular/core';
import { ExpenseKpiService } from './services/expense-kpi.service';

@Component({
  selector: 'app-expense-dashboard',
  templateUrl: './expense-dashboard.component.html'
})
export class ExpenseDashboardComponent implements OnInit {
  totalKpi: any;
  monthlyExpenses: any;
  expenseStats: any;
  residenceId = 'your-residence-id';

  constructor(private kpiService: ExpenseKpiService) {}

  ngOnInit() {
	this.loadDashboard();
  }

  loadDashboard() {
	// Load total KPI
	this.kpiService.getTotalKpi(this.residenceId).subscribe(
	  data => this.totalKpi = data,
	  error => console.error('Error loading KPI:', error)
	);

	// Load monthly breakdown
	this.kpiService.getMonthlyExpenses(this.residenceId).subscribe(
	  data => this.monthlyExpenses = data,
	  error => console.error('Error loading monthly:', error)
	);

	// Load type statistics
	this.kpiService.getExpenseStatsByType(this.residenceId).subscribe(
	  data => this.expenseStats = data,
	  error => console.error('Error loading stats:', error)
	);
  }
}
```

### Template Example

```html
<div class="expense-dashboard">
  <!-- KPI Cards -->
  <div class="kpi-cards">
	<div class="card">
	  <h3>Total Expenses</h3>
	  <p class="amount">€{{ totalKpi?.totalAmount | number: '1.2-2' }}</p>
	  <small>{{ totalKpi?.totalExpenseCount }} expenses</small>
	</div>

	<div class="card">
	  <h3>Average Expense</h3>
	  <p class="amount">€{{ totalKpi?.averageExpense | number: '1.2-2' }}</p>
	</div>

	<div class="card">
	  <h3>Highest Expense</h3>
	  <p class="amount">€{{ totalKpi?.maxExpense | number: '1.2-2' }}</p>
	</div>

	<div class="card">
	  <h3>Lowest Expense</h3>
	  <p class="amount">€{{ totalKpi?.minExpense | number: '1.2-2' }}</p>
	</div>
  </div>

  <!-- Monthly Trend Chart -->
  <div class="chart-container">
	<h3>Monthly Trend</h3>
	<canvas id="monthlyChart"></canvas>
  </div>

  <!-- Type Distribution Pie Chart -->
  <div class="chart-container">
	<h3>Expenses by Category</h3>
	<canvas id="typeChart"></canvas>
  </div>

  <!-- Highest & Lowest Categories -->
  <div class="category-summary">
	<div class="card">
	  <h3>Highest Category</h3>
	  <p>{{ expenseStats?.highestCategory?.typeName }}</p>
	  <p class="amount">€{{ expenseStats?.highestCategory?.totalAmount | number: '1.2-2' }}</p>
	  <small>{{ expenseStats?.highestCategory?.count }} expenses</small>
	</div>

	<div class="card">
	  <h3>Lowest Category</h3>
	  <p>{{ expenseStats?.lowestCategory?.typeName }}</p>
	  <p class="amount">€{{ expenseStats?.lowestCategory?.totalAmount | number: '1.2-2' }}</p>
	  <small>{{ expenseStats?.lowestCategory?.count }} expenses</small>
	</div>
  </div>
</div>
```

---

## 🔧 Implementation Details

### Service Layer (`ExpenseService.cs`)

**Method Signatures:**

```csharp
/// Get total expense KPI for a residence
public async Task<TotalExpenseKpiDto> GetTotalExpenseKpiAsync(Guid residenceId)

/// Get monthly expense breakdown for a residence
public async Task<MonthlyExpensesDto> GetMonthlyExpensesAsync(Guid residenceId)

/// Get expense statistics by type for a residence
public async Task<ExpenseStatsDto> GetExpenseStatsByTypeAsync(Guid residenceId)
```

### Repository Methods

Extended `IExpenseRepository` with:
- `GetAllByResidenceAsync()` - Get all expenses for aggregation
- `GetExpensesByMonthAsync()` - Group by month
- `GetExpensesByTypeAsync()` - Group by type
- `GetCountAsync()` - Count total expenses
- `GetMinAmountAsync()` - Minimum amount
- `GetMaxAmountAsync()` - Maximum amount
- `GetEarliestDateAsync()` - First expense date
- `GetLatestDateAsync()` - Last expense date

### Data Transfer Objects (DTOs)

**TotalExpenseKpiDto**
```csharp
public record TotalExpenseKpiDto(
	decimal TotalAmount,
	int TotalExpenseCount,
	decimal AverageExpense,
	decimal MaxExpense,
	decimal MinExpense,
	DateTime? EarliestExpenseDate,
	DateTime? LatestExpenseDate
);
```

**MonthlyExpensesDto**
```csharp
public record MonthlyExpenseDto(
	int Year,
	int Month,
	string MonthName,
	decimal TotalAmount,
	int ExpenseCount,
	decimal AverageExpense
);

public record MonthlyExpensesDto(
	List<MonthlyExpenseDto> Data,
	decimal TotalAmount,
	int TotalExpenseCount,
	int MonthsWithData
);
```

**ExpenseStatsDto**
```csharp
public record ExpenseTypeStatsDto(
	ExpenseType Type,
	string TypeName,
	int Count,
	decimal TotalAmount,
	decimal AverageAmount,
	decimal PercentageOfTotal
);

public record ExpenseStatsDto(
	List<ExpenseTypeStatsDto> Data,
	decimal TotalAmount,
	int TotalExpenseCount,
	ExpenseTypeStatsDto HighestCategory,
	ExpenseTypeStatsDto LowestCategory
);
```

---

## 📝 Endpoint Mapping

In `ExpenseEndpoints.cs`:

```csharp
// KPI and Statistics Endpoints
var kpiGroup = app.MapGroup("/api/residences/{residenceId}/expenses/kpi")
	.WithTags("Expense KPI")
	.WithOpenApi();

kpiGroup.MapGet("/total", GetTotalExpenseKpi)
	.WithName("GetTotalExpenseKpi")
	.WithSummary("Get total expense KPI");

kpiGroup.MapGet("/monthly", GetMonthlyExpenses)
	.WithName("GetMonthlyExpenses")
	.WithSummary("Get expense breakdown by month");

kpiGroup.MapGet("/by-type", GetExpenseStatsByType)
	.WithName("GetExpenseStatsByType")
	.WithSummary("Get expense statistics by category type");
```

---

## 🧪 Testing

### cURL Examples

**Total KPI:**
```bash
curl -X GET "http://localhost:5000/api/residences/{residenceId}/expenses/kpi/total" \
  -H "accept: application/json"
```

**Monthly Breakdown:**
```bash
curl -X GET "http://localhost:5000/api/residences/{residenceId}/expenses/kpi/monthly" \
  -H "accept: application/json"
```

**Statistics by Type:**
```bash
curl -X GET "http://localhost:5000/api/residences/{residenceId}/expenses/kpi/by-type" \
  -H "accept: application/json"
```

### Postman Collection

Import these requests:
1. **Total Expense KPI** - GET /api/residences/{{residenceId}}/expenses/kpi/total
2. **Monthly Expenses** - GET /api/residences/{{residenceId}}/expenses/kpi/monthly
3. **Expense Stats by Type** - GET /api/residences/{{residenceId}}/expenses/kpi/by-type

Set variable: `residenceId` = your-residence-id-here

---

## 📊 Dashboard Ideas

### 1. KPI Dashboard
- 4 cards: Total, Average, Max, Min
- Date range indicator
- Total expense count

### 2. Trend Analysis
- Line chart: Monthly trend
- Bar chart: Month comparison
- Percentage change calculations

### 3. Category Distribution
- Pie chart: Type breakdown
- Bar chart: Spending by type
- Highlight highest/lowest categories

### 4. Detailed Report
- Table: All monthly data
- Filters: By type, date range
- Export: CSV/PDF options

---

## 🎯 Performance Notes

- **GET /kpi/total**: O(n) - Single pass through expenses
- **GET /kpi/monthly**: O(n log n) - Sorting by month
- **GET /kpi/by-type**: O(n log n) - Grouping by type

**Optimization Tips:**
- Add caching for frequently accessed KPIs
- Consider materialized views for large datasets
- Use pagination for monthly breakdown if 100+ months

---

## 🔐 Security Considerations

- Endpoints require `residenceId` parameter
- Consider adding authorization checks in endpoint handlers
- Validate that user has access to specified residence
- Return 403 Forbidden if unauthorized

**Example Authorization Check:**
```csharp
private static async Task<IResult> GetTotalExpenseKpi(
	IExpenseService service, 
	HttpContext httpContext,
	Guid residenceId)
{
	// Add authorization check
	var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
	if (!await service.UserHasAccessAsync(userId, residenceId))
		return Results.Forbid();

	try
	{
		var result = await service.GetTotalExpenseKpiAsync(residenceId);
		return Results.Ok(result);
	}
	catch (Exception ex)
	{
		return Results.BadRequest(new { message = ex.Message });
	}
}
```

---

## 📚 Related Endpoints

- `GET /api/residences/{residenceId}/expenses` - Get all expenses
- `POST /api/residences/{residenceId}/expenses` - Create expense
- `PUT /api/residences/{residenceId}/expenses/{id}` - Update expense
- `DELETE /api/residences/{residenceId}/expenses/{id}` - Delete expense

---

## ✅ Checklist for Integration

- [ ] Review API endpoints
- [ ] Create Angular service
- [ ] Build KPI dashboard component
- [ ] Add charts (Chart.js or similar)
- [ ] Style KPI cards
- [ ] Test all 3 endpoints
- [ ] Implement error handling
- [ ] Add loading indicators
- [ ] Create monthly trend chart
- [ ] Create category distribution chart
- [ ] Deploy to production

---

**Version:** 1.0  
**Last Updated:** 2025  
**Status:** ✅ Production Ready
