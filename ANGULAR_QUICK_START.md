# Angular Services Quick Start - Donation & Employee

## 📁 File Structure to Create

```
src/app/
├── models/
│   ├── donation.model.ts          ← Create first
│   ├── employee.model.ts          ← Create first
│   └── enums.ts                   ← Create first
├── services/
│   ├── api.service.ts             ← Create second (optional)
│   ├── donation.service.ts        ← Create third
│   └── employee.service.ts        ← Create third
├── interceptors/
│   └── error.interceptor.ts       ← Create (optional)
└── components/
	├── donation/
	│   ├── donation-list/
	│   ├── donation-detail/
	│   └── donation-form/
	└── employee/
		├── employee-list/
		├── employee-detail/
		└── employee-form/
```

## 🚀 Step-by-Step Implementation

### Step 1: Create Models

```bash
# Create donation model
ng generate interface models/donation

# Create employee model
ng generate interface models/employee
```

Copy content from the complete guide's "Models & Interfaces" section.

### Step 2: Create Services

```bash
# Create donation service
ng generate service services/donation

# Create employee service
ng generate service services/employee

# Create base API service (optional)
ng generate service services/api
```

Copy content from the complete guide's "Services" sections.

### Step 3: Update App Module

Add `HttpClientModule` and services to your `app.module.ts`:

```typescript
import { HttpClientModule } from '@angular/common/http';
import { DonationService } from './services/donation.service';
import { EmployeeService } from './services/employee.service';

@NgModule({
  imports: [HttpClientModule, ...],
  providers: [DonationService, EmployeeService, ...],
  ...
})
export class AppModule { }
```

### Step 4: Create Components

```bash
# Donation components
ng generate component components/donation/donation-list
ng generate component components/donation/donation-detail
ng generate component components/donation/donation-form

# Employee components
ng generate component components/employee/employee-list
ng generate component components/employee/employee-detail
ng generate component components/employee/employee-form
```

### Step 5: Implement Components

Use the component examples from the complete guide.

## 🎯 Core Service Methods

### DonationService

```typescript
// CRUD
createDonation(donation: CreateDonationDto)
getDonationById(id: string)
updateDonation(id: string, donation: UpdateDonationDto)
deleteDonation(id: string)

// Queries
getAllDonations()
getDonationsByHouse(houseId: string)
getDonationsByDonor(donorId: string)
getDonationsByDateRange(startDate: Date, endDate: Date)

// Statistics
getTotalDonationsByHouse(houseId: string)
getTotalDonationsByDonor(donorId: string)
getDonationDetails(id: string)
```

### EmployeeService

```typescript
// CRUD
createEmployee(employee: CreateEmployeeDto)
getEmployee(id: string)
getEmployeeDetail(id: string)
updateEmployee(id: string, employee: UpdateEmployeeDto)
deleteEmployee(id: string)

// Queries
getAllEmployees()
getEmployeesByResidence(residenceId: string)
getActiveEmployees(residenceId: string)
getEmployeesByPosition(residenceId: string, position: string)
getEmployeeCount(residenceId: string)

// Salary
getCurrentSalary(employeeId: string)
getSalaryHistory(employeeId: string)
changeSalary(employeeId: string, salary: CreateEmployeeSalaryDto)

// Payroll
getTotalPayroll(residenceId: string)
getPositionAverage(residenceId: string)
getPayrollSummary(residenceId: string)
```

## 💡 Common Usage Patterns

### Pattern 1: Load Data on Init

```typescript
ngOnInit(): void {
  this.employeeService.getEmployeesByResidence(this.residenceId)
	.pipe(
	  takeUntil(this.destroy$),
	  catchError(error => {
		console.error('Error loading employees:', error);
		return [];
	  })
	)
	.subscribe(employees => {
	  this.employees = employees;
	});
}
```

### Pattern 2: Create with Validation

```typescript
createEmployee(form: any): void {
  if (form.invalid) {
	alert('Please fill all required fields');
	return;
  }

  this.employeeService.createEmployee(form.value)
	.pipe(takeUntil(this.destroy$))
	.subscribe({
	  next: (created) => {
		this.employees.push(created);
		this.showSuccessMessage('Employee created');
	  },
	  error: (error) => {
		this.showErrorMessage(error.message);
	  }
	});
}
```

### Pattern 3: Update with Confirmation

```typescript
updateEmployee(id: string, data: any): void {
  if (!confirm('Update employee?')) return;

  this.employeeService.updateEmployee(id, data)
	.pipe(takeUntil(this.destroy$))
	.subscribe({
	  next: (updated) => {
		const index = this.employees.findIndex(e => e.id === id);
		if (index > -1) {
		  this.employees[index] = updated;
		}
		this.showSuccessMessage('Employee updated');
	  },
	  error: (error) => {
		this.showErrorMessage(error.message);
	  }
	});
}
```

### Pattern 4: Delete with Confirmation

```typescript
deleteEmployee(id: string): void {
  if (!confirm('Are you sure you want to delete this employee?')) return;

  this.employeeService.deleteEmployee(id)
	.pipe(takeUntil(this.destroy$))
	.subscribe({
	  next: () => {
		this.employees = this.employees.filter(e => e.id !== id);
		this.showSuccessMessage('Employee deleted');
	  },
	  error: (error) => {
		this.showErrorMessage(error.message);
	  }
	});
}
```

## 📊 Data Binding Examples

### Donation List in Template

