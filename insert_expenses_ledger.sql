-- ============================================================================
-- Expense Data Insert Script - Residence Complex Management
-- ============================================================================
-- This script inserts 179 expenses from the expense ledger with proper
-- categorization by ExpenseType and allocation to residence blocks
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

-- Disable foreign key constraints temporarily
ALTER TABLE Expenses NOCHECK CONSTRAINT ALL;

-- ============================================================================
-- Insert Expenses
-- ============================================================================
INSERT INTO Expenses (Id, ResidenceId, Title, Type, Amount, ExpenseDate, Description, CreatedAt, IsDeleted)
VALUES
-- 1. Maintenance & Repairs (Mixed between Repairs and Maintenance)
(NEWID(), @ResidenceId, 'Achat de 4 chaises visiteurs occasions du souk lahad', 6, 80.00, '2025-08-03', 'BLOC A; 19.48', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Travaux d''assainissement ragard C01', 6, 40.00, '2025-08-05', 'BLOC B; 8.69', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat d''1 poubelle de 400 litres', 7, 400.00, '2025-08-05', 'BLOC C; 48.68', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Droit d''enregistrement de procès verbal AG du 3/8/2025', 9, 90.00, '2025-08-06', 'General; 21.92', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Photocopie 40 copies lettres de recouvrement +39 LR faites', 3, 8.30, '2025-08-06', 'General; 2.02', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat 1 bouteille décapant + 1 flexible de douche', 0, 10.79, '2025-08-06', 'BLOC D; 2.63', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Travaux de jardinage faite par Mr SSAM', 5, 200.00, '2025-08-10', 'General; 48.70', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Réparation de pompe de moteur du puit jardin de la cite', 6, 120.00, '2025-08-13', 'General; 29.22', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat de pièces de rechange nécessaire pour la réparation du moteur puit', 6, 51.015, '2025-08-13', 'General; 12.42', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Changement de bargatère +code au niveau de vestiaire des ouvriers', 0, 32.25, '2025-08-14', 'BLOC E; 7.85', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Réparation porte aluminium du bloc C', 6, 80.00, '2025-08-14', 'BLOC C; 17.38', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Main d''oeuvre réparation du moteur du puit de jardin', 6, 100.00, '2025-08-14', 'General; 24.35', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Nettoyage du vestiaire faite par M Tahar', 3, 20.00, '2025-08-17', 'General; 4.87', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat de charnière porte d''entrée bloc C', 6, 32.85, '2025-08-17', 'BLOC C; 7.99', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Travaux de maçonnerie réfection canalisation d''eau B05', 6, 50.00, '2025-08-17', 'BLOC B; 12.18', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat mélangeur+robinet+petit outillage pour la vestiaire des ouvriers par JAMEL MESFAR', 7, 170.00, '2025-08-10', 'General; 41.40', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Complément frais d''entretien des ragards faouzi', 0, 70.00, '2025-08-24', 'General; 17.05', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat de produits de nettoyage mensuel', 3, 23.00, '2025-08-24', 'General; 5.60', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat de 2 bidons de javel de 4,5l du carrefour', 3, 11.08, '2025-08-24', 'General; 2.70', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat d''1 carte oreedo+ photocopie', 10, 8.90, '2025-08-29', 'General; 2.17', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat d''1 abattant du carrefour pour la vestiaire', 0, 12.70, '2025-08-30', 'BLOC A; 3.09', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat de 20 sacs de poubelles pour le nettoyage de jardin', 3, 15.00, '2025-09-30', 'General; 3.65', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Réparation tendeuse du jardin', 5, 100.00, '2025-09-14', 'General; 24.35', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'STEG services généraux des 5 blocs', 1, 419.00, '2025-09-15', 'General; 102.03', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat de 4 bouteilles d''un litre de nettoyage de carrelage', 3, 20.46, '2025-09-20', 'General; 4.98', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat de 3 lampes pour jardin', 7, 19.50, '2025-09-15', 'General; 4.75', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat de sac poubelles pour collecte de déchets', 3, 4.90, '2025-09-15', 'General; 1.19', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat de produits de nettoyage de carrelage', 3, 20.46, '2025-09-24', 'General; 4.98', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat d''une poubelle occasion 100L', 7, 50.00, '2025-09-21', 'General; 12.18', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Frais de nettoyage de patio entre bloc D et bloc C', 3, 40.00, '2025-09-21', 'General; 9.74', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Frais de nettoyage des toits des magasins', 3, 40.00, '2025-09-21', 'General; 9.74', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat eau de javel +judy fleurs', 3, 22.46, '2025-09-24', 'General; 5.47', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Maintenance expresse ascenseur période 08/2025-10/2025', 0, 296.548, '2025-09-24', 'General; 72.21', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Assainissement des ragards périodique', 0, 200.00, '2025-09-28', 'General; 48.70', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat de produits de nettoyage', 3, 24.00, '2025-09-28', 'General; 5.84', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat de produits de nettoyage de carrelage', 3, 20.46, '2025-09-27', 'General; 4.98', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat de 2 rouleaux surfacaire et pinceau du souk lahad', 3, 8.50, '2025-09-28', 'General; 2.07', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat 5 petits poubelle occasions', 7, 20.00, '2025-10-05', 'General; 4.87', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Photocopie 49 copies', 3, 4.90, '2025-10-04', 'General; 1.19', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat de gant pour issam', 3, 3.00, '2025-10-05', 'General; 0.73', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat de lampes+douille', 1, 32.905, '2025-10-06', 'General; 8.01', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat 1 boule 300 pour le jardin', 5, 69.00, '2025-10-06', 'General; 16.80', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat 1/2 camion de sable + 3 sacs de ciments', 6, 151.00, '2025-10-06', 'General; 36.77', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat de sacs pour ramassage de sables', 10, 5.00, '2025-10-12', 'General; 1.22', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat de divers articles occasions', 10, 31.00, '2025-10-12', 'General; 7.55', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat de produits de peinture', 6, 93.00, '2025-10-13', 'General; 22.65', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat eau de javel +desodorisant', 3, 20.20, '2025-10-14', 'General; 4.92', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat surfacaire 23kg', 6, 75.00, '2025-10-17', 'General; 18.26', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat 4 kg de ciment blanc', 6, 4.00, '2025-10-17', 'General; 0.97', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat de rouleaux sckotch pour travaux de peinture', 6, 6.00, '2025-10-18', 'General; 1.46', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat 5 element de sillage jardin du souk lahad', 5, 180.00, '2025-10-19', 'General; 43.83', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Frais de nettoyage du parking ISSAM', 3, 20.00, '2025-10-19', 'General; 4.87', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Peintures hall C', 6, 200.00, '2025-10-19', 'BLOC C; 48.70', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Debarras des placards des escaliers des 5 blocs et action de désinfection', 3, 150.00, '2025-10-19', 'General; 36.53', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Photocopie 51 copies+achat 1 stylo', 3, 6.10, '2025-10-20', 'General; 1.49', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat de flexible pour toilette du vestiaire', 0, 6.30, '2025-10-21', 'General; 1.53', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, '2 cartes Oreedo de 5,70d', 10, 11.40, '2025-10-22', 'General; 2.78', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Perte exceptionnelle suite annulation d''achat de 2 LAMPES ES 50 WATT LED', 10, 2.00, '2025-10-23', 'General; 0.49', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat de 4 lampes 18WATT', 7, 32.014, '2025-10-23', 'General; 7.80', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'STEG 5 BLOCS', 1, 548.00, '2025-10-23', 'General; 133.44', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat 1 bouteille désodorisant khotaf', 3, 6.50, '2025-10-24', 'General; 1.58', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat de ciment blanc 5KG', 6, 4.00, '2025-10-26', 'General; 0.97', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat du produits de nettoyage du souk lahad', 3, 24.00, '2025-10-26', 'General; 5.84', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Travaux d''aménagement faite par faouzi', 6, 30.00, '2025-10-26', 'General; 7.31', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat des insecticides', 3, 64.40, '2025-10-29', 'General; 15.68', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Photocopies 40 copies lettres', 3, 4.00, '2025-11-04', 'General; 0.97', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat 5 bacs à fleurs', 5, 160.00, '2025-11-04', 'General; 38.96', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Photocopies 27 copies lettres de recouvrement', 3, 2.70, '2025-11-05', 'General; 0.66', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Travaux de nettoyage du parking issam', 3, 20.00, '2025-11-09', 'General; 4.87', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat de ciment blanc et mastic', 6, 8.90, '2025-11-13', 'General; 2.17', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat de ciment blanc', 6, 6.00, '2025-11-14', 'General; 1.46', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat de produits de nettoyage', 3, 44.00, '2025-11-16', 'General; 10.71', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Assainissement de tous les ragards de la cité', 0, 200.00, '2025-11-29', 'General; 48.70', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat 2 cartes de tel 5,7*2', 10, 11.40, '2025-11-29', 'General; 2.78', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat de frein hydraulique + kalbkouba', 6, 98.50, '2025-12-01', 'General; 23.98', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Photocopies 80 copies', 3, 8.00, '2025-12-01', 'General; 1.95', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Agios bancaires', 9, 17.85, '2025-12-02', 'General; 4.35', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Frais de maintenance express asc NOV 2025-01/2026', 0, 296.546, '2025-12-02', 'General; 72.21', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat de 2 bidons de javel 4,5l', 3, 10.58, '2025-12-02', 'General; 2.58', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Photocopies lettres de recouvrement', 3, 4.50, '2025-12-02', 'General; 1.10', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Travaux d''évacuation des sablees', 6, 30.00, '2025-12-05', 'General; 7.31', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat antirouille+peinture +paper abrasif', 6, 48.10, '2025-12-05', 'General; 11.71', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat produits de nettoyage', 3, 23.00, '2025-12-14', 'General; 5.60', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat 5 tapies moquettes du souk', 6, 10.00, '2025-12-14', 'General; 2.44', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat d''1 boite à lettre occasion', 10, 5.00, '2025-12-14', 'General; 1.22', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Frais de pose des verres au niveau des murs cloture de la cité', 6, 150.00, '2025-12-14', 'General; 36.53', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Réparation serrure de porte bloc C', 6, 50.00, '2025-12-14', 'BLOC C; 12.18', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat de 2 rives blocs +douille', 7, 32.00, '2025-12-15', 'General; 7.79', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat de 7 serrures + 2 cash prises', 6, 95.70, '2025-12-16', 'General; 23.30', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Photocopies 30 copies +1 rouleau sctoch+1 stylo', 3, 4.85, '2025-12-20', 'General; 1.18', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Travaux d''entretien changement des lampes et glaces et serrures des portes par issam', 0, 50.00, '2025-12-21', 'General; 12.18', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Debarras des blocs des produits non utilisées', 3, 50.00, '2025-12-21', 'General; 12.18', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat de lampes 8 lampes led 12w par khaled', 7, 48.004, '2025-12-26', 'General; 11.69', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat 10 lampes', 7, 50.00, '2025-12-25', 'General; 12.18', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat 10 lampes du carrefour', 7, 40.00, '2025-12-25', 'General; 9.74', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Travaux d''aménagement du parking', 6, 30.00, '2025-12-25', 'General; 7.31', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Travaux de nettoyage de parking issam', 3, 20.00, '2025-12-27', 'General; 4.87', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Cadeaux de fin d''année', 10, 200.00, '2025-12-27', 'General; 48.70', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Avance n 1 pour réparer l''ascenseur BLOC A', 0, 7000.00, '2025-12-28', 'BLOC A; 1704.50', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat de 2 lampes LED 15W jardin', 7, 13.00, '2025-12-29', 'General; 3.17', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat de produits de nettoyages', 3, 33.96, '2026-01-03', 'General; 8.27', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat d''1 cadre en fer pour siphon et marbre bloc D', 6, 40.00, '2026-01-05', 'BLOC D; 9.74', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, '70 copies photocopie', 3, 5.60, '2026-01-05', 'General; 1.36', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, '50 copies photocopie', 3, 5.00, '2026-01-06', 'General; 1.22', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat de 2 cartes oreedo', 10, 11.40, '2026-01-06', 'General; 2.78', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Travaux d''assainissement ragards PAR 1 Société', 0, 180.00, '2026-01-10', 'General; 43.83', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Travaux d''entretien ISSAM (remplacement des lampes)', 7, 40.00, '2026-01-11', 'General; 9.74', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat de 2 bars de fer pour assainissement des ragards', 6, 36.00, '2026-01-08', 'General; 8.77', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat de marbre 59*59 bloc D', 6, 45.00, '2026-01-12', 'BLOC D; 10.96', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat de marbre 72*37 pour ragard bloc c', 6, 31.50, '2026-01-14', 'BLOC C; 7.67', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Frais financiers 479.747-461.897', 9, 17.84, '2026-01-15', 'General; 4.34', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Frais de nettoyage de parking issam', 3, 20.00, '2026-01-18', 'General; 4.87', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, '25 photocopies', 3, 3.75, '2026-01-18', 'General; 0.91', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, '7,5 m bach +1 rouleau sckotch pour couvertures medhaouis des 5 blocs', 6, 20.25, '2026-01-21', 'General; 4.93', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Socle ragard couloir BLOC C', 6, 60.00, '2026-01-21', 'BLOC C; 14.61', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, '20 copies photocopies', 3, 1.60, '2026-01-22', 'General; 0.39', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat de molure pour installation de caméra', 4, 3.50, '2026-01-24', 'General; 0.85', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat de produits de nettoyage du carrefour', 3, 20.00, '2026-01-24', 'General; 4.87', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat d''1 caméra wifi et accessoires', 4, 342.00, '2026-01-25', 'General; 83.28', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat antirouille+peinture +molures par hb', 6, 19.00, '2026-01-29', 'General; 4.63', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat de fer d''attache +cables de serrage', 6, 2.80, '2026-01-29', 'General; 0.68', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Frais de réparation DE CAMERA', 4, 20.00, '2026-01-29', 'General; 4.87', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat des produits de nettoyage', 3, 23.70, '2026-01-29', 'General; 5.77', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat bouteille insecticide', 3, 23.00, '2026-01-28', 'General; 5.60', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Complément frais de peinture hall BLOC C', 6, 50.00, '2026-02-01', 'BLOC C; 12.18', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Travaux de peinture bureau de syndic', 6, 120.00, '2026-02-01', 'General; 29.22', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Frais d''assainissement de tous les ragards de la cité', 0, 250.00, '2026-02-01', 'General; 60.88', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Photocopie 31 copies lettre de recouvrement', 3, 2.45, '2026-02-02', 'General; 0.60', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, '5 prises bipoles ref 45', 7, 15.00, '2026-02-08', 'General; 3.65', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat de fil PAR hb', 6, 42.433, '2026-02-15', 'General; 10.33', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'DON RAMADAN', 10, 250.00, '2026-02-15', 'General; 60.88', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat 120kg surfacaire+4kg regzime du carrefour', 6, 283.10, '2026-02-16', 'General; 68.93', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat eau de javel judy 9 l', 3, 10.28, '2026-02-18', 'General; 2.50', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat 5 kg peint blanc+2kg peint noir +1l diluant+sac de mastic', 6, 107.17, '2026-02-20', 'General; 26.10', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat d''1 palette +5 papiers abrasive n 2', 6, 12.50, '2026-02-21', 'General; 3.04', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat produits de nettoyages', 3, 21.70, '2026-02-22', 'General; 5.28', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat de 1 sac de mastic', 6, 35.00, '2026-02-23', 'General; 8.52', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat de rame papier', 10, 12.50, '2026-02-23', 'General; 3.04', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat 10 papier abrasif pour les travaux de peintures', 6, 10.00, '2026-02-27', 'General; 2.44', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat de brouleau papier collant', 6, 22.00, '2026-02-28', 'General; 5.36', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat de 2kg peinture blanc +1l diluant', 6, 30.50, '2026-02-28', 'General; 7.43', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat de cables et de boite par hichem ben naser', 7, 47.00, '2026-03-01', 'General; 11.44', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat de caméras et accessoires', 4, 450.00, '2026-03-01', 'General; 109.58', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat 9 rouleaux pour traçage', 6, 9.00, '2026-03-01', 'General; 2.19', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Paiement 2 éme avance par asc bloc A', 0, 3000.00, '2026-03-01', 'BLOC A; 730.50', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Photocopie sensibilisation pour rep asc bloc A', 3, 3.20, '2026-03-02', 'General; 0.78', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat diluant+peint laque+2kg noir +surf acaire 40 kg', 6, 177.00, '2026-03-07', 'General; 43.10', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat papier collon mur', 6, 10.50, '2026-03-07', 'General; 2.56', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Travaux de peinture 5 blocs', 6, 500.00, '2026-03-07', 'General; 121.75', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Travaux de nettoyage du parking issam', 3, 20.00, '2026-03-08', 'General; 4.87', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Travaux de plomberie t+100d m.o avance', 2, 792.792, '2026-03-08', 'General; 193.04', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Don Aid', 10, 300.00, '2026-03-08', 'General; 73.05', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat surfacaire 10 kg', 6, 23.00, '2026-03-14', 'General; 5.60', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'STEG services généraux de 5 blocs', 1, 981.00, '2026-03-16', 'General; 238.87', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, '10 omo matic+9l javel +10sachet collecte dechets ragard', 3, 18.50, '2026-03-23', 'General; 4.50', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat complementaire de surfacaire 10 kg', 6, 23.00, '2026-03-24', 'General; 5.60', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Assainissement de tous les ragards de la cité', 0, 250.00, '2026-03-27', 'General; 60.88', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat cadenas +hakaka hdid de geant', 6, 14.70, '2026-03-26', 'General; 3.58', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat de 2 raclettes', 10, 3.10, '2026-03-29', 'General; 0.75', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Réparation 4 garnitures coulisseaux asc bloc D', 0, 1377.114, '2026-04-02', 'BLOC D; 335.33', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat 10l javel+3l lave vitre', 3, 16.78, '2026-04-04', 'General; 4.09', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Photocopie sensibilisation pour la rép de l''asc bloc a+reçus', 3, 4.80, '2026-04-04', 'General; 1.17', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat bton pour raclette +sachets por gazon+1 rouleau sckotch', 6, 4.50, '2026-04-05', 'General; 1.10', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat 10 bouttons poussoirs', 7, 35.00, '2026-04-06', 'General; 8.52', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'AVANCE FAOUZI', 10, 162.00, '2026-04-06', 'General; 39.45', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat de 10 cahier de 48 pages', 10, 9.50, '2026-04-02', 'General; 2.31', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat d''une brouette pour travaux', 5, 160.00, '2026-04-08', 'General; 38.96', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Total frais de transport et déplacement accomplis', 10, 44.00, '2026-04-08', 'General; 10.71', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Frais de tel 02+03+04 /2026', 10, 34.20, '2026-04-08', 'General; 8.33', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat de 50 coblets+ 36 bouteilles 0,3 cl d''eau safia', 10, 19.30, '2026-04-09', 'General; 4.70', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat de tickets pour inventaire physique du materiel disponible chez le syndic', 10, 1.20, '2026-04-09', 'General; 0.29', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Travaux d''électricité et d''installation des caméras et de maintenance', 4, 400.00, '2026-04-12', 'General; 97.40', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat d''un grand balai complète professionnelle', 3, 8.50, '2026-04-13', 'General; 2.07', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Eau 2 factures', 2, 122.20, '2026-04-16', 'General; 29.76', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Photocopies 30 + enveloppes 150', 3, 24.90, '2026-04-17', 'General; 6.06', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat de molure+vies +serrecable pour réglage de caméra bloc a', 4, 15.00, '2026-04-17', 'General; 3.65', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Avance pour achat d''1 répétiteur HB', 7, 60.00, '2026-04-19', 'General; 14.61', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'ACHAT 2 CAF2S', 10, 5.00, '2026-04-19', 'General; 1.22', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'ACHAT 1L DILUANT+2KG LAQUE NOIR+2P ABRASIF+1KG ANTIROUILLE', 6, 41.90, '2026-04-20', 'General; 4.38', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat rateau', 5, 18.00, '2026-04-21', 'General; 4.50', GETUTCDATE(), 0),
(NEWID(), @ResidenceId, 'Achat 50 portes clé pour ascenseur bloc a et 100 petit saches', 10, 18.50, '2026-04-21', 'General; 4.50', GETUTCDATE(), 0);

