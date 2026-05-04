-- ============================================================================
-- Practical Payment Insert Script - First 50 Records
-- Ready-to-Use Template with Complete Examples
-- ============================================================================
-- Prerequisites:
-- 1. All Houses with identifiers A02, A11-A46, B01-B45, C01-C43, D01-D44, E01-E46
-- 2. All Residents with matching first and last names
-- 3. Payments table with proper schema
-- ============================================================================

SET NOCOUNT ON;
SET XACT_ABORT ON;

BEGIN TRANSACTION;

DECLARE @CreatedDate DATETIME = GETUTCDATE();
DECLARE @PaymentMethod_Cash INT = 1; -- espèce
DECLARE @PaymentMethod_Check INT = 2; -- chèque
DECLARE @PaymentStatus_Completed INT = 1; -- Contribution

-- Insert the first 50 payment records
INSERT INTO [dbo].[Payments] (
	[Id], [HouseId], [ResidentId], [Amount], 
	[Method], [PeriodStart], [PeriodEnd], 
	[PaymentDate], [Status], [Notes], [CreatedAt], [UpdatedAt]
)
SELECT 
	NEWID() as [Id],
	h.[Id] as [HouseId],
	r.[Id] as [ResidentId],
	PaymentData.[Amount],
	PaymentData.[Method],
	PaymentData.[PeriodStart],
	PaymentData.[PeriodEnd],
	PaymentData.[PaymentDate],
	@PaymentStatus_Completed,
	PaymentData.[Notes],
	@CreatedDate,
	@CreatedDate
