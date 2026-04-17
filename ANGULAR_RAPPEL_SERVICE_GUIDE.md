# Angular Rappel Service Implementation Guide

## Overview
This guide provides complete instructions for creating and integrating the `RappelService` (or `rappel-services.ts`) in your Angular application to consume the Rappel API endpoints.

---

## API Endpoints Summary

```
POST   /api/residences/{residenceId}/rappels
GET    /api/residences/{residenceId}/rappels/{id}
PUT    /api/residences/{residenceId}/rappels/{id}
DELETE /api/residences/{residenceId}/rappels/{id}
GET    /api/residences/{residenceId}/rappels/house/{houseId}
GET    /api/residences/{residenceId}/rappels
```

---

## Step 1: Create TypeScript Models

### File: `src/app/models/rappel.model.ts`

```typescript
/**
 * Rappel Status Enum
 */
export enum RappelStatus {
  Unpaid = 0,
  Paid = 1
}

/**
 * DTO for creating a new rappel
 */
export interface CreateRappelDto {
  houseId: string;
  amount: number;
  notes?: string;
}

/**
 * DTO for updating a rappel
 */
export interface UpdateRappelDto {
  amount?: number;
  notes?: string;
  status?: RappelStatus;
}

/**
 * Response DTO for rappel
 */
export interface RappelDto {
  id: string;
  houseId: string;
  amount: number;
  status: RappelStatus;
  notes?: string;
  paymentDate?: Date;
  createdAt: Date;
  updatedAt?: Date;
}

/**
 * Paginated response for rappels
 */
export interface PaginatedRappelsResponse {
  items: RappelDto[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
  hasNextPage: boolean;
  hasPreviousPage: boolean;
}

/**
 * Pagination query parameters
 */
export interface PaginationDto {
  pageNumber?: number;
  pageSize?: number;
}
```

---

## Step 2: Create the Rappel Service

### File: `src/app/services/rappel.service.ts`

```typescript
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import {
  RappelDto,
  CreateRappelDto,
  UpdateRappelDto,
  PaginatedRappelsResponse,
  PaginationDto
} from '../models/rappel.model';

@Injectable({
  providedIn: 'root'
})
export class RappelService {
  private apiUrl = '/api/residences';

  constructor(private http: HttpClient) {}

  /**
   * Create a new rappel
   * @param residenceId The residence ID
   * @param dto The create rappel DTO
   * @returns Observable of created RappelDto
   */
  createRappel(residenceId: string, dto: CreateRappelDto): Observable<RappelDto> {
    return this.http.post<RappelDto>(
      `${this.apiUrl}/${residenceId}/rappels`,
      dto
    );
  }

  /**
   * Get a rappel by ID
   * @param rappelId The rappel ID
   * @returns Observable of RappelDto
   */
  getRappelById(rappelId: string): Observable<RappelDto> {
    return this.http.get<RappelDto>(
      `${this.apiUrl}/rappels/${rappelId}`
    );
  }

  /**
   * Update a rappel
   * @param rappelId The rappel ID
   * @param dto The update rappel DTO
   * @returns Observable of updated RappelDto
   */
  updateRappel(rappelId: string, dto: UpdateRappelDto): Observable<RappelDto> {
    return this.http.put<RappelDto>(
      `${this.apiUrl}/rappels/${rappelId}`,
      dto
    );
  }

  /**
   * Delete a rappel
   * @param rappelId The rappel ID
   * @returns Observable of void
   */
  deleteRappel(rappelId: string): Observable<void> {
    return this.http.delete<void>(
      `${this.apiUrl}/rappels/${rappelId}`
    );
  }

  /**
   * Get all rappels for a specific house with pagination
   * @param houseId The house ID
   * @param pagination Pagination parameters
   * @returns Observable of paginated rappels
   */
  getRappelsByHouse(
    houseId: string,
    pagination?: PaginationDto
  ): Observable<PaginatedRappelsResponse> {
    let params = new HttpParams();

    if (pagination?.pageNumber) {
      params = params.set('pageNumber', pagination.pageNumber.toString());
    }
    if (pagination?.pageSize) {
      params = params.set('pageSize', pagination.pageSize.toString());
    }

    return this.http.get<PaginatedRappelsResponse>(
      `${this.apiUrl}/rappels/house/${houseId}`,
      { params }
    );
  }

  /**
   * Get all rappels for a residence with pagination
   * @param residenceId The residence ID
   * @param pagination Pagination parameters
   * @returns Observable of paginated rappels
   */
  getRappelsByResidence(
    residenceId: string,
    pagination?: PaginationDto
  ): Observable<PaginatedRappelsResponse> {
    let params = new HttpParams();

    if (pagination?.pageNumber) {
      params = params.set('pageNumber', pagination.pageNumber.toString());
    }
    if (pagination?.pageSize) {
      params = params.set('pageSize', pagination.pageSize.toString());
    }

    return this.http.get<PaginatedRappelsResponse>(
      `${this.apiUrl}/${residenceId}/rappels`,
      { params }
    );
  }
}
```