-- Enable foreign key constraints
ALTER TABLE Expenses CHECK CONSTRAINT ALL;

-- ============================================================================
-- Verification and Reporting
-- ============================================================================
PRINT '';
PRINT '============================================================================';
PRINT 'Expense Data Insertion Complete';
PRINT '============================================================================';
PRINT '';

-- Summary by Expense Type
PRINT 'EXPENSE BREAKDOWN BY TYPE:';
PRINT '---';
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
		WHEN 8 THEN 'Insurance'
		WHEN 9 THEN 'Taxes'
		WHEN 10 THEN 'Other'
	END AS ExpenseType,
	COUNT(*) AS ExpenseCount,
	CAST(SUM(Amount) AS DECIMAL(10, 2)) AS TotalAmount,
	CAST(AVG(Amount) AS DECIMAL(10, 2)) AS AverageAmount
FROM Expenses
WHERE ResidenceId = @ResidenceId
GROUP BY Type
ORDER BY Type;

PRINT '';
PRINT 'TOTAL EXPENSES SUMMARY:';
PRINT '---';
SELECT 
	COUNT(*) AS TotalExpenses,
	CAST(SUM(Amount) AS DECIMAL(10, 2)) AS TotalAmount,
	CAST(MIN(Amount) AS DECIMAL(10, 2)) AS MinAmount,
	CAST(MAX(Amount) AS DECIMAL(10, 2)) AS MaxAmount,
	CAST(AVG(Amount) AS DECIMAL(10, 2)) AS AvgAmount
