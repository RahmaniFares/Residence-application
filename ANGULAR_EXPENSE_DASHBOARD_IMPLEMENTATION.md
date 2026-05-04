# 🎯 Angular Expense KPI Dashboard Implementation Guide

## Overview

This guide walks you through implementing a complete expense dashboard with KPI cards, charts, and statistics.

---

## Step 1: Create the Service

**File:** `src/app/services/expense-kpi.service.ts`

```typescript
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

export interface TotalExpenseKpi {
  totalAmount: number;
  totalExpenseCount: number;
  averageExpense: number;
  maxExpense: number;
  minExpense: number;
  earliestExpenseDate: Date;
  latestExpenseDate: Date;
}

export interface MonthlyExpense {
  year: number;
  month: number;
  monthName: string;
  totalAmount: number;
  expenseCount: number;
  averageExpense: number;
}

export interface MonthlyExpenses {
  data: MonthlyExpense[];
  totalAmount: number;
  totalExpenseCount: number;
  monthsWithData: number;
}

export interface ExpenseTypeStats {
  type: number;
  typeName: string;
  count: number;
  totalAmount: number;
  averageAmount: number;
  percentageOfTotal: number;
}

export interface ExpenseStats {
  data: ExpenseTypeStats[];
  totalAmount: number;
  totalExpenseCount: number;
  highestCategory: ExpenseTypeStats;
  lowestCategory: ExpenseTypeStats;
}

@Injectable({
  providedIn: 'root'
})
export class ExpenseKpiService {
  private apiUrl = `${environment.apiUrl}/residences`;

  constructor(private http: HttpClient) {}

  getTotalKpi(residenceId: string): Observable<TotalExpenseKpi> {
	return this.http.get<TotalExpenseKpi>(
	  `${this.apiUrl}/${residenceId}/expenses/kpi/total`
	);
  }

  getMonthlyExpenses(residenceId: string): Observable<MonthlyExpenses> {
	return this.http.get<MonthlyExpenses>(
	  `${this.apiUrl}/${residenceId}/expenses/kpi/monthly`
	);
  }

  getExpenseStatsByType(residenceId: string): Observable<ExpenseStats> {
	return this.http.get<ExpenseStats>(
	  `${this.apiUrl}/${residenceId}/expenses/kpi/by-type`
	);
  }
}
```

---

## Step 2: Create Dashboard Component

**File:** `src/app/components/expense-dashboard/expense-dashboard.component.ts`

