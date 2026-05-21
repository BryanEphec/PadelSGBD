namespace Padel.SGBD.Api.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Padel.SGBD.Api.Data;
    using Padel.SGBD.Api.Model;
    using Padel.SGBD.Api.Dtos;

    [ApiController]
    [Route("api/[controller]")]
    public class MembreController : ControllerBase
    {
        private readonly PadelSGBDContext _context;

        public MembreController(PadelSGBDContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult CreateMembre(MembreCreateDtos membreDto) // Mis au singulier si ton fichier n'a pas de 's'
        {
            var membre = new Membre
            {
                // C'est ICI que l'API génère le matricule de 7 caractères automatiquement !
                Matricule = Guid.NewGuid().ToString().Substring(0, 7).ToUpper(),
                Nom = membreDto.Nom,
                Prenom = membreDto.Prenom,
                Type = membreDto.Type,
                IdSiteRatt = membreDto.IdSiteRatt,
                SousPenalite = membreDto.SousPenalite
            };

            _context.Membres.Add(membre);
            _context.SaveChanges();

            // Attention : on utilise "matricule" en minuscule pour correspondre au paramètre du GET en dessous
            return CreatedAtAction(nameof(GetMembre), new { matricule = membre.Matricule }, membre);
        }

        [HttpGet("{matricule}")]
        public IActionResult GetMembre(string matricule)
        {
            var membre = _context.Membres.Find(matricule);
            if (membre == null)
            {
                return NotFound();
            }
            return Ok(membre);
        }
    }
}