# Residence App – Backend Entities Reference

This document defines **backend domain entities** derived from the frontend models. It is intended to guide **database schema design**, **EF Core entities**, and **API contracts**. All entities follow a **consistent ID strategy**, **audit properties**, and **multi-tenant support**.

---

## Global Conventions

### ID Strategy

* All primary keys use **UUID (GUID)**
* Generated on backend (`Guid.NewGuid()`)

### Multi-Tenancy

* All business entities include `ResidenceId`
* Enforced via global query filters

### Audit Properties (BaseEntity)

All entities inherit from `BaseEntity`:

```csharp
Id : Guid
ResidenceId : Guid
CreatedAt : DateTime
CreatedBy : Guid?
UpdatedAt : DateTime?
UpdatedBy : Guid?
IsDeleted : bool
```

---

## Base Entities

### BaseEntity (Abstract)

Used by all persistent entities.

| Field       | Type      | Description        |
| ----------- | --------- | ------------------ |
| Id          | UUID      | Primary key        |
| ResidenceId | UUID      | Tenant isolation   |
| CreatedAt   | DateTime  | Creation timestamp |
| CreatedBy   | UUID?     | User who created   |
| UpdatedAt   | DateTime? | Last update        |
| UpdatedBy   | UUID?     | User who updated   |
| IsDeleted   | bool      | Soft delete flag   |

---

## 1. Post System

### Post

| Field    | Type    | Notes                      |
| -------- | ------- | -------------------------- |
| AuthorId | UUID    | FK → User                  |
| Content  | string  | Text content               |
| ImageUrl | string? | Stored in Supabase Storage |
| GifUrl   | string? | Optional                   |

#### Relationships

* One Post → Many Likes
* One Post → Many Comments

---

### PostLike

| Field  | Type |
| ------ | ---- |
| PostId | UUID |
| UserId | UUID |

---

### PostComment

| Field    | Type   |
| -------- | ------ |
| PostId   | UUID   |
| AuthorId | UUID   |
| Content  | string |

---

## 2. Property Management

### House

| Field             | Type   | Notes             |
| ----------------- | ------ | ----------------- |
| Block             | string | e.g. A, B         |
| Unit              | string | Apartment number  |
| Floor             | string | Optional          |
| Status            | enum   | Occupied / Vacant |
| CurrentResidentId | UUID?  | FK → Resident     |

---

## 3. Expense Management

### Expense

| Field       | Type    | Notes            |
| ----------- | ------- | ---------------- |
| Title       | string  | Expense label    |
| Type        | enum    | ExpenseType      |
| Amount      | decimal | Precision (18,2) |
| ExpenseDate | Date    |                  |
| Description | string  |                  |

---

### ExpenseImage

| Field     | Type   |
| --------- | ------ |
| ExpenseId | UUID   |
| ImageUrl  | string |

---

### ExpenseType (Enum)

Maintenance, Electricity, Water, Cleaning, Security, Gardening, Repairs, Equipment, Insurance, Taxes, Other

---

## 4. Resident Management

### Resident

| Field     | Type   | Notes             |
| --------- | ------ | ----------------- |
| FirstName | string |                   |
| LastName  | string |                   |
| Email     | string | Unique            |
| Phone     | string |                   |
| Address   | string |                   |
| BirthDate | Date?  |                   |
| Status    | enum   | Active / MovedOut |
| HouseId   | UUID?  | FK → House        |

---

## 5. Payments

### Payment

| Field       | Type    | Notes                    |
| ----------- | ------- | ------------------------ |
| HouseId     | UUID    |                          |
| ResidentId  | UUID    |                          |
| Amount      | decimal |                          |
| PeriodStart | Date    |                          |
| PeriodEnd   | Date    |                          |
| PaymentDate | Date?   |                          |
| Status      | enum    | Paid / Pending / Overdue |

---

## 6. Incident Management

### Incident

| Field       | Type   | Notes                        |
| ----------- | ------ | ---------------------------- |
| ResidentId  | UUID   | Reporter                     |
| HouseId     | UUID   |                              |
| Category    | string | Plumbing, etc                |
| Description | string |                              |
| Status      | enum   | Open / InProgress / Resolved |
| Priority    | enum   | Low / Medium / High          |

---

### IncidentComment

| Field      | Type   |
| ---------- | ------ |
| IncidentId | UUID   |
| AuthorId   | UUID   |
| Text       | string |

---

## 7. Users & Settings

### User

| Field        | Type   | Notes            |
| ------------ | ------ | ---------------- |
| Email        | string | Unique           |
| PasswordHash | string | BCrypt           |
| FirstName    | string |                  |
| LastName     | string |                  |
| Phone        | string |                  |
| Role         | enum   | Admin / Resident |

---

### ResidenceSettings

| Field          | Type    |
| -------------- | ------- |
| ResidenceId    | UUID    |
| InitialBudget  | decimal |
| ResidenceName  | string  |
| ResidencePlace | string  |

---

## Soft Delete Strategy

* `IsDeleted = true`
* Records hidden via global filters
* Physical delete avoided

---

## Notes

* Frontend helper fields (e.g. `isCurrentUser`) are **not persisted**
* Denormalized display fields handled via projections

---

## Status

✅ Backend entity model aligned with frontend models and SaaS-ready.