FROM (
	VALUES
	-- Record 1: Check 743
	('E22', 'NORHENE', 'LABIEDH', 120000.00, @PaymentMethod_Cash, '2025-07-01', '2025-09-30', '2025-08-03', 'Check #743 - Contribution (3 mois) du 07-2025 à 09-2025'),
	-- Record 2: Check 744
	('C01', 'NAIMA', 'KHETIRI', 40000.00, @PaymentMethod_Cash, '2025-07-01', '2025-07-31', '2025-08-03', 'Check #744 - Contribution juil-25'),
	-- Record 3: Check 745
	('C02', 'LOC', 'AHMED', 40000.00, @PaymentMethod_Cash, '2025-08-01', '2025-08-31', '2025-08-03', 'Check #745 - Contribution août-25'),
	-- Record 4: Check 747
	('B22', 'AWAREF', 'ABDELWAHED', 40000.00, @PaymentMethod_Cash, '2025-08-01', '2025-08-31', '2025-08-03', 'Check #747 - Contribution août-25'),
	-- Record 5: Check 748
	('A21', 'MOEZ', 'CHAIEB', 550000.00, @PaymentMethod_Cash, '2024-05-01', '2025-07-31', '2025-08-03', 'Check #748 - Contribution 05/2024 à 07/2025'),
	-- Record 6: Check 749
	('C11', 'MOEZ', 'CHAIEB', 550000.00, @PaymentMethod_Cash, '2024-05-01', '2026-07-31', '2025-08-03', 'Check #749 - Contribution 05/2024 à 07/2026'),
	-- Record 7: Check 750
	('A41', 'MAHER', 'HEDDA', 40000.00, @PaymentMethod_Cash, '2025-08-01', '2025-08-31', '2025-08-03', 'Check #750 - Contribution août-25'),
	-- Record 8: Check 751
	('A02', 'FATMA', 'FARHANI', 40000.00, @PaymentMethod_Cash, '2025-05-01', '2025-05-31', '2025-08-03', 'Check #751 - Contribution mai-25'),
	-- Record 9: Check 752
	('A25', 'HOBBI', 'HABIBA', 40000.00, @PaymentMethod_Cash, '2025-05-01', '2025-05-31', '2025-08-03', 'Check #752 - Contribution mai-25'),
	-- Record 10: Check 753
	('A13', 'KAMEL', 'BABOURI', 80000.00, @PaymentMethod_Cash, '2025-05-01', '2025-06-30', '2025-08-03', 'Check #753 - Contribution 05/2025 à 06/2025'),
	-- Record 11: Check 754
	('D42', 'KAMEL', 'BABOURI', 80000.00, @PaymentMethod_Cash, '2025-05-01', '2025-06-30', '2025-08-03', 'Check #754 - Contribution 05/2025 à 06/2025'),
	-- Record 12: Check 756
	('A16', 'SAHNOUN', 'NOUREDDINE', 160000.00, @PaymentMethod_Cash, '2025-05-01', '2025-08-31', '2025-08-10', 'Check #756 - Contribution 05/2025 à 08/2025'),
	-- Record 13: Check 757
	('B05', 'CHALGHAF', 'EZZEDDINE', 340000.00, @PaymentMethod_Cash, '2025-04-01', '2026-01-31', '2025-08-10', 'Check #757 - Contribution 04/2025 à 01/2026'),
	-- Record 14: Check 758
	('E12', 'NAJET', 'HAMDI', 160000.00, @PaymentMethod_Cash, '2025-02-01', '2025-05-31', '2025-08-10', 'Check #758 - Contribution 02/2025 à 05/2025'),
	-- Record 15: Check 759
	('A46', 'SAMIRA', 'SGHAIER', 80000.00, @PaymentMethod_Cash, '2025-06-01', '2025-07-31', '2025-08-10', 'Check #759 - Contribution 06/2025 à 07/2025'),
	-- Record 16: Check 760
	('E13', 'MESBAHI', 'FETHIA', 80000.00, @PaymentMethod_Cash, '2025-06-01', '2025-07-31', '2025-08-17', 'Check #760 - Contribution 06+07/2025'),
	-- Record 17: Check 761
	('E04', 'HADHBA', 'CHERNI', 40000.00, @PaymentMethod_Cash, '2025-08-01', '2025-08-31', '2025-08-17', 'Check #761 - Contribution août-25'),
	-- Record 18: Check 762
	('D24', 'DORSAF', 'MATMATI', 330000.00, @PaymentMethod_Cash, '2023-12-01', '2024-10-31', '2025-08-22', 'Check #762 - Contribution 12/2023 à 10/2024'),
	-- Record 19: Check 763
	('E46', 'SALWA', 'AZZABI', 40000.00, @PaymentMethod_Cash, '2025-08-01', '2025-08-31', '2025-08-24', 'Check #763 - Contribution août-25'),
	-- Record 20: Check 764
	('E31', 'NIZAR', 'LASSOUED', 120000.00, @PaymentMethod_Cash, '2025-06-01', '2025-08-31', '2025-08-24', 'Check #764 - Contribution 06/2025 à 08/2025'),
	-- Record 21: Check 765
	('E21', 'RABIA', 'AYED', 40000.00, @PaymentMethod_Cash, '2025-09-01', '2025-09-30', '2025-08-24', 'Check #765 - Contribution sept-25'),
	-- Record 22: Check 766
	('A41', 'MAHER', 'HEDDA', 40000.00, @PaymentMethod_Cash, '2025-09-01', '2025-09-30', '2025-08-31', 'Check #766 - Contribution sept-25'),
	-- Record 23: Check 767
	('A02', 'FATMA', 'FARHANI', 40000.00, @PaymentMethod_Cash, '2025-06-01', '2025-06-30', '2025-08-31', 'Check #767 - Contribution juin-25'),
	-- Record 24: Check 768
	('A25', 'HOBBI', 'HABIBA', 40000.00, @PaymentMethod_Cash, '2025-06-01', '2025-06-30', '2025-08-31', 'Check #768 - Contribution juin-25'),
	-- Record 25: Check 769
	('C02', 'LOC', 'AHMED', 40000.00, @PaymentMethod_Cash, '2025-09-01', '2025-09-30', '2025-08-31', 'Check #769 - Contribution sept-25'),
	-- Record 26: Check 770
	('C23', 'RIM', 'KOUROUGHLI', 40000.00, @PaymentMethod_Cash, '2025-08-01', '2025-08-31', '2025-08-31', 'Check #770 - Contribution août-25'),
	-- Record 27: Check 771
	('B34', 'CHOKRI', 'BEN NASR', 270000.00, @PaymentMethod_Cash, '2022-07-01', '2023-03-31', '2025-08-31', 'Check #771 - Contribution 07/2022 à 03/2023'),
	-- Record 28: Check 773
	('A42', 'ABDELLATIF', 'KAROUI', 100000.00, @PaymentMethod_Cash, '2020-12-01', '2025-08-31', '2025-08-31', 'Check #773 - ARRIERES 12/2020 à 08/2025'),
	-- Record 29: Check 774
	('E41', 'HEDI', 'JMAIEL', 40000.00, @PaymentMethod_Cash, '2025-08-01', '2025-08-31', '2025-09-07', 'Check #774 - Contribution août-25'),
	-- Record 30: Check 775
	('E23', 'HEDI', 'JJEMAIEL', 40000.00, @PaymentMethod_Cash, '2025-08-01', '2025-08-31', '2025-09-07', 'Check #775 - Contribution août-25'),
	-- Record 31: Check 776
	('B42', 'FAICAL', 'MAATOUH', 1080000.00, @PaymentMethod_Cash, '2022-11-01', '2025-07-31', '2025-09-07', 'Check #776 - Contribution 11/2022 à 07/2025'),
	-- Record 32: Check 777
	('E01', 'FAYCEL', 'MAATOUG', 390000.00, @PaymentMethod_Cash, '2024-01-01', '2024-12-31', '2025-09-07', 'Check #777 - Contribution (12 mois) du 01-2024 à 12-2024'),
	-- Record 33: Check 778
	('D44', 'KHEMIRI', 'AZIZA', 80000.00, @PaymentMethod_Cash, '2025-07-01', '2025-08-31', '2025-09-07', 'Check #778 - Contribution 07/2025 à 08/2025'),
	-- Record 34: Check 779
	('C01', 'NAIMA', 'KHETIRI', 40000.00, @PaymentMethod_Cash, '2025-08-01', '2025-08-31', '2025-09-07', 'Check #779 - Contribution août-25'),
	-- Record 35: Check 780
	('A21', 'MOEZ', 'CHAIEB', 80000.00, @PaymentMethod_Cash, '2025-08-01', '2025-09-30', '2025-09-07', 'Check #780 - Contribution 08+09/2025'),
	-- Record 36: Check 781
	('C11', 'MOEZ', 'CHAIEB', 80000.00, @PaymentMethod_Cash, '2025-08-01', '2025-09-30', '2025-09-07', 'Check #781 - Contribution 08+09/2025'),
	-- Record 37: Check 782
	('B22', 'AWAREF', 'ABDELWAHED', 40000.00, @PaymentMethod_Cash, '2025-09-01', '2025-09-30', '2025-09-07', 'Check #782 - Contribution sept-25'),
	-- Record 38: Check 784
	('D34', 'RAMZI', 'KHLIFI', 80000.00, @PaymentMethod_Cash, '2025-07-01', '2025-08-31', '2025-09-07', 'Check #784 - Contribution 07/2025 à 08/2025'),
	-- Record 39: Check 785
	('E36', 'ZAIDI', 'YASSINE', 120000.00, @PaymentMethod_Cash, '2025-10-01', '2025-12-31', '2025-09-07', 'Check #785 - Contribution 10/2025 à 12/2025'),
	-- Record 40: Check 788
	('A16', 'SAHNOUN', 'NOUREDDINE', 40000.00, @PaymentMethod_Cash, '2025-09-01', '2025-09-30', '2025-09-14', 'Check #788 - Contribution sept-25'),
	-- Record 41: Check 789
	('E26', 'MALIKA', 'HAMMAS', 80000.00, @PaymentMethod_Cash, '2025-07-01', '2025-08-31', '2025-09-14', 'Check #789 - Contribution 07/2025 à 08/2025'),
	-- Record 42: Check 790
	('C32', 'BELGHADI', 'MED SALAH', 480000.00, @PaymentMethod_Cash, '2025-01-01', '2025-12-31', '2025-09-17', 'Check #790 - Contribution (12 mois) du 01-2025 à 12-2025'),
	-- Record 43: Check 792
	('E04', 'HADHBA', 'CHERNI', 40000.00, @PaymentMethod_Cash, '2025-09-01', '2025-09-30', '2025-09-21', 'Check #792 - Contribution sept-25'),
	-- Record 44: Check 793
	('E11', 'CHELBI', 'AICHA', 160000.00, @PaymentMethod_Cash, '2025-07-01', '2025-10-31', '2025-09-21', 'Check #793 - Contribution 07/2025 à 10/2025'),
	-- Record 45: Check 794
	('A11', 'HOUSSINE', 'BESSGHAIR', 80000.00, @PaymentMethod_Cash, '2025-07-01', '2025-08-31', '2025-09-21', 'Check #794 - Contribution 07+08/2025'),
	-- Record 46: Check 795
	('B04', 'KAMEL', 'KANZARI', 200000.00, @PaymentMethod_Cash, '2024-07-01', '2025-09-30', '2025-09-28', 'Check #795 - ARRIERES SOMME DUE 600D 07/2024 à 09/2025'),
	-- Record 47: Check 796
	('A12', 'AKRAM', 'BEN RABEH', 200000.00, @PaymentMethod_Cash, '2025-02-01', '2025-06-30', '2025-09-28', 'Check #796 - Contribution 02/2025 à 06/2025'),
	-- Record 48: Check 797
	('E02', 'MESSAOUDI', 'ABDESSELEM', 240000.00, @PaymentMethod_Cash, '2025-08-01', '2026-01-31', '2025-09-28', 'Check #797 - Contribution 08/2025 à 01/2026'),
	-- Record 49: Check 798
	('E21', 'RABIA', 'AYED', 40000.00, @PaymentMethod_Cash, '2025-10-01', '2025-10-31', '2025-09-28', 'Check #798 - Contribution oct-25'),
	-- Record 50: Check 799
	('A15', 'ALOUI', 'MANNOUBI', 280000.00, @PaymentMethod_Cash, '2025-06-01', '2025-12-31', '2025-09-28', 'Check #799 - Contribution 06/2025 à 12/2025')
) AS PaymentData(
	[HouseIdentifier], 
	[ResidentFirstName], 
	[ResidentLastName], 
	[Amount], 
	[Method], 
	[PeriodStart], 
	[PeriodEnd], 
	[PaymentDate], 
	[Notes]
)
INNER JOIN [dbo].[Houses] h ON h.[HouseIdentifier] = PaymentData.[HouseIdentifier]
INNER JOIN [dbo].[Residents] r ON 
	(r.[FirstName] LIKE '%' + PaymentData.[ResidentFirstName] + '%' 
	 OR PaymentData.[ResidentFirstName] LIKE '%' + r.[FirstName] + '%')
	AND 
	(r.[LastName] LIKE '%' + PaymentData.[ResidentLastName] + '%' 
	 OR PaymentData.[ResidentLastName] LIKE '%' + r.[LastName] + '%');

