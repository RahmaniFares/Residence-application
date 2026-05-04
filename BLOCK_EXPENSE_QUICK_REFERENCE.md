# Block-Based Expense Allocation - Quick Reference

## 🎯 System Overview

**What**: Each residential block (A, B, C, D, E) has a coefficient for cost-sharing
**Why**: Distribute common expenses fairly across blocks
**How**: Shared expenses (BlockId = NULL) × Block Coefficient = Block's share

---

## 📊 The 5 Blocks & Coefficients

| Block | Coefficient | Percentage | Example Share on €100 |
|-------|-------------|-----------|----------------------|
| **A** | 0.235 | 23.5% | **€23.50** |
| **B** | 0.2173 | 21.73% | **€21.73** |
| **C** | 0.1217 | 12.17% | **€12.17** |
| **D** | 0.1739 | 17.39% | **€17.39** |
| **E** | 0.2435 | 24.35% | **€24.35** |
| **TOTAL** | **1.0000** | **100%** | **€100.00** |

---

## 🗂️ Entity Structure

### Block Entity
```csharp
public class Block : BaseEntity
{
	public string Name { get; set; }              // "A", "B", "C", "D", "E"
	public decimal Coefficient { get; set; }    // 0.235, 0.2173, etc.
	public Guid ResidenceId { get; set; }       // Which residence
	public ICollection<Expense> Expenses { get; set; } // Navigation
}
```

### Expense Entity (Updated)
```csharp
public class Expense : BaseEntity
{
	public string Title { get; set; }
	public ExpenseType Type { get; set; }
	public decimal Amount { get; set; }
	public DateTime ExpenseDate { get; set; }
	public Guid? BlockId { get; set; }          // ← NEW: nullable for shared
	public Block? Block { get; set; }           // ← NEW: navigation property
	// ... other properties
}
```

---

## 💾 Database Tables

### Blocks Table
```
Blocks
├── Id (uniqueidentifier, PK)
├── ResidenceId (FK)
├── Name (nvarchar(1)) → "A", "B", "C", "D", "E"
├── Coefficient (decimal(5,4))
├── CreatedAt, UpdatedAt, IsDeleted
```

### Expenses Table (Updated)
```
Expenses
├── [existing columns]
├── BlockId (uniqueidentifier, FK, NULLABLE) ← NEW
│   └── If NULL: Shared expense
│   └── If set: Block-specific expense
└── [relationships to ExpenseImage, Block]
```

---

## 🔑 Key Design Pattern

### Shared vs Block-Specific

**Shared Expense** (BlockId = NULL)
```sql
INSERT INTO Expenses (BlockId, Title, Amount, ...)
VALUES (NULL, 'STEG electricity bill', 419.00, ...)
-- → Distribute: A: 98.47, B: 91.05, C: 51.07, D: 72.92, E: 102.03
```

**Block-Specific Expense** (BlockId = @BlockC)
```sql
INSERT INTO Expenses (BlockId, Title, Amount, ...)
VALUES (@BlockC, 'Repair door in Block C', 500.00, ...)
-- → Only Block C bears this cost
```

---

## 🚀 Running the Scripts

### 1️⃣ Initialize Blocks (Run First)
```bash
# Create the 5 blocks with coefficients
EXECUTE insert_blocks.sql

-- Output:
-- Creating 5 blocks A-E
-- Validating: Sum of coefficients = 1.0 ✓
-- SUCCESS
```

### 2️⃣ Insert Expenses (Run Second)
```bash
# Insert 179 expenses with proper block allocation
EXECUTE insert_expenses_with_blocks.sql

-- Output:
-- Loading block IDs...
-- Inserting 179 expenses...
-- SUMMARY:
--   Shared expenses: ~165
--   Block-specific: ~14
--   Total amount: €XX,XXX.XX
```

---

## 📋 Expense Categories Used

| Type ID | Name | Count | Example |
|---------|------|-------|---------|
| 0 | Maintenance | 8 | Ascenseur maintenance |
| 1 | Electricity | 2 | STEG bill |
| 2 | Water | 1 | Water supply |
| 3 | Cleaning | 42 | Cleaning supplies |
| 4 | Security | 5 | Camera systems |
| 5 | Gardening | 5 | Landscaping |
| 6 | Repairs | 79 | Door repairs, etc |
| 7 | Equipment | 19 | Lamps, furniture |
| 8 | Insurance | 0 | (None) |
| 9 | Taxes | 2 | Fees, registration |
| 10 | Other | 16 | Donations, misc |

---

## 💡 Practical Examples

### Example 1: Shared STEG Bill (€419)
```
Amount: 419.00
BlockId: NULL (shared)

Distribution:
Block A: 419.00 × 0.235 = €98.47
Block B: 419.00 × 0.2173 = €91.05
Block C: 419.00 × 0.1217 = €51.07
Block D: 419.00 × 0.1739 = €72.92
Block E: 419.00 × 0.2435 = €102.03
				Total = €415.54 ✓
```

