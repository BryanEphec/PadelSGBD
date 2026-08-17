using Padel.SGBD.Api.Dtos;
using Padel.SGBD.Api.Model;

namespace Padel.SGBD.Api.Services.Interfaces
{
    public interface IMatchService
    {
        Task<Match?> GetMatchByIdAsync(int id);
        Task<IEnumerable<Match>> GetAllMatchesAsync();
        Task<Match> CreerReservationAsync(string matriculeOrganisateur, MatchCreateDtos dto);
        Task AnnulerMatchAsync(int idMatch, string matriculeDemandeur);
    }
}