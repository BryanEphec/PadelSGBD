using Padel.SGBD.Api.Data;
using Padel.SGBD.Api.Dtos;
using Padel.SGBD.Api.Model;
using Padel.SGBD.Api.Repositories.Interfaces;
using Padel.SGBD.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Padel.SGBD.Api.Services
{
    public class MatchService : IMatchService
    {
        private readonly IMatchRepository _matchRepository;
        private readonly IMembreRepository _membreRepository;
        private readonly PadelSGBDContext _context;

        public MatchService(
            IMatchRepository matchRepository,
            IMembreRepository membreRepository,
            PadelSGBDContext context)
        {
            _matchRepository = matchRepository;
            _membreRepository = membreRepository;
            _context = context;
        }

        public async Task<Match?> GetMatchByIdAsync(int id)
        {
            return await _matchRepository.GetMatchByIdAsync(id);
        }

        public async Task<IEnumerable<Match>> GetAllMatchesAsync()
        {
            return await _matchRepository.GetAllAsync();
        }

        public async Task<Match> CreerReservationAsync(string matriculeOrganisateur, MatchCreateDtos dto)
        {
            // 1. Vérification du membre organisateur
            var membre = await _membreRepository.GetByMatriculeAsync(matriculeOrganisateur);
            if (membre == null)
            {
                throw new InvalidOperationException($"Le membre {matriculeOrganisateur} n'existe pas.");
            }

            // 2. Vérification des impayés (Solde dû)
            if (membre.SoldeDu > 0)
            {
                throw new InvalidOperationException($"Réservation impossible : vous avez un solde impayé de {membre.SoldeDu} €.");
            }

            // 3. Vérification des pénalités
            if (membre.DateFinPenalite.HasValue && membre.DateFinPenalite.Value > DateTime.Now)
            {
                throw new InvalidOperationException($"Réservation impossible : vous êtes sous pénalité jusqu'au {membre.DateFinPenalite.Value:dd/MM/yyyy HH:mm}.");
            }

            // 4. Vérification du terrain
            var terrain = await _context.Terrains.Include(t => t.Site).FirstOrDefaultAsync(t => t.IdTerrain == dto.IdTerrain);
            if (terrain == null)
            {
                throw new InvalidOperationException("Le terrain spécifié n'existe pas.");
            }

            // 5. Vérification des délais de réservation selon le type de membre
            var maintenant = DateTime.Now;
            var delaiMax = membre.Type.ToUpper() switch
            {
                "G" => TimeSpan.FromDays(21), // 3 semaines
                "S" => TimeSpan.FromDays(14), // 2 semaines
                "L" => TimeSpan.FromDays(5),  // 5 jours
                _ => throw new InvalidOperationException($"Type de membre invalide : {membre.Type}")
            };

            if (dto.DateHeure < maintenant)
            {
                throw new InvalidOperationException("Impossible de réserver un créneau dans le passé.");
            }

            if (dto.DateHeure > maintenant.Add(delaiMax))
            {
                throw new InvalidOperationException($"Votre statut ({membre.Type}) ne vous autorise à réserver que jusqu'au {maintenant.Add(delaiMax):dd/MM/yyyy}.");
            }

            // Vérification du site de rattachement pour Membre S
            if (membre.Type.ToUpper() == "S" && membre.IdSiteRatt != terrain.IdSite)
            {
                throw new InvalidOperationException("En tant que membre de type 'S', vous ne pouvez réserver que sur votre site de rattachement.");
            }

            // 6. Vérification des jours de fermeture
            var dateReservation = DateOnly.FromDateTime(dto.DateHeure);
            var estFerme = await _context.Fermeture.AnyAsync(f => 
                f.DateFermeture == dateReservation && (f.IdSite == null || f.IdSite == terrain.IdSite));

            if (estFerme)
            {
                throw new InvalidOperationException("Le site est fermé à cette date.");
            }

            // 7. Vérification du chevauchement de créneau (1h30 match + 15 min battement = 105 min)
            var debutNouveau = dto.DateHeure;
            var finNouveau = debutNouveau.AddMinutes(90 + 15);

            var matchsDuJour = await _matchRepository.GetMatchesByDateAsync(dto.IdTerrain, dto.DateHeure);
            foreach (var matchExistant in matchsDuJour)
            {
                var debutExistant = matchExistant.DateHeure;
                var finExistant = debutExistant.AddMinutes(90 + 15);

                // Chevauchement si (DebutA < FinB) ET (FinA > DebutB)
                if (debutNouveau < finExistant && finNouveau > debutExistant)
                {
                    throw new InvalidOperationException("Ce créneau chevauche une réservation existante (créneau de 1h30 + 15 min de battement).");
                }
            }

            // 8. Création du match
            var match = new Match
            {
                EstPrive = dto.EstPrive,
                IdTerrain = dto.IdTerrain,
                DateHeure = dto.DateHeure,
                TarifTotal = 60.00m
            };

            var matchCree = await _matchRepository.CreateAsync(match);

            // 9. Inscription automatique de l'organisateur dans la table Participation
            var participationOrganisateur = new Participations
            {
                Matricule = matriculeOrganisateur,
                IdMatch = matchCree.IdMatch,
                EstOrganisateur = true,
                APaye = false,
                MontantPaye = 0m
            };

            _context.Participations.Add(participationOrganisateur);
            await _context.SaveChangesAsync();

            return matchCree;
        }

        public async Task AnnulerMatchAsync(int idMatch, string matriculeDemandeur)
        {
            var match = await _matchRepository.GetMatchByIdAsync(idMatch);
            if (match == null)
            {
                throw new InvalidOperationException("Match introuvable.");
            }

            var organisateur = match.Participations.FirstOrDefault(p => p.EstOrganisateur);
            if (organisateur != null && organisateur.Matricule != matriculeDemandeur)
            {
                throw new InvalidOperationException("Seul l'organisateur peut annuler ce match.");
            }

            await _matchRepository.DeleteAsync(idMatch);
        }
    }
}