### Example 2: Block C Door Repair (€500)
```
Amount: 500.00
BlockId: @BlockC (specific)

Distribution:
Block A: €0.00
Block B: €0.00
Block C: €500.00 (100%)
Block D: €0.00
Block E: €0.00
```

### Example 3: Elevator Repair - Block A (€7,000)
```
Amount: 7,000.00
BlockId: @BlockA (specific)

Distribution:
Block A: €7,000.00 (100%)
Block B-E: €0.00 each
```

---

## 🔍 Verification Queries

### See All Blocks
```sql
SELECT Name, Coefficient, CreatedAt
FROM Blocks
WHERE ResidenceId = @ResidenceId
ORDER BY Name;
```

### Count Expenses by Block
```sql
SELECT 
	ISNULL(b.Name, 'SHARED') AS Block,
	COUNT(*) AS Count,
	SUM(Amount) AS Total
FROM Expenses e
LEFT JOIN Blocks b ON e.BlockId = b.Id
WHERE e.ResidenceId = @ResidenceId
GROUP BY b.Name;
```

### Calculate a Block's Share of Shared Expenses
```sql
DECLARE @BlockName CHAR(1) = 'A';
DECLARE @ResidenceId UNIQUEIDENTIFIER = '...';

SELECT 
	e.Title,
	e.Amount,
	b.Coefficient,
	e.Amount * b.Coefficient AS BlockShare
FROM Expenses e
CROSS JOIN Blocks b
WHERE e.ResidenceId = @ResidenceId
  AND b.ResidenceId = @ResidenceId
  AND b.Name = @BlockName
  AND e.BlockId IS NULL;  -- Only shared expenses
```

---

## 📱 API Integration (Future)

### Service Method Example
```csharp
public class BlockService : IBlockService
{
	// Get all blocks
	public async Task<List<BlockDto>> GetBlocksAsync(Guid residenceId)
	{
		return await _repo.GetBlocksByResidenceAsync(residenceId);
	}

	// Calculate expense distribution
	public decimal CalculateBlockShare(decimal expenseAmount, decimal coefficient)
	{
		return Math.Round(expenseAmount * coefficient, 2);
	}

	// Get block expenses (both direct + allocated shared)
	public async Task<BlockExpenseSummaryDto> GetBlockSummaryAsync(Guid blockId)
	{
		// Return block info with allocated expenses
	}
}
```

### REST Endpoints (Future)
```
GET    /api/blocks                          → List all blocks
GET    /api/blocks/{blockId}                → Get block details
GET    /api/blocks/{blockId}/expenses       → Block's expenses
GET    /api/blocks/{blockId}/cost-share     → Calculate shares
```

---

## ✨ Migration Details

**Migration Name**: `AddBlockExpenseAllocation`
**Date**: Auto-generated on first creation
**Status**: ✅ Applied to database

**Changes**:
1. Created `Blocks` table
2. Added `BlockId` column to `Expenses`
3. Created FK constraint with `SetNull` behavior
4. Created index on `BlockId` for query performance

**Rollback**:
```bash
dotnet ef migrations remove
dotnet ef database update
```

---

## 🎯 Implementation Checklist

- [x] Block entity created
- [x] Expense entity updated
- [x] EF Core configuration complete
- [x] Migration created & applied
- [x] Database tables created
- [x] Foreign key relationships set up
- [x] Initialization script created
- [x] Data insertion script created
- [x] Build verified ✓
- [x] Documentation complete

---

## 🆘 Troubleshooting

### Issue: FK constraint errors
**Solution**: Ensure blocks are created before inserting expenses
```sql
-- Check block count
SELECT COUNT(*) FROM Blocks WHERE ResidenceId = @ResidenceId;
-- Should return 5
```

### Issue: Coefficient sum ≠ 1.0
**Solution**: Run verification in insert script
```sql
SELECT SUM(Coefficient) FROM Blocks WHERE ResidenceId = @ResidenceId;
-- Should show 1.0000
```

### Issue: BlockId is NULL but should be assigned
**Solution**: Check expense insertion logic
```sql
SELECT BlockId, COUNT(*) FROM Expenses GROUP BY BlockId;
-- Verify expected distribution
```

---

## 📚 Related Files

| File | Purpose |
|------|---------|
| `residence.domain/Entities/Block.cs` | Block entity definition |
| `residence.domain/Entities/Expense.cs` | Expense with BlockId |
| `residence.infrastructure/Configurations/BlockConfiguration.cs` | EF mapping |
| `residence.infrastructure/Configurations/ExpenseConfiguration.cs` | EF mapping (updated) |
| `insert_blocks.sql` | Initialize blocks |
| `insert_expenses_with_blocks.sql` | Load expenses |
| `BLOCK_EXPENSE_ALLOCATION_GUIDE.md` | Full documentation |

---

## 🚀 Next: Running in Your System

```bash
# 1. Verify migration applied
dotnet ef migrations list
# Should show: AddBlockExpenseAllocation ✓

# 2. Open SQL Server Management Studio
# 3. Connect to your database
# 4. Execute: insert_blocks.sql
# 5. Execute: insert_expenses_with_blocks.sql
# 6. Run verification queries
```

**Done!** Your block-based expense system is ready. 🎉