---

## Step 3: Create Models Alternative (TypeScript Interface File)

If you prefer a separate models file, create:

### File: `src/app/models/index.ts`

```typescript
// Export all models from a central location
export * from './rappel.model';
export * from './tarif.model';
export * from './house.model';
// ... other models
```

---

## Step 4: Component Integration Examples

### Example 1: Display Rappels Grid

#### File: `src/app/components/rappel-list/rappel-list.component.ts`

```typescript
import { Component, OnInit } from '@angular/core';
import { RappelService } from '../../services/rappel.service';
import { RappelDto, PaginationDto } from '../../models/rappel.model';

@Component({
  selector: 'app-rappel-list',
  templateUrl: './rappel-list.component.html',
  styleUrls: ['./rappel-list.component.scss']
})
export class RappelListComponent implements OnInit {
  rappels: RappelDto[] = [];
  isLoading = false;
  error: string | null = null;
  residenceId: string = ''; // Set this from route params or input

  pagination: PaginationDto = {
    pageNumber: 1,
    pageSize: 10
  };

  constructor(private rappelService: RappelService) {}

  ngOnInit(): void {
    this.loadRappels();
  }

  loadRappels(): void {
    if (!this.residenceId) {
      this.error = 'Residence ID is required';
      return;
    }

    this.isLoading = true;
    this.error = null;

    this.rappelService
      .getRappelsByResidence(this.residenceId, this.pagination)
      .subscribe({
        next: (response) => {
          this.rappels = response.items;
          this.isLoading = false;
        },
        error: (err) => {
          this.error = err.error?.message || 'Failed to load rappels';
          this.isLoading = false;
          console.error('Error loading rappels:', err);
        }
      });
  }

  onPageChange(newPage: number): void {
    this.pagination.pageNumber = newPage;
    this.loadRappels();
  }
}
```

#### Template: `src/app/components/rappel-list/rappel-list.component.html`

```html
<div class="rappel-container">
  <h2>Rappels (Retroactive Payments)</h2>

  <!-- Loading State -->
  <mat-spinner *ngIf="isLoading" diameter="40"></mat-spinner>

  <!-- Error State -->
  <div class="error-message" *ngIf="error">
    {{ error }}
  </div>

  <!-- Rappels Table -->
  <table mat-table [dataSource]="rappels" class="rappel-table">
    <!-- ID Column -->
    <ng-container matColumnDef="id">
      <th mat-header-cell *matHeaderCellDef>ID</th>
      <td mat-cell *matCellDef="let element">{{ element.id }}</td>
    </ng-container>

    <!-- House Column -->
    <ng-container matColumnDef="houseId">
      <th mat-header-cell *matHeaderCellDef>House</th>
      <td mat-cell *matCellDef="let element">{{ element.houseId }}</td>
    </ng-container>

    <!-- Amount Column -->
    <ng-container matColumnDef="amount">
      <th mat-header-cell *matHeaderCellDef>Amount</th>
      <td mat-cell *matCellDef="let element">{{ element.amount | currency }}</td>
    </ng-container>

    <!-- Status Column -->
    <ng-container matColumnDef="status">
      <th mat-header-cell *matHeaderCellDef>Status</th>
      <td mat-cell *matCellDef="let element">
        <span [ngClass]="element.status === 0 ? 'unpaid' : 'paid'">
          {{ element.status === 0 ? 'Unpaid' : 'Paid' }}
        </span>
      </td>
    </ng-container>

    <!-- Notes Column -->
    <ng-container matColumnDef="notes">
      <th mat-header-cell *matHeaderCellDef>Notes</th>
      <td mat-cell *matCellDef="let element">{{ element.notes }}</td>
    </ng-container>

    <!-- Actions Column -->
    <ng-container matColumnDef="actions">
      <th mat-header-cell *matHeaderCellDef>Actions</th>
      <td mat-cell *matCellDef="let element">
        <button mat-icon-button (click)="editRappel(element)">
          <mat-icon>edit</mat-icon>
        </button>
        <button mat-icon-button (click)="deleteRappel(element.id)">
          <mat-icon>delete</mat-icon>
        </button>
      </td>
    </ng-container>

    <!-- Table Definition -->
    <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
    <tr mat-row *matRowDef="let row; columns: displayedColumns;"></tr>
  </table>

  <!-- Pagination -->
  <mat-paginator
    [length]="totalCount"
    [pageSize]="pagination.pageSize"
    [pageSizeOptions]="[5, 10, 25, 100]"
    (page)="onPageChange($event.pageIndex + 1)"
  ></mat-paginator>
</div>
```

