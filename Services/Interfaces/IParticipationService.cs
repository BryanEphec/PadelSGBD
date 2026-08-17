using Padel.SGBD.Api.Model;

namespace Padel.SGBD.Api.Services.Interfaces
{
    public interface IParticipationService
    {
        Task<Participations> RejoindreMatchAsync(string matricule, int idMatch);
        Task EnregistrerPaiementAsync(string matricule, int idMatch, decimal montant);
        Task QuitterMatchAsync(string matricule, int idMatch);
    }
}