```typescript
import { Component, OnInit, Input } from '@angular/core';
import { ExpenseKpiService, TotalExpenseKpi, MonthlyExpenses, ExpenseStats } from '../../services/expense-kpi.service';

@Component({
  selector: 'app-expense-dashboard',
  templateUrl: './expense-dashboard.component.html',
  styleUrls: ['./expense-dashboard.component.scss']
})
export class ExpenseDashboardComponent implements OnInit {
  @Input() residenceId: string = '';

  // Data
  totalKpi: TotalExpenseKpi | null = null;
  monthlyExpenses: MonthlyExpenses | null = null;
  expenseStats: ExpenseStats | null = null;

  // Loading states
  loadingTotal = true;
  loadingMonthly = true;
  loadingStats = true;

  // Error states
  errorTotal: string | null = null;
  errorMonthly: string | null = null;
  errorStats: string | null = null;

  // Chart data
  monthlyChartData: any;
  typeChartData: any;

  constructor(private kpiService: ExpenseKpiService) {}

  ngOnInit() {
	this.loadDashboard();
  }

  loadDashboard() {
	if (!this.residenceId) {
	  console.error('Residence ID is required');
	  return;
	}

	this.loadTotalKpi();
	this.loadMonthlyExpenses();
	this.loadExpenseStats();
  }

  private loadTotalKpi() {
	this.loadingTotal = true;
	this.errorTotal = null;

	this.kpiService.getTotalKpi(this.residenceId).subscribe(
	  data => {
		this.totalKpi = data;
		this.loadingTotal = false;
	  },
	  error => {
		this.errorTotal = 'Failed to load KPI data';
		this.loadingTotal = false;
		console.error('Error loading total KPI:', error);
	  }
	);
  }

  private loadMonthlyExpenses() {
	this.loadingMonthly = true;
	this.errorMonthly = null;

	this.kpiService.getMonthlyExpenses(this.residenceId).subscribe(
	  data => {
		this.monthlyExpenses = data;
		this.prepareMonthlyChartData();
		this.loadingMonthly = false;
	  },
	  error => {
		this.errorMonthly = 'Failed to load monthly data';
		this.loadingMonthly = false;
		console.error('Error loading monthly expenses:', error);
	  }
	);
  }

  private loadExpenseStats() {
	this.loadingStats = true;
	this.errorStats = null;

	this.kpiService.getExpenseStatsByType(this.residenceId).subscribe(
	  data => {
		this.expenseStats = data;
		this.prepareTypeChartData();
		this.loadingStats = false;
	  },
	  error => {
		this.errorStats = 'Failed to load statistics';
		this.loadingStats = false;
		console.error('Error loading expense stats:', error);
	  }
	);
  }

  private prepareMonthlyChartData() {
	if (!this.monthlyExpenses) return;

	const labels = this.monthlyExpenses.data.map(m => m.monthName);
	const amounts = this.monthlyExpenses.data.map(m => m.totalAmount);
	const counts = this.monthlyExpenses.data.map(m => m.expenseCount);

	this.monthlyChartData = {
	  labels,
	  datasets: [
		{
		  label: 'Total Amount (€)',
		  data: amounts,
		  borderColor: '#3b82f6',
		  backgroundColor: 'rgba(59, 130, 246, 0.1)',
		  tension: 0.3,
		  fill: true,
		  yAxisID: 'y'
		},
		{
		  label: 'Expense Count',
		  data: counts,
		  borderColor: '#ef4444',
		  backgroundColor: 'rgba(239, 68, 68, 0.1)',
		  tension: 0.3,
		  fill: true,
		  yAxisID: 'y1'
		}
	  ]
	};
  }

  private prepareTypeChartData() {
	if (!this.expenseStats) return;

	const labels = this.expenseStats.data.map(d => d.typeName);
	const amounts = this.expenseStats.data.map(d => d.totalAmount);

	const colors = [
	  '#3b82f6', '#ef4444', '#10b981', '#f59e0b', '#8b5cf6',
	  '#ec4899', '#14b8a6', '#06b6d4', '#6366f1', '#84cc16', '#64748b'
	];

	this.typeChartData = {
	  labels,
	  datasets: [
		{
		  data: amounts,
		  backgroundColor: colors.slice(0, labels.length),
		  borderColor: '#ffffff',
		  borderWidth: 2
		}
	  ]
	};
  }

  // Utility methods
  formatCurrency(value: number | undefined): string {
	if (!value) return '€0.00';
	return new Intl.NumberFormat('fr-FR', {
	  style: 'currency',
	  currency: 'EUR'
	}).format(value);
  }

  getExpenseTypeName(typeId: number): string {
	const types: { [key: number]: string } = {
	  0: 'Maintenance',
	  1: 'Electricity',
	  2: 'Water',
	  3: 'Cleaning',
	  4: 'Security',
	  5: 'Gardening',
	  6: 'Repairs',
	  7: 'Equipment',
	  8: 'Insurance',
	  9: 'Taxes',
	  10: 'Other'
	};
	return types[typeId] || 'Unknown';
  }

  // Export functionality
  exportToCSV() {
	if (!this.monthlyExpenses) return;

	let csv = 'Month,Total Amount,Expense Count,Average\n';
	this.monthlyExpenses.data.forEach(month => {
	  csv += `${month.monthName},${month.totalAmount},${month.expenseCount},${month.averageExpense}\n`;
	});

	const blob = new Blob([csv], { type: 'text/csv' });
	const url = window.URL.createObjectURL(blob);
	const a = document.createElement('a');
	a.href = url;
	a.download = 'expenses.csv';
	a.click();
	window.URL.revokeObjectURL(url);
  }
}
```

---