-- Log the results
DECLARE @InsertedCount INT = @@ROWCOUNT;
PRINT 'Successfully inserted ' + CAST(@InsertedCount AS VARCHAR(10)) + ' payment records.';

COMMIT TRANSACTION;

-- ============================================================================
-- Verification Queries
-- ============================================================================

-- 1. Count total payments inserted
SELECT 
	COUNT(*) as [TotalPayments],
	COUNT(DISTINCT [HouseId]) as [UniqueHouses],
	COUNT(DISTINCT [ResidentId]) as [UniqueResidents],
	SUM([Amount]) as [TotalAmount],
	MIN([PaymentDate]) as [EarliestPayment],
	MAX([PaymentDate]) as [LatestPayment]
FROM [dbo].[Payments];

-- 2. Payments by month
SELECT 
	YEAR([PaymentDate]) as [Year],
	MONTH([PaymentDate]) as [Month],
	DATENAME(MONTH, [PaymentDate]) as [MonthName],
	COUNT(*) as [PaymentCount],
	SUM([Amount]) as [MonthlyTotal]
FROM [dbo].[Payments]
GROUP BY YEAR([PaymentDate]), MONTH([PaymentDate]), DATENAME(MONTH, [PaymentDate])
ORDER BY [Year], [Month];

-- 3. Top 10 largest payments
SELECT TOP 10
	h.[HouseIdentifier],
	r.[FirstName] + ' ' + r.[LastName] as [ResidentName],
	p.[Amount],
	p.[PaymentDate],
	p.[Notes]