---

### Example 2: Create Rappel Dialog

#### File: `src/app/components/rappel-create/rappel-create.component.ts`

```typescript
import { Component, Inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { RappelService } from '../../services/rappel.service';
import { CreateRappelDto } from '../../models/rappel.model';

@Component({
  selector: 'app-rappel-create',
  templateUrl: './rappel-create.component.html',
  styleUrls: ['./rappel-create.component.scss']
})
export class RappelCreateComponent {
  form: FormGroup;
  isLoading = false;
  error: string | null = null;

  constructor(
    private formBuilder: FormBuilder,
    private rappelService: RappelService,
    public dialogRef: MatDialogRef<RappelCreateComponent>,
    @Inject(MAT_DIALOG_DATA) public data: {
      residenceId: string;
      houseId?: string;
    }
  ) {
    this.form = this.createForm();
  }

  private createForm(): FormGroup {
    return this.formBuilder.group({
      houseId: [this.data.houseId || '', Validators.required],
      amount: ['', [Validators.required, Validators.min(0)]],
      notes: ['']
    });
  }

  onSubmit(): void {
    if (this.form.invalid) {
      return;
    }

    this.isLoading = true;
    this.error = null;

    const createDto: CreateRappelDto = this.form.value;

    this.rappelService
      .createRappel(this.data.residenceId, createDto)
      .subscribe({
        next: (result) => {
          this.isLoading = false;
          this.dialogRef.close(result);
        },
        error: (err) => {
          this.isLoading = false;
          this.error = err.error?.message || 'Failed to create rappel';
          console.error('Error creating rappel:', err);
        }
      });
  }

  onCancel(): void {
    this.dialogRef.close();
  }
}
```

#### Template: `src/app/components/rappel-create/rappel-create.component.html`

```html
<h2 mat-dialog-title>Create Rappel (Retroactive Payment)</h2>

<mat-dialog-content>
  <form [formGroup]="form">
    <mat-form-field appearance="outline" class="full-width">
      <mat-label>House ID</mat-label>
      <input matInput formControlName="houseId" />
      <mat-error *ngIf="form.get('houseId')?.hasError('required')">
        House ID is required
      </mat-error>
    </mat-form-field>

    <mat-form-field appearance="outline" class="full-width">
      <mat-label>Amount</mat-label>
      <input matInput type="number" formControlName="amount" step="0.01" />
      <mat-error *ngIf="form.get('amount')?.hasError('required')">
        Amount is required
      </mat-error>
      <mat-error *ngIf="form.get('amount')?.hasError('min')">
        Amount must be positive
      </mat-error>
    </mat-form-field>

    <mat-form-field appearance="outline" class="full-width">
      <mat-label>Notes</mat-label>
      <textarea matInput formControlName="notes" rows="3"></textarea>
    </mat-form-field>

    <div class="error-message" *ngIf="error">
      {{ error }}
    </div>
  </form>
</mat-dialog-content>

<mat-dialog-actions align="end">
  <button mat-button (click)="onCancel()">Cancel</button>
  <button
    mat-raised-button
    color="primary"
    (click)="onSubmit()"
    [disabled]="isLoading || form.invalid"
  >
    <mat-spinner diameter="20" *ngIf="isLoading"></mat-spinner>
    {{ isLoading ? 'Creating...' : 'Create' }}
  </button>
</mat-dialog-actions>
```

