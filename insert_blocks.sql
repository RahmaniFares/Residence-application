-- ============================================================================
-- Block Initialization Script
-- ============================================================================
-- This script creates the 5 residential blocks (A, B, C, D, E) with their
-- cost-sharing coefficients for expense allocation
-- ============================================================================

-- First, verify the residence exists and get its ID
DECLARE @ResidenceId UNIQUEIDENTIFIER;
SELECT TOP 1 @ResidenceId = Id FROM Residences ORDER BY CreatedAt ASC;

IF @ResidenceId IS NULL
BEGIN
	PRINT 'ERROR: No residence found. Please create a residence first.';
	RETURN;
END

PRINT 'Using Residence ID: ' + CAST(@ResidenceId AS NVARCHAR(36));
PRINT '============================================================================';

-- Check if blocks already exist
DECLARE @ExistingBlockCount INT;
SELECT @ExistingBlockCount = COUNT(*) FROM Blocks WHERE ResidenceId = @ResidenceId AND IsDeleted = 0;

IF @ExistingBlockCount > 0
BEGIN
	PRINT 'WARNING: ' + CAST(@ExistingBlockCount AS NVARCHAR(10)) + ' blocks already exist for this residence.';
	PRINT 'Skipping initialization.';
	PRINT '============================================================================';
	SELECT 
		Name,
		Coefficient,
		CreatedAt
	FROM Blocks 
	WHERE ResidenceId = @ResidenceId AND IsDeleted = 0
	ORDER BY Name;
	RETURN;
END

-- ============================================================================
-- Insert the 5 Blocks with their coefficients
-- ============================================================================

INSERT INTO Blocks (Id, ResidenceId, Name, Coefficient, CreatedAt, IsDeleted)
VALUES
	(NEWID(), @ResidenceId, 'A', 0.235, GETUTCDATE(), 0),
	(NEWID(), @ResidenceId, 'B', 0.2173, GETUTCDATE(), 0),
	(NEWID(), @ResidenceId, 'C', 0.1217, GETUTCDATE(), 0),
	(NEWID(), @ResidenceId, 'D', 0.1739, GETUTCDATE(), 0),
	(NEWID(), @ResidenceId, 'E', 0.2435, GETUTCDATE(), 0);

PRINT '';
PRINT '============================================================================';
PRINT 'Blocks Initialization Complete';
PRINT '============================================================================';
PRINT '';

-- ============================================================================
-- Verification and Reporting
-- ============================================================================

PRINT 'BLOCKS CREATED:';
PRINT '---';
SELECT 
	Name AS BlockName,
	Coefficient,
	CAST(Coefficient * 100 AS DECIMAL(5, 2)) AS [Percentage%],
	CreatedAt
FROM Blocks 
WHERE ResidenceId = @ResidenceId AND IsDeleted = 0
ORDER BY Name;

PRINT '';
PRINT 'COEFFICIENT VALIDATION:';
PRINT '---';
SELECT 
	SUM(Coefficient) AS TotalCoefficient,
	CASE 
		WHEN ABS(SUM(Coefficient) - 1.0) < 0.0001 THEN 'VALID: Coefficients sum to 1.0'
		ELSE 'ERROR: Coefficients do not sum to 1.0 (Sum = ' + CAST(SUM(Coefficient) AS NVARCHAR(10)) + ')'
	END AS ValidationStatus
FROM Blocks 
WHERE ResidenceId = @ResidenceId AND IsDeleted = 0;

PRINT '';
PRINT '============================================================================';
PRINT 'Process Completed Successfully!';
PRINT '============================================================================';
