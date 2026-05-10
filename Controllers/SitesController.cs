
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Padel.SGBD.Api.Data;
using Padel.SGBD.Api.Model;

namespace Padel.SGBD.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SitesController : ControllerBase
    {
        private readonly PadelSGBDContext _context;

        public SitesController(PadelSGBDContext context)
        {
            _context = context;
        }

        // GET: api/SitesControllers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Site>>> GetSites()
        {
            return await _context.Site.ToListAsync();
        }

        // GET: api/SitesControllers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Site>> GetSite(int id)
        {
            var site = await _context.Site.FindAsync(id);

            if (site == null)
            {
                return NotFound();
            }

            return site;
        }
    }
}