## Step 3: Create Template

**File:** `src/app/components/expense-dashboard/expense-dashboard.component.html`

```html
<div class="expense-dashboard">
  <div class="dashboard-header">
	<h1>Expense Dashboard</h1>
	<button (click)="exportToCSV()" class="btn-export">
	  📥 Export to CSV
	</button>
  </div>

  <!-- KPI Cards -->
  <div class="kpi-section" *ngIf="!loadingTotal">
	<h2>Summary</h2>
	<div class="kpi-grid" *ngIf="totalKpi; else loadingKpi">
	  <div class="kpi-card">
		<div class="card-header">Total Expenses</div>
		<div class="card-value">{{ formatCurrency(totalKpi.totalAmount) }}</div>
		<div class="card-footer">{{ totalKpi.totalExpenseCount }} expenses</div>
	  </div>

	  <div class="kpi-card">
		<div class="card-header">Average Expense</div>
		<div class="card-value">{{ formatCurrency(totalKpi.averageExpense) }}</div>
		<div class="card-footer">per expense</div>
	  </div>

	  <div class="kpi-card">
		<div class="card-header">Highest Expense</div>
		<div class="card-value">{{ formatCurrency(totalKpi.maxExpense) }}</div>
		<div class="card-footer">maximum</div>
	  </div>

	  <div class="kpi-card">
		<div class="card-header">Lowest Expense</div>
		<div class="card-value">{{ formatCurrency(totalKpi.minExpense) }}</div>
		<div class="card-footer">minimum</div>
	  </div>
	</div>

	<ng-template #loadingKpi>
	  <div class="loading">Loading KPI data...</div>
	</ng-template>

	<div *ngIf="errorTotal" class="error-message">{{ errorTotal }}</div>
  </div>

  <!-- Date Range -->
  <div class="date-range" *ngIf="totalKpi">
	<span>From: {{ totalKpi.earliestExpenseDate | date: 'mediumDate' }}</span>
	<span>To: {{ totalKpi.latestExpenseDate | date: 'mediumDate' }}</span>
  </div>

  <!-- Monthly Trend Chart -->
  <div class="chart-section" *ngIf="!loadingMonthly">
	<h2>Monthly Trend</h2>
	<div *ngIf="monthlyChartData; else loadingMonthly" class="chart-container">
	  <canvas 
		id="monthlyChart" 
		baseChart 
		[data]="monthlyChartData"
		type="line"
		[options]="monthlyChartOptions">
	  </canvas>
	</div>
	<ng-template #loadingMonthly>
	  <div class="loading">Loading monthly data...</div>
	</ng-template>
	<div *ngIf="errorMonthly" class="error-message">{{ errorMonthly }}</div>
  </div>

  <!-- Type Distribution Chart -->
  <div class="chart-section" *ngIf="!loadingStats">
	<h2>Expenses by Category</h2>
	<div *ngIf="typeChartData; else loadingStats" class="chart-container">
	  <canvas 
		id="typeChart" 
		baseChart 
		[data]="typeChartData"
		type="doughnut"
		[options]="typeChartOptions">
	  </canvas>
	</div>
	<ng-template #loadingStats>
	  <div class="loading">Loading statistics...</div>
	</ng-template>
	<div *ngIf="errorStats" class="error-message">{{ errorStats }}</div>
  </div>

  <!-- Category Summary -->
  <div class="summary-section" *ngIf="expenseStats">
	<h2>Category Summary</h2>
	<div class="summary-grid">
	  <div class="summary-card highest">
		<h3>Highest Category</h3>
		<p class="category-name">{{ expenseStats.highestCategory.typeName }}</p>
		<p class="category-amount">{{ formatCurrency(expenseStats.highestCategory.totalAmount) }}</p>
		<p class="category-info">
		  {{ expenseStats.highestCategory.count }} expenses • 
		  {{ expenseStats.highestCategory.percentageOfTotal | number: '1.1-1' }}%
		</p>
	  </div>

	  <div class="summary-card lowest">
		<h3>Lowest Category</h3>
		<p class="category-name">{{ expenseStats.lowestCategory.typeName }}</p>
		<p class="category-amount">{{ formatCurrency(expenseStats.lowestCategory.totalAmount) }}</p>
		<p class="category-info">
		  {{ expenseStats.lowestCategory.count }} expenses • 
		  {{ expenseStats.lowestCategory.percentageOfTotal | number: '1.1-1' }}%
		</p>
	  </div>
	</div>

	<!-- All Categories Table -->
	<div class="categories-table">
	  <h3>All Categories</h3>
	  <table>
		<thead>
		  <tr>
			<th>Category</th>
			<th>Count</th>
			<th>Total Amount</th>
			<th>Average</th>
			<th>% of Total</th>
		  </tr>
		</thead>
		<tbody>
		  <tr *ngFor="let stat of expenseStats.data">
			<td>{{ stat.typeName }}</td>
			<td>{{ stat.count }}</td>
			<td>{{ formatCurrency(stat.totalAmount) }}</td>
			<td>{{ formatCurrency(stat.averageAmount) }}</td>
			<td>{{ stat.percentageOfTotal | number: '1.1-1' }}%</td>
		  </tr>
		</tbody>
	  </table>
	</div>
  </div>
</div>
```

