namespace Padel.SGBD.Api.Controllers
{
    using 
        Microsoft.AspNetCore.Mvc;
    using Padel.SGBD.Api.Data;
    using Padel.SGBD.Api.Dtos;
    using Padel.SGBD.Api.Model;

    [ApiController]
    [Route("api/[controller]")]
    public class ParticipationController : ControllerBase
    {
        private readonly PadelSGBDContext _context;

        public ParticipationController(PadelSGBDContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult CreateParticipation([FromBody] ParticipationCreateDto participationDto)
        {
            if (participationDto == null)
            {
                return BadRequest("Participation data is required.");
            }

            // Vérifier que le membre existe
            var membre = _context.Membres.Find(participationDto.Matricule);
            if (membre == null)
            {
                return NotFound($"Membre with Matricule {participationDto.Matricule} not found.");
            }

            // Vérifier que le match existe
            var match = _context.Match.Find(participationDto.IdMatch);
            if (match == null)
            {
                return NotFound($"Match with Id {participationDto.IdMatch} not found.");
            }

            // Créer la participation
            var participation = new Participations
            {
                Matricule = participationDto.Matricule,
                IdMatch = participationDto.IdMatch,
                EstOrganisateur = participationDto.EstOrganisateur
            };

            _context.Participations.Add(participation);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetParticipation), new { matricule = participation.Matricule, idMatch = participation.IdMatch }, participation);
        }

        [HttpGet("{matricule}/{idMatch}")]
        public IActionResult GetParticipation(string matricule, int idMatch)
        {
            var participation = _context.Participations.Find(matricule, idMatch);
            if (participation == null)
            {
                return NotFound($"Participation with Matricule {matricule} and IdMatch {idMatch} not found.");
            }
            return Ok(participation);
        }
        [HttpDelete("{matricule}/{idMatch}")]
        public IActionResult DeleteParticipation(string matricule, int idMatch)
        {
            var participation = _context.Participations.Find(matricule, idMatch);
            if (participation == null)
            {
                return NotFound($"Participation with Matricule {matricule} and IdMatch {idMatch} not found.");
            }
            _context.Participations.Remove(participation);
            _context.SaveChanges();
            return NoContent();
        }
        [HttpPut("{matricule}/{idMatch}")]
        public IActionResult UpdateParticipation(string matricule, int idMatch, [FromBody] ParticipationCreateDto participationDto)
        {
            if (participationDto == null)
            {
                return BadRequest("Participation data is required.");
            }

            var participation = _context.Participations.Find(matricule, idMatch);
            if (participation == null)
            {
                return NotFound($"Participation with Matricule {matricule} and IdMatch {idMatch} not found.");
            }

            // Mettre à jour les propriétés de la participation
            participation.EstOrganisateur = participationDto.EstOrganisateur;

            _context.Participations.Update(participation);
            _context.SaveChanges();

            return NoContent();
        }
        [HttpGet]
        public IActionResult GetAllParticipations()
        {
            var participations = _context.Participations.ToList();
            return Ok(participations);
        }
    }    

}