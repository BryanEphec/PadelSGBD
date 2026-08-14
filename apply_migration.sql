IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260814134011_InitialMigration'
)
BEGIN
    CREATE TABLE [Site] (
        [IdSite] int NOT NULL IDENTITY,
        [Nom] nvarchar(max) NOT NULL,
        [Ville] nvarchar(max) NOT NULL,
        [HOuverture] time NOT NULL,
        [HFermeture] time NOT NULL,
        CONSTRAINT [PK_Site] PRIMARY KEY ([IdSite])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260814134011_InitialMigration'
)
BEGIN
    CREATE TABLE [Fermeture] (
        [IdFermeture] int NOT NULL IDENTITY,
        [DateFermeture] date NOT NULL,
        [IdSite] int NULL,
        CONSTRAINT [PK_Fermeture] PRIMARY KEY ([IdFermeture]),
        CONSTRAINT [FK_Fermeture_Site_IdSite] FOREIGN KEY ([IdSite]) REFERENCES [Site] ([IdSite]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260814134011_InitialMigration'
)
BEGIN
    CREATE TABLE [Membre] (
        [Matricule] nvarchar(10) NOT NULL,
        [Nom] nvarchar(100) NOT NULL,
        [Prenom] nvarchar(100) NOT NULL,
        [Type] nvarchar(1) NOT NULL,
        [IdSiteRatt] int NULL,
        [DateFinPenalite] datetime2 NULL,
        [SoldeDu] decimal(10,2) NOT NULL,
        CONSTRAINT [PK_Membre] PRIMARY KEY ([Matricule]),
        CONSTRAINT [CK_Membre_Type] CHECK ([Type] IN ('G', 'S', 'L')),
        CONSTRAINT [FK_Membre_Site_IdSiteRatt] FOREIGN KEY ([IdSiteRatt]) REFERENCES [Site] ([IdSite])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260814134011_InitialMigration'
)
BEGIN
    CREATE TABLE [Terrain] (
        [IdTerrain] int NOT NULL IDENTITY,
        [Nom_Terrain] nvarchar(max) NOT NULL,
        [IdSite] int NOT NULL,
        [Type] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Terrain] PRIMARY KEY ([IdTerrain]),
        CONSTRAINT [FK_Terrain_Site_IdSite] FOREIGN KEY ([IdSite]) REFERENCES [Site] ([IdSite]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260814134011_InitialMigration'
)
BEGIN
    CREATE TABLE [Match] (
        [IdMatch] int NOT NULL IDENTITY,
        [EstPrive] bit NOT NULL,
        [IdTerrain] int NOT NULL,
        [DateHeure] datetime2 NOT NULL,
        [TarifTotal] decimal(10,2) NOT NULL,
        CONSTRAINT [PK_Match] PRIMARY KEY ([IdMatch]),
        CONSTRAINT [FK_Match_Terrain_IdTerrain] FOREIGN KEY ([IdTerrain]) REFERENCES [Terrain] ([IdTerrain]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260814134011_InitialMigration'
)
BEGIN
    CREATE TABLE [Participation] (
        [Matricule] nvarchar(10) NOT NULL,
        [IdMatch] int NOT NULL,
        [EstOrganisateur] bit NOT NULL,
        [APaye] bit NOT NULL,
        [DatePaiement] datetime2 NULL,
        [MontantPaye] decimal(10,2) NOT NULL,
        CONSTRAINT [PK_Participation] PRIMARY KEY ([Matricule], [IdMatch]),
        CONSTRAINT [FK_Participation_Match_IdMatch] FOREIGN KEY ([IdMatch]) REFERENCES [Match] ([IdMatch]) ON DELETE CASCADE,
        CONSTRAINT [FK_Participation_Membre_Matricule] FOREIGN KEY ([Matricule]) REFERENCES [Membre] ([Matricule]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260814134011_InitialMigration'
)
BEGIN
    CREATE INDEX [IX_Fermeture_IdSite] ON [Fermeture] ([IdSite]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260814134011_InitialMigration'
)
BEGIN
    CREATE INDEX [IX_Match_IdTerrain] ON [Match] ([IdTerrain]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260814134011_InitialMigration'
)
BEGIN
    CREATE INDEX [IX_Membre_IdSiteRatt] ON [Membre] ([IdSiteRatt]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260814134011_InitialMigration'
)
BEGIN
    CREATE INDEX [IX_Participation_IdMatch] ON [Participation] ([IdMatch]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260814134011_InitialMigration'
)
BEGIN
    CREATE INDEX [IX_Terrain_IdSite] ON [Terrain] ([IdSite]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260814134011_InitialMigration'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260814134011_InitialMigration', N'10.0.7');
END;

COMMIT;
GO

