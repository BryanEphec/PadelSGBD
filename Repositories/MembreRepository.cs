using Microsoft.EntityFrameworkCore;
using Padel.SGBD.Api.Data;
using Padel.SGBD.Api.Model;
using Padel.SGBD.Api.Repositories.Interfaces;

namespace Padel.SGBD.Api.Repositories
{
    public class MembreRepository : IMembreRepository
    {
        private readonly PadelSGBDContext _context;

        public MembreRepository(PadelSGBDContext context)
        {
            _context = context;
        }

        public async Task<Membre?> GetByMatriculeAsync(string matricule)
        {
            return await _context.Membres
                .Include(m => m.Site)
                .Include(m => m.Participations)
                .FirstOrDefaultAsync(m => m.Matricule == matricule);
        }

        public async Task<IEnumerable<Membre>> GetAllAsync()
        {
            return await _context.Membres
                .Include(m => m.Site)
                .ToListAsync();
        }

        public async Task<Membre> CreateAsync(Membre membre)
        {
            _context.Membres.Add(membre);
            await _context.SaveChangesAsync();
            return membre;
        }

        public async Task UpdateAsync(Membre membre)
        {
            _context.Membres.Update(membre);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string matricule)
        {
            var membre = await _context.Membres.FindAsync(matricule);
            if (membre != null)
            {
                _context.Membres.Remove(membre);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(string matricule)
        {
            return await _context.Membres.AnyAsync(m => m.Matricule == matricule);
        }
    }
}