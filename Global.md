# Residence App – Backend API Documentation

## Overview

This backend API powers the **Residence App SaaS**, providing secure, scalable services for managing residents, houses, payments, expenses, incidents, notifications, and media storage. It is designed with a **multi-tenant architecture** (one or more residences per platform) and optimized for low-to-medium traffic with strong admin usage.

---

## Goals

* Centralize residence management logic
* Provide a secure REST API for the Angular frontend
* Support SaaS multi-tenancy (per residence)
* Remain **free-tier friendly** for MVP and early users
* Be easily extensible (subscriptions, roles, analytics)

---

## Technical Stack

### Core

* **Framework**: ASP.NET Core Web API (.NET 8)
* **Language**: C#
* **Architecture**: Clean Architecture (Domain / Application / Infrastructure / API)
* **Authentication**: JWT (Access + Refresh tokens)

### Data & Persistence

* **Database**: SQLServer 
* **ORM**: Entity Framework Core
* **Migrations**: EF Core Migrations

### External Services

* **Email**: Resend (Transactional emails)
* **SMS**: Twilio (Limited usage – admin notifications)
* **Storage**: Supabase Storage (Images & documents)

### Infrastructure

* **Hosting**: Fly.io / Railway
* **Logging**: Serilog
* **Validation**: FluentValidation
* **API Docs**: Swagger / OpenAPI

---

## High-Level Architecture

```text
src/
├── Api
│   ├── Controllers
│   ├── Middlewares
│   ├── Filters
│   └── Program.cs
├── Application
│   ├── DTOs
│   ├── Interfaces
│   ├── Services
│   ├── UseCases
│   └── Validators
├── Domain
│   ├── Entities
│   ├── Enums
│   ├── ValueObjects
│   └── Events
├── Infrastructure
│   ├── Persistence
│   │   ├── DbContext
│   │   └── Configurations
│   ├── Email
│   ├── Sms
│   └── Storage
```

---

## Core Domain Entities

### User

* Id
* FullName
* Email
* PhoneNumber
* PasswordHash
* Role (Admin, Resident)
* ResidenceId
* CreatedAt / UpdatedAt

### Residence

* Id
* Name
* Address
* City
* Settings (JSON)

### House

* Id
* ResidenceId
* Number / Label
* Floor
* Status (Occupied / Free)

### Resident

* Id
* UserId
* HouseId
* MoveInDate
* MoveOutDate

### Payment

* Id
* ResidentId
* Amount
* Method (Cash, Transfer, Card)
* Status
* PaidAt

### Expense (Dépense)

* Id
* ResidenceId
* Category
* Amount
* ReceiptUrl
* CreatedAt

### Incident

* Id
* ResidenceId
* HouseId
* Title
* Description
* Status (Open, InProgress, Resolved)
* Priority

---

## Multi-Tenancy Strategy

* Every entity is linked to a `ResidenceId`
* JWT token contains `ResidenceId` and `Role`
* Global query filters enforce tenant isolation

---

## Authentication & Authorization

### Authentication

* Login via email + password
* JWT Access Token (short-lived)
* Refresh Token (stored securely)

### Authorization

* Role-based access control (RBAC)
* Admin vs Resident permissions

---

## API Modules

### Authentication

* POST /auth/login
* POST /auth/register
* POST /auth/refresh

### Residents

* GET /residents
* GET /residents/{id}
* POST /residents
* PUT /residents/{id}

### Houses

* GET /houses
* POST /houses

### Payments

* GET /payments
* POST /payments

### Expenses

* GET /expenses
* POST /expenses

### Incidents

* GET /incidents
* POST /incidents
* POST /incidents/{id}/comments

---

## Media & File Storage

* Images stored in **Supabase Storage**
* Buckets:

  * avatars/
  * incidents/
  * expenses/
* URLs persisted in database

---

## Notifications

### Email

* Payment confirmation
* Incident updates
* Admin alerts

### SMS (Limited)

* Critical admin notifications only

---

## Background Jobs

* Email sending
* SMS dispatch
* Cleanup tasks

(Using `IHostedService` or BackgroundService)

---

## Environment Configuration

```env
DB_CONNECTION=
JWT_SECRET=
SUPABASE_URL=
SUPABASE_KEY=
RESEND_API_KEY=
TWILIO_SID=
TWILIO_TOKEN=
```

---

## Security Best Practices

* Password hashing (BCrypt)
* HTTPS enforced
* Rate limiting on auth endpoints
* Input validation
* Soft delete for critical entities

---

## Future Enhancements

* Subscription plans (Free / Pro)
* Payment gateway integration
* Advanced reporting
* Mobile push notifications

---

## Status

🚧 Backend under active development – MVP focused, SaaS-ready by design.
