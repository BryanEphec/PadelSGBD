using Microsoft.EntityFrameworkCore;
using Padel.SGBD.Api.Model;


namespace Padel.SGBD.Api.Data
{
    public class PadelSGBDContext : DbContext
    {
        public PadelSGBDContext(DbContextOptions<PadelSGBDContext> options) : base(options)
        {
            
        }
        public DbSet<Site> Site { get; set; }
        public DbSet<Membre> Membre { get; set; }
        public DbSet<Terrain> Terrain { get; set; }
        public DbSet<Match> Match { get; set; }
        public DbSet<Participations> Participations { get; set; }
        public DbSet<Fermeture> Fermeture { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // CONFIGURATION CRUCIALE : Clé composite pour Participation
            modelBuilder.Entity<Participations>().ToTable("Participation")
                .HasKey(p => new { p.Matricule, p.IdMatch });

            base.OnModelCreating(modelBuilder);
        }
    }
}