---

### Example 3: Edit Rappel Dialog

#### File: `src/app/components/rappel-edit/rappel-edit.component.ts`

```typescript
import { Component, Inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { RappelService } from '../../services/rappel.service';
import { RappelDto, UpdateRappelDto } from '../../models/rappel.model';

@Component({
  selector: 'app-rappel-edit',
  templateUrl: './rappel-edit.component.html',
  styleUrls: ['./rappel-edit.component.scss']
})
export class RappelEditComponent {
  form: FormGroup;
  isLoading = false;
  error: string | null = null;

  constructor(
    private formBuilder: FormBuilder,
    private rappelService: RappelService,
    public dialogRef: MatDialogRef<RappelEditComponent>,
    @Inject(MAT_DIALOG_DATA) public data: {
      rappel: RappelDto;
    }
  ) {
    this.form = this.createForm();
  }

  private createForm(): FormGroup {
    const rappel = this.data.rappel;
    return this.formBuilder.group({
      amount: [rappel.amount, [Validators.required, Validators.min(0)]],
      notes: [rappel.notes || ''],
      status: [rappel.status]
    });
  }

  onSubmit(): void {
    if (this.form.invalid) {
      return;
    }

    this.isLoading = true;
    this.error = null;

    const updateDto: UpdateRappelDto = this.form.value;

    this.rappelService
      .updateRappel(this.data.rappel.id, updateDto)
      .subscribe({
        next: (result) => {
          this.isLoading = false;
          this.dialogRef.close(result);
        },
        error: (err) => {
          this.isLoading = false;
          this.error = err.error?.message || 'Failed to update rappel';
          console.error('Error updating rappel:', err);
        }
      });
  }

  onCancel(): void {
    this.dialogRef.close();
  }
}
```

#### Template: `src/app/components/rappel-edit/rappel-edit.component.html`

```html
<h2 mat-dialog-title>Edit Rappel (Retroactive Payment)</h2>

<mat-dialog-content>
  <form [formGroup]="form">
    <mat-form-field appearance="outline" class="full-width">
      <mat-label>Amount</mat-label>
      <input matInput type="number" formControlName="amount" step="0.01" />
      <mat-error *ngIf="form.get('amount')?.hasError('required')">
        Amount is required
      </mat-error>
      <mat-error *ngIf="form.get('amount')?.hasError('min')">
        Amount must be positive
      </mat-error>
    </mat-form-field>

    <mat-form-field appearance="outline" class="full-width">
      <mat-label>Status</mat-label>
      <mat-select formControlName="status">
        <mat-option [value]="0">Unpaid</mat-option>
        <mat-option [value]="1">Paid</mat-option>
      </mat-select>
    </mat-form-field>

    <mat-form-field appearance="outline" class="full-width">
      <mat-label>Notes</mat-label>
      <textarea matInput formControlName="notes" rows="3"></textarea>
    </mat-form-field>

    <div class="error-message" *ngIf="error">
      {{ error }}
    </div>
  </form>
</mat-dialog-content>

<mat-dialog-actions align="end">
  <button mat-button (click)="onCancel()">Cancel</button>
  <button
    mat-raised-button
    color="primary"
    (click)="onSubmit()"
    [disabled]="isLoading || form.invalid"
  >
    <mat-spinner diameter="20" *ngIf="isLoading"></mat-spinner>
    {{ isLoading ? 'Updating...' : 'Update' }}
  </button>
</mat-dialog-actions>
```

---

### Example 4: Rappel by House Component

#### File: `src/app/components/rappel-by-house/rappel-by-house.component.ts`