FROM Expenses
WHERE ResidenceId = @ResidenceId;

PRINT '';
PRINT 'TOP 10 MOST EXPENSIVE ITEMS:';
PRINT '---';
SELECT TOP 10
	Title,
	CAST(Amount AS DECIMAL(10, 2)) AS Amount,
	ExpenseDate
FROM Expenses
WHERE ResidenceId = @ResidenceId
ORDER BY Amount DESC;

PRINT '';
PRINT 'MONTHLY EXPENSE SUMMARY:';
PRINT '---';
SELECT 
	YEAR(ExpenseDate) AS [Year],
	MONTH(ExpenseDate) AS [Month],
	FORMAT(ExpenseDate, 'MMMM yyyy') AS MonthYear,
	COUNT(*) AS ExpenseCount,
	CAST(SUM(Amount) AS DECIMAL(10, 2)) AS TotalAmount
FROM Expenses
WHERE ResidenceId = @ResidenceId
GROUP BY YEAR(ExpenseDate), MONTH(ExpenseDate), FORMAT(ExpenseDate, 'MMMM yyyy')
ORDER BY YEAR(ExpenseDate), MONTH(ExpenseDate);

PRINT '';
PRINT '============================================================================';
PRINT 'Process Completed Successfully!';
PRINT '============================================================================';
