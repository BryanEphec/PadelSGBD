using Padel.SGBD.Api.Model;

namespace Padel.SGBD.Api.Repositories.Interfaces
{
    public interface IParticipationRepository
    {
        Task<Participations?> GetParticipationAsync(string matricule, int idMatch);
        Task<IEnumerable<Participations>> GetByMatchIdAsync(int idMatch);
        Task<Participations> AddAsync(Participations participation);
        Task UpdateAsync(Participations participation);
        Task DeleteAsync(string matricule, int idMatch);
        Task<int> CountByMatchIdAsync(int idMatch);
    }
}