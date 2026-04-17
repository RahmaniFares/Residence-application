# Angular House Service Implementation Guide

## Overview

This document provides the complete specification for implementing an Angular service to consume the .NET 8 House API endpoints from the Residence Application backend.

## API Base URL

```
Base: {apiUrl}/api/residences/{residenceId}/houses
Example: http://localhost:5000/api/residences/550e8400-e29b-41d4-a716-446655440000/houses
```

## Backend Endpoints

| Method | Endpoint | Summary | Returns |
|--------|----------|---------|---------|
| POST | `/` | Create a new house | `HouseDto` (201 Created) |
| GET | `/{id}` | Get house by ID | `HouseDto` (200 OK) |
| GET | `/{id}/details` | Get house with resident details | `HouseDetailDto` (200 OK) |
| PUT | `/{id}` | Update house | `HouseDto` (200 OK) |
| DELETE | `/{id}` | Delete house | void (204 No Content) |
| GET | `/` | Get all houses by residence (paginated) | `PagedResult<HouseDetailDto>` (200 OK) |
| GET | `/{id}/financial-statement` | Get house financial statement | `HouseFinancialStatementDto` (200 OK) |

## TypeScript Interfaces

```typescript
// DTOs
interface CreateHouseDto {
  residentId?: string;
  block: string;
  unit: string;
  floor?: string;
}

interface UpdateHouseDto {
  residentId?: string;
  block: string;
  unit: string;
  floor?: string;
  status: number;
}

interface HouseDto {
  id: string;
  block: string;
  unit: string;
  floor?: string;
  status: number;
  currentResidentId?: string;
  createdAt: string;
  updatedAt?: string;
}

interface ResidentDto {
  id: string;
  houseId: string;
  firstName: string;
  lastName: string;
  email?: string;
  phoneNumber?: string;
  address?: string;
  birthDate?: string | null;
  status: number;
  moveInDate?: string | null;
  moveOutDate?: string | null;
  createdAt: string;
  updatedAt?: string;
}

interface HouseDetailDto extends HouseDto {
  currentResident?: ResidentDto | null;
  residentsCount: number;
}

interface PaginationDto {
  pageNumber?: number;
  pageSize?: number;
}

interface PagedResult<T> {
  items: T[];
  total: number;
  pageNumber: number;
  pageSize: number;
  totalPages: number;
}

interface HouseFinancialStatementDto {
  houseId: string;
  totalRappelPaid: number;
  totalRappelToPay: number;
}

interface MonthlyStatementItemDto {
  month: number;
  year: number;
  amountPaid: number;
  activeTarifAmount: number;
  difference: number;
}
```

## Service Methods Specification

### 1. Create House
```typescript
createHouse(residenceId: string, dto: CreateHouseDto): Observable<HouseDto>
```
- **HTTP Method:** POST
- **URL:** `/api/residences/{residenceId}/houses`
- **Body:** CreateHouseDto
- **Response:** HouseDto with 201 Created status
- **Error Handling:** 400 Bad Request

**Example:**
```typescript
this.houseService.createHouse(residenceId, {
  block: 'A',
  unit: '101',
  floor: '1',
  residentId: '...'
}).subscribe(house => console.log(house));
```

---

### 2. Get House by ID
```typescript
getHouse(residenceId: string, id: string): Observable<HouseDto>
```
- **HTTP Method:** GET
- **URL:** `/api/residences/{residenceId}/houses/{id}`
- **Response:** HouseDto with 200 OK status
- **Error Handling:** 404 Not Found

**Example:**
```typescript
this.houseService.getHouse(residenceId, houseId).subscribe(house => {
  console.log(house);
});
```

---

### 3. Get House Details
```typescript
getHouseDetails(residenceId: string, id: string): Observable<HouseDetailDto>
```
- **HTTP Method:** GET
- **URL:** `/api/residences/{residenceId}/houses/{id}/details`
- **Response:** HouseDetailDto with current resident and resident count
- **Error Handling:** 404 Not Found

