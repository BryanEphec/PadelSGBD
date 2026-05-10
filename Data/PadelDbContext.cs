using Microsoft.EntityFrameworkCore;


namespace Padel.SGBD.Api.Data
{
    public class PadelSGBDContext : DbContext
    {
        public PadelSGBDContext(DbContextOptions<PadelSGBDContext> options) : base(options)
        {
            
        }
        public DbSet<Model.Site> Sites { get; set; }
        public DbSet<Model.Membre> Membres { get; set; }
        public DbSet<Model.Terrain> Terrains { get; set; }
    }
}