```typescript
import { Component, Input, OnInit } from '@angular/core';
import { RappelService } from '../../services/rappel.service';
import { RappelDto, PaginationDto } from '../../models/rappel.model';

@Component({
  selector: 'app-rappel-by-house',
  templateUrl: './rappel-by-house.component.html',
  styleUrls: ['./rappel-by-house.component.scss']
})
export class RappelByHouseComponent implements OnInit {
  @Input() houseId: string = '';

  rappels: RappelDto[] = [];
  isLoading = false;
  error: string | null = null;
  totalUnpaid = 0;

  pagination: PaginationDto = {
    pageNumber: 1,
    pageSize: 10
  };

  constructor(private rappelService: RappelService) {}

  ngOnInit(): void {
    if (this.houseId) {
      this.loadRappels();
    }
  }

  loadRappels(): void {
    this.isLoading = true;
    this.error = null;

    this.rappelService
      .getRappelsByHouse(this.houseId, this.pagination)
      .subscribe({
        next: (response) => {
          this.rappels = response.items;
          this.calculateTotalUnpaid();
          this.isLoading = false;
        },
        error: (err) => {
          this.error = err.error?.message || 'Failed to load rappels';
          this.isLoading = false;
          console.error('Error loading rappels:', err);
        }
      });
  }

  private calculateTotalUnpaid(): void {
    this.totalUnpaid = this.rappels
      .filter(r => r.status === 0) // Unpaid status
      .reduce((sum, r) => sum + r.amount, 0);
  }

  markAsPaid(rappelId: string): void {
    this.rappelService
      .updateRappel(rappelId, { status: 1 })
      .subscribe({
        next: () => {
          this.loadRappels(); // Reload after update
        },
        error: (err) => {
          console.error('Error marking rappel as paid:', err);
        }
      });
  }
}
```

---

## Step 5: Module Configuration

### File: `src/app/app.module.ts`

```typescript
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { ReactiveFormsModule } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

import { AppComponent } from './app.component';
import { RappelService } from './services/rappel.service';

// Import components
import { RappelListComponent } from './components/rappel-list/rappel-list.component';
import { RappelCreateComponent } from './components/rappel-create/rappel-create.component';
import { RappelEditComponent } from './components/rappel-edit/rappel-edit.component';
import { RappelByHouseComponent } from './components/rappel-by-house/rappel-by-house.component';

@NgModule({
  declarations: [
    AppComponent,
    RappelListComponent,
    RappelCreateComponent,
    RappelEditComponent,
    RappelByHouseComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    ReactiveFormsModule,
    MatTableModule,
    MatPaginatorModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule
  ],
  providers: [RappelService],
  bootstrap: [AppComponent]
})
export class AppModule {}
```

---

## Step 6: Unit Testing Examples

### File: `src/app/services/rappel.service.spec.ts`

