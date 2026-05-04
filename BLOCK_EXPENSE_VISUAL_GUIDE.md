# Block-Based Expense System - Visual Architecture

## 🏗️ System Architecture

```
┌─────────────────────────────────────────────────────────────┐
│                    RESIDENCE COMPLEX                         │
├─────────────────────────────────────────────────────────────┤
│                                                              │
│  ┌──────────┐  ┌──────────┐  ┌──────────┐  ┌──────────┐   │
│  │ BLOCK A  │  │ BLOCK B  │  │ BLOCK C  │  │ BLOCK D  │   │
│  │ 23.5%    │  │ 21.73%   │  │ 12.17%   │  │ 17.39%   │   │
│  └──────────┘  └──────────┘  └──────────┘  └──────────┘   │
│                                                              │
│              ┌──────────┐                                   │
│              │ BLOCK E  │                                   │
│              │ 24.35%   │                                   │
│              └──────────┘                                   │
│                                                              │
│  ═══════════════════════════════════════════════════════   │
│  COMMON EXPENSES (Distributed by Coefficient)              │
│  • Electricity (STEG)                                       │
│  • Water                                                     │
│  • General Maintenance                                       │
│  • Cleaning Supplies                                         │
│  ═══════════════════════════════════════════════════════   │
│                                                              │
│  ───────────────────────────────────────────────────────   │
│  BLOCK-SPECIFIC EXPENSES (100% to one block)               │
│  • Block A: Elevator repair (€7,000)                        │
│  • Block C: Door repair (€500)                              │
│  ───────────────────────────────────────────────────────   │
│                                                              │
└─────────────────────────────────────────────────────────────┘
```

---

## 📊 Entity Relationship Diagram

```
┌──────────────────────┐
│    Residence         │
├──────────────────────┤
│ Id (PK)              │
│ Name                 │
│ Address              │
│ CreatedAt            │
└──────────────────────┘
		 │ 1
		 │ ResidenceId (FK)
		 │
		 ├─────────────────────────┬──────────────────────┐
		 │                         │                      │
		 ▼ Many                    ▼ Many                 ▼ Many
┌──────────────────────┐  ┌──────────────────────┐  ┌──────────────────────┐
│      Block           │  │      Expense         │  │    Employee (etc)    │
├──────────────────────┤  ├──────────────────────┤  ├──────────────────────┤
│ Id (PK)              │  │ Id (PK)              │  │ ...                  │
│ ResidenceId (FK)     │  │ ResidenceId (FK)     │  │                      │
│ Name (A-E)           │  │ BlockId (FK) ◄──────┼──┤ (Optional relation)  │
│ Coefficient (0-1)    │  │ Title                │  │                      │
│ CreatedAt            │  │ Type (enum)          │  └──────────────────────┘
│ IsDeleted            │  │ Amount               │
│                      │  │ ExpenseDate          │
└──────────────────────┘  │ Description          │
		 ▲ 1              │ CreatedAt            │
		 │                │ IsDeleted            │
		 │                └──────────────────────┘
		 │                      │ Many
		 └──────────────────────┘

FK Constraint Details:
  BlockId → Block.Id
  Delete Behavior: SET NULL
  (Expense remains if block deleted)
```

---

## 🔄 Data Flow: Expense Distribution

### Scenario 1: Shared Expense

```
┌─────────────────────────────────────────────────────┐
│ INSERT Expense                                      │
│ - Title: "STEG Electricity Bill"                   │
│ - Amount: 419.00                                   │
│ - BlockId: NULL ← Shared                           │
└─────────────────────────────────────────────────────┘
					│
					▼
	┌───────────────────────────────────┐
	│ Distribute Using Coefficients     │
	└───────────────────────────────────┘
		   │    │    │    │    │
	  23.5%│    │21.73%  │    │
		  │    │    │ 12.17%  │
		  │    │    │    │17.39%  24.35%
		  ▼    ▼    ▼    ▼    ▼
	┌────────────────────────────────────┐
	│ Block Distribution                 │
	├────────────────────────────────────┤
	│ Block A: 419.00 × 0.235  = €98.47  │
	│ Block B: 419.00 × 0.2173 = €91.05  │
	│ Block C: 419.00 × 0.1217 = €51.07  │
	│ Block D: 419.00 × 0.1739 = €72.92  │
	│ Block E: 419.00 × 0.2435 = €102.03 │
	│                    TOTAL = €415.54 │
	└────────────────────────────────────┘
```

### Scenario 2: Block-Specific Expense

