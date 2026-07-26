# 📋 API Documentation - Update: ReceiptNumber Field Added to Payments

## 📅 Date: July 25, 2026
## 🔄 Version: 1.1.0

---

## 📝 Overview

A new field `receiptNumber` has been added to the Payment API endpoints. This field allows tracking and referencing payment receipts within the system.

**Status**: ✅ Released and ready for integration

---

## 🔧 What Changed

### New Field Added
- **Field Name**: `receiptNumber`
- **Type**: String (nullable)
- **Description**: A unique identifier or reference number for the payment receipt
- **Required**: No (optional field)

---

## 📊 Affected DTOs

### 1. **PaymentDto** (Response Model)
Updated response now includes:

```json
{
  "id": "550e8400-e29b-41d4-a716-446655440000",
  "houseId": "123e4567-e89b-12d3-a456-426614174000",
  "residentId": "987fcdeb-51a2-43d1-95f6-426614174999",
  "amount": 2500.00,
  "method": "Transfer",
  "periodStart": "2026-07-01T00:00:00Z",
  "periodEnd": "2026-07-31T23:59:59Z",
  "paymentDate": "2026-07-20T14:30:00Z",
  "receiptNumber": "REC-2026-07-001",
  "status": "Paid",
  "notes": "Payment received",
  "createdAt": "2026-07-20T14:30:00Z",
  "updatedAt": "2026-07-20T14:30:00Z",
  "lines": [
	{
	  "id": "550e8400-e29b-41d4-a716-446655440001",
	  "paymentId": "550e8400-e29b-41d4-a716-446655440000",
	  "fromMonth": 7,
	  "fromYear": 2026,
	  "toMonth": 7,
	  "toYear": 2026,
	  "tarif": 2500.00,
	  "createdAt": "2026-07-20T14:30:00Z"
	}
  ]
}
```

### 2. **CreatePaymentDto** (Request Model for POST)
When creating a payment, you can now include `receiptNumber`:

```json
{
  "houseId": "123e4567-e89b-12d3-a456-426614174000",
  "residentId": "987fcdeb-51a2-43d1-95f6-426614174999",
  "amount": 2500.00,
  "method": "Transfer",
  "periodStart": "2026-07-01T00:00:00Z",
  "periodEnd": "2026-07-31T23:59:59Z",
  "paymentDate": "2026-07-20T14:30:00Z",
  "receiptNumber": "REC-2026-07-001",
  "notes": "Payment received",
  "lines": [
	{
	  "fromMonth": 7,
	  "fromYear": 2026,
	  "toMonth": 7,
	  "toYear": 2026,
	  "tarif": 2500.00
	}
  ]
}
```

### 3. **UpdatePaymentDto** (Request Model for PUT)
When updating a payment, you can optionally update the `receiptNumber`:

```json
{
  "status": "Paid",
  "paymentDate": "2026-07-20T14:30:00Z",
  "receiptNumber": "REC-2026-07-001",
  "notes": "Payment received and verified",
  "periodStart": "2026-07-01T00:00:00Z",
  "periodEnd": "2026-07-31T23:59:59Z",
  "amount": 2500.00,
  "lines": []
}
```

---

## 🔗 Affected Endpoints

### All Payment Endpoints Return Updated PaymentDto

#### 1. **POST** `/api/residences/{residenceId}/payments`
- **Create Payment** - Returns PaymentDto with receiptNumber

#### 2. **GET** `/api/residences/{residenceId}/payments`
- **Get Payments by Residence** - Returns list of PaymentDto with receiptNumber

#### 3. **GET** `/api/residences/payments/resident/{residentId}`
- **Get Payments by Resident** - Returns list of PaymentDto with receiptNumber

#### 4. **GET** `/api/residences/payments/house/{houseId}`
- **Get Payments by House** - Returns list of PaymentDto with receiptNumber

#### 5. **GET** `/api/residences/payments/{id}`
- **Get Payment by ID** - Returns PaymentDto with receiptNumber

#### 6. **PUT** `/api/residences/payments/{id}`
- **Update Payment** - Accepts and returns receiptNumber

---

## 📝 Usage Examples

### Angular (TypeScript)

#### Example 1: Create a Payment with Receipt Number

```typescript
import { HttpClient } from '@angular/common/http';

export class PaymentService {
  constructor(private http: HttpClient) {}

  createPayment(residenceId: string, paymentData: CreatePaymentDto) {
	return this.http.post(
	  `/api/residences/${residenceId}/payments`,
	  paymentData
	);
  }
}

// Usage
const payment = {
  houseId: '123e4567-e89b-12d3-a456-426614174000',
  residentId: '987fcdeb-51a2-43d1-95f6-426614174999',
  amount: 2500.00,
  method: 'Transfer',
  periodStart: new Date('2026-07-01'),
  periodEnd: new Date('2026-07-31'),
  paymentDate: new Date(),
  receiptNumber: 'REC-2026-07-001', // ✅ NEW FIELD
  notes: 'Payment received'
};

this.paymentService.createPayment(residenceId, payment).subscribe(response => {
  console.log('Payment created:', response);
  console.log('Receipt Number:', response.receiptNumber); // ✅ Can now access
});
```

#### Example 2: Update Payment with Receipt Number