**Example:**
```typescript
this.houseService.getHouseDetails(residenceId, houseId).subscribe(detail => {
  console.log('Resident:', detail.currentResident);
  console.log('Count:', detail.residentsCount);
});
```

---

### 4. Update House
```typescript
updateHouse(residenceId: string, id: string, dto: UpdateHouseDto): Observable<HouseDto>
```
- **HTTP Method:** PUT
- **URL:** `/api/residences/{residenceId}/houses/{id}`
- **Body:** UpdateHouseDto
- **Response:** Updated HouseDto with 200 OK status
- **Error Handling:** 400 Bad Request

**Example:**
```typescript
this.houseService.updateHouse(residenceId, houseId, {
  block: 'B',
  unit: '102',
  floor: '2',
  status: 1,
  residentId: '...'
}).subscribe(updated => console.log(updated));
```

---

### 5. Delete House
```typescript
deleteHouse(residenceId: string, id: string): Observable<void>
```
- **HTTP Method:** DELETE
- **URL:** `/api/residences/{residenceId}/houses/{id}`
- **Response:** 204 No Content
- **Error Handling:** 400 Bad Request

**Example:**
```typescript
this.houseService.deleteHouse(residenceId, houseId).subscribe(() => {
  console.log('House deleted');
});
```

---

### 6. Get Houses by Residence (Paginated)
```typescript
getHousesByResidence(
  residenceId: string, 
  pagination?: PaginationDto
): Observable<PagedResult<HouseDetailDto>>
```
- **HTTP Method:** GET
- **URL:** `/api/residences/{residenceId}/houses`
- **Query Parameters:** pageNumber (default: 1), pageSize (default: 10)
- **Response:** PagedResult<HouseDetailDto> with 200 OK status
- **Error Handling:** 400 Bad Request

**Example:**
```typescript
this.houseService.getHousesByResidence(residenceId, {
  pageNumber: 1,
  pageSize: 20
}).subscribe(result => {
  console.log('Total:', result.total);
  console.log('Houses:', result.items);
  console.log('Total Pages:', result.totalPages);
});
```

---

### 7. Get House Financial Statement
```typescript
getHouseFinancialStatement(
  residenceId: string, 
  id: string
): Observable<HouseFinancialStatementDto>
```
- **HTTP Method:** GET
- **URL:** `/api/residences/{residenceId}/houses/{id}/financial-statement`
- **Response:** HouseFinancialStatementDto with 200 OK status
- **Error Handling:** 404 Not Found

**Example:**
```typescript
this.houseService.getHouseFinancialStatement(residenceId, houseId).subscribe(statement => {
  console.log('Total Paid:', statement.totalRappelPaid);
  console.log('Total to Pay:', statement.totalRappelToPay);
});
```

---

## Environment Configuration

Add the API base URL to your `environment.ts` and `environment.prod.ts`:

**environment.ts:**
```typescript
export const environment = {
  production: false,
  apiUrl: 'http://localhost:5000'
};
```

**environment.prod.ts:**
```typescript
export const environment = {
  production: true,
  apiUrl: 'https://api.residence-app.com'
};
```

## Service Implementation Guidelines

### 1. Service Class Structure
```typescript
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from '../environments/environment';

@Injectable({ providedIn: 'root' })
export class HouseService {
  private readonly baseUrl = `${environment.apiUrl}/api/residences`;

  constructor(private http: HttpClient) {}

  // Methods implementation...
}
```

### 2. Error Handling Pattern
```typescript
private handleError(error: any) {
  const errorMessage = error?.error?.message || 'An error occurred';
  console.error('House Service Error:', errorMessage);
  return throwError(() => new Error(errorMessage));
}
```

### 3. Pagination Defaults
```typescript
private buildPaginationParams(pagination?: PaginationDto): HttpParams {
  const pageNumber = pagination?.pageNumber ?? 1;
  const pageSize = pagination?.pageSize ?? 10;
  return new HttpParams()
    .set('pageNumber', pageNumber.toString())
    .set('pageSize', pageSize.toString());
}
```

