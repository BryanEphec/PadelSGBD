Create Procedure sp_GetStatistiquesJoueur
	@Matricule VARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		Count(*) AS TotalMatchsJoues,
		SUM(CASE WHEN EstOrganisateur = 1 THEN 1 ELSE 0 END) AS MatchsOrganises,
		SUM(CASE WHEN APaye = 0 THEN 1 ELSE 0 END) AS NombreDettesFinancieres
	FROM Participation
	WHERE Matricule = @Matricule;
END;