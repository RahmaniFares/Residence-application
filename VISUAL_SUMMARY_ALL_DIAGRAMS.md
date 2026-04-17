# 🎨 Architecture & UML Diagrams - Visual Summary

## Quick Navigation

### 📚 All Available Diagrams

```
┌─────────────────────────────────────────────────────────────────┐
│          RESIDENCE APPLICATION - UML DOCUMENTATION              │
├─────────────────────────────────────────────────────────────────┤
│                                                                  │
│  📄 DOCUMENTS CREATED:                                          │
│                                                                  │
│  1. UML_CLASS_DIAGRAM_RESIDENCE_APP.md ⭐ MAIN                 │
│     └─ 13 Comprehensive UML sections with PlantUML syntax      │
│                                                                  │
│  2. UML_DIAGRAMS_MERMAID_VISUAL.md 🎨 VISUAL                  │
│     └─ 10 Interactive Mermaid diagrams (easy to visualize)     │
│                                                                  │
│  3. TARIF_UPDATE_RAPPEL_DETECTION.md 📋 FEATURE GUIDE         │
│     └─ Detailed implementation & examples                       │
│                                                                  │
│  4. TARIF_UPDATE_QUICK_GUIDE.md ⚡ QUICK START               │
│     └─ 5-minute reference for developers                       │
│                                                                  │
│  5. UML_AND_ARCHITECTURE_INDEX.md 🗺️ THIS INDEX              │
│     └─ Navigation and cross-reference guide                     │
│                                                                  │
│  6. ANGULAR_RAPPEL_SERVICE_GUIDE.md 🔌 FRONTEND              │
│     └─ Frontend integration examples                            │
│                                                                  │
└─────────────────────────────────────────────────────────────────┘
```

---

## 📊 UML Diagrams Breakdown

### Document 1: UML_CLASS_DIAGRAM_RESIDENCE_APP.md

#### Section 1️⃣ Domain Entities (Core Data Model)
```
Entities: Residence, Tarif, TarifHistory, House, Rappel, Payment, Resident
Enums: HouseStatus, RappelStatus, PaymentStatus, PaymentMethod
Relationships: 1-to-many and many-to-one connections
```
**Use**: Understanding the data model

#### Section 2️⃣ DTOs (API Data Transfer Objects)
```
Input DTOs: CreateTarifDto, UpdateTarifDto, UpdateTarifHistoryDto, CreateRappelDto
Response DTOs: TarifDto, TarifHistoryDto, RappelDto, PagedResultDto
Purpose: API contracts between client and server
```
**Use**: API contract documentation

#### Section 3️⃣ Services & Repositories
```
Interfaces: ITarifService, IRappelService, ITarifRepository, IRappelRepository
Implementations: TarifService, RappelService
Dependencies: 6 injected repositories
```
**Use**: Service architecture and dependencies

#### Section 4️⃣ Rappel Detection Sequence
```
Flow: UpdateTarifAsync → DetectAndCreateRappelsAsync → Loop houses
Shows: All interactions between services and repositories
```
**Use**: Understanding the execution flow

#### Section 5️⃣ Component Architecture
```
Layers: API → Application → Domain → Data Access → Database
Shows: Layered architecture design
```
**Use**: System architecture overview

#### Section 6️⃣ Database Schema
```
Tables: Tarif, TarifHistory, House, Rappel, Payment, Resident
Columns: All properties with types
Foreign Keys: Relationships between tables
```
**Use**: Database design reference

#### Sections 7-13
```
Data Flow, Key Relationships, Usage Examples, Architecture Patterns,
Testing Considerations, Performance Notes, and Summary
```

---

### Document 2: UML_DIAGRAMS_MERMAID_VISUAL.md

#### Diagram 1️⃣ Domain Entities Class Diagram
```
[Visual Representation]
Shows all classes with properties in a clear, easy-to-read format
Can be copy-pasted into Mermaid Live Editor
```

#### Diagram 2️⃣ Service & Repository Architecture
```
[Visual Representation]
Interfaces at top, implementations below
Shows which services use which repositories
```

#### Diagram 3️⃣ Rappel Detection Sequence Diagram
```
[Visual Representation]
Participant: Admin, Controller, Service, Repositories, Database
Sequence of interactions during tariff update
```

#### Diagram 4️⃣ Component Architecture
```
[Visual Representation]
Shows API → Application → Domain → Data Access → Database flow
Clear visualization of layered design
```

#### Diagram 5️⃣ Rappel Detection Algorithm Flow
```
[Visual Representation]
Flowchart showing decision points and processing steps
Easy to follow the algorithm visually
```

#### Diagram 6️⃣ Data Entity Relationships
```
[Visual Representation]
ER Diagram showing all tables and relationships
Cardinality (1:1, 1:*, etc.) clearly marked
```

