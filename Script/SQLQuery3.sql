USE PadelDB;
GO

-- Ajoute la colonne 'Type' à la table Terrain
ALTER TABLE Terrain 
ADD Type VARCHAR(50); 
GO