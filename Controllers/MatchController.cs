namespace Padel.SGBD.Api.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Padel.SGBD.Api.Data;
    using Padel.SGBD.Api.Model;
    using Padel.SGBD.Api.Dtos;
    [ApiController]
    [Route("api/[controller]")]
    public class MatchController : ControllerBase
    {
        private readonly PadelSGBDContext _context;

        public MatchController(PadelSGBDContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult CreateMatch(MatchCreateDtos matchDto)
        {
            var match = new Match
            {
                EstPrive = matchDto.EstPrive,
                IdTerrain = matchDto.IdTerrain,
                DateHeure = matchDto.DateHeure
            };

            _context.Match.Add(match);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetMatchById), new { id = match.IdMatch }, match);
        }

        [HttpGet("{id}")]
        public IActionResult GetMatchById(int id)
        {
            var match = _context.Match.Find(id);
            if (match == null)
            {
                return NotFound();
            }
            return Ok(match);
        }
        [HttpGet]
        public IActionResult GetAllMatches()
        {
            var matches = _context.Match.ToList();
            return Ok(matches);
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteMatch(int id)
        {
            var match = _context.Match.Find(id);
            if (match == null)
            {
                return NotFound();
            }
            _context.Match.Remove(match);
            _context.SaveChanges();
            return NoContent();
        }
    }
}