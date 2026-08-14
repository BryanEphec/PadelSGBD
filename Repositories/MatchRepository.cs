using Microsoft.EntityFrameworkCore;
using Padel.SGBD.Api.Data;
using Padel.SGBD.Api.Model;
using Padel.SGBD.Api.Repositories.Interfaces;

namespace Padel.SGBD.Api.Repositories
{
    public class MatchRepository : IMatchRepository
    {
        private readonly PadelSGBDContext _context;

        public MatchRepository(PadelSGBDContext context)
        {
            _context = context;
        }

        public async Task<Match?> GetMatchByIdAsync(int id)
        {
            return await _context.Match
                .Include(m => m.Terrain)
                .Include(m => m.Participations)
                    .ThenInclude(p => p.Membre)
                .FirstOrDefaultAsync(m => m.IdMatch == id);
        }

        public async Task<IEnumerable<Match>> GetAllAsync()
        {
            return await _context.Match
                .Include(m => m.Terrain)
                .Include(m => m.Participations)
                .ToListAsync();
        }

        public async Task<IEnumerable<Match>> GetMatchesByDateAsync(int idTerrain, DateTime date)
        {
            var debutJour = date.Date;
            var finJour = debutJour.AddDays(1);

            return await _context.Match
                .Where(m => m.IdTerrain == idTerrain && m.DateHeure >= debutJour && m.DateHeure < finJour)
                .ToListAsync();
        }

        public async Task<Match> CreateAsync(Match match)
        {
            _context.Match.Add(match);
            await _context.SaveChangesAsync();
            return match;
        }

        public async Task UpdateAsync(Match match)
        {
            _context.Match.Update(match);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var match = await _context.Match.FindAsync(id);
            if (match != null)
            {
                _context.Match.Remove(match);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int idMatch)
        {
            return await _context.Match.AnyAsync(m => m.IdMatch == idMatch);
        }
    }
}