```
┌─────────────────────────────────────────────────────┐
│ INSERT Expense                                      │
│ - Title: "Door Repair Block C"                     │
│ - Amount: 500.00                                   │
│ - BlockId: @BlockC ← Specific Block               │
└─────────────────────────────────────────────────────┘
					│
					▼
		┌─────────────────────┐
		│ Direct Assignment   │
		└─────────────────────┘
		   │    │    │    │    │
		   ▼    ▼    ▼    ▼    ▼
	┌────────────────────────────────────┐
	│ Block Distribution                 │
	├────────────────────────────────────┤
	│ Block A:        €0.00               │
	│ Block B:        €0.00               │
	│ Block C:      €500.00 (100%)        │
	│ Block D:        €0.00               │
	│ Block E:        €0.00               │
	│                    TOTAL = €500.00 │
	└────────────────────────────────────┘
```

---

## 💾 Database Schema Evolution

### Before (Original)
```
Expenses Table
├── Id (PK)
├── ResidenceId (FK)
├── Title
├── Type
├── Amount
├── ExpenseDate
├── Description
├── CreatedAt
├── IsDeleted
└── ExpenseImages (Navigation)
```

### After (Updated)
```
Expenses Table
├── Id (PK)
├── ResidenceId (FK)
├── BlockId (FK) ◄────── NEW: links to Block
├── Title
├── Type
├── Amount
├── ExpenseDate
├── Description
├── CreatedAt
├── IsDeleted
├── ExpenseImages (Navigation)
└── Block (Navigation) ◄────── NEW: reference to Block entity


Blocks Table ◄────── NEW TABLE
├── Id (PK)
├── ResidenceId (FK)
├── Name (A, B, C, D, E)
├── Coefficient (0.235, 0.2173, ...)
├── CreatedAt
├── IsDeleted
└── Expenses (Navigation)
```

---

## 🔑 Key SQL Operations

### Create Block with Coefficient
```sql
INSERT INTO Blocks (Id, ResidenceId, Name, Coefficient, CreatedAt, IsDeleted)
VALUES (NEWID(), @ResidenceId, 'A', 0.235, GETUTCDATE(), 0);
```

### Insert Shared Expense
```sql
INSERT INTO Expenses (Id, ResidenceId, BlockId, Title, Amount, ...)
VALUES (NEWID(), @ResidenceId, NULL, 'STEG Bill', 419.00, ...);
```

### Insert Block-Specific Expense
```sql
INSERT INTO Expenses (Id, ResidenceId, BlockId, Title, Amount, ...)
VALUES (NEWID(), @ResidenceId, @BlockC, 'Door Repair', 500.00, ...);
```

### Calculate Block's Share
```sql
SELECT SUM(Amount * Coefficient) AS BlockShare
FROM Expenses e
CROSS JOIN Blocks b
WHERE e.ResidenceId = @ResidenceId
  AND e.BlockId IS NULL
  AND b.Name = 'A';
```

---

## 🎯 Process Flow Diagram

```
START
  │
  ├─── Create Residence
  │     └─── Residential complex with 5 blocks
  │
  ├─── Create Blocks (A-E) with Coefficients
  │     └─── Each block gets: Name, Coefficient (sums to 1.0)
  │
  ├─── Add Expense
  │     │
  │     ├─── Expense is SHARED (BlockId = NULL)?
  │     │     └─── Yes: Distribute using coefficients
  │     │
  │     └─── Expense is SPECIFIC (BlockId = @Block)?
  │           └─── No: Assign 100% to that block
  │
  ├─── Calculate Block Costs
  │     ├─── Shared Expenses: Amount × Coefficient
  │     └─── Specific Expenses: Amount (if BlockId = this block)
  │
  ├─── Generate Reports
  │     ├─── Block A Summary
  │     ├─── Block B Summary
  │     └─── etc.
  │
  └─── END
```

---

## 📈 Cost Allocation Timeline

```
Timeline: Aug 2025 - Apr 2026

┌──────────────────────────────────────────────────────┐
│ AUGUST 2025                                          │
├──────────────────────────────────────────────────────┤
│ [1] Chairs (80) - Shared                             │
│     A: 18.80, B: 17.38, C: 9.74, D: 13.92, E: 19.48│
│                                                      │
│ [2] STEG (419) - Shared                              │
│     A: 98.47, B: 91.05, C: 51.07, D: 72.92, E: 102 │
│                                                      │
│ [3] Poubelle Bloc C (400) - Block Specific          │
│     A: —, B: —, C: 400, D: —, E: —                 │
└──────────────────────────────────────────────────────┘

		 ... [months of expenses] ...

┌──────────────────────────────────────────────────────┐
│ APRIL 2026                                           │
├──────────────────────────────────────────────────────┤
│ [179] Doors for Ascenseur (18.50) - Shared          │
│       A: 4.35, B: 4.02, C: 2.25, D: 3.22, E: 4.67  │
└──────────────────────────────────────────────────────┘

TOTAL ACROSS ALL EXPENSES:
Block A: €X,XXX.XX
Block B: €X,XXX.XX
Block C: €X,XXX.XX
Block D: €X,XXX.XX
Block E: €X,XXX.XX
```