---

## Step 4: Add Styles

**File:** `src/app/components/expense-dashboard/expense-dashboard.component.scss`

```scss
.expense-dashboard {
  padding: 20px;
  max-width: 1400px;
  margin: 0 auto;
}

.dashboard-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 30px;

  h1 {
	margin: 0;
	font-size: 2rem;
	color: #1f2937;
  }

  .btn-export {
	padding: 10px 20px;
	background-color: #3b82f6;
	color: white;
	border: none;
	border-radius: 6px;
	cursor: pointer;
	font-size: 1rem;

	&:hover {
	  background-color: #2563eb;
	}
  }
}

.kpi-section {
  margin-bottom: 40px;

  h2 {
	margin: 0 0 20px 0;
	font-size: 1.5rem;
	color: #374151;
  }
}

.kpi-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
  gap: 20px;
  margin-bottom: 20px;
}

.kpi-card {
  background: white;
  border: 1px solid #e5e7eb;
  border-radius: 8px;
  padding: 20px;
  text-align: center;
  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
  transition: all 0.3s ease;

  &:hover {
	box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
	border-color: #3b82f6;
  }

  .card-header {
	font-size: 0.875rem;
	color: #6b7280;
	margin-bottom: 10px;
	text-transform: uppercase;
	letter-spacing: 0.5px;
  }

  .card-value {
	font-size: 1.875rem;
	font-weight: bold;
	color: #111827;
	margin: 10px 0;
  }

  .card-footer {
	font-size: 0.875rem;
	color: #9ca3af;
  }
}

.date-range {
  display: flex;
  gap: 40px;
  margin-bottom: 30px;
  font-size: 0.875rem;
  color: #6b7280;

  span {
	display: flex;
	align-items: center;
  }
}

.chart-section {
  background: white;
  border: 1px solid #e5e7eb;
  border-radius: 8px;
  padding: 20px;
  margin-bottom: 30px;
  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);

  h2 {
	margin: 0 0 20px 0;
	font-size: 1.25rem;
	color: #374151;
  }

  .chart-container {
	position: relative;
	height: 300px;
	margin-bottom: 20px;

	canvas {
	  max-height: 300px;
	}
  }
}

.summary-section {
  h2 {
	margin: 0 0 20px 0;
	font-size: 1.5rem;
	color: #374151;
  }
}

.summary-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
  gap: 20px;
  margin-bottom: 30px;
}

.summary-card {
  background: white;
  border-radius: 8px;
  padding: 20px;
  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);

  h3 {
	margin: 0 0 10px 0;
	font-size: 0.875rem;
	text-transform: uppercase;
	color: #6b7280;
  }

  .category-name {
	margin: 10px 0;
	font-size: 1.25rem;
	font-weight: bold;
	color: #111827;
  }

  .category-amount {
	margin: 10px 0;
	font-size: 1.875rem;
	font-weight: bold;
	margin: 10px 0;
  }

  .category-info {
	margin: 10px 0;
	font-size: 0.875rem;
	color: #9ca3af;
  }

  &.highest {
	border-left: 4px solid #10b981;

	.category-amount {
	  color: #10b981;
	}
  }

  &.lowest {
	border-left: 4px solid #ef4444;

	.category-amount {
	  color: #ef4444;
	}
  }
}

.categories-table {
  background: white;
  border-radius: 8px;
  padding: 20px;
  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);

  h3 {
	margin: 0 0 15px 0;
	font-size: 1.125rem;
	color: #374151;
  }

  table {
	width: 100%;
	border-collapse: collapse;

	thead {
	  background-color: #f9fafb;
	  border-bottom: 2px solid #e5e7eb;

	  th {
		padding: 12px;
		text-align: left;
		font-weight: 600;
		color: #374151;
		font-size: 0.875rem;
		text-transform: uppercase;
	  }
	}

	tbody {
	  tr {
		border-bottom: 1px solid #e5e7eb;
		transition: background-color 0.2s;

		&:hover {
		  background-color: #f9fafb;
		}

		td {
		  padding: 12px;
		  color: #111827;
		}
	  }
	}
  }
}

.loading {
  display: flex;
  justify-content: center;
  align-items: center;
  height: 200px;
  color: #6b7280;
  font-size: 1.125rem;
}

.error-message {
  padding: 12px;
  background-color: #fee2e2;
  color: #991b1b;
  border-radius: 6px;
  margin-top: 10px;
}

// Responsive
@media (max-width: 768px) {
  .expense-dashboard {
	padding: 10px;
  }

  .dashboard-header {
	flex-direction: column;
	gap: 15px;
	align-items: flex-start;

	h1 {
	  font-size: 1.5rem;
	}
  }

  .kpi-grid {
	grid-template-columns: 1fr 1fr;
  }

  .date-range {
	flex-direction: column;
	gap: 10px;
  }

  .summary-grid {
	grid-template-columns: 1fr;
  }

  table {
	font-size: 0.875rem;

	th, td {
	  padding: 8px !important;
	}
  }
}
```

