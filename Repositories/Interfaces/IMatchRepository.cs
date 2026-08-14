using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Padel.SGBD.Api.Model;

namespace Padel.SGBD.Api.Repositories.Interfaces
{
    public interface IMatchRepository
    {
        Task<Match?> GetMatchByIdAsync(int id);
        Task<IEnumerable<Match>> GetAllAsync();
        Task<IEnumerable<Match>> GetMatchesByDateAsync(int idTerrain, DateTime date);
        Task<Match> CreateAsync(Match match);
        Task UpdateAsync(Match match);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int idMatch);
    }
}