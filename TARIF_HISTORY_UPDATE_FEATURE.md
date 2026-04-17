# Tariff History Update Feature - Frontend Integration Guide

## Overview
This document provides guidance for the Angular frontend team on how to integrate the new tariff history update feature. The backend now supports updating individual tariff history records with correction information.

## New API Endpoint

### Update Tariff History
**Endpoint:** `PUT /api/residences/{residenceId}/tarifs/{tarifId}/history/{historyId}`

**Description:** Update a specific tariff history record with corrected information.

**Path Parameters:**
- `residenceId` (string, UUID): The ID of the residence
- `tarifId` (string, UUID): The ID of the tariff
- `historyId` (string, UUID): The ID of the history record to update

**Request Body:**
```json
{
  "previousAmount": 100.00,
  "newAmount": 150.00,
  "previousDescription": "Old description",
  "newDescription": "New description",
  "effectiveDate": "2024-01-15T00:00:00Z",
  "changeReason": "Correction due to calculation error"
}
```

**Request DTO Properties:**
- `previousAmount` (decimal, optional): The corrected previous amount
- `newAmount` (decimal, optional): The corrected new amount
- `previousDescription` (string, optional): The corrected previous description
- `newDescription` (string, optional): The corrected new description
- `effectiveDate` (DateTime, optional): The corrected effective date when the change became/will become effective
- `changeReason` (string, optional): The reason for the correction

**Response:**
```json
{
  "id": "550e8400-e29b-41d4-a716-446655440000",
  "tarifId": "550e8400-e29b-41d4-a716-446655440001",
  "residenceId": "550e8400-e29b-41d4-a716-446655440002",
  "previousAmount": 100.00,
  "newAmount": 150.00,
  "previousDescription": "Old description",
  "newDescription": "New description",
  "effectiveDate": "2024-01-15T00:00:00Z",
  "changedBy": "admin@example.com",
  "changeReason": "Correction due to calculation error",
  "changedAt": "2024-01-20T10:30:00Z"
}
```

**HTTP Status Codes:**
- `200 OK`: History record successfully updated
- `400 Bad Request`: Invalid request or history record doesn't belong to the specified tariff/residence
- `404 Not Found`: History record not found
- `500 Internal Server Error`: Server error

## Angular Service Implementation

Update your `tarif-services.ts` to include the new method:

```typescript
updateTarifHistory(
  residenceId: string,
  tarifId: string,
  historyId: string,
  updateData: UpdateTarifHistoryDto
): Observable<TarifHistoryDto> {
  return this.http.put<TarifHistoryDto>(
    `${this.apiUrl}/residences/${residenceId}/tarifs/${tarifId}/history/${historyId}`,
    updateData
  );
}
```

### TypeScript Models

Add these interfaces to your models file:

```typescript
export interface UpdateTarifHistoryDto {
  previousAmount?: number;
  newAmount?: number;
  previousDescription?: string;
  newDescription?: string;
  effectiveDate?: Date;
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
  changeReason?: string;
  changedAt: Date;
}
```

## UI Component Implementation

### History Grid Update

Update your tariff history grid component to include an "Edit" action:

```typescript
// In your history grid component
columns: GridColumn[] = [
  { field: 'id', title: 'ID', width: '200px' },
  { field: 'previousAmount', title: 'Previous Amount', width: '120px', type: 'number' },
  { field: 'newAmount', title: 'New Amount', width: '120px', type: 'number' },
  { field: 'previousDescription', title: 'Previous Description', width: '200px' },
  { field: 'newDescription', title: 'New Description', width: '200px' },
  { field: 'changeReason', title: 'Reason', width: '200px' },
  { field: 'changedAt', title: 'Changed At', width: '180px', type: 'date' },
  { field: 'changedBy', title: 'Changed By', width: '150px' },
  { 
    title: 'Actions', 
    width: '100px',
    cellTemplate: this.editActionTemplate // Add edit button
  }
];

editHistory(historyRecord: TarifHistoryDto) {
  // Open modal or drawer with update form
  this.openHistoryEditDialog(historyRecord);
}

private openHistoryEditDialog(historyRecord: TarifHistoryDto) {
  // Implementation depends on your UI framework
  // e.g., MatDialog, NgbModal, or custom modal
}
```