### 4. Date Handling
- Backend returns ISO 8601 date strings (e.g., `"2024-01-15T10:30:00Z"`)
- Convert to Date objects in components as needed:
  ```typescript
  createdDate = new Date(house.createdAt);
  ```

## Component Usage Example

```typescript
import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { HouseService } from './services/house.service';

@Component({
  selector: 'app-house-list',
  template: `
    <div *ngIf="houses$ | async as result">
      <div *ngFor="let house of result.items">
        <h3>{{ house.block }}-{{ house.unit }}</h3>
        <p>Status: {{ house.status }}</p>
      </div>
      <p>Total: {{ result.total }} | Page {{ result.pageNumber }} of {{ result.totalPages }}</p>
    </div>
  `
})
export class HouseListComponent implements OnInit, OnDestroy {
  residenceId = '550e8400-e29b-41d4-a716-446655440000';
  houses$ = this.houseService.getHousesByResidence(this.residenceId, {
    pageNumber: 1,
    pageSize: 10
  });

  private destroy$ = new Subject<void>();

  constructor(private houseService: HouseService) {}

  ngOnInit() {
    this.houses$
      .pipe(takeUntil(this.destroy$))
      .subscribe(result => {
        console.log('Houses loaded:', result.items);
      });
  }

  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
```

## Unit Testing Template

```typescript
import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { HouseService } from './house.service';

describe('HouseService', () => {
  let service: HouseService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [HouseService]
    });
    service = TestBed.inject(HouseService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should fetch houses by residence', () => {
    const residenceId = 'test-residence-id';
    const mockResult = {
      items: [{ id: '1', block: 'A', unit: '101' }],
      total: 1,
      pageNumber: 1,
      pageSize: 10,
      totalPages: 1
    };

    service.getHousesByResidence(residenceId).subscribe(result => {
      expect(result.items.length).toBe(1);
      expect(result.total).toBe(1);
    });

    const req = httpMock.expectOne(req =>
      req.url.includes(`/api/residences/${residenceId}/houses`)
    );
    expect(req.request.method).toBe('GET');
    req.flush(mockResult);
  });

  it('should handle 404 error on getHouseDetails', () => {
    const residenceId = 'test-id';
    const houseId = 'house-id';

    service.getHouseDetails(residenceId, houseId).subscribe(
      () => fail('should have failed'),
      (error: Error) => {
        expect(error.message).toContain('House not found');
      }
    );

    const req = httpMock.expectOne(
      `http://localhost:5000/api/residences/${residenceId}/houses/${houseId}/details`
    );
    req.flush({ message: 'House not found' }, { status: 404, statusText: 'Not Found' });
  });
});
```

## Setup Steps

1. **Create Service File:**
   ```bash
   ng generate service services/house
   ```

2. **Add HttpClientModule** to `app.module.ts`:
   ```typescript
   import { HttpClientModule } from '@angular/common/http';

   @NgModule({
     imports: [HttpClientModule, ...]
   })
   export class AppModule { }
   ```

3. **Configure Environment URLs** in `environment.ts` and `environment.prod.ts`

4. **Implement Service** with all methods from this specification

5. **Create Components** that inject and use `HouseService`

6. **Add Unit Tests** using the provided template

## Notes

- The endpoint `GET /` actually calls `GetHousesByResidenceWithDetailsAsync`, so it returns `PagedResult<HouseDetailDto>` (includes resident details)
- All date strings from the backend are ISO 8601 format; convert to Date in components as needed
- Error responses follow pattern: `{ message: "Error description" }` (HTTP 4xx/5xx)
- Support RxJS unsubscribe with `takeUntil` to avoid memory leaks
- Use Angular's async pipe in templates for automatic subscription management

## GitHub Repository

Repository: https://github.com/RahmaniFares/Residence-application

Branch: `master`

---

**Last Updated:** 2024
**Version:** 1.0