```typescript
import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { RappelService } from './rappel.service';
import { RappelDto, CreateRappelDto, RappelStatus } from '../models/rappel.model';

describe('RappelService', () => {
  let service: RappelService;
  let httpMock: HttpTestingController;
  const baseUrl = '/api/residences';

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [RappelService]
    });
    service = TestBed.inject(RappelService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  describe('createRappel', () => {
    it('should create a new rappel', () => {
      const residenceId = '550e8400-e29b-41d4-a716-446655440000';
      const createDto: CreateRappelDto = {
        houseId: '550e8400-e29b-41d4-a716-446655440001',
        amount: 200.00,
        notes: 'Retroactive payment'
      };

      const mockResponse: RappelDto = {
        id: '550e8400-e29b-41d4-a716-446655440002',
        houseId: createDto.houseId,
        amount: createDto.amount,
        status: RappelStatus.Unpaid,
        notes: createDto.notes,
        createdAt: new Date()
      };

      service.createRappel(residenceId, createDto).subscribe((result) => {
        expect(result).toEqual(mockResponse);
      });

      const req = httpMock.expectOne(`${baseUrl}/${residenceId}/rappels`);
      expect(req.request.method).toBe('POST');
      expect(req.request.body).toEqual(createDto);
      req.flush(mockResponse);
    });
  });

  describe('getRappelById', () => {
    it('should get a rappel by ID', () => {
      const rappelId = '550e8400-e29b-41d4-a716-446655440002';
      const mockResponse: RappelDto = {
        id: rappelId,
        houseId: '550e8400-e29b-41d4-a716-446655440001',
        amount: 200.00,
        status: RappelStatus.Unpaid,
        notes: 'Retroactive payment',
        createdAt: new Date()
      };

      service.getRappelById(rappelId).subscribe((result) => {
        expect(result).toEqual(mockResponse);
      });

      const req = httpMock.expectOne(`${baseUrl}/rappels/${rappelId}`);
      expect(req.request.method).toBe('GET');
      req.flush(mockResponse);
    });
  });

  describe('updateRappel', () => {
    it('should update a rappel', () => {
      const rappelId = '550e8400-e29b-41d4-a716-446655440002';
      const updateDto = { status: RappelStatus.Paid };

      const mockResponse: RappelDto = {
        id: rappelId,
        houseId: '550e8400-e29b-41d4-a716-446655440001',
        amount: 200.00,
        status: RappelStatus.Paid,
        notes: 'Retroactive payment',
        createdAt: new Date(),
        updatedAt: new Date()
      };

      service.updateRappel(rappelId, updateDto).subscribe((result) => {
        expect(result.status).toBe(RappelStatus.Paid);
      });

      const req = httpMock.expectOne(`${baseUrl}/rappels/${rappelId}`);
      expect(req.request.method).toBe('PUT');
      expect(req.request.body).toEqual(updateDto);
      req.flush(mockResponse);
    });
  });

  describe('deleteRappel', () => {
    it('should delete a rappel', () => {
      const rappelId = '550e8400-e29b-41d4-a716-446655440002';

      service.deleteRappel(rappelId).subscribe(() => {
        expect(true).toBe(true); // Verify subscription completed
      });

      const req = httpMock.expectOne(`${baseUrl}/rappels/${rappelId}`);
      expect(req.request.method).toBe('DELETE');
      req.flush(null);
    });
  });

  describe('getRappelsByHouse', () => {
    it('should get paginated rappels for a house', () => {
      const houseId = '550e8400-e29b-41d4-a716-446655440001';
      const pagination = { pageNumber: 1, pageSize: 10 };

      const mockResponse = {
        items: [
          {
            id: '550e8400-e29b-41d4-a716-446655440002',
            houseId: houseId,
            amount: 200.00,
            status: RappelStatus.Unpaid,
            notes: 'Retroactive payment',
            createdAt: new Date()
          }
        ],
        totalCount: 1,
        pageNumber: 1,
        pageSize: 10,
        hasNextPage: false,
        hasPreviousPage: false
      };

      service.getRappelsByHouse(houseId, pagination).subscribe((result) => {
        expect(result.items.length).toBe(1);
        expect(result.totalCount).toBe(1);
      });

      const req = httpMock.expectOne((r) =>
        r.url.includes(`rappels/house/${houseId}`) &&
        r.params.get('pageNumber') === '1' &&
        r.params.get('pageSize') === '10'
      );
      expect(req.request.method).toBe('GET');
      req.flush(mockResponse);
    });
  });
});
```

---

## Step 7: Error Handling Best Practices

### File: `src/app/services/error-handler.service.ts`

```typescript
import { Injectable } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { throwError } from 'rxjs';

export interface ErrorResponse {
  message: string;
  statusCode: number;
  details?: any;
}

@Injectable({
  providedIn: 'root'
})
export class ErrorHandlerService {
  handleError(error: HttpErrorResponse): ErrorResponse {
    let errorResponse: ErrorResponse = {
      message: 'An unexpected error occurred',
      statusCode: error.status || 500,
      details: error.error
    };

    if (error.status === 0) {
      errorResponse.message = 'Network error. Please check your connection.';
    } else if (error.status === 400) {
      errorResponse.message = error.error?.message || 'Invalid request';
    } else if (error.status === 401) {
      errorResponse.message = 'Unauthorized. Please login.';
    } else if (error.status === 403) {
      errorResponse.message = 'Access denied.';
    } else if (error.status === 404) {
      errorResponse.message = 'Resource not found.';
    } else if (error.status === 500) {
      errorResponse.message = 'Server error. Please try again later.';
    }

    return errorResponse;
  }
}
```

### Enhanced Service with Error Handling