### Edit Dialog Component

Create a history edit dialog component:

```typescript
import { Component, Inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { TarifService } from '../services/tarif.service';
import { TarifHistoryDto, UpdateTarifHistoryDto } from '../models/tarif.model';

@Component({
  selector: 'app-tarif-history-edit',
  templateUrl: './tarif-history-edit.component.html',
  styleUrls: ['./tarif-history-edit.component.scss']
})
export class TarifHistoryEditComponent {
  historyForm: FormGroup;
  isLoading = false;
  error: string | null = null;

  constructor(
    private formBuilder: FormBuilder,
    private tarifService: TarifService,
    public dialogRef: MatDialogRef<TarifHistoryEditComponent>,
    @Inject(MAT_DIALOG_DATA) public data: {
      residenceId: string;
      tarifId: string;
      history: TarifHistoryDto;
    }
  ) {
    this.historyForm = this.createForm();
  }

  private createForm(): FormGroup {
    return this.formBuilder.group({
      previousAmount: [this.data.history.previousAmount],
      newAmount: [this.data.history.newAmount],
      previousDescription: [this.data.history.previousDescription],
      newDescription: [this.data.history.newDescription],
      effectiveDate: [this.data.history.effectiveDate],
      changeReason: [this.data.history.changeReason]
    });
  }

  onSubmit() {
    if (this.historyForm.invalid) {
      return;
    }

    this.isLoading = true;
    this.error = null;

    const updateData: UpdateTarifHistoryDto = this.historyForm.value;

    this.tarifService.updateTarifHistory(
      this.data.residenceId,
      this.data.tarifId,
      this.data.history.id,
      updateData
    ).subscribe({
      next: (result) => {
        this.isLoading = false;
        this.dialogRef.close(result);
      },
      error: (err) => {
        this.isLoading = false;
        this.error = err.error?.message || 'Failed to update history record';
        console.error('Error updating history:', err);
      }
    });
  }

  onCancel() {
    this.dialogRef.close();
  }
}
```

### Edit Dialog Template

Create the HTML template for the edit dialog:

```html
<h2 mat-dialog-title>Edit Tariff History</h2>

<mat-dialog-content>
  <form [formGroup]="historyForm">
    <mat-form-field appearance="outline" class="full-width">
      <mat-label>Previous Amount</mat-label>
      <input matInput type="number" formControlName="previousAmount" step="0.01" />
    </mat-form-field>

    <mat-form-field appearance="outline" class="full-width">
      <mat-label>New Amount</mat-label>
      <input matInput type="number" formControlName="newAmount" step="0.01" />
    </mat-form-field>

    <mat-form-field appearance="outline" class="full-width">
      <mat-label>Previous Description</mat-label>
      <textarea matInput formControlName="previousDescription" rows="2"></textarea>
    </mat-form-field>

    <mat-form-field appearance="outline" class="full-width">
      <mat-label>New Description</mat-label>
      <textarea matInput formControlName="newDescription" rows="2"></textarea>
    </mat-form-field>

    <mat-form-field appearance="outline" class="full-width">
      <mat-label>Effective Date</mat-label>
      <input matInput type="datetime-local" formControlName="effectiveDate" />
    </mat-form-field>

    <mat-form-field appearance="outline" class="full-width">
      <mat-label>Change Reason</mat-label>
      <textarea matInput formControlName="changeReason" rows="2" placeholder="Why are you making this correction?"></textarea>
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
    [disabled]="isLoading || historyForm.invalid"
  >
    <mat-spinner diameter="20" *ngIf="isLoading"></mat-spinner>
    {{ isLoading ? 'Updating...' : 'Update' }}
  </button>
</mat-dialog-actions>
```

### Styling

Add to your component stylesheet:

```scss
.full-width {
  width: 100%;
  margin-bottom: 16px;
}

mat-dialog-content {
  min-width: 400px;
  max-width: 600px;
}

.error-message {
  color: #d32f2f;
  margin: 16px 0;
  padding: 8px;
  border: 1px solid #d32f2f;
  border-radius: 4px;
}

mat-spinner {
  display: inline-block;
  margin-right: 8px;
}
```