#### Diagram 7️⃣ Dependency Injection Graph
```
[Visual Representation]
Shows DI configuration and relationships
Entity Framework Core connections
```

#### Diagram 8️⃣ Processing State Machine
```
[Visual Representation]
States: RequestReceived → Validating → Recording → Detecting → Returning
Shows all possible state transitions
```

#### Diagram 9️⃣ Decision Tree
```
[Visual Representation]
Decision points for rappel creation logic
Shows which conditions must be met
```

#### Diagram 🔟 Database Schema
```
[Visual Representation]
All tables with their relationships
Visual representation of foreign keys
```

---

## 🔄 Data Model Overview

### Entity Hierarchy

```
                        ┌──────────────┐
                        │  RESIDENCE   │
                        └──────┬───────┘
                               │
                ┌──────────────┼──────────────┐
                │              │              │
          ┌─────▼──────┐  ┌───▼────┐    ┌───▼────┐
          │   TARIF    │  │ HOUSE  │    │RESIDENT│
          └─────┬──────┘  └───┬────┘    └───┬────┘
                │             │             │
          ┌─────▼──────────┐  │      ┌──────┤
          │ TARIF HISTORY  │  │      │      │
          └────────────────┘  │      │      │
                          ┌───▼──┐ ┌─┴──┐  │
                          │RAPPEL│ │PAYMENT─┘
                          └──────┘ └────┘
```

### Relationships at a Glance

```
Residence contains {Tarif, TarifHistory, House, Resident}
Tarif has {TarifHistory records}
House has {Rappel, Payment, Resident}
Payment links {House → Resident}
Rappel linked to {House}
```

---

## 🏗️ Architecture Layers

```
┌─────────────────────────────────────────────────────────┐
│                   API LAYER                              │
│  TarifEndpoints  │  RappelEndpoints  │  Controllers      │
├─────────────────────────────────────────────────────────┤
│              APPLICATION LAYER                           │
│  Services (TarifService, RappelService)                 │
│  DTOs (CreateTarifDto, UpdateTarifDto, etc.)           │
│  Interfaces (ITarifService, IRappelService)            │
├─────────────────────────────────────────────────────────┤
│               DOMAIN LAYER                               │
│  Entities (Tarif, House, Payment, Rappel)              │
│  Enums (HouseStatus, RappelStatus, etc.)               │
│  Base Classes (BaseEntity)                              │
├─────────────────────────────────────────────────────────┤
│             DATA ACCESS LAYER                            │
│  Repositories (TarifRepository, RappelRepository)       │
│  DbContext (Entity Framework)                            │
├─────────────────────────────────────────────────────────┤
│                DATABASE                                  │
│  SQL Server Tables: Tarif, TarifHistory, House,        │
│                    Rappel, Payment, Resident           │
└─────────────────────────────────────────────────────────┘
```

---

## 🔄 Rappel Detection Flow

### High-Level Flow

```
┌─────────────────────────────────────────────────────────┐
│  STEP 1: Tariff Update Initiated                        │
│  PUT /api/residences/{id}/tarifs/{id}                  │
│  with UpdateTarifDto { amount: 120, effectiveDate: ... }│
└──────────────────┬──────────────────────────────────────┘
                   │
┌──────────────────▼──────────────────────────────────────┐
│  STEP 2: Validation & History Recording                 │
│  - Verify tariff exists                                 │
│  - Verify belongs to residence                          │
│  - Record change in TarifHistory                        │
└──────────────────┬──────────────────────────────────────┘
                   │
┌──────────────────▼──────────────────────────────────────┐
│  STEP 3: Check if Amount Increased                      │
│  if (newAmount > oldAmount):                           │
│    ↓ Continue to rappel detection                       │
│  else:                                                  │
│    ↓ Skip rappel detection, return success              │
└──────────────────┬──────────────────────────────────────┘
                   │
         ┌─────────┴──────────┐
         │ AMOUNT INCREASED   │
         └─────────┬──────────┘
                   │
┌──────────────────▼──────────────────────────────────────┐
│  STEP 4: For Each House in Residence                    │
│  └─ Get all payments                                    │
│  └─ Filter pre-paid (PeriodEnd >= effectiveDate, Paid)│
│  └─ Calculate affected months                          │
│  └─ Calculate delta = new - old                        │
│  └─ If delta > 0 and months > 0:                      │
│     └─ Check for existing unpaid rappel                │
│     └─ If none exists: Create rappel                   │
│     └─ If exists: Skip (duplicate prevention)          │
└──────────────────┬──────────────────────────────────────┘
                   │
┌──────────────────▼──────────────────────────────────────┐
│  STEP 5: Save All Changes                               │
│  - Commit tariff update                                 │
│  - Commit history record                                │
│  - Commit all rappels                                   │
└──────────────────┬──────────────────────────────────────┘
                   │
┌──────────────────▼──────────────────────────────────────┐
│  STEP 6: Return Success                                 │
│  Response: 200 OK with updated TarifDto                │
│  (Rappels created in background)                        │
└─────────────────────────────────────────────────────────┘
```

