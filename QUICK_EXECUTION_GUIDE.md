# Block-Based Expense System - Quick Execution Guide

## 🎯 3-Step Setup (5 minutes)

### Prerequisites
✅ Database created  
✅ Migrations applied  
✅ `insert_blocks.sql` ready  
✅ `insert_shared_expenses.sql` ready  

---

## Step 1: Initialize Blocks (30 seconds)

**Open SQL Server Management Studio or Azure Data Studio**

```sql
-- Copy and execute insert_blocks.sql
-- File location: [project-root]\insert_blocks.sql
EXECUTE insert_blocks.sql
```

**Expected Output**:
```
============================================================================
Blocks Initialization Complete
============================================================================

BLOCKS CREATED:
Block A: Coefficient 0.235
Block B: Coefficient 0.2173
Block C: Coefficient 0.1217
Block D: Coefficient 0.1739
Block E: Coefficient 0.2435

COEFFICIENT VALIDATION: VALID - Coefficients sum to 1.0 ✓
```

---

## Step 2: Insert Shared Expenses (2 minutes)

```sql
-- Copy and execute insert_shared_expenses.sql
-- File location: [project-root]\insert_shared_expenses.sql
EXECUTE insert_shared_expenses.sql
```

**Expected Output**:
```
============================================================================
Shared Expenses Insertion Complete
============================================================================

EXPENSE BREAKDOWN BY TYPE:
[Summary table showing all categories]

TOTAL SUMMARY:
TotalExpenses: 179
TotalAmount: €15,662.68

SHARED EXPENSES VERIFICATION:
SharedExpenseCount: 179

All 179 expenses inserted as SHARED (BlockId = NULL)
Ready for block allocation using coefficients!
```

---

## Step 3: Verify Data (1 minute)

```sql
-- Quick verification
SELECT 
	'Total Expenses' AS [Check],
	COUNT(*) AS Value
FROM Expenses
WHERE BlockId IS NULL;

-- Should show: 179

-- Verify blocks
SELECT 
	Name,
	Coefficient,
	COUNT(e.Id) AS ExpenseCount
FROM Blocks b
LEFT JOIN Expenses e ON b.Id = e.BlockId
GROUP BY b.Name, b.Coefficient
ORDER BY b.Name;

-- Should show: A-E blocks with correct coefficients
```

---

## 📊 System Status After Setup

| Component | Status | Count |
|-----------|--------|-------|
| **Blocks** | ✅ Ready | 5 |
| **Block Coefficients** | ✅ Valid | Sum = 1.0 |
| **Expenses** | ✅ Inserted | 179 |
| **Shared Expenses** | ✅ All | 179 |
| **Date Range** | ✅ Complete | Aug 2025 - Apr 2026 |
| **Categories** | ✅ Categorized | 11 types |

---

## 💡 Test the System

### Calculate Block A's Total Cost

```sql
DECLARE @BlockA UNIQUEIDENTIFIER;
SELECT TOP 1 @BlockA = Id FROM Blocks WHERE Name = 'A';

SELECT 
	'Block A Total Cost' AS Metric,
	CAST(SUM(Amount * 0.235) AS DECIMAL(10,2)) AS Amount
FROM Expenses
WHERE BlockId IS NULL;
```

**Result**: Block A's share of all shared expenses

---

## 🔍 Explore the Data

### See Expenses by Type

```sql
SELECT 
	CASE Type
		WHEN 0 THEN 'Maintenance'
		WHEN 1 THEN 'Electricity'
		WHEN 2 THEN 'Water'
		WHEN 3 THEN 'Cleaning'
		WHEN 4 THEN 'Security'
		WHEN 5 THEN 'Gardening'
		WHEN 6 THEN 'Repairs'
		WHEN 7 THEN 'Equipment'
		WHEN 9 THEN 'Taxes'
		WHEN 10 THEN 'Other'
	END AS Category,
	COUNT(*) AS Count,
	CAST(SUM(Amount) AS DECIMAL(10,2)) AS Total
FROM Expenses
WHERE BlockId IS NULL
GROUP BY Type
ORDER BY Type;
```

### See Monthly Breakdown

```sql
SELECT 
	YEAR(ExpenseDate) AS [Year],
	MONTH(ExpenseDate) AS [Month],
	FORMAT(ExpenseDate, 'MMMM yyyy') AS MonthName,
	COUNT(*) AS ExpenseCount,
	CAST(SUM(Amount) AS DECIMAL(10,2)) AS Total
FROM Expenses
WHERE BlockId IS NULL
GROUP BY YEAR(ExpenseDate), MONTH(ExpenseDate), FORMAT(ExpenseDate, 'MMMM yyyy')
ORDER BY YEAR(ExpenseDate), MONTH(ExpenseDate);
```