```html
<div *ngIf="loading" class="spinner">Loading...</div>

<div *ngIf="!loading && donations.length === 0" class="empty-state">
  No donations found
</div>

<table *ngIf="!loading && donations.length > 0">
  <thead>
	<tr>
	  <th>Date</th>
	  <th>Amount</th>
	  <th>House</th>
	  <th>Actions</th>
	</tr>
  </thead>
  <tbody>
	<tr *ngFor="let donation of donations">
	  <td>{{ donation.donationDate | date: 'short' }}</td>
	  <td>{{ donation.amount | currency: 'EUR' }}</td>
	  <td>{{ donation.houseId }}</td>
	  <td>
		<button (click)="editDonation(donation)">Edit</button>
		<button (click)="deleteDonation(donation.id)">Delete</button>
	  </td>
	</tr>
  </tbody>
</table>
```

### Employee List in Template

```html
<div class="employee-list">
  <h2>Employees ({{ employees.length }})</h2>

  <div *ngIf="loading" class="spinner">Loading...</div>

  <table *ngIf="!loading" class="table">
	<thead>
	  <tr>
		<th>Name</th>
		<th>Position</th>
		<th>Email</th>
		<th>Status</th>
		<th>Salary</th>
		<th>Actions</th>
	  </tr>
	</thead>
	<tbody>
	  <tr *ngFor="let emp of employees">
		<td>{{ emp.firstName }} {{ emp.lastName }}</td>
		<td>{{ emp.position }}</td>
		<td>{{ emp.email }}</td>
		<td>
		  <span [class]="'status-' + getStatusClass(emp.status)">
			{{ getStatusLabel(emp.status) }}
		  </span>
		</td>
		<td>
		  <button (click)="viewSalary(emp.id)">View</button>
		</td>
		<td>
		  <button (click)="editEmployee(emp.id)">Edit</button>
		  <button (click)="deleteEmployee(emp.id)">Delete</button>
		</td>
	  </tr>
	</tbody>
  </table>
</div>
```

## 🔗 API Base URL Configuration

**File**: `src/environments/environment.ts`

```typescript
export const environment = {
  production: false,
  apiUrl: 'http://localhost:5062/api'
};
```

**File**: `src/environments/environment.prod.ts`

```typescript
export const environment = {
  production: true,
  apiUrl: 'https://your-production-api.com/api'
};
```

## 🧪 Testing Services

### Test Donation Service

```typescript
describe('DonationService', () => {
  let service: DonationService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
	TestBed.configureTestingModule({
	  imports: [HttpClientTestingModule],
	  providers: [DonationService]
	});
	service = TestBed.inject(DonationService);
	httpMock = TestBed.inject(HttpTestingController);
  });

  it('should get all donations', () => {
	service.getAllDonations().subscribe(donations => {
	  expect(donations.length).toBeGreaterThan(0);
	});

	const req = httpMock.expectOne('http://localhost:5062/api/donations/');
	expect(req.request.method).toBe('GET');
	req.flush([{ id: '1', amount: 100 }]);
  });
});
```

## 📝 Environment Setup

### Install Dependencies

```bash
npm install @angular/common
npm install rxjs
```

### Import in App Module

```typescript
import { HttpClientModule } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';

@NgModule({
  imports: [
	CommonModule,
	HttpClientModule,
	ReactiveFormsModule,
	FormsModule
  ]
})
export class AppModule { }
```

## ⚙️ Service Dependency Injection

### Constructor Injection

```typescript
constructor(
  private donationService: DonationService,
  private employeeService: EmployeeService
) {}
```

### Usage in Component

```typescript
@Component({
  selector: 'app-my-component',
  templateUrl: './my-component.html'
})
export class MyComponent implements OnInit {
  donations$ = this.donationService.donations$;

  constructor(private donationService: DonationService) {}

  ngOnInit(): void {
	this.donationService.getAllDonations().subscribe();
  }
}
```

## 🔄 Observable Patterns

### Using async Pipe (Recommended)

```html
<!-- Template -->
<div *ngIf="donations$ | async as donations">
  <div *ngFor="let donation of donations">
	{{ donation.amount | currency }}
  </div>
</div>
```

```typescript
// Component
donations$ = this.donationService.getAllDonations();
```

### Using Subjects

```typescript
donations$ = new BehaviorSubject<DonationDto[]>([]);

loadDonations(): void {
  this.donationService.getAllDonations().subscribe(data => {
	this.donations$.next(data);
  });
}
```

## ✨ Best Practices

1. **Always unsubscribe**: Use `takeUntil` with a destroy subject
2. **Use async pipe**: Avoid manual subscriptions in templates
3. **Error handling**: Implement proper error handling in services
4. **Type safety**: Use TypeScript interfaces for all data
5. **Caching**: Use `shareReplay()` for repeated requests
6. **Lazy loading**: Load data only when needed
7. **Null checks**: Always check for null/undefined values
8. **Testing**: Write unit tests for all services

## 📚 Related Files

- **Models**: Defined in `ANGULAR_SERVICES_GUIDE.md` Part 1
- **Services**: Complete implementation in Part 3 & 4
- **Components**: Examples in Part 6
- **Testing**: Unit test examples in Part 9

## 🆘 Troubleshooting

| Issue | Solution |
|-------|----------|
| 404 errors | Check API URL in environment files |
| CORS errors | Configure CORS on backend API |
| No data | Check network tab, verify API responses |
| Type errors | Ensure all DTOs match API contracts |
| Memory leaks | Always use takeUntil in subscriptions |

---

**Ready to implement!** Follow the steps above and refer to `ANGULAR_SERVICES_GUIDE.md` for detailed code.