```typescript
import { catchError } from 'rxjs/operators';
import { ErrorHandlerService } from './error-handler.service';

export class RappelServiceWithErrorHandling {
  constructor(
    private http: HttpClient,
    private errorHandler: ErrorHandlerService
  ) {}

  createRappel(residenceId: string, dto: CreateRappelDto): Observable<RappelDto> {
    return this.http
      .post<RappelDto>(`/api/residences/${residenceId}/rappels`, dto)
      .pipe(
        catchError((error) => {
          const errorResponse = this.errorHandler.handleError(error);
          return throwError(() => errorResponse);
        })
      );
  }
}
```

---

## Step 8: HTTP Interceptor for Common Headers

### File: `src/app/interceptors/auth.interceptor.ts`

```typescript
import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from '../services/auth.service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private authService: AuthService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    const token = this.authService.getToken();

    if (token) {
      request = request.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`
        }
      });
    }

    return next.handle(request);
  }
}
```

### Register Interceptor in Module

```typescript
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthInterceptor } from './interceptors/auth.interceptor';

@NgModule({
  // ...
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    }
  ]
})
export class AppModule {}
```

---

## Step 9: Usage in Parent Component

### File: `src/app/components/residence-dashboard/residence-dashboard.component.ts`

```typescript
import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { RappelService } from '../../services/rappel.service';
import { RappelCreateComponent } from '../rappel-create/rappel-create.component';
import { RappelEditComponent } from '../rappel-edit/rappel-edit.component';
import { RappelDto } from '../../models/rappel.model';

@Component({
  selector: 'app-residence-dashboard',
  templateUrl: './residence-dashboard.component.html',
  styleUrls: ['./residence-dashboard.component.scss']
})
export class ResidenceDashboardComponent implements OnInit {
  residenceId: string = '';

  constructor(
    private rappelService: RappelService,
    private dialog: MatDialog
  ) {}

  ngOnInit(): void {
    // Load residence ID from route or parent component
  }

  openCreateRappelDialog(): void {
    const dialogRef = this.dialog.open(RappelCreateComponent, {
      width: '500px',
      data: { residenceId: this.residenceId }
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        console.log('Rappel created:', result);
        // Refresh rappels list
      }
    });
  }

  openEditRappelDialog(rappel: RappelDto): void {
    const dialogRef = this.dialog.open(RappelEditComponent, {
      width: '500px',
      data: { rappel }
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        console.log('Rappel updated:', result);
        // Refresh rappels list
      }
    });
  }

  deleteRappel(rappelId: string): void {
    if (confirm('Are you sure you want to delete this rappel?')) {
      this.rappelService.deleteRappel(rappelId).subscribe({
        next: () => {
          console.log('Rappel deleted');
          // Refresh rappels list
        },
        error: (err) => {
          console.error('Error deleting rappel:', err);
        }
      });
    }
  }
}
```

---

## Step 10: Styling (SCSS)

### File: `src/app/styles/rappel.scss`

```scss
.rappel-container {
  padding: 20px;

  .error-message {
    color: #d32f2f;
    padding: 12px;
    margin-bottom: 16px;
    border: 1px solid #d32f2f;
    border-radius: 4px;
    background-color: #ffebee;
  }

  .full-width {
    width: 100%;
    margin-bottom: 16px;
  }

  .rappel-table {
    width: 100%;
    margin-top: 20px;

    .unpaid {
      color: #d32f2f;
      font-weight: bold;
    }

    .paid {
      color: #388e3c;
      font-weight: bold;
    }
  }

  mat-spinner {
    margin: 20px auto;
  }
}

// Dialog styling
mat-dialog-content {
  min-width: 400px;
  padding: 20px;
}

mat-dialog-actions {
  padding-top: 20px;

  button {
    margin-left: 8px;
  }
}
```

---

## Complete File Structure

```
src/
├── app/
│   ├── models/
│   │   ├── rappel.model.ts
│   │   ├── index.ts
│   │   └── ...
│   ├── services/
│   │   ├── rappel.service.ts
│   │   ├── error-handler.service.ts
│   │   ├── auth.service.ts
│   │   └── ...
│   ├── components/
│   │   ├── rappel-list/
│   │   │   ├── rappel-list.component.ts
│   │   │   ├── rappel-list.component.html
│   │   │   └── rappel-list.component.scss
│   │   ├── rappel-create/
│   │   │   ├── rappel-create.component.ts
│   │   │   ├── rappel-create.component.html
│   │   │   └── rappel-create.component.scss
│   │   ├── rappel-edit/
│   │   │   ├── rappel-edit.component.ts
│   │   │   ├── rappel-edit.component.html
│   │   │   └── rappel-edit.component.scss
│   │   ├── rappel-by-house/
│   │   │   ├── rappel-by-house.component.ts
│   │   │   ├── rappel-by-house.component.html
│   │   │   └── rappel-by-house.component.scss
│   │   └── ...
│   ├── interceptors/
│   │   ├── auth.interceptor.ts
│   │   └── ...
│   ├── styles/
│   │   ├── rappel.scss
│   │   └── ...
│   ├── app.module.ts
│   └── app.component.ts
└── ...
```

---

## API Endpoint Reference

### 1. Create Rappel
```http
POST /api/residences/{residenceId}/rappels
Content-Type: application/json

