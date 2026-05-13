
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

        [HttpPost]
        public async Task<ActionResult<Site>> PostSite(Site site)
        {
            _context.Site.Add(site);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSite), new { id = site.IdSite }, site);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Site>> PutSite(int id, Site site)
        {
            if (id != site.IdSite)    return BadRequest();
            

            _context.Entry(site).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Site.Any(e => e.IdSite == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Site>> DeleteSite(int id)
        {
            var site = await _context.Site.FindAsync(id);
            if (site == null)
            {
                return NotFound();
            }

            _context.Site.Remove(site);
            await _context.SaveChangesAsync();

            return site;
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