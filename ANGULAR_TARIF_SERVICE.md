# Angular Tarif Service Implementation Guide

## Overview

This guide provides step-by-step instructions to create an Angular service that communicates with the Tariff API endpoints from the backend Residence application.

## Table of Contents

1. [Service Structure](#service-structure)
2. [Models/Interfaces](#modelsinterfaces)
3. [Service Implementation](#service-implementation)
4. [Module Configuration](#module-configuration)
5. [Component Usage](#component-usage)
6. [Advanced Patterns](#advanced-patterns)
7. [Error Handling](#error-handling)

---

## Service Structure

### Project Structure
```
src/
├── app/
│   ├── services/
│   │   └── tarif/
│   │       ├── tarif.service.ts
│   │       └── tarif.service.spec.ts
│   ├── models/
│   │   └── tarif.model.ts
│   ├── components/
│   │   ├── tarif-list/
│   │   ├── tarif-create/
│   │   ├── tarif-edit/
│   │   └── tarif-history/
│   └── app.module.ts
```

---

## Models/Interfaces

### Step 1: Create Tarif Models
**File:** `src/app/models/tarif.model.ts`

```typescript
/**
 * Tarif response DTO
 */
export interface TarifDto {
  id: string;
  residenceId: string;
  description: string;
  amount: number;
  currency: string;
  effectiveDate: Date | string;
  endDate: Date | string | null;
  isActive: boolean;
  notes?: string;
  createdAt: Date | string;
  updatedAt: Date | string | null;
}

/**
 * Create Tarif Request DTO
 */
export interface CreateTarifDto {
  description: string;
  amount: number;
  currency: string;
  effectiveDate: Date | string;
  notes?: string;
}

/**
 * Update Tarif Request DTO
 */
export interface UpdateTarifDto {
  description?: string;
  amount?: number;
  currency?: string;
  notes?: string;
  changeReason?: string;
}

/**
 * Tarif History response DTO
 */
export interface TarifHistoryDto {
  id: string;
  tarifId: string;
  residenceId: string;
  previousAmount: number;
  newAmount: number;
  previousDescription: string;
  newDescription: string;
  effectiveDate: Date | string;
  changedBy: string;
  changeReason?: string;
  changedAt: Date | string;
}

/**
 * Paginated response wrapper
 */
export interface PagedResult<T> {
  data: T[];
  pageNumber: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
}

/**
 * API Error response
 */
export interface ApiError {
  message: string;
  statusCode: number;
  details?: string;
}
```

---

## Service Implementation

### Step 2: Create Tarif Service
**File:** `src/app/services/tarif/tarif.service.ts`

```typescript
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError, BehaviorSubject } from 'rxjs';
import { 
  map, 
  catchError, 
  tap, 
  shareReplay,
  finalize,
  startWith
} from 'rxjs/operators';

import {
  TarifDto,
  CreateTarifDto,
  UpdateTarifDto,
  TarifHistoryDto,
  ApiError
} from '../../models/tarif.model';

@Injectable({
  providedIn: 'root'
})
export class TarifService {
  private readonly apiUrl = '/api/residences';

  // Loading state management
  private loadingSubject = new BehaviorSubject<boolean>(false);
  public loading$ = this.loadingSubject.asObservable();

  // Error state management
  private errorSubject = new BehaviorSubject<ApiError | null>(null);
  public error$ = this.errorSubject.asObservable();

  constructor(private http: HttpClient) {}

  /**
   * Create a new tariff for a residence
   */
  createTarif(residenceId: string, dto: CreateTarifDto): Observable<TarifDto> {
    return this.executeRequest(() =>
      this.http.post<TarifDto>(
        `${this.apiUrl}/${residenceId}/tarifs`,
        this.normalizeDates(dto)
      )
    );
  }

  /**
   * Get tariff by ID
   */
  getTarifById(residenceId: string, tarifId: string): Observable<TarifDto> {
    return this.executeRequest(() =>
      this.http.get<TarifDto>(
        `${this.apiUrl}/${residenceId}/tarifs/${tarifId}`
      ).pipe(
        map(tarif => this.normalizeTarifDates(tarif))
      )
    );
  }

  /**
   * Get all tariffs for a residence (both active and inactive)
   */
  getTarifsByResidence(residenceId: string): Observable<TarifDto[]> {
    return this.executeRequest(() =>
      this.http.get<TarifDto[]>(
        `${this.apiUrl}/${residenceId}/tarifs`
      ).pipe(
        map(tarifs => tarifs.map(t => this.normalizeTarifDates(t))),
        shareReplay(1)
      )
    );
  }

  /**
   * Get current active tariff for a residence
   */
  getCurrentTarif(residenceId: string): Observable<TarifDto> {
    return this.executeRequest(() =>
      this.http.get<TarifDto>(
        `${this.apiUrl}/${residenceId}/tarifs/current/active`
      ).pipe(
        map(tarif => this.normalizeTarifDates(tarif))
      )
    );
  }

  /**
   * Update a tariff
   * When updating, a history entry is automatically created
   */
  updateTarif(
    residenceId: string,
    tarifId: string,
    dto: UpdateTarifDto
  ): Observable<TarifDto> {
    return this.executeRequest(() =>
      this.http.put<TarifDto>(
        `${this.apiUrl}/${residenceId}/tarifs/${tarifId}`,
        dto
      ).pipe(
        map(tarif => this.normalizeTarifDates(tarif))
      )
    );
  }

  /**
   * Delete a tariff (soft delete)
   */
  deleteTarif(residenceId: string, tarifId: string): Observable<void> {
    return this.executeRequest(() =>
      this.http.delete<void>(
        `${this.apiUrl}/${residenceId}/tarifs/${tarifId}`
      )
    );
  }

  /**
   * Get history of changes for a specific tariff
   */
  getTarifHistory(residenceId: string, tarifId: string): Observable<TarifHistoryDto[]> {
    return this.executeRequest(() =>
      this.http.get<TarifHistoryDto[]>(
        `${this.apiUrl}/${residenceId}/tarifs/${tarifId}/history`
      ).pipe(
        map(history => history.map(h => this.normalizeHistoryDates(h))),
        shareReplay(1)
      )
    );
  }

  /**
   * Get all tariff changes for a residence
   */
  getResidenceTarifHistory(residenceId: string): Observable<TarifHistoryDto[]> {
    return this.executeRequest(() =>
      this.http.get<TarifHistoryDto[]>(
        `${this.apiUrl}/${residenceId}/tarifs/history/all`
      ).pipe(
        map(history => history.map(h => this.normalizeHistoryDates(h))),
        shareReplay(1)
      )
    );
  }

  /**
   * Get tariff changes within a date range
   */
  getTarifHistoryByDateRange(
    residenceId: string,
    startDate: Date,
    endDate: Date
  ): Observable<TarifHistoryDto[]> {
    const params = new HttpParams()
      .set('startDate', startDate.toISOString())
      .set('endDate', endDate.toISOString());

    return this.executeRequest(() =>
      this.http.get<TarifHistoryDto[]>(
        `${this.apiUrl}/${residenceId}/tarifs/history/range`,
        { params }
      ).pipe(
        map(history => history.map(h => this.normalizeHistoryDates(h))),
        shareReplay(1)
      )
    );
  }

  /**
   * Export tariff history as CSV
   */
  exportTarifHistoryCsv(residenceId: string): Observable<Blob> {
    return this.http.get(
      `${this.apiUrl}/${residenceId}/tarifs/history/export`,
      { responseType: 'blob' }
    );
  }

  /**
   * Get loading state
   */
  isLoading(): Observable<boolean> {
    return this.loading$;
  }

  /**
   * Get error state
   */
  getError(): Observable<ApiError | null> {
    return this.error$;
  }

  /**
   * Clear error state
   */
  clearError(): void {
    this.errorSubject.next(null);
  }

  // ============= Private Helper Methods =============

  /**
   * Execute HTTP request with loading and error handling
   */
  private executeRequest<T>(
    request: () => Observable<T>
  ): Observable<T> {
    this.loadingSubject.next(true);
    this.errorSubject.next(null);

    return request().pipe(
      tap(() => this.errorSubject.next(null)),
      catchError((error: HttpErrorResponse) => {
        const apiError: ApiError = {
          message: error.error?.message || 'An error occurred',
          statusCode: error.status,
          details: error.error?.details
        };
        this.errorSubject.next(apiError);
        return throwError(() => apiError);
      }),
      finalize(() => this.loadingSubject.next(false))
    );
  }

  /**
   * Normalize date strings to Date objects
   */
  private normalizeDates(dto: CreateTarifDto): CreateTarifDto {
    return {
      ...dto,
      effectiveDate: typeof dto.effectiveDate === 'string' 
        ? new Date(dto.effectiveDate).toISOString()
        : dto.effectiveDate.toISOString()
    };
  }

  /**
   * Normalize tarif dates
   */
  private normalizeTarifDates(tarif: TarifDto): TarifDto {
    return {
      ...tarif,
      effectiveDate: new Date(tarif.effectiveDate),
      endDate: tarif.endDate ? new Date(tarif.endDate) : null,
      createdAt: new Date(tarif.createdAt),
      updatedAt: tarif.updatedAt ? new Date(tarif.updatedAt) : null
    };
  }

  /**
   * Normalize history dates
   */
  private normalizeHistoryDates(history: TarifHistoryDto): TarifHistoryDto {
    return {
      ...history,
      effectiveDate: new Date(history.effectiveDate),
      changedAt: new Date(history.changedAt)
    };
  }
}
```

### Step 3: Create Service Spec (Tests)
**File:** `src/app/services/tarif/tarif.service.spec.ts`

```typescript
import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { TarifService } from './tarif.service';
import { TarifDto, CreateTarifDto } from '../../models/tarif.model';

describe('TarifService', () => {
  let service: TarifService;
  let httpMock: HttpTestingController;
  const residenceId = 'test-residence-id';
  const tarifId = 'test-tarif-id';
  const apiUrl = '/api/residences';

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [TarifService]
    });
    service = TestBed.inject(TarifService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should create a tarif', () => {
    const mockTarif: TarifDto = {
      id: tarifId,
      residenceId: residenceId,
      description: 'Monthly fee',
      amount: 100,
      currency: 'USD',
      effectiveDate: '2024-03-01',
      endDate: null,
      isActive: true,
      createdAt: '2024-03-01T00:00:00Z',
      updatedAt: null
    };

    const dto: CreateTarifDto = {
      description: 'Monthly fee',
      amount: 100,
      currency: 'USD',
      effectiveDate: new Date('2024-03-01')
    };

    service.createTarif(residenceId, dto).subscribe(tarif => {
      expect(tarif.id).toBe(tarifId);
      expect(tarif.amount).toBe(100);
    });

    const req = httpMock.expectOne(`${apiUrl}/${residenceId}/tarifs`);
    expect(req.request.method).toBe('POST');
    req.flush(mockTarif);
  });

  it('should get current tarif', () => {
    const mockTarif: TarifDto = {
      id: tarifId,
      residenceId: residenceId,
      description: 'Monthly fee',
      amount: 150,
      currency: 'USD',
      effectiveDate: '2024-03-01',
      endDate: null,
      isActive: true,
      createdAt: '2024-03-01T00:00:00Z',
      updatedAt: null
    };

    service.getCurrentTarif(residenceId).subscribe(tarif => {
      expect(tarif.isActive).toBe(true);
      expect(tarif.amount).toBe(150);
    });

    const req = httpMock.expectOne(`${apiUrl}/${residenceId}/tarifs/current/active`);
    expect(req.request.method).toBe('GET');
    req.flush(mockTarif);
  });
});
```

---

## Module Configuration

### Step 4: Configure HttpClientModule
**File:** `src/app/app.module.ts`

```typescript
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppComponent } from './app.component';
import { TarifService } from './services/tarif/tarif.service';
// Import your components and other services here

@NgModule({
  declarations: [
    AppComponent,
    // Declare your components here
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    HttpClientModule,
    // Import other modules here
  ],
  providers: [
    TarifService,
    // Other providers
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
```

Or if using **standalone components** (Angular 14+):

```typescript
import { importProvidersFrom } from '@angular/core';
import { bootstrapApplication } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { AppComponent } from './app/app.component';
import { TarifService } from './app/services/tarif/tarif.service';

bootstrapApplication(AppComponent, {
  providers: [
    importProvidersFrom(HttpClientModule),
    TarifService
  ]
});
```

---

## Component Usage

### Step 5: Create Tarif List Component
**File:** `src/app/components/tarif-list/tarif-list.component.ts`

```typescript
import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

import { TarifService } from '../../services/tarif/tarif.service';
import { TarifDto, ApiError } from '../../models/tarif.model';

@Component({
  selector: 'app-tarif-list',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="tarif-container">
      <h2>Tariffs Management</h2>

      <!-- Loading Indicator -->
      <div *ngIf="isLoading$ | async" class="loading">
        <p>Loading tariffs...</p>
      </div>

      <!-- Error Message -->
      <div *ngIf="error$ | async as error" class="error-message">
        <p>{{ error.message }}</p>
      </div>

      <!-- Current Tariff -->
      <div *ngIf="currentTarif" class="current-tarif">
        <h3>Current Active Tariff</h3>
        <p><strong>Description:</strong> {{ currentTarif.description }}</p>
        <p><strong>Amount:</strong> {{ currentTarif.amount }} {{ currentTarif.currency }}</p>
        <p><strong>Effective Date:</strong> {{ currentTarif.effectiveDate | date }}</p>
        <button (click)="editTarif(currentTarif)">Edit</button>
      </div>

      <!-- All Tariffs List -->
      <div class="tarifs-list">
        <h3>All Tariffs</h3>
        <table>
          <thead>
            <tr>
              <th>Description</th>
              <th>Amount</th>
              <th>Currency</th>
              <th>Effective Date</th>
              <th>Status</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            <tr *ngFor="let tarif of tarifs">
              <td>{{ tarif.description }}</td>
              <td>{{ tarif.amount }}</td>
              <td>{{ tarif.currency }}</td>
              <td>{{ tarif.effectiveDate | date }}</td>
              <td>
                <span [class.active]="tarif.isActive" [class.inactive]="!tarif.isActive">
                  {{ tarif.isActive ? 'Active' : 'Inactive' }}
                </span>
              </td>
              <td>
                <button (click)="viewHistory(tarif.id)">History</button>
                <button (click)="editTarif(tarif)">Edit</button>
                <button (click)="deleteTarif(tarif.id)">Delete</button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>

      <button (click)="openCreateModal()" class="create-btn">
        Create New Tariff
      </button>
    </div>
  `,
  styles: [`
    .tarif-container {
      padding: 20px;
    }

    .loading, .error-message {
      padding: 10px;
      margin-bottom: 20px;
      border-radius: 4px;
    }

    .loading {
      background-color: #e3f2fd;
      color: #1976d2;
    }

    .error-message {
      background-color: #ffebee;
      color: #c62828;
    }

    .current-tarif {
      background-color: #f0f9ff;
      padding: 15px;
      border-radius: 4px;
      margin-bottom: 20px;
      border-left: 4px solid #1976d2;
    }

    table {
      width: 100%;
      border-collapse: collapse;
    }

    th, td {
      padding: 12px;
      text-align: left;
      border-bottom: 1px solid #ddd;
    }

    th {
      background-color: #f5f5f5;
      font-weight: bold;
    }

    .active {
      color: #4caf50;
      font-weight: bold;
    }

    .inactive {
      color: #999;
    }

    button {
      padding: 6px 12px;
      margin-right: 5px;
      cursor: pointer;
      border: 1px solid #ddd;
      border-radius: 4px;
      background-color: #f5f5f5;
    }

    button:hover {
      background-color: #e0e0e0;
    }

    .create-btn {
      background-color: #1976d2;
      color: white;
      margin-top: 20px;
    }

    .create-btn:hover {
      background-color: #1565c0;
    }
  `]
})
export class TarifListComponent implements OnInit, OnDestroy {
  tarifs: TarifDto[] = [];
  currentTarif: TarifDto | null = null;
  isLoading$ = this.tarifService.isLoading();
  error$ = this.tarifService.error$;

  private destroy$ = new Subject<void>();
  private residenceId = 'your-residence-id'; // Get from route or service

  constructor(private tarifService: TarifService) {}

  ngOnInit(): void {
    this.loadTarifs();
    this.loadCurrentTarif();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  loadTarifs(): void {
    this.tarifService.getTarifsByResidence(this.residenceId)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (tarifs) => {
          this.tarifs = tarifs;
        },
        error: (err) => {
          console.error('Error loading tarifs:', err);
        }
      });
  }

  loadCurrentTarif(): void {
    this.tarifService.getCurrentTarif(this.residenceId)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (tarif) => {
          this.currentTarif = tarif;
        },
        error: (err) => {
          console.log('No active tarif found');
        }
      });
  }

  editTarif(tarif: TarifDto): void {
    console.log('Edit tarif:', tarif);
    // Open edit modal/dialog
  }

  viewHistory(tarifId: string): void {
    this.tarifService.getTarifHistory(this.residenceId, tarifId)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (history) => {
          console.log('Tarif history:', history);
          // Open history modal
        }
      });
  }

  deleteTarif(tarifId: string): void {
    if (confirm('Are you sure you want to delete this tariff?')) {
      this.tarifService.deleteTarif(this.residenceId, tarifId)
        .pipe(takeUntil(this.destroy$))
        .subscribe({
          next: () => {
            this.loadTarifs();
            alert('Tariff deleted successfully');
          },
          error: (err) => {
            console.error('Error deleting tarif:', err);
          }
        });
    }
  }

  openCreateModal(): void {
    console.log('Open create modal');
    // Open create modal/dialog
  }
}
```

### Step 6: Create Tarif Create Component
**File:** `src/app/components/tarif-create/tarif-create.component.ts`

```typescript
import { Component, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

import { TarifService } from '../../services/tarif/tarif.service';
import { CreateTarifDto } from '../../models/tarif.model';

@Component({
  selector: 'app-tarif-create',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  template: `
    <div class="dialog-container">
      <h2>Create New Tariff</h2>

      <form [formGroup]="form" (ngSubmit)="onSubmit()">
        <div class="form-group">
          <label for="description">Description</label>
          <input
            id="description"
            type="text"
            formControlName="description"
            placeholder="e.g., Monthly maintenance fee"
            required
          />
          <span *ngIf="form.get('description')?.invalid && form.get('description')?.touched" 
                class="error">
            Description is required
          </span>
        </div>

        <div class="form-group">
          <label for="amount">Amount</label>
          <input
            id="amount"
            type="number"
            formControlName="amount"
            placeholder="0.00"
            step="0.01"
            required
          />
          <span *ngIf="form.get('amount')?.invalid && form.get('amount')?.touched" 
                class="error">
            Amount is required and must be greater than 0
          </span>
        </div>

        <div class="form-group">
          <label for="currency">Currency</label>
          <select id="currency" formControlName="currency">
            <option value="USD">USD</option>
            <option value="EUR">EUR</option>
            <option value="GBP">GBP</option>
            <option value="CAD">CAD</option>
          </select>
        </div>

        <div class="form-group">
          <label for="effectiveDate">Effective Date</label>
          <input
            id="effectiveDate"
            type="date"
            formControlName="effectiveDate"
            required
          />
          <span *ngIf="form.get('effectiveDate')?.invalid && form.get('effectiveDate')?.touched" 
                class="error">
            Effective date is required
          </span>
        </div>

        <div class="form-group">
          <label for="notes">Notes (Optional)</label>
          <textarea
            id="notes"
            formControlName="notes"
            placeholder="Add any notes about this tariff"
            rows="3"
          ></textarea>
        </div>

        <div class="form-actions">
          <button type="button" (click)="onCancel()" class="cancel-btn">
            Cancel
          </button>
          <button type="submit" [disabled]="form.invalid || isSubmitting" class="submit-btn">
            {{ isSubmitting ? 'Creating...' : 'Create Tariff' }}
          </button>
        </div>

        <div *ngIf="errorMessage" class="error-message">
          {{ errorMessage }}
        </div>
      </form>
    </div>
  `,
  styles: [`
    .dialog-container {
      padding: 20px;
      min-width: 400px;
    }

    .form-group {
      margin-bottom: 20px;
      display: flex;
      flex-direction: column;
    }

    label {
      margin-bottom: 5px;
      font-weight: bold;
    }

    input, select, textarea {
      padding: 8px;
      border: 1px solid #ddd;
      border-radius: 4px;
      font-size: 14px;
    }

    input:focus, select:focus, textarea:focus {
      outline: none;
      border-color: #1976d2;
      box-shadow: 0 0 0 2px rgba(25, 118, 210, 0.1);
    }

    .error {
      color: #c62828;
      font-size: 12px;
      margin-top: 4px;
    }

    .error-message {
      background-color: #ffebee;
      color: #c62828;
      padding: 10px;
      border-radius: 4px;
      margin-top: 10px;
    }

    .form-actions {
      display: flex;
      justify-content: flex-end;
      gap: 10px;
      margin-top: 20px;
    }

    button {
      padding: 8px 16px;
      border: none;
      border-radius: 4px;
      cursor: pointer;
      font-size: 14px;
    }

    .cancel-btn {
      background-color: #f5f5f5;
      color: #333;
    }

    .cancel-btn:hover {
      background-color: #e0e0e0;
    }

    .submit-btn {
      background-color: #1976d2;
      color: white;
    }

    .submit-btn:hover:not(:disabled) {
      background-color: #1565c0;
    }

    .submit-btn:disabled {
      opacity: 0.5;
      cursor: not-allowed;
    }
  `]
})
export class TarifCreateComponent {
  form: FormGroup;
  isSubmitting = false;
  errorMessage: string | null = null;
  residenceId: string;

  constructor(
    private fb: FormBuilder,
    private tarifService: TarifService,
    public dialogRef: MatDialogRef<TarifCreateComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { residenceId: string }
  ) {
    this.residenceId = data.residenceId;
    this.form = this.createForm();
  }

  private createForm(): FormGroup {
    const today = new Date().toISOString().split('T')[0];
    return this.fb.group({
      description: ['', [Validators.required, Validators.minLength(3)]],
      amount: ['', [Validators.required, Validators.min(0.01)]],
      currency: ['USD', Validators.required],
      effectiveDate: [today, Validators.required],
      notes: ['']
    });
  }

  onSubmit(): void {
    if (this.form.invalid) {
      return;
    }

    this.isSubmitting = true;
    this.errorMessage = null;

    const dto: CreateTarifDto = this.form.value;

    this.tarifService.createTarif(this.residenceId, dto).subscribe({
      next: (tarif) => {
        this.isSubmitting = false;
        this.dialogRef.close(tarif);
      },
      error: (err) => {
        this.isSubmitting = false;
        this.errorMessage = err.message || 'Failed to create tariff';
      }
    });
  }

  onCancel(): void {
    this.dialogRef.close();
  }
}
```

### Step 7: Create Tarif History Component
**File:** `src/app/components/tarif-history/tarif-history.component.ts`

```typescript
import { Component, OnInit, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

import { TarifService } from '../../services/tarif/tarif.service';
import { TarifHistoryDto } from '../../models/tarif.model';

@Component({
  selector: 'app-tarif-history',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="history-container">
      <h2>Tariff Change History</h2>

      <div *ngIf="isLoading" class="loading">
        Loading history...
      </div>

      <div *ngIf="history.length === 0 && !isLoading" class="no-data">
        No history found
      </div>

      <div *ngIf="history.length > 0" class="history-list">
        <div *ngFor="let entry of history" class="history-item">
          <div class="history-header">
            <span class="date">{{ entry.changedAt | date: 'short' }}</span>
            <span class="changed-by">by {{ entry.changedBy }}</span>
          </div>

          <div class="history-changes">
            <div *ngIf="entry.previousAmount !== entry.newAmount" class="change-item">
              <strong>Amount:</strong>
              <span class="old">{{ entry.previousAmount }}</span>
              →
              <span class="new">{{ entry.newAmount }}</span>
            </div>

            <div *ngIf="entry.previousDescription !== entry.newDescription" class="change-item">
              <strong>Description:</strong>
              <span class="old">{{ entry.previousDescription }}</span>
              →
              <span class="new">{{ entry.newDescription }}</span>
            </div>

            <div *ngIf="entry.changeReason" class="reason">
              <strong>Reason:</strong> {{ entry.changeReason }}
            </div>
          </div>
        </div>
      </div>

      <button (click)="close()" class="close-btn">Close</button>
    </div>
  `,
  styles: [`
    .history-container {
      padding: 20px;
      min-width: 500px;
      max-height: 600px;
      overflow-y: auto;
    }

    .loading, .no-data {
      text-align: center;
      padding: 20px;
      color: #999;
    }

    .history-list {
      margin: 20px 0;
    }

    .history-item {
      border-left: 4px solid #1976d2;
      padding: 15px;
      margin-bottom: 15px;
      background-color: #f5f5f5;
      border-radius: 4px;
    }

    .history-header {
      display: flex;
      justify-content: space-between;
      margin-bottom: 10px;
      font-size: 12px;
    }

    .date {
      font-weight: bold;
      color: #1976d2;
    }

    .changed-by {
      color: #666;
    }

    .history-changes {
      margin-top: 10px;
    }

    .change-item {
      margin-bottom: 8px;
      padding: 8px;
      background-color: white;
      border-radius: 3px;
    }

    .old {
      color: #c62828;
      text-decoration: line-through;
    }

    .new {
      color: #2e7d32;
      font-weight: bold;
    }

    .reason {
      margin-top: 10px;
      padding: 8px;
      background-color: #fff3cd;
      border-radius: 3px;
      font-size: 13px;
    }

    .close-btn {
      display: block;
      margin-top: 20px;
      padding: 8px 16px;
      background-color: #f5f5f5;
      border: 1px solid #ddd;
      border-radius: 4px;
      cursor: pointer;
    }

    .close-btn:hover {
      background-color: #e0e0e0;
    }
  `]
})
export class TarifHistoryComponent implements OnInit {
  history: TarifHistoryDto[] = [];
  isLoading = false;

  constructor(
    private tarifService: TarifService,
    public dialogRef: MatDialogRef<TarifHistoryComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { residenceId: string; tarifId: string }
  ) {}

  ngOnInit(): void {
    this.loadHistory();
  }

  loadHistory(): void {
    this.isLoading = true;
    this.tarifService.getTarifHistory(this.data.residenceId, this.data.tarifId)
      .subscribe({
        next: (history) => {
          this.history = history;
          this.isLoading = false;
        },
        error: (err) => {
          console.error('Error loading history:', err);
          this.isLoading = false;
        }
      });
  }

  close(): void {
    this.dialogRef.close();
  }
}
```

---

## Advanced Patterns

### Step 8: State Management with NgRx (Optional)

**File:** `src/app/store/tarif/tarif.reducer.ts`

```typescript
import { createReducer, on } from '@ngrx/store';
import { TarifDto } from '../../models/tarif.model';
import * as TarifActions from './tarif.actions';

export interface TarifState {
  tarifs: TarifDto[];
  currentTarif: TarifDto | null;
  loading: boolean;
  error: string | null;
}

export const initialState: TarifState = {
  tarifs: [],
  currentTarif: null,
  loading: false,
  error: null
};

export const tarifReducer = createReducer(
  initialState,
  on(TarifActions.loadTarifs, (state) => ({
    ...state,
    loading: true,
    error: null
  })),
  on(TarifActions.loadTarifsSuccess, (state, { tarifs }) => ({
    ...state,
    tarifs,
    loading: false
  })),
  on(TarifActions.loadTarifsFailure, (state, { error }) => ({
    ...state,
    loading: false,
    error
  }))
);
```

---

## Error Handling

### Step 9: HTTP Interceptor for Error Handling

**File:** `src/app/interceptors/error.interceptor.ts`

```typescript
import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  constructor() {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError((error: HttpErrorResponse) => {
        let errorMessage = 'An unknown error occurred';

        if (error.error instanceof ErrorEvent) {
          // Client-side error
          errorMessage = `Error: ${error.error.message}`;
        } else {
          // Server-side error
          errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
        }

        console.error(errorMessage);
        return throwError(() => new Error(errorMessage));
      })
    );
  }
}
```

Register in `app.module.ts`:

```typescript
providers: [
  {
    provide: HTTP_INTERCEPTORS,
    useClass: ErrorInterceptor,
    multi: true
  }
]
```

---

## Usage Summary

### Quick Start Checklist

1. ✅ Create models file (`tarif.model.ts`)
2. ✅ Create service file (`tarif.service.ts`)
3. ✅ Configure HttpClientModule in `app.module.ts`
4. ✅ Create list component (`tarif-list.component.ts`)
5. ✅ Create create component (`tarif-create.component.ts`)
6. ✅ Create history component (`tarif-history.component.ts`)
7. ✅ (Optional) Set up state management with NgRx
8. ✅ (Optional) Create error interceptor

### Common Service Usage Patterns

```typescript
// Get current tariff
this.tarifService.getCurrentTarif(residenceId).subscribe(
  tarif => console.log(tarif)
);

// Create new tariff
this.tarifService.createTarif(residenceId, {
  description: 'Monthly fee',
  amount: 150,
  currency: 'USD',
  effectiveDate: new Date()
}).subscribe(
  tarif => console.log('Created:', tarif)
);

// Update tariff with reason
this.tarifService.updateTarif(residenceId, tarifId, {
  amount: 160,
  changeReason: 'Service enhancement'
}).subscribe(
  tarif => console.log('Updated:', tarif)
);

// Get history
this.tarifService.getTarifHistory(residenceId, tarifId).subscribe(
  history => console.log(history)
);

// Get history by date range
this.tarifService.getTarifHistoryByDateRange(
  residenceId,
  new Date('2024-01-01'),
  new Date('2024-12-31')
).subscribe(
  history => console.log(history)
);
```

---

## Best Practices

1. **Always unsubscribe** - Use `takeUntil` with destroy subject
2. **Handle errors** - Always provide error callbacks
3. **Loading states** - Show loading indicators during API calls
4. **Type safety** - Use interfaces/models for all data
5. **Reactive Forms** - Use ReactiveFormsModule for forms
6. **Share data** - Use `shareReplay()` to prevent multiple HTTP calls
7. **Date handling** - Normalize dates from ISO strings
8. **Unit tests** - Test service methods with HttpClientTestingModule

---

## Next Steps

1. Integrate with your existing Angular app
2. Create routing for tarif management pages
3. Add Material Design components (MatDialog, MatTable, etc.)
4. Implement animations for better UX
5. Add data export functionality
6. Create dashboards to visualize tariff trends