FROM [dbo].[Payments] p
JOIN [dbo].[Houses] h ON p.[HouseId] = h.[Id]
JOIN [dbo].[Residents] r ON p.[ResidentId] = r.[Id]
ORDER BY p.[Amount] DESC;

-- 4. Payments by house block
SELECT 
	SUBSTRING(h.[HouseIdentifier], 1, 1) as [Block],
	COUNT(*) as [PaymentCount],
	SUM(p.[Amount]) as [BlockTotal],
	AVG(p.[Amount]) as [AveragePayment]
FROM [dbo].[Payments] p
JOIN [dbo].[Houses] h ON p.[HouseId] = h.[Id]
GROUP BY SUBSTRING(h.[HouseIdentifier], 1, 1)
ORDER BY [Block];

-- 5. Residents with multiple payments
SELECT 
	r.[FirstName] + ' ' + r.[LastName] as [ResidentName],
	COUNT(*) as [PaymentCount],
	SUM(p.[Amount]) as [TotalAmount],
	COUNT(DISTINCT p.[HouseId]) as [UniqueHouses]
FROM [dbo].[Payments] p
JOIN [dbo].[Residents] r ON p.[ResidentId] = r.[Id]
GROUP BY r.[Id], r.[FirstName], r.[LastName]
HAVING COUNT(*) > 1
ORDER BY [PaymentCount] DESC;

-- ============================================================================
-- Notes
-- ============================================================================
-- This script inserts the first 50 payment records as a template.
-- To insert all 200+ records:
-- 1. Continue adding more payment rows in the VALUES clause
-- 2. Or use UNION ALL with additional SELECT statements
-- 3. Or import from CSV using BULK INSERT
-- 4. The same INNER JOIN logic will work for all records
-- ============================================================================