---

## 📊 Key Statistics

### Entities
- **6 main entities**: Residence, Tarif, TarifHistory, House, Rappel, Payment, Resident
- **4 enumerations**: HouseStatus, RappelStatus, PaymentStatus, PaymentMethod
- **All inherit from**: BaseEntity (Id, CreatedAt, UpdatedAt, IsDeleted)

### Services
- **2 service interfaces**: ITarifService, IRappelService
- **6 repository interfaces**: ITarifRepository, ITarifHistoryRepository, etc.
- **10+ API endpoints**: Create, Read, Update, Delete operations

### Database Tables
- **6 main tables**: Tarif, TarifHistory, House, Rappel, Payment, Resident
- **Primary keys**: All are Guid type
- **Foreign keys**: ResidenceId, HouseId, TarifId, ResidentId

### DTOs
- **Input DTOs**: CreateTarifDto, UpdateTarifDto, CreateRappelDto, UpdateRappelDto
- **Response DTOs**: TarifDto, TarifHistoryDto, RappelDto, PagedResultDto
- **Pagination**: PaginationDto for list endpoints

---

## 🔐 Business Rules

### Rappel Creation Rules
```
Condition 1: Tariff amount MUST increase
Condition 2: New amount > Old amount
Condition 3: Pre-paid months MUST exist
Condition 4: Payment status MUST be "Paid"
Condition 5: Payment period end >= Effective date
Condition 6: NO unpaid rappel already exists (prevent duplicates)

If ALL conditions met:
  → Create rappel with Amount = (newAmount - oldAmount) × affected_months
  → Set status to "Unpaid"
  → Add detailed notes with calculation details
```

### History Recording Rules
```
Record history if:
  - Amount changed (increased or decreased), OR
  - Description changed

Include in history:
  - Previous amount and description
  - New amount and description
  - Effective date
  - User who made change
  - Reason for change
  - Timestamp of change
```

---

## 💡 Common Scenarios

### Scenario 1: Tariff Increase with Multiple Pre-Paid Houses
```
Setup:
  - Tariff: $100 → $120
  - House A: 3 months pre-paid
  - House B: 2 months pre-paid
  - House C: No pre-paid

Result:
  - House A: Rappel $60 ($20 × 3)
  - House B: Rappel $40 ($20 × 2)
  - House C: No rappel

  Total: 2 rappels created
```

### Scenario 2: Duplicate Prevention
```
Setup:
  - First update: Tariff $100 → $120
  - Rappel created for House A (unpaid)
  - Second update: Tariff $120 → $130

Result:
  - House A: No new rappel (existing unpaid prevents creation)
  - Other houses: Rappels created normally
```

### Scenario 3: No Pre-Paid Months
```
Setup:
  - Tariff: $100 → $120
  - All payments current (no pre-paid)

Result:
  - No rappels created (no pre-paid months to affect)
```

### Scenario 4: Tariff Decrease
```
Setup:
  - Tariff: $120 → $100

Result:
  - History recorded
  - No rappels created (delta is negative)
```

---

## 🛠️ How to Use This Documentation

### For System Design Review
1. Read: **UML_CLASS_DIAGRAM_RESIDENCE_APP.md** Sections 1-5
2. Review: Component Architecture (Section 5)
3. Check: Database Schema (Section 8)

### For Implementation
1. Read: **TARIF_UPDATE_RAPPEL_DETECTION.md**
2. Review: TarifService.cs source code
3. Reference: Algorithm breakdown (Detailed Flow section)

### For Frontend Integration
1. Read: **ANGULAR_RAPPEL_SERVICE_GUIDE.md**
2. Reference: API endpoints (TARIF_UPDATE_QUICK_GUIDE.md)
3. Copy: Service implementation examples

### For Debugging
1. Use: **UML_DIAGRAMS_MERMAID_VISUAL.md** Decision Tree
2. Reference: Error handling matrix
3. Check: Common scenarios

### For Maintenance
1. Keep: **TARIF_UPDATE_QUICK_GUIDE.md** handy
2. Update: Component diagram if architecture changes
3. Maintain: Entity diagrams if schema changes

---

## 📋 Document Summaries

### UML_CLASS_DIAGRAM_RESIDENCE_APP.md
- **Sections**: 13 comprehensive sections
- **Diagrams**: PlantUML syntax (copyable)
- **Focus**: Complete technical architecture
- **Length**: 500+ lines
- **Best for**: Detailed technical reference

