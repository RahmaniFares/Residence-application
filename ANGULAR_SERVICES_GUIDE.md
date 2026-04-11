# Angular Payment & Tarif Service Documentation

This guide provides complete instructions for implementing Payment KPI and Tarif management services in your Angular frontend.

## Table of Contents

1. [Payment KPI Service](#payment-kpi-service)
2. [Tarif Service](#tarif-service)
3. [Models & Interfaces](#models--interfaces)
4. [Usage Examples](#usage-examples)
5. [Components Implementation](#components-implementation)

---

## Payment KPI Service

### 1. Create Payment KPI Models

Create file: `src/app/models/payment-kpi.model.ts`

```typescript
export interface PaymentKpiDto {
  totalPaidAmount: number;
  totalPaidCount: number;
  totalPendingAmount: number;
  totalPendingCount: number;
  totalOverdueAmount: number;
  totalOverdueCount: number;
  collectionRate: number;
  totalExpectedAmount: number;
  outstandingBalance: number;
  averagePaymentAmount: number;
  periodStartDate: Date | null;
  periodEndDate: Date | null;
  residenceId: string;
  calculatedAt: Date;
}

export interface MonthlyPaymentSummaryDto {
  year: number;
  month: number;
  totalExpected: number;
  totalPaid: number;
  totalPending: number;
  totalPayments: number;
  paidCount: number;
  pendingCount: number;
  collectionPercentage: number;
}

export interface PaymentTrendDto {
  date: Date;
  amountPaid: number;
  amountPending: number;
  cumulativePaid: number;
  collectionRate: number;
}

export interface PaymentDto {
  id: string;
  houseId: string;
  residentId: string;
  amount: number;
  method: PaymentMethod;
  periodStart: Date;
  periodEnd: Date;
  paymentDate: Date | null;
  status: PaymentStatus;
  notes: string | null;
  createdAt: Date;
  updatedAt: Date | null;
}

export enum PaymentMethod {
  Transfer = 0,
  Cash = 1,
  Check = 2,
  CreditCard = 3,
  Other = 4
}

export enum PaymentStatus {
  Pending = 0,
  Paid = 1,
  Cancelled = 2
}
```

### 2. Create Payment KPI Service

Create file: `src/app/services/payment-kpi.service.ts`

```typescript
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import {
  PaymentKpiDto,
  MonthlyPaymentSummaryDto,
  PaymentTrendDto,
  PaymentDto
} from '../models/payment-kpi.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class PaymentKpiService {
  private readonly apiUrl = `${environment.apiUrl}/api/residences`;

  constructor(private http: HttpClient) {}

  /**
   * Get payment KPI dashboard for current year
   */
  getPaymentKpi(residenceId: string): Observable<PaymentKpiDto> {
    return this.http.get<PaymentKpiDto>(
      `${this.apiUrl}/${residenceId}/payments/kpi/dashboard`
    );
  }

  /**
   * Get payment KPI for a specific date range
   */
  getPaymentKpiByDateRange(
    residenceId: string,
    startDate: Date,
    endDate: Date
  ): Observable<PaymentKpiDto> {
    const params = new HttpParams()
      .set('startDate', startDate.toISOString())
      .set('endDate', endDate.toISOString());

    return this.http.get<PaymentKpiDto>(
      `${this.apiUrl}/${residenceId}/payments/kpi/range`,
      { params }
    );
  }

  /**
   * Get monthly payment summaries
   */
  getMonthlySummary(
    residenceId: string,
    months: number = 12
  ): Observable<MonthlyPaymentSummaryDto[]> {
    const params = new HttpParams().set('months', months.toString());

    return this.http.get<MonthlyPaymentSummaryDto[]>(
      `${this.apiUrl}/${residenceId}/payments/kpi/monthly-summary`,
      { params }
    );
  }

  /**
   * Get payment trend data for charts
   */
  getPaymentTrend(
    residenceId: string,
    startDate: Date,
    endDate: Date
  ): Observable<PaymentTrendDto[]> {
    const params = new HttpParams()
      .set('startDate', startDate.toISOString())
      .set('endDate', endDate.toISOString());

    return this.http.get<PaymentTrendDto[]>(
      `${this.apiUrl}/${residenceId}/payments/kpi/trend`,
      { params }
    );
  }
}
```

### 3. Create Payment KPI Dashboard Component

Create file: `src/app/components/payment-kpi-dashboard/payment-kpi-dashboard.component.ts`

```typescript
import { Component, OnInit, Input } from '@angular/core';
import { PaymentKpiService } from '../../services/payment-kpi.service';
import {
  PaymentKpiDto,
  MonthlyPaymentSummaryDto,
  PaymentTrendDto
} from '../../models/payment-kpi.model';

@Component({
  selector: 'app-payment-kpi-dashboard',
  templateUrl: './payment-kpi-dashboard.component.html',
  styleUrls: ['./payment-kpi-dashboard.component.scss']
})
export class PaymentKpiDashboardComponent implements OnInit {
  @Input() residenceId!: string;

  kpi: PaymentKpiDto | null = null;
  monthlySummary: MonthlyPaymentSummaryDto[] = [];
  trends: PaymentTrendDto[] = [];
  loading = false;
  selectedMonth = 12;

  constructor(private paymentKpiService: PaymentKpiService) {}

  ngOnInit(): void {
    this.loadDashboard();
  }

  loadDashboard(): void {
    this.loading = true;
    this.paymentKpiService.getPaymentKpi(this.residenceId).subscribe({
      next: (data) => {
        this.kpi = data;
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading KPI:', error);
        this.loading = false;
      }
    });

    this.loadMonthlySummary();
    this.loadTrends();
  }

  loadMonthlySummary(): void {
    this.paymentKpiService
      .getMonthlySummary(this.residenceId, this.selectedMonth)
      .subscribe({
        next: (data) => {
          this.monthlySummary = data;
        },
        error: (error) => {
          console.error('Error loading monthly summary:', error);
        }
      });
  }

  loadTrends(): void {
    const endDate = new Date();
    const startDate = new Date();
    startDate.setMonth(startDate.getMonth() - this.selectedMonth);

    this.paymentKpiService
      .getPaymentTrend(this.residenceId, startDate, endDate)
      .subscribe({
        next: (data) => {
          this.trends = data;
        },
        error: (error) => {
          console.error('Error loading trends:', error);
        }
      });
  }

  onMonthsChange(months: number): void {
    this.selectedMonth = months;
    this.loadMonthlySummary();
    this.loadTrends();
  }

  /**
   * Get status color for collection rate
   */
  getCollectionRateColor(): string {
    if (!this.kpi) return 'gray';
    if (this.kpi.collectionRate >= 90) return 'green';
    if (this.kpi.collectionRate >= 70) return 'orange';
    return 'red';
  }

  /**
   * Format currency
   */
  formatCurrency(value: number): string {
    return new Intl.NumberFormat('en-US', {
      style: 'currency',
      currency: 'USD',
      minimumFractionDigits: 2
    }).format(value);
  }

  /**
   * Format percentage
   */
  formatPercentage(value: number): string {
    return `${value.toFixed(2)}%`;
  }
}
```

Create file: `src/app/components/payment-kpi-dashboard/payment-kpi-dashboard.component.html`

```html
<div class="payment-kpi-dashboard" *ngIf="kpi">
  <div class="dashboard-header">
    <h2>Payment KPI Dashboard</h2>
    <button (click)="loadDashboard()" [disabled]="loading" class="refresh-btn">
      {{ loading ? 'Loading...' : 'Refresh' }}
    </button>
  </div>

  <!-- Main KPI Cards -->
  <div class="kpi-cards">
    <!-- Total Paid -->
    <div class="kpi-card card-primary">
      <div class="card-icon">💰</div>
      <div class="card-content">
        <h3>Total Paid</h3>
        <p class="amount">{{ formatCurrency(kpi.totalPaidAmount) }}</p>
        <p class="count">{{ kpi.totalPaidCount }} payments</p>
      </div>
    </div>

    <!-- Total Pending -->
    <div class="kpi-card card-warning">
      <div class="card-icon">⏳</div>
      <div class="card-content">
        <h3>Pending Payments</h3>
        <p class="amount">{{ formatCurrency(kpi.totalPendingAmount) }}</p>
        <p class="count">{{ kpi.totalPendingCount }} payments</p>
      </div>
    </div>

    <!-- Total Overdue -->
    <div class="kpi-card card-danger">
      <div class="card-icon">⚠️</div>
      <div class="card-content">
        <h3>Overdue Payments</h3>
        <p class="amount">{{ formatCurrency(kpi.totalOverdueAmount) }}</p>
        <p class="count">{{ kpi.totalOverdueCount }} payments</p>
      </div>
    </div>

    <!-- Collection Rate -->
    <div class="kpi-card card-info">
      <div class="card-icon">📊</div>
      <div class="card-content">
        <h3>Collection Rate</h3>
        <p class="amount">{{ formatPercentage(kpi.collectionRate) }}</p>
        <div class="progress-bar">
          <div
            class="progress-fill"
            [style.width.%]="kpi.collectionRate"
            [ngClass]="'progress-' + (kpi.collectionRate >= 90 ? 'success' : kpi.collectionRate >= 70 ? 'warning' : 'danger')"
          ></div>
        </div>
      </div>
    </div>
  </div>

  <!-- Summary Row -->
  <div class="summary-row">
    <div class="summary-item">
      <label>Total Expected:</label>
      <span>{{ formatCurrency(kpi.totalExpectedAmount) }}</span>
    </div>
    <div class="summary-item">
      <label>Outstanding Balance:</label>
      <span>{{ formatCurrency(kpi.outstandingBalance) }}</span>
    </div>
    <div class="summary-item">
      <label>Average Payment:</label>
      <span>{{ formatCurrency(kpi.averagePaymentAmount) }}</span>
    </div>
  </div>

  <!-- Monthly Summary -->
  <div class="section-box">
    <div class="section-header">
      <h3>Monthly Summary</h3>
      <select [(ngModel)]="selectedMonth" (change)="onMonthsChange(selectedMonth)">
        <option [value]="3">Last 3 months</option>
        <option [value]="6">Last 6 months</option>
        <option [value]="12">Last 12 months</option>
        <option [value]="24">Last 24 months</option>
      </select>
    </div>

    <div class="monthly-table">
      <table>
        <thead>
          <tr>
            <th>Month</th>
            <th>Expected</th>
            <th>Paid</th>
            <th>Pending</th>
            <th>Collection %</th>
            <th>Total</th>
          </tr>
        </thead>
        <tbody>
          <tr *ngFor="let month of monthlySummary">
            <td>{{ month.month }}/{{ month.year }}</td>
            <td>{{ formatCurrency(month.totalExpected) }}</td>
            <td class="success">{{ formatCurrency(month.totalPaid) }}</td>
            <td class="warning">{{ formatCurrency(month.totalPending) }}</td>
            <td>{{ formatPercentage(month.collectionPercentage) }}</td>
            <td>{{ month.totalPayments }}</td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>

  <!-- Payment Trend Chart (requires ng2-charts) -->
  <div class="section-box" *ngIf="trends.length > 0">
    <h3>Payment Trend</h3>
    <div class="trend-chart">
      <!-- Add chart library of your choice (Chart.js, etc.) -->
      <p>Chart data available: {{ trends.length }} data points</p>
    </div>
  </div>
</div>
```

Create file: `src/app/components/payment-kpi-dashboard/payment-kpi-dashboard.component.scss`

```scss
.payment-kpi-dashboard {
  padding: 20px;
  background: #f5f5f5;
  border-radius: 8px;

  .dashboard-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 30px;

    h2 {
      font-size: 28px;
      font-weight: 600;
      margin: 0;
    }

    .refresh-btn {
      padding: 8px 16px;
      background: #007bff;
      color: white;
      border: none;
      border-radius: 4px;
      cursor: pointer;
      font-size: 14px;

      &:hover:not(:disabled) {
        background: #0056b3;
      }

      &:disabled {
        background: #ccc;
        cursor: not-allowed;
      }
    }
  }

  .kpi-cards {
    display: grid;
    grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
    gap: 20px;
    margin-bottom: 30px;

    .kpi-card {
      display: flex;
      align-items: center;
      padding: 20px;
      background: white;
      border-radius: 8px;
      box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
      transition: transform 0.2s;

      &:hover {
        transform: translateY(-2px);
        box-shadow: 0 4px 8px rgba(0, 0, 0, 0.15);
      }

      .card-icon {
        font-size: 40px;
        margin-right: 15px;
      }

      .card-content {
        flex: 1;

        h3 {
          margin: 0 0 8px 0;
          font-size: 14px;
          color: #666;
          font-weight: 500;
        }

        .amount {
          margin: 0;
          font-size: 24px;
          font-weight: 600;
          color: #333;
        }

        .count {
          margin: 4px 0 0 0;
          font-size: 12px;
          color: #999;
        }
      }

      .progress-bar {
        width: 100%;
        height: 4px;
        background: #e0e0e0;
        border-radius: 2px;
        margin-top: 8px;
        overflow: hidden;

        .progress-fill {
          height: 100%;
          transition: width 0.3s;

          &.progress-success {
            background: #28a745;
          }

          &.progress-warning {
            background: #ffc107;
          }

          &.progress-danger {
            background: #dc3545;
          }
        }
      }

      &.card-primary {
        border-left: 4px solid #007bff;
      }

      &.card-warning {
        border-left: 4px solid #ffc107;
      }

      &.card-danger {
        border-left: 4px solid #dc3545;
      }

      &.card-info {
        border-left: 4px solid #17a2b8;
      }
    }
  }

  .summary-row {
    display: grid;
    grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
    gap: 20px;
    margin-bottom: 30px;

    .summary-item {
      background: white;
      padding: 15px;
      border-radius: 4px;
      display: flex;
      justify-content: space-between;

      label {
        font-weight: 600;
        color: #666;
      }

      span {
        font-weight: 600;
        color: #333;
      }
    }
  }

  .section-box {
    background: white;
    padding: 20px;
    border-radius: 8px;
    margin-bottom: 20px;
    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);

    .section-header {
      display: flex;
      justify-content: space-between;
      align-items: center;
      margin-bottom: 20px;

      h3 {
        margin: 0;
        font-size: 18px;
        font-weight: 600;
      }

      select {
        padding: 8px 12px;
        border: 1px solid #ddd;
        border-radius: 4px;
        font-size: 14px;
      }
    }

    .monthly-table {
      overflow-x: auto;

      table {
        width: 100%;
        border-collapse: collapse;

        thead {
          background: #f8f9fa;
        }

        th,
        td {
          padding: 12px;
          text-align: right;
          border-bottom: 1px solid #eee;

          &:first-child {
            text-align: left;
          }
        }

        th {
          font-weight: 600;
          color: #666;
          font-size: 12px;
        }

        td {
          &.success {
            color: #28a745;
            font-weight: 600;
          }

          &.warning {
            color: #ffc107;
            font-weight: 600;
          }
        }

        tbody tr:hover {
          background: #f8f9fa;
        }
      }
    }

    .trend-chart {
      height: 300px;
      display: flex;
      align-items: center;
      justify-content: center;
      background: #f8f9fa;
      border-radius: 4px;
    }
  }
}
```

---

## Tarif Service

### 1. Create Tarif Models

Create file: `src/app/models/tarif.model.ts`

```typescript
export interface TarifDto {
  id: string;
  residenceId: string;
  description: string;
  amount: number;
  currency: string;
  effectiveDate: Date;
  endDate: Date | null;
  isActive: boolean;
  notes: string | null;
  createdAt: Date;
  updatedAt: Date | null;
}

export interface CreateTarifDto {
  description: string;
  amount: number;
  currency: string;
  effectiveDate: Date;
  notes?: string;
}

export interface UpdateTarifDto {
  description?: string;
  amount?: number;
  currency?: string;
  notes?: string;
  changeReason?: string;
}

export interface TarifHistoryDto {
  id: string;
  tarifId: string;
  residenceId: string;
  previousAmount: number;
  newAmount: number;
  previousDescription: string;
  newDescription: string;
  effectiveDate: Date;
  changedBy: string;
  changeReason: string | null;
  changedAt: Date;
}
```

### 2. Create Tarif Service

Create file: `src/app/services/tarif.service.ts`

```typescript
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import {
  TarifDto,
  CreateTarifDto,
  UpdateTarifDto,
  TarifHistoryDto
} from '../models/tarif.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class TarifService {
  private readonly apiUrl = `${environment.apiUrl}/api/residences`;

  constructor(private http: HttpClient) {}

  /**
   * Create a new tariff
   */
  createTarif(residenceId: string, dto: CreateTarifDto): Observable<TarifDto> {
    return this.http.post<TarifDto>(
      `${this.apiUrl}/${residenceId}/tarifs`,
      dto
    );
  }

  /**
   * Get tariff by ID
   */
  getTarifById(residenceId: string, tarifId: string): Observable<TarifDto> {
    return this.http.get<TarifDto>(
      `${this.apiUrl}/${residenceId}/tarifs/${tarifId}`
    );
  }

  /**
   * Get all tariffs for a residence
   */
  getTarifsByResidence(residenceId: string): Observable<TarifDto[]> {
    return this.http.get<TarifDto[]>(
      `${this.apiUrl}/${residenceId}/tarifs`
    );
  }

  /**
   * Get current active tariff
   */
  getCurrentTarif(residenceId: string): Observable<TarifDto> {
    return this.http.get<TarifDto>(
      `${this.apiUrl}/${residenceId}/tarifs/current/active`
    );
  }

  /**
   * Update a tariff
   */
  updateTarif(
    residenceId: string,
    tarifId: string,
    dto: UpdateTarifDto
  ): Observable<TarifDto> {
    return this.http.put<TarifDto>(
      `${this.apiUrl}/${residenceId}/tarifs/${tarifId}`,
      dto
    );
  }

  /**
   * Delete a tariff
   */
  deleteTarif(residenceId: string, tarifId: string): Observable<void> {
    return this.http.delete<void>(
      `${this.apiUrl}/${residenceId}/tarifs/${tarifId}`
    );
  }

  /**
   * Get history for a specific tariff
   */
  getTarifHistory(residenceId: string, tarifId: string): Observable<TarifHistoryDto[]> {
    return this.http.get<TarifHistoryDto[]>(
      `${this.apiUrl}/${residenceId}/tarifs/${tarifId}/history`
    );
  }

  /**
   * Get all tariff changes for a residence
   */
  getResidenceTarifHistory(residenceId: string): Observable<TarifHistoryDto[]> {
    return this.http.get<TarifHistoryDto[]>(
      `${this.apiUrl}/${residenceId}/tarifs/history/all`
    );
  }

  /**
   * Get tariff changes by date range
   */
  getTarifHistoryByDateRange(
    residenceId: string,
    startDate: Date,
    endDate: Date
  ): Observable<TarifHistoryDto[]> {
    const params = new HttpParams()
      .set('startDate', startDate.toISOString())
      .set('endDate', endDate.toISOString());

    return this.http.get<TarifHistoryDto[]>(
      `${this.apiUrl}/${residenceId}/tarifs/history/range`,
      { params }
    );
  }
}
```

### 3. Create Tarif Management Component

Create file: `src/app/components/tarif-management/tarif-management.component.ts`

```typescript
import { Component, OnInit, Input } from '@angular/core';
import { TarifService } from '../../services/tarif.service';
import {
  TarifDto,
  CreateTarifDto,
  UpdateTarifDto,
  TarifHistoryDto
} from '../../models/tarif.model';

@Component({
  selector: 'app-tarif-management',
  templateUrl: './tarif-management.component.html',
  styleUrls: ['./tarif-management.component.scss']
})
export class TarifManagementComponent implements OnInit {
  @Input() residenceId!: string;

  tarifs: TarifDto[] = [];
  currentTarif: TarifDto | null = null;
  tarifHistory: TarifHistoryDto[] = [];
  loading = false;
  showForm = false;
  editingTarif: TarifDto | null = null;

  formData: UpdateTarifDto = {
    description: '',
    amount: undefined,
    currency: 'USD',
    notes: '',
    changeReason: ''
  };

  constructor(private tarifService: TarifService) {}

  ngOnInit(): void {
    this.loadTarifs();
  }

  loadTarifs(): void {
    this.loading = true;
    this.tarifService.getTarifsByResidence(this.residenceId).subscribe({
      next: (data) => {
        this.tarifs = data.sort((a, b) =>
          new Date(b.effectiveDate).getTime() - new Date(a.effectiveDate).getTime()
        );
        this.currentTarif = data.find((t) => t.isActive) || null;
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading tariffs:', error);
        this.loading = false;
      }
    });
  }

  loadTarifHistory(tarifId: string): void {
    this.tarifService.getTarifHistory(this.residenceId, tarifId).subscribe({
      next: (data) => {
        this.tarifHistory = data;
      },
      error: (error) => {
        console.error('Error loading history:', error);
      }
    });
  }

  openCreateForm(): void {
    this.editingTarif = null;
    this.formData = {
      description: '',
      amount: undefined,
      currency: 'USD',
      notes: ''
    };
    this.showForm = true;
  }

  openEditForm(tarif: TarifDto): void {
    this.editingTarif = tarif;
    this.formData = {
      description: tarif.description,
      amount: tarif.amount,
      currency: tarif.currency,
      notes: tarif.notes || ''
    };
    this.showForm = true;
  }

  saveTarif(): void {
    if (!this.editingTarif) {
      // Create new tariff
      const createDto: CreateTarifDto = {
        description: this.formData.description || '',
        amount: this.formData.amount || 0,
        currency: this.formData.currency || 'USD',
        effectiveDate: new Date(),
        notes: this.formData.notes
      };

      this.tarifService.createTarif(this.residenceId, createDto).subscribe({
        next: () => {
          this.showForm = false;
          this.loadTarifs();
        },
        error: (error) => {
          console.error('Error creating tariff:', error);
        }
      });
    } else {
      // Update existing tariff
      this.tarifService
        .updateTarif(this.residenceId, this.editingTarif.id, this.formData)
        .subscribe({
          next: () => {
            this.showForm = false;
            this.loadTarifs();
            this.loadTarifHistory(this.editingTarif!.id);
          },
          error: (error) => {
            console.error('Error updating tariff:', error);
          }
        });
    }
  }

  deleteTarif(tarifId: string): void {
    if (confirm('Are you sure you want to delete this tariff?')) {
      this.tarifService.deleteTarif(this.residenceId, tarifId).subscribe({
        next: () => {
          this.loadTarifs();
        },
        error: (error) => {
          console.error('Error deleting tariff:', error);
        }
      });
    }
  }

  viewHistory(tarifId: string): void {
    this.loadTarifHistory(tarifId);
  }

  formatCurrency(value: number): string {
    return new Intl.NumberFormat('en-US', {
      style: 'currency',
      currency: 'USD',
      minimumFractionDigits: 2
    }).format(value);
  }

  formatDate(date: Date | string): string {
    return new Date(date).toLocaleDateString();
  }
}
```

Create file: `src/app/components/tarif-management/tarif-management.component.html`

```html
<div class="tarif-management">
  <div class="header">
    <h2>Tariff Management</h2>
    <button (click)="openCreateForm()" class="btn btn-primary">
      + Add New Tariff
    </button>
  </div>

  <!-- Current Active Tariff -->
  <div *ngIf="currentTarif" class="current-tarif-box">
    <h3>Current Active Tariff</h3>
    <div class="tarif-details">
      <p><strong>Description:</strong> {{ currentTarif.description }}</p>
      <p><strong>Amount:</strong> {{ formatCurrency(currentTarif.amount) }}</p>
      <p><strong>Currency:</strong> {{ currentTarif.currency }}</p>
      <p><strong>Effective Date:</strong> {{ formatDate(currentTarif.effectiveDate) }}</p>
      <p *ngIf="currentTarif.notes"><strong>Notes:</strong> {{ currentTarif.notes }}</p>
    </div>
  </div>

  <!-- Form -->
  <div *ngIf="showForm" class="form-box">
    <h3>{{ editingTarif ? 'Edit Tariff' : 'Create New Tariff' }}</h3>
    <form (ngSubmit)="saveTarif()">
      <div class="form-group">
        <label>Description *</label>
        <input
          [(ngModel)]="formData.description"
          name="description"
          type="text"
          required
        />
      </div>

      <div class="form-group">
        <label>Amount *</label>
        <input
          [(ngModel)]="formData.amount"
          name="amount"
          type="number"
          step="0.01"
          required
        />
      </div>

      <div class="form-group">
        <label>Currency</label>
        <select [(ngModel)]="formData.currency" name="currency">
          <option value="USD">USD</option>
          <option value="EUR">EUR</option>
          <option value="GBP">GBP</option>
        </select>
      </div>

      <div class="form-group">
        <label>Notes</label>
        <textarea [(ngModel)]="formData.notes" name="notes"></textarea>
      </div>

      <div *ngIf="editingTarif" class="form-group">
        <label>Change Reason</label>
        <input
          [(ngModel)]="formData.changeReason"
          name="changeReason"
          type="text"
        />
      </div>

      <div class="form-actions">
        <button type="submit" class="btn btn-primary">Save</button>
        <button
          type="button"
          (click)="showForm = false"
          class="btn btn-secondary"
        >
          Cancel
        </button>
      </div>
    </form>
  </div>

  <!-- Tariffs List -->
  <div class="tarifs-list">
    <h3>All Tariffs</h3>
    <div class="loading" *ngIf="loading">Loading...</div>

    <div *ngIf="!loading && tarifs.length === 0" class="empty-state">
      No tariffs found. Create one to get started.
    </div>

    <div *ngFor="let tarif of tarifs" class="tarif-card">
      <div class="tarif-header">
        <div>
          <h4>{{ tarif.description }}</h4>
          <span class="status-badge" [ngClass]="tarif.isActive ? 'active' : 'inactive'">
            {{ tarif.isActive ? 'Active' : 'Inactive' }}
          </span>
        </div>
        <div class="amount">{{ formatCurrency(tarif.amount) }}</div>
      </div>

      <div class="tarif-details">
        <p>
          <strong>Effective:</strong>
          {{ formatDate(tarif.effectiveDate) }}
          <span *ngIf="tarif.endDate">
            to {{ formatDate(tarif.endDate) }}
          </span>
        </p>
        <p *ngIf="tarif.notes"><strong>Notes:</strong> {{ tarif.notes }}</p>
      </div>

      <div class="tarif-actions">
        <button (click)="openEditForm(tarif)" class="btn btn-sm btn-info">
          Edit
        </button>
        <button
          (click)="viewHistory(tarif.id)"
          class="btn btn-sm btn-secondary"
        >
          History
        </button>
        <button
          (click)="deleteTarif(tarif.id)"
          class="btn btn-sm btn-danger"
        >
          Delete
        </button>
      </div>
    </div>
  </div>

  <!-- History -->
  <div *ngIf="tarifHistory.length > 0" class="history-box">
    <h3>Tariff Change History</h3>
    <table>
      <thead>
        <tr>
          <th>Date</th>
          <th>Changed By</th>
          <th>Previous Amount</th>
          <th>New Amount</th>
          <th>Reason</th>
        </tr>
      </thead>
      <tbody>
        <tr *ngFor="let history of tarifHistory">
          <td>{{ formatDate(history.changedAt) }}</td>
          <td>{{ history.changedBy }}</td>
          <td>{{ formatCurrency(history.previousAmount) }}</td>
          <td>{{ formatCurrency(history.newAmount) }}</td>
          <td>{{ history.changeReason || '-' }}</td>
        </tr>
      </tbody>
    </table>
  </div>
</div>
```

Create file: `src/app/components/tarif-management/tarif-management.component.scss`

```scss
.tarif-management {
  padding: 20px;
  background: #f5f5f5;
  border-radius: 8px;

  .header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 30px;

    h2 {
      margin: 0;
      font-size: 28px;
      font-weight: 600;
    }
  }

  .current-tarif-box {
    background: white;
    padding: 20px;
    border-radius: 8px;
    margin-bottom: 20px;
    border-left: 4px solid #28a745;
    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);

    h3 {
      margin-top: 0;
      color: #28a745;
    }

    .tarif-details {
      display: grid;
      grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
      gap: 15px;

      p {
        margin: 0;
        padding: 10px;
        background: #f8f9fa;
        border-radius: 4px;

        strong {
          display: block;
          color: #666;
          font-size: 12px;
          margin-bottom: 4px;
        }
      }
    }
  }

  .form-box {
    background: white;
    padding: 20px;
    border-radius: 8px;
    margin-bottom: 20px;
    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);

    form {
      display: grid;
      grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
      gap: 15px;

      .form-group {
        label {
          display: block;
          margin-bottom: 5px;
          font-weight: 600;
          color: #333;
          font-size: 14px;
        }

        input,
        select,
        textarea {
          width: 100%;
          padding: 8px 12px;
          border: 1px solid #ddd;
          border-radius: 4px;
          font-size: 14px;

          &:focus {
            outline: none;
            border-color: #007bff;
            box-shadow: 0 0 0 3px rgba(0, 123, 255, 0.1);
          }
        }

        textarea {
          min-height: 80px;
          resize: vertical;
        }
      }

      .form-actions {
        grid-column: 1 / -1;
        display: flex;
        gap: 10px;
      }
    }
  }

  .tarifs-list {
    background: white;
    padding: 20px;
    border-radius: 8px;
    margin-bottom: 20px;
    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);

    h3 {
      margin-top: 0;
    }

    .loading {
      text-align: center;
      padding: 40px;
      color: #666;
    }

    .empty-state {
      text-align: center;
      padding: 40px;
      color: #999;
    }

    .tarif-card {
      background: #f8f9fa;
      padding: 15px;
      border-radius: 4px;
      margin-bottom: 15px;
      border-left: 4px solid #007bff;

      .tarif-header {
        display: flex;
        justify-content: space-between;
        align-items: center;
        margin-bottom: 10px;

        h4 {
          margin: 0;
          color: #333;
        }

        .status-badge {
          display: inline-block;
          padding: 4px 8px;
          border-radius: 4px;
          font-size: 12px;
          font-weight: 600;
          margin-left: 10px;

          &.active {
            background: #d4edda;
            color: #155724;
          }

          &.inactive {
            background: #f8d7da;
            color: #721c24;
          }
        }

        .amount {
          font-size: 18px;
          font-weight: 600;
          color: #007bff;
        }
      }

      .tarif-details {
        margin-bottom: 10px;
        font-size: 14px;
        color: #666;

        p {
          margin: 5px 0;
        }
      }

      .tarif-actions {
        display: flex;
        gap: 10px;

        button {
          padding: 6px 12px;
          font-size: 12px;
        }
      }
    }
  }

  .history-box {
    background: white;
    padding: 20px;
    border-radius: 8px;
    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);

    table {
      width: 100%;
      border-collapse: collapse;

      thead {
        background: #f8f9fa;
      }

      th,
      td {
        padding: 12px;
        text-align: left;
        border-bottom: 1px solid #eee;

        th {
          font-weight: 600;
          color: #666;
          font-size: 12px;
        }
      }

      tbody tr:hover {
        background: #f8f9fa;
      }
    }
  }

  /* Button styles */
  .btn {
    padding: 8px 16px;
    border: none;
    border-radius: 4px;
    cursor: pointer;
    font-size: 14px;
    font-weight: 500;
    transition: all 0.2s;

    &:hover {
      transform: translateY(-1px);
      box-shadow: 0 2px 4px rgba(0, 0, 0, 0.2);
    }

    &:disabled {
      opacity: 0.6;
      cursor: not-allowed;
      transform: none;
    }

    &.btn-primary {
      background: #007bff;
      color: white;

      &:hover:not(:disabled) {
        background: #0056b3;
      }
    }

    &.btn-secondary {
      background: #6c757d;
      color: white;

      &:hover:not(:disabled) {
        background: #545b62;
      }
    }

    &.btn-info {
      background: #17a2b8;
      color: white;

      &:hover:not(:disabled) {
        background: #117a8b;
      }
    }

    &.btn-danger {
      background: #dc3545;
      color: white;

      &:hover:not(:disabled) {
        background: #c82333;
      }
    }

    &.btn-sm {
      padding: 4px 8px;
      font-size: 12px;
    }
  }
}
```

---

## Usage Examples

### Example 1: Display KPI Dashboard in Your App

```typescript
// In your residence detail component
<app-payment-kpi-dashboard [residenceId]="currentResidenceId"></app-payment-kpi-dashboard>
```

### Example 2: Display Tarif Management

```typescript
// In your residence settings component
<app-tarif-management [residenceId]="currentResidenceId"></app-tarif-management>
```

### Example 3: Use Services Directly

```typescript
import { PaymentKpiService } from './services/payment-kpi.service';
import { TarifService } from './services/tarif.service';

export class MyComponent {
  constructor(
    private paymentKpi: PaymentKpiService,
    private tarifService: TarifService
  ) {}

  getKpiData() {
    this.paymentKpi.getPaymentKpi(residenceId).subscribe(kpi => {
      console.log('KPI:', kpi);
    });
  }

  getCurrentTarif() {
    this.tarifService.getCurrentTarif(residenceId).subscribe(tarif => {
      console.log('Current Tarif:', tarif);
    });
  }
}
```

---

## Module Registration

Add these to your `app.module.ts`:

```typescript
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { PaymentKpiDashboardComponent } from './components/payment-kpi-dashboard/payment-kpi-dashboard.component';
import { TarifManagementComponent } from './components/tarif-management/tarif-management.component';
import { PaymentKpiService } from './services/payment-kpi.service';
import { TarifService } from './services/tarif.service';

@NgModule({
  declarations: [
    PaymentKpiDashboardComponent,
    TarifManagementComponent
  ],
  imports: [
    HttpClientModule,
    FormsModule
  ],
  providers: [
    PaymentKpiService,
    TarifService
  ]
})
export class AppModule { }
```

---

## API Endpoints Reference

### Payment KPI Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/residences/{residenceId}/payments/kpi/dashboard` | Get current year KPI |
| GET | `/api/residences/{residenceId}/payments/kpi/range?startDate=X&endDate=Y` | Get KPI for date range |
| GET | `/api/residences/{residenceId}/payments/kpi/monthly-summary?months=12` | Get monthly summaries |
| GET | `/api/residences/{residenceId}/payments/kpi/trend?startDate=X&endDate=Y` | Get trend data |

### Tarif Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/residences/{residenceId}/tarifs` | Create new tariff |
| GET | `/api/residences/{residenceId}/tarifs` | Get all tariffs |
| GET | `/api/residences/{residenceId}/tarifs/{tarifId}` | Get specific tariff |
| GET | `/api/residences/{residenceId}/tarifs/current/active` | Get active tariff |
| PUT | `/api/residences/{residenceId}/tarifs/{tarifId}` | Update tariff |
| DELETE | `/api/residences/{residenceId}/tarifs/{tarifId}` | Delete tariff |
| GET | `/api/residences/{residenceId}/tarifs/{tarifId}/history` | Get tariff history |
| GET | `/api/residences/{residenceId}/tarifs/history/all` | Get all changes |
| GET | `/api/residences/{residenceId}/tarifs/history/range?startDate=X&endDate=Y` | Get history by range |

---

## Notes

- Ensure your `environment.ts` has the correct API URL
- Use `HttpClientModule` and `FormsModule` in your app module
- Components use OnInit lifecycle hook for initial data loading
- All dates are handled in UTC format
- Currency formatting uses US locale (customize as needed)
- Add error handling and loading states as per your app's design

For more details, refer to the backend API documentation or check the Swagger UI at `/swagger/index.html`
