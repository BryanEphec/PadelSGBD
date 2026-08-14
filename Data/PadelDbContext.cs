using Microsoft.EntityFrameworkCore;
using Padel.SGBD.Api.Model;

namespace Padel.SGBD.Api.Data
{
    public class PadelSGBDContext : DbContext
    {
        public PadelSGBDContext(DbContextOptions<PadelSGBDContext> options) : base(options)
        {
        }

        public DbSet<Site> Sites { get; set; }
        public DbSet<Membre> Membres { get; set; }
        public DbSet<Terrain> Terrains { get; set; }
        public DbSet<Match> Matchs { get; set; }
        public DbSet<Participations> Participations { get; set; }
        public DbSet<Fermeture> Fermetures { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1. Clé composite pour Participation
            modelBuilder.Entity<Participations>()
                .HasKey(p => new { p.Matricule, p.IdMatch });

            // 2. Relations Participation -> Membre et Participation -> Match
            modelBuilder.Entity<Participations>()
                .HasOne(p => p.Membre)
                .WithMany(m => m.Participations)
                .HasForeignKey(p => p.Matricule)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Participations>()
                .HasOne(p => p.Match)
                .WithMany(m => m.Participations)
                .HasForeignKey(p => p.IdMatch)
                .OnDelete(DeleteBehavior.Cascade);

            // 3. Relation Terrain -> Site
            modelBuilder.Entity<Terrain>()
                .HasOne(t => t.Site)
                .WithMany(s => s.Terrains)
                .HasForeignKey(t => t.IdSite)
                .OnDelete(DeleteBehavior.Cascade);

            // 4. Relation Match -> Terrain
            modelBuilder.Entity<Match>()
                .HasOne(m => m.Terrain)
                .WithMany()
                .HasForeignKey(m => m.IdTerrain)
                .OnDelete(DeleteBehavior.Restrict);

            // 5. Relation Fermeture -> Site (IdSite nullable pour fermeture globale)
            modelBuilder.Entity<Fermeture>()
                .HasOne(f => f.Site)
                .WithMany()
                .HasForeignKey(f => f.IdSite)
                .OnDelete(DeleteBehavior.Cascade);

            // 6. Contrainte sur le format du type de membre (G, S, L)
            modelBuilder.Entity<Membre>()
                .ToTable(t => t.HasCheckConstraint("CK_Membre_Type", "[Type] IN ('G', 'S', 'L')"));
        }
    }
}