## Service Integration Example

```typescript
// In your tariff management component
import { MatDialog } from '@angular/material/dialog';
import { TarifHistoryEditComponent } from './tarif-history-edit/tarif-history-edit.component';

export class TarifManagementComponent {
  constructor(
    private tarifService: TarifService,
    private dialog: MatDialog
  ) {}

  editHistory(history: TarifHistoryDto) {
    const dialogRef = this.dialog.open(TarifHistoryEditComponent, {
      width: '600px',
      data: {
        residenceId: this.residenceId,
        tarifId: this.tarifId,
        history: history
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        // Refresh the history grid
        this.loadTarifHistory();
        // Show success message
        this.showSuccessMessage('History record updated successfully');
      }
    });
  }

  private loadTarifHistory() {
    // Reload history from service
    this.tarifService.getTarifHistory(this.tarifId).subscribe(
      data => {
        this.historyData = data;
      }
    );
  }

  private showSuccessMessage(message: string) {
    // Implementation depends on your notification system
    // e.g., MatSnackBar, ToastrService, etc.
  }
}
```

## Error Handling

The API returns specific error messages for common issues:

1. **History not found:** Returns 404 with message "History record with ID {id} not found."
2. **Invalid residence:** Returns 400 with message "History record does not belong to the specified residence."
3. **Invalid tariff:** Returns 400 with message "History record does not belong to the specified tariff."

Ensure your error handling in Angular catches these scenarios:

```typescript
.subscribe({
  next: (result) => {
    // Handle success
  },
  error: (error) => {
    if (error.status === 404) {
      console.error('History record not found');
    } else if (error.status === 400) {
      console.error('Invalid request:', error.error?.message);
    } else {
      console.error('Unexpected error:', error);
    }
  }
});
```

## Usage Flow

1. **Display History Grid:** Load and display tariff history records
2. **Edit Action:** User clicks "Edit" button on a history record
3. **Open Dialog:** Modal opens with pre-filled data from the history record
4. **Make Changes:** User can update the following fields:
   - Previous Amount
   - New Amount
   - Previous Description
   - New Description
   - Change Reason
5. **Submit:** User clicks "Update" button
6. **API Call:** Angular service calls the new endpoint
7. **Success:** History record is updated and grid is refreshed
8. **Feedback:** Success message is shown to user

## Testing

### Unit Test Example

```typescript
import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { TarifService } from './tarif.service';
import { UpdateTarifHistoryDto, TarifHistoryDto } from './models/tarif.model';

describe('TarifService - Update History', () => {
  let service: TarifService;
  let httpMock: HttpTestingController;
  const baseUrl = '/api/residences';

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

  it('should update tariff history', () => {
    const residenceId = '550e8400-e29b-41d4-a716-446655440000';
    const tarifId = '550e8400-e29b-41d4-a716-446655440001';
    const historyId = '550e8400-e29b-41d4-a716-446655440002';

    const updateData: UpdateTarifHistoryDto = {
      previousAmount: 100,
      newAmount: 150,
      changeReason: 'Test correction'
    };

    const mockResponse: TarifHistoryDto = {
      id: historyId,
      tarifId,
      residenceId,
      previousAmount: 100,
      newAmount: 150,
      previousDescription: '',
      newDescription: '',
      effectiveDate: new Date(),
      changedBy: 'user',
      changeReason: 'Test correction',
      changedAt: new Date()
    };

    service.updateTarifHistory(residenceId, tarifId, historyId, updateData).subscribe(result => {
      expect(result).toEqual(mockResponse);
    });

    const req = httpMock.expectOne(
      `${baseUrl}/${residenceId}/tarifs/${tarifId}/history/${historyId}`
    );
    expect(req.request.method).toBe('PUT');
    expect(req.request.body).toEqual(updateData);
    req.flush(mockResponse);
  });
});
```

## Backward Compatibility

This feature is fully backward compatible:
- All existing endpoints remain unchanged
- Existing history records can be read without modification
- The update feature is optional and additive

## Support

For issues or questions regarding the API implementation, contact the backend development team.
