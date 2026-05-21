namespace Padel.SGBD.Api.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Padel.SGBD.Api.Data;
    using Padel.SGBD.Api.Model;
    using Padel.SGBD.Api.Dtos;

    [ApiController]
    [Route("api/[controller]")]
    public class FermetureController : ControllerBase
    {
        private readonly PadelSGBDContext _context;

        public FermetureController(PadelSGBDContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult CreateFermeture(FermetureCreateDtos fermetureDto)
        {
            var fermeture = new Fermeture
            {
                DateFermeture = fermetureDto.DateFermeture,
                IdSite = fermetureDto.IdSite
            };

            _context.Fermeture.Add(fermeture);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetFermeture), new { id = fermeture.IdFermeture }, fermeture);
        }

        [HttpGet("{id}")]
        public IActionResult GetFermeture(int id)
        {
            var fermeture = _context.Fermeture.Find(id);
            if (fermeture == null)
            {
                return NotFound();
            }
            return Ok(fermeture);
        }
    }
}