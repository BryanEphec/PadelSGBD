create Trigger trg_CheckMembrePenalite
ON Participation
AFTER INSERT, UPDATE
AS
BEGIN
	Set NOCOUNT ON;

	--Si un membre inséré ou modifié est sous pénalité
	IF EXISTS (
		Select 1 
		From inserted i
		JOIN Membre m ON i.Matricule = m.Matricule
		WHERE m.SousPenalite = 1
	)
	BEGIN

		-- On lève une erreur et on annule la transaction
		RAISERROR('Action annulée : Ce joueur est actuellement sous pénalité et ne peut pas rejoindre de match.',16,1);
		ROLLBACK TRANSACTION;
	END
END;