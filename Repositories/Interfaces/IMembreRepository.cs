using Padel.SGBD.Api.Model;

namespace Padel.SGBD.Api.Repositories.Interfaces
{
    public interface IMembreRepository
    {
        Task<Membre?> GetByMatriculeAsync(string matricule);
        Task<IEnumerable<Membre>> GetAllAsync();
        Task<Membre> CreateAsync(Membre membre);
        Task UpdateAsync(Membre membre);
        Task DeleteAsync(string matricule);
        Task<bool> ExistsAsync(string matricule);
    }
}