---

## 🔐 Constraint Behavior

### Foreign Key Cascade Rules
```
┌──────────────────────────────────────────┐
│ Block → Expenses Relationship            │
├──────────────────────────────────────────┤
│ Delete Block?                            │
│   ↓                                      │
│   Constraint: ON DELETE SET NULL         │
│   ↓                                      │
│   Behavior: Expense.BlockId → NULL       │
│   Result: Expense becomes "shared"       │
│                                          │
│ Meaning: Block can be deleted safely     │
│          Expenses are preserved          │
└──────────────────────────────────────────┘
```

---

## 📋 Implementation Checklist Flow

```
┌─────────────────────────────────────────────────┐
│ Step 1: Create Block Entity                     │
│ File: residence.domain/Entities/Block.cs        │
│ Status: ✅ DONE                                 │
└─────────────────────────────────────────────────┘
			  │
			  ▼
┌─────────────────────────────────────────────────┐
│ Step 2: Update Expense Entity                   │
│ File: residence.domain/Entities/Expense.cs      │
│ Add: BlockId?, Block navigation                 │
│ Status: ✅ DONE                                 │
└─────────────────────────────────────────────────┘
			  │
			  ▼
┌─────────────────────────────────────────────────┐
│ Step 3: Configure EF Core                       │
│ Files: BlockConfiguration, ExpenseConfiguration │
│        ApplicationDbContext                     │
│ Status: ✅ DONE                                 │
└─────────────────────────────────────────────────┘
			  │
			  ▼
┌─────────────────────────────────────────────────┐
│ Step 4: Create & Apply Migration                │
│ Migration: AddBlockExpenseAllocation             │
│ Tables: Blocks created, Expenses updated        │
│ Status: ✅ DONE                                 │
└─────────────────────────────────────────────────┘
			  │
			  ▼
┌─────────────────────────────────────────────────┐
│ Step 5: Initialize Block Data                   │
│ Script: insert_blocks.sql                       │
│ Creates: 5 blocks with coefficients             │
│ Status: ✅ READY (Run manually)                 │
└─────────────────────────────────────────────────┘
			  │
			  ▼
┌─────────────────────────────────────────────────┐
│ Step 6: Load Expense Data                       │
│ Script: insert_expenses_with_blocks.sql         │
│ Loads: 179 expenses with allocations            │
│ Status: ✅ READY (Run manually)                 │
└─────────────────────────────────────────────────┘
			  │
			  ▼
		✨ COMPLETE ✨
```

---

## 🚀 From Code to Database

```
C# Code                         SQL Database              Visualization
─────────────────────────────   ──────────────────────    ──────────────

Block Entity                    Blocks Table
├─ Id                           ├─ Id (PK)
├─ ResidenceId                  ├─ ResidenceId (FK)
├─ Name: "A"       ──────────▶  ├─ Name: 'A'
├─ Coefficient: 0.235 ──────▶   ├─ Coefficient: 0.235
└─ Expenses: [...] ──────────▶  └─ [References to Expenses]
									  ▲
									  │
Expense Entity                        │
├─ Id                           Expenses Table
├─ Title: "STEG Bill"           ├─ Id (PK)
├─ Amount: 419.00               ├─ Title: 'STEG Bill'
├─ BlockId: null ◄──────────────├─ Amount: 419.00
└─ Block: null                  └─ BlockId: null (FK)

Cost Distribution:
419.00 × 0.235 = 98.47      Block A Share
419.00 × 0.2173 = 91.05     Block B Share
419.00 × 0.1217 = 51.07     Block C Share
419.00 × 0.1739 = 72.92     Block D Share
419.00 × 0.2435 = 102.03    Block E Share
```

---

## 🎓 Summary Visual

```
┌─────────────────────────────────────────────────────────┐
│ BLOCK-BASED EXPENSE ALLOCATION SYSTEM                  │
├─────────────────────────────────────────────────────────┤
│                                                         │
│ Input: Residence with 5 Blocks (A-E)                  │
│        Each block has fixed coefficient (0-1)         │
│                                                         │
│ Process:                                               │
│  1. Add Expense to database                            │
│  2. Specify: Shared (NULL) or Block-Specific          │
│  3. For Shared: Calculate share = Amount × Coef       │
│  4. For Specific: Allocate 100% to that block         │
│                                                         │
│ Output: Cost reports showing each block's share       │
│                                                         │
│ Result: Fair distribution of common costs             │
│         Block-specific items assigned correctly        │
│                                                         │
└─────────────────────────────────────────────────────────┘
```

---

**Visual Guide Complete!** 🎨

See `BLOCK_EXPENSE_ALLOCATION_GUIDE.md` and `BLOCK_EXPENSE_QUICK_REFERENCE.md` for detailed documentation.
