using Microsoft.EntityFrameworkCore;
using Padel.SGBD.Api.Data;
using Padel.SGBD.Api.Model;
using Padel.SGBD.Api.Repositories.Interfaces;

namespace Padel.SGBD.Api.Repositories
{
    public class ParticipationRepository : IParticipationRepository
    {
        private readonly PadelSGBDContext _context;

        public ParticipationRepository(PadelSGBDContext context)
        {
            _context = context;
        }

        public async Task<Participations?> GetParticipationAsync(string matricule, int idMatch)
        {
            return await _context.Participations
                .Include(p => p.Membre)
                .Include(p => p.Match)
                .FirstOrDefaultAsync(p => p.Matricule == matricule && p.IdMatch == idMatch);
        }

        public async Task<IEnumerable<Participations>> GetByMatchIdAsync(int idMatch)
        {
            return await _context.Participations
                .Include(p => p.Membre)
                .Where(p => p.IdMatch == idMatch)
                .ToListAsync();
        }

        public async Task<Participations> AddAsync(Participations participation)
        {
            _context.Participations.Add(participation);
            await _context.SaveChangesAsync();
            return participation;
        }

        public async Task UpdateAsync(Participations participation)
        {
            _context.Participations.Update(participation);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string matricule, int idMatch)
        {
            var participation = await _context.Participations
                .FirstOrDefaultAsync(p => p.Matricule == matricule && p.IdMatch == idMatch);

            if (participation != null)
            {
                _context.Participations.Remove(participation);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> CountByMatchIdAsync(int idMatch)
        {
            return await _context.Participations.CountAsync(p => p.IdMatch == idMatch);
        }
    }
}