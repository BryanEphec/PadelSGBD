using Microsoft.EntityFrameworkCore;


namespace Padel.SGBD.Api.Data
{
    public class PadelSGBDContext : DbContext
    {
        public PadelSGBDContext(DbContextOptions<PadelSGBDContext> options) : base(options)
        {
            
        }
    }
}