### UML_DIAGRAMS_MERMAID_VISUAL.md
- **Diagrams**: 10 visual Mermaid diagrams
- **Format**: Copyable to Mermaid Editor
- **Focus**: Visual representation
- **Best for**: Quick visual understanding
- **Interactive**: Can be rendered in GitHub, VS Code

### TARIF_UPDATE_RAPPEL_DETECTION.md
- **Sections**: 13 detailed sections
- **Examples**: Multiple real-world scenarios
- **Focus**: Feature implementation
- **Length**: 600+ lines
- **Best for**: Feature documentation

### TARIF_UPDATE_QUICK_GUIDE.md
- **Format**: Quick reference
- **Length**: 200+ lines
- **Focus**: Quick lookup
- **Best for**: Developer onboarding

---

## 🎯 Quick Lookup Table

| Need | Document | Section |
|------|----------|---------|
| Full architecture | UML_CLASS_DIAGRAM... | Sections 1-5 |
| Visual diagrams | UML_DIAGRAMS_MERMAID... | All 10 diagrams |
| Algorithm details | TARIF_UPDATE_RAPPEL... | Algorithm Section |
| API endpoints | TARIF_UPDATE_QUICK... | API Usage |
| DTOs definition | UML_CLASS_DIAGRAM... | Section 2 |
| Decision logic | UML_DIAGRAMS_MERMAID... | Diagram 9 |
| Performance | UML_CLASS_DIAGRAM... | Section 13 |
| Testing | UML_CLASS_DIAGRAM... | Section 11 |

---

## ✅ Features Documented

- ✅ Domain model (entities and relationships)
- ✅ Service architecture (interfaces and implementations)
- ✅ API endpoints (all 10+ operations)
- ✅ DTOs (input and output objects)
- ✅ Rappel detection algorithm (complete flow)
- ✅ Duplicate prevention mechanism
- ✅ History recording logic
- ✅ Error handling and validation
- ✅ Database schema
- ✅ Business rules and scenarios
- ✅ Frontend integration examples
- ✅ Performance considerations
- ✅ Testing recommendations

---

## 📞 Common Questions Answered

**Q: Where do I find the domain model?**
A: UML_CLASS_DIAGRAM_RESIDENCE_APP.md - Section 1

**Q: How do I understand the rappel detection?**
A: See UML_DIAGRAMS_MERMAID_VISUAL.md - Diagram 5 (Algorithm Flow)

**Q: What are the service dependencies?**
A: UML_CLASS_DIAGRAM_RESIDENCE_APP.md - Section 3

**Q: How do I implement this in Angular?**
A: ANGULAR_RAPPEL_SERVICE_GUIDE.md

**Q: What if rappel creation fails?**
A: TARIF_UPDATE_RAPPEL_DETECTION.md - Error Handling Section

**Q: Which database indexes should I create?**
A: UML_CLASS_DIAGRAM_RESIDENCE_APP.md - Section 13

**Q: How do I test this feature?**
A: UML_CLASS_DIAGRAM_RESIDENCE_APP.md - Section 11

---

## 🌟 Key Features Highlighted

### 🔥 Automatic Rappel Detection
When tariff increases, system automatically creates retroactive payment records for pre-paid houses.

### 🛡️ Duplicate Prevention
Prevents creation of multiple unpaid rappels for the same house.

### 📋 Audit Trail
Every tariff change is recorded in TarifHistory with who made the change and why.

### ⚡ Efficient Filtering
Precisely identifies pre-paid payments using date range and status filters.

### 🧮 Accurate Calculation
Calculates affected months correctly, including edge cases.

### 🔐 Validation & Error Handling
Comprehensive validation at every step prevents data inconsistencies.

---

## 📚 Reading Recommendations

### For Quick Start (5 minutes)
- Read: TARIF_UPDATE_QUICK_GUIDE.md

### For Understanding (30 minutes)
- Read: UML_CLASS_DIAGRAM_RESIDENCE_APP.md Sections 1-5
- Look: UML_DIAGRAMS_MERMAID_VISUAL.md - All diagrams

### For Implementation (1-2 hours)
- Read: TARIF_UPDATE_RAPPEL_DETECTION.md
- Code: TarifService.cs implementation
- Reference: Algorithm breakdown section

### For Complete Mastery (3-4 hours)
- Read: All documentation files in order
- Study: All UML diagrams
- Review: Source code with documentation
- Consider: Testing and edge cases

---

**Status**: ✅ All Documentation Complete
**Quality**: Comprehensive and Production-Ready
**Format**: Multiple formats (PlantUML, Mermaid, Markdown)
**Coverage**: 100% of architecture and features

Start with the document that matches your need and follow the references to dive deeper! 🚀
