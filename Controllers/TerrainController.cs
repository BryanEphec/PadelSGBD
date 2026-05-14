using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Padel.SGBD.Api.Data;
using Padel.SGBD.Api.Model;

namespace Padel.SGBD.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TerrainController : ControllerBase
    {
        private readonly PadelSGBDContext _context;

        public TerrainController(PadelSGBDContext context)
        {
            _context = context;
        }

        // GET: api/Terrain
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Terrain>>> GetTerrains()
        {
            return await _context.Terrains.ToListAsync();
        }

        // GET: api/Terrain/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Terrain>> GetTerrain(int id)
        {
            var terrain = await _context.Terrains.FindAsync(id);

            return terrain == null ? NotFound() : terrain;
        }

        // POST: api/Terrain
        [HttpPost]
        public async Task<ActionResult<Terrain>> PostTerrain(Terrain terrain)
        {
            _context.Terrains.Add(terrain);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTerrain), new { id = terrain.IdTerrain }, terrain);
        }
    }
}