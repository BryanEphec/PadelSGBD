USE master;
GO

-- On reste sur le nom d'origine que tu as utilisé pour tes deux projets
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'PadelDB')
BEGIN
    CREATE DATABASE PadelDB;
END
GO

USE PadelDB;
GO

-- 2. SUPPRESSION DES TABLES (dans l'ordre inverse des dépendances)
IF OBJECT_ID('Participation', 'U') IS NOT NULL DROP TABLE Participation;
IF OBJECT_ID('Fermeture', 'U') IS NOT NULL DROP TABLE Fermeture;
IF OBJECT_ID('Match', 'U') IS NOT NULL DROP TABLE Match;
IF OBJECT_ID('Membre', 'U') IS NOT NULL DROP TABLE Membre;
IF OBJECT_ID('Terrain', 'U') IS NOT NULL DROP TABLE Terrain;
IF OBJECT_ID('Site', 'U') IS NOT NULL DROP TABLE Site;
GO

-- 3. CREATION DES TABLES
CREATE TABLE Site (
    IdSite INT PRIMARY KEY IDENTITY(1,1),
    Nom VARCHAR(50) NOT NULL,
    Ville VARCHAR(50) NOT NULL,
    HOuverture TIME NOT NULL,
    HFermeture TIME NOT NULL
);

CREATE TABLE Terrain (
    IdTerrain INT PRIMARY KEY IDENTITY(1,1),
    Nom_Terrain VARCHAR(20) NOT NULL,
    IdSite INT NOT NULL,
    CONSTRAINT FK_Terrain_Site FOREIGN KEY (IdSite) REFERENCES Site(IdSite)
);

CREATE TABLE Membre (
    Matricule VARCHAR(6) PRIMARY KEY,
    Nom VARCHAR(50) NOT NULL,
    Prenom VARCHAR(50) NOT NULL,
    Type CHAR(1) NOT NULL,
    IdSiteRatt INT NULL,
    SousPenalite BIT DEFAULT 0,
    CONSTRAINT CK_Membre_Type CHECK (Type IN ('G', 'S', 'L')),
    CONSTRAINT CK_Matricule_Format CHECK (Matricule LIKE '[GSL]%'),
    CONSTRAINT FK_Membre_Site FOREIGN KEY (IdSiteRatt) REFERENCES Site(IdSite)
);

CREATE TABLE Match (
    IdMatch INT PRIMARY KEY IDENTITY(1,1),
    DateHeure DATETIME NOT NULL,
    EstPrive BIT NOT NULL,
    IdTerrain INT NOT NULL,
    CONSTRAINT FK_Match_Terrain FOREIGN KEY (IdTerrain) REFERENCES Terrain(IdTerrain)
);

CREATE TABLE Participation (
    Matricule VARCHAR(6) NOT NULL,
    IdMatch INT NOT NULL,
    EstOrganisateur BIT NOT NULL,
    APaye BIT DEFAULT 0,
    PRIMARY KEY (Matricule, IdMatch),
    CONSTRAINT FK_Part_Membre FOREIGN KEY (Matricule) REFERENCES Membre(Matricule),
    CONSTRAINT FK_Part_Match FOREIGN KEY (IdMatch) REFERENCES Match(IdMatch)
);

CREATE TABLE Fermeture (
    IdFermeture INT PRIMARY KEY IDENTITY(1,1),
    DateFermeture DATE NOT NULL,
    IdSite INT NULL,
    CONSTRAINT FK_Ferm_Site FOREIGN KEY (IdSite) REFERENCES Site(IdSite)
);