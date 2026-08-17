using Padel.SGBD.Api.Model;
using Padel.SGBD.Api.Repositories.Interfaces;
using Padel.SGBD.Api.Services.Interfaces;

namespace Padel.SGBD.Api.Services
{
    public class ParticipationService : IParticipationService
    {
        private readonly IParticipationRepository _participationRepo;
        private readonly IMatchRepository _matchRepo;
        private readonly IMembreRepository _membreRepo;

        public ParticipationService(
            IParticipationRepository participationRepo,
            IMatchRepository matchRepo,
            IMembreRepository membreRepo)
        {
            _participationRepo = participationRepo;
            _matchRepo = matchRepo;
            _membreRepo = membreRepo;
        }

        public async Task<Participations> RejoindreMatchAsync(string matricule, int idMatch)
        {
            var match = await _matchRepo.GetMatchByIdAsync(idMatch);
            if (match == null)
            {
                throw new InvalidOperationException("Match introuvable.");
            }

            var membre = await _membreRepo.GetByMatriculeAsync(matricule);
            if (membre == null)
            {
                throw new InvalidOperationException("Membre introuvable.");
            }

            if (membre.SoldeDu > 0)
            {
                throw new InvalidOperationException("Impossible de rejoindre un match avec un solde impayé.");
            }

            if (membre.DateFinPenalite.HasValue && membre.DateFinPenalite.Value > DateTime.Now)
            {
                throw new InvalidOperationException("Impossible de rejoindre un match sous le coup d'une pénalité.");
            }

            var nombreParticipants = await _participationRepo.CountByMatchIdAsync(idMatch);
            if (nombreParticipants >= 4)
            {
                throw new InvalidOperationException("Le match est complet (4 joueurs maximum).");
            }

            var participationExistante = await _participationRepo.GetParticipationAsync(matricule, idMatch);
            if (participationExistante != null)
            {
                throw new InvalidOperationException("Le joueur participe déjà à ce match.");
            }

            var nouvelleParticipation = new Participations
            {
                Matricule = matricule,
                IdMatch = idMatch,
                EstOrganisateur = false,
                APaye = false,
                MontantPaye = 0m
            };

            return await _participationRepo.AddAsync(nouvelleParticipation);
        }

        public async Task EnregistrerPaiementAsync(string matricule, int idMatch, decimal montant)
        {
            var participation = await _participationRepo.GetParticipationAsync(matricule, idMatch);
            if (participation == null)
            {
                throw new InvalidOperationException("Participation introuvable pour ce match.");
            }

            if (montant < 15.00m)
            {
                throw new InvalidOperationException("La part individuelle pour un match est de 15.00 € minimum.");
            }

            participation.APaye = true;
            participation.MontantPaye = montant;
            participation.DatePaiement = DateTime.Now;

            await _participationRepo.UpdateAsync(participation);
        }

        public async Task QuitterMatchAsync(string matricule, int idMatch)
        {
            var participation = await _participationRepo.GetParticipationAsync(matricule, idMatch);
            if (participation == null)
            {
                throw new InvalidOperationException("Participation introuvable.");
            }

            if (participation.EstOrganisateur)
            {
                throw new InvalidOperationException("L'organisateur ne peut pas quitter le match, il doit l'annuler.");
            }

            await _participationRepo.DeleteAsync(matricule, idMatch);
        }
    }
}