{
  "houseId": "550e8400-e29b-41d4-a716-446655440001",
  "amount": 200.00,
  "notes": "Optional notes"
}
```

**Response (201 Created):**
```json
{
  "id": "550e8400-e29b-41d4-a716-446655440002",
  "houseId": "550e8400-e29b-41d4-a716-446655440001",
  "amount": 200.00,
  "status": 0,
  "notes": "Optional notes",
  "paymentDate": null,
  "createdAt": "2024-01-20T10:00:00Z"
}
```

### 2. Get Rappel by ID
```http
GET /api/residences/{residenceId}/rappels/{id}
```

### 3. Update Rappel
```http
PUT /api/residences/{residenceId}/rappels/{id}
Content-Type: application/json

{
  "amount": 250.00,
  "status": 1,
  "notes": "Updated notes"
}
```

### 4. Delete Rappel
```http
DELETE /api/residences/{residenceId}/rappels/{id}
```

### 5. Get Rappels by House
```http
GET /api/residences/{residenceId}/rappels/house/{houseId}?pageNumber=1&pageSize=10
```

### 6. Get Rappels by Residence
```http
GET /api/residences/{residenceId}/rappels?pageNumber=1&pageSize=10
```

---

## Common Patterns & Best Practices

### 1. Observable Subscription Management
```typescript
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

export class RappelListComponent implements OnInit, OnDestroy {
  private destroy$ = new Subject<void>();

  ngOnInit(): void {
    this.rappelService
      .getRappelsByResidence(this.residenceId)
      .pipe(takeUntil(this.destroy$))
      .subscribe(...);
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
```

### 2. Caching Strategy
```typescript
@Injectable({ providedIn: 'root' })
export class RappelServiceWithCache {
  private cache = new Map<string, Observable<RappelDto>>();

  getRappelById(rappelId: string): Observable<RappelDto> {
    if (!this.cache.has(rappelId)) {
      this.cache.set(
        rappelId,
        this.http.get<RappelDto>(`/api/residences/rappels/${rappelId}`).pipe(
          shareReplay(1)
        )
      );
    }
    return this.cache.get(rappelId)!;
  }
}
```

### 3. Retry Logic
```typescript
import { retry, catchError } from 'rxjs/operators';

getRappelsByResidence(residenceId: string): Observable<PaginatedRappelsResponse> {
  return this.http
    .get<PaginatedRappelsResponse>(
      `/api/residences/${residenceId}/rappels`
    )
    .pipe(
      retry(2), // Retry 2 times on failure
      catchError(this.handleError)
    );
}
```

---

## Troubleshooting

### Issue: CORS Error
**Solution:** Ensure backend has CORS configured for your frontend URL

### Issue: 401 Unauthorized
**Solution:** Check token is being sent in Authorization header via interceptor

### Issue: Type Errors
**Solution:** Ensure TypeScript models match backend response DTOs

### Issue: Pagination Not Working
**Solution:** Verify `pageNumber` starts from 1 (not 0)

---

## Summary

You now have a complete Angular service implementation for the Rappel API endpoints including:

✅ Service class with all endpoint methods
✅ TypeScript models and interfaces
✅ Component examples (list, create, edit, by-house)
✅ Form handling with validation
✅ Pagination support
✅ Error handling
✅ Unit testing examples
✅ HTTP interceptors
✅ Best practices and patterns
✅ Complete file structure
✅ Styling examples

This guide provides everything needed to integrate the Rappel API into your Angular application!