```typescript
updatePayment(paymentId: string, updateData: UpdatePaymentDto) {
  return this.http.put(
	`/api/residences/payments/${paymentId}`,
	updateData
  );
}

// Usage
const updatePayment = {
  status: 'Paid',
  paymentDate: new Date(),
  receiptNumber: 'REC-2026-07-001', // ✅ NEW FIELD - Update receipt if needed
  notes: 'Payment verified and processed'
};

this.paymentService.updatePayment(paymentId, updatePayment).subscribe(response => {
  console.log('Receipt Number:', response.receiptNumber);
});
```

#### Example 3: Get Payment with Receipt Number

```typescript
getPaymentById(paymentId: string) {
  return this.http.get(`/api/residences/payments/${paymentId}`);
}

// Usage
this.paymentService.getPaymentById(paymentId).subscribe((payment: PaymentDto) => {
  console.log('Full Receipt Number:', payment.receiptNumber); // ✅ NEW FIELD
  // Display in UI
  this.displayReceipt(payment.receiptNumber);
});
```

### TypeScript Interfaces

Define these interfaces in your Angular application:

```typescript
// payment.model.ts

export interface PaymentDto {
  id: string;
  houseId: string;
  residentId: string;
  amount: number;
  method: PaymentMethod;
  periodStart: Date;
  periodEnd: Date;
  paymentDate: Date;
  receiptNumber?: string; // ✅ NEW FIELD
  status: PaymentStatus;
  notes?: string;
  createdAt: Date;
  updatedAt?: Date;
  lines?: PaymentLineDto[];
}

export interface CreatePaymentDto {
  houseId: string;
  residentId: string;
  amount: number;
  method: PaymentMethod;
  periodStart: Date;
  periodEnd: Date;
  paymentDate: Date;
  receiptNumber?: string; // ✅ NEW FIELD
  notes?: string;
  lines?: CreatePaymentLineDto[];
}

export interface UpdatePaymentDto {
  status: PaymentStatus;
  paymentDate?: Date;
  receiptNumber?: string; // ✅ NEW FIELD
  notes?: string;
  periodStart?: Date;
  periodEnd?: Date;
  amount?: number;
  lines?: UpdatePaymentLineDto[];
}

export enum PaymentMethod {
  Transfer = 0,
  Cash = 1,
  Check = 2,
  Card = 3
}

export enum PaymentStatus {
  Pending = 0,
  Paid = 1,
  Failed = 2,
  Cancelled = 3
}
```

---

## 🔄 Migration Guide

### For Existing Angular Projects

1. **Update your interface definitions**:
   - Add `receiptNumber?: string` to `PaymentDto`
   - Add `receiptNumber?: string` to `CreatePaymentDto`
   - Add `receiptNumber?: string` to `UpdatePaymentDto`

2. **Update UI forms** (if applicable):
   - Add a receipt number input field in payment creation/editing forms
   - Display the receipt number in payment details views

3. **Update payment list displays**:
   - Add receipt number column to payment tables
   - Update payment card/detail views to show receipt number

4. **No breaking changes**:
   - The field is optional (`receiptNumber?: string`)
   - Existing code will continue to work without modification
   - Add the field to your UI at your own pace

### Example Form Control Update

```typescript
// In your payment form component
paymentFormGroup = this.formBuilder.group({
  houseId: ['', Validators.required],
  residentId: ['', Validators.required],
  amount: ['', [Validators.required, Validators.min(0)]],
  method: ['Transfer', Validators.required],
  periodStart: ['', Validators.required],
  periodEnd: ['', Validators.required],
  paymentDate: [new Date(), Validators.required],
  receiptNumber: [''], // ✅ NEW FIELD - Optional
  notes: ['']
});
```

---

## 📋 Database/Backend Info

### Entity Updated
- **Entity**: `Payment` (residence.domain/Entities/Payment.cs)
- **Column**: `ReceiptNumber` (VARCHAR, nullable)
- **Migration**: Required if using Entity Framework migrations

### Backend Services Updated
- **PaymentService**: CreatePaymentAsync, UpdatePaymentAsync, MapToDto
- **PaymentDto Classes**: All three (PaymentDto, CreatePaymentDto, UpdatePaymentDto)

---

## ✅ Testing Checklist

- [ ] Create payment with receipt number
- [ ] Create payment without receipt number (should still work)
- [ ] Update payment receipt number
- [ ] Get payment and verify receipt number is returned
- [ ] Get payments by residence, resident, and house - all include receipt number
- [ ] Display receipt number in Angular UI
- [ ] Export/print payment with receipt number

---

## 📞 Support & Questions

If you encounter any issues integrating this new field:

1. **Check the examples** above
2. **Verify the PaymentDto interfaces** match the new structure
3. **Test with Swagger/Postman** first: http://localhost:5000/swagger
4. **Contact Backend Team** with error messages

---

## 🎯 Summary

| Item | Details |
|------|---------|
| **New Field** | `receiptNumber` (string, optional) |
| **Affected DTOs** | PaymentDto, CreatePaymentDto, UpdatePaymentDto |
| **Status** | ✅ Ready for Integration |
| **Breaking Changes** | ❌ None (field is optional) |
| **API Version** | v1.1.0 |
| **Release Date** | July 25, 2026 |

---

*Last Updated: July 25, 2026*  
*Document Version: 1.0*  
*Maintained by: Backend Team*