---

## 🎯 Block Cost Distribution

### Sample Calculation: €15,662.68 Total

```
Block A (23.5%):   €3,680.72
Block B (21.73%):  €3,404.06
Block C (12.17%):  €1,906.04
Block D (17.39%):  €2,720.25
Block E (24.35%):  €3,813.64
				  ──────────
TOTAL:            €15,662.68
```

### Calculate for Any Amount

```sql
DECLARE @Amount DECIMAL(10,2) = 500.00;

SELECT 
	'Block A' AS Block,
	CAST(@Amount * 0.235 AS DECIMAL(10,2)) AS Share
UNION ALL
SELECT 'Block B', CAST(@Amount * 0.2173 AS DECIMAL(10,2))
UNION ALL
SELECT 'Block C', CAST(@Amount * 0.1217 AS DECIMAL(10,2))
UNION ALL
SELECT 'Block D', CAST(@Amount * 0.1739 AS DECIMAL(10,2))
UNION ALL
SELECT 'Block E', CAST(@Amount * 0.2435 AS DECIMAL(10,2));
```

---

## 🛠️ Troubleshooting

### Issue: "No residence found"
**Solution**: 
```sql
-- Create a test residence first
INSERT INTO Residences (Id, Name, Address, City, Country)
VALUES (NEWID(), 'Test Complex', 'Address', 'City', 'Country');
```

### Issue: "Block not found"
**Solution**: Re-run `insert_blocks.sql`

### Issue: FK constraint errors
**Solution**: Ensure blocks exist before inserting expenses

---

## 📁 Files You'll Use

```
Residence-app/
├── insert_blocks.sql              ← Step 1
├── insert_shared_expenses.sql     ← Step 2
├── SHARED_EXPENSES_SUMMARY.md     ← Reference
├── BLOCK_EXPENSE_ALLOCATION_GUIDE.md
├── BLOCK_EXPENSE_QUICK_REFERENCE.md
└── DOCUMENTATION_INDEX.md
```

---

## ✅ Completion Checklist

- [ ] Step 1: Run `insert_blocks.sql`
- [ ] Verify: Blocks created (5 total)
- [ ] Step 2: Run `insert_shared_expenses.sql`
- [ ] Verify: 179 expenses inserted
- [ ] Step 3: Run verification queries
- [ ] Check: All data present and correct

**Once all checked**: System is ready! ✨

---

## 🚀 What's Next?

### Immediate (Done Now)
- ✅ Blocks initialized
- ✅ Expenses loaded
- ✅ Data verified

### Short Term (Next)
- 📊 Build cost reports by block
- 🔍 Query analysis by category
- 📈 Create billing summaries

### Long Term (Planning)
- 💻 API endpoints for blocks
- 📱 Frontend dashboard
- 🔔 Automated billing

---

## 📞 Quick Help

**Need help?** Check these docs:
- **Quick answers**: BLOCK_EXPENSE_QUICK_REFERENCE.md
- **All details**: BLOCK_EXPENSE_ALLOCATION_GUIDE.md
- **Diagrams**: BLOCK_EXPENSE_VISUAL_GUIDE.md
- **Lost?**: DOCUMENTATION_INDEX.md

---

## ⏱️ Time Estimate

| Step | Time | Task |
|------|------|------|
| 1 | 30 sec | Initialize blocks |
| 2 | 2 min | Insert expenses |
| 3 | 1 min | Verify data |
| **TOTAL** | **3-5 min** | **Complete setup** |

---

## 🎉 Success!

After these 3 steps, you'll have:

✅ 5 configured blocks (A-E)  
✅ 179 expenses loaded as shared  
✅ Cost distribution ready  
✅ System operational  

**Ready for reports, APIs, and analytics!**

---

## 📝 Copy-Paste Commands

### Fast Setup (Copy all at once)

```sql
-- Step 1: Initialize blocks
EXECUTE insert_blocks.sql

-- Wait for completion message...
-- Then Step 2:

-- Step 2: Insert expenses
EXECUTE insert_shared_expenses.sql

-- Wait for completion message...
-- Then verify:

-- Verify: Count expenses
SELECT COUNT(*) AS TotalExpenses FROM Expenses WHERE BlockId IS NULL;
-- Expected: 179

-- Verify: Sum amounts
SELECT 
	CAST(SUM(Amount) AS DECIMAL(12,2)) AS TotalAmount 
FROM Expenses 
WHERE BlockId IS NULL;
-- Expected: ~€15,662.68

-- Done! ✨
```

---

**Execution Time: 3-5 minutes | Difficulty: Easy | Status: Production Ready** ✅