---

## Step 5: Module Configuration

**File:** `src/app/app.module.ts`

```typescript
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { NgChartsModule } from 'ng2-charts';

import { AppComponent } from './app.component';
import { ExpenseDashboardComponent } from './components/expense-dashboard/expense-dashboard.component';

@NgModule({
  declarations: [
	AppComponent,
	ExpenseDashboardComponent
  ],
  imports: [
	BrowserModule,
	HttpClientModule,
	NgChartsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
```

---

## Step 6: Use in App Component

**File:** `src/app/app.component.html`

```html
<app-expense-dashboard [residenceId]="'your-residence-guid-here'"></app-expense-dashboard>
```

---

## Step 7: Install Dependencies

```bash
npm install ng2-charts chart.js
```

---

## 📊 Chart Options (TypeScript)

```typescript
monthlyChartOptions = {
  responsive: true,
  maintainAspectRatio: false,
  interaction: {
	mode: 'index',
	intersect: false
  },
  scales: {
	y: {
	  type: 'linear',
	  display: true,
	  position: 'left'
	},
	y1: {
	  type: 'linear',
	  display: true,
	  position: 'right',
	  grid: {
		drawOnChartArea: false
	  }
	}
  }
};

typeChartOptions = {
  responsive: true,
  maintainAspectRatio: false,
  plugins: {
	legend: {
	  position: 'bottom'
	}
  }
};
```

---

## ✅ Checklist

- [ ] Create ExpenseKpiService
- [ ] Create ExpenseDashboardComponent
- [ ] Create component template (HTML)
- [ ] Add component styles (SCSS)
- [ ] Update AppModule with NgChartsModule
- [ ] Install ng2-charts and chart.js
- [ ] Add component to routing
- [ ] Test with your residence ID
- [ ] Customize colors/styles
- [ ] Add date range filters (optional)
- [ ] Add refresh button (optional)

---

**Version:** 1.0  
**Status:** ✅ Ready to Implement
