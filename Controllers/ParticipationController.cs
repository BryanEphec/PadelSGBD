using Microsoft.AspNetCore.Mvc;
using Padel.SGBD.Api.Services.Interfaces;

namespace Padel.SGBD.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ParticipationController : ControllerBase
    {
        private readonly IParticipationService _participationService;

        public ParticipationController(IParticipationService participationService)
        {
            _participationService = participationService;
        }

        [HttpPost("matchs/{idMatch}/rejoindre")]
        public async Task<IActionResult> RejoindreMatch(int idMatch, [FromHeader(Name = "X-Matricule")] string matricule)
        {
            if (string.IsNullOrWhiteSpace(matricule))
            {
                return BadRequest("Le matricule du joueur est requis dans l'en-tête X-Matricule.");
            }

            try
            {
                var participation = await _participationService.RejoindreMatchAsync(matricule, idMatch);
                return Ok(participation);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Une erreur inattendue est survenue lors de l'inscription au match.");
            }
        }

        [HttpPost("matchs/{idMatch}/payer")]
        public async Task<IActionResult> PayerParticipation(int idMatch, [FromHeader(Name = "X-Matricule")] string matricule, [FromQuery] decimal montant = 15.00m)
        {
            if (string.IsNullOrWhiteSpace(matricule))
            {
                return BadRequest("Le matricule du joueur est requis dans l'en-tête X-Matricule.");
            }

            try
            {
                await _participationService.EnregistrerPaiementAsync(matricule, idMatch, montant);
                return Ok(new { message = $"Paiement de {montant} € enregistré avec succès pour le joueur {matricule}." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Une erreur inattendue est survenue lors du paiement.");
            }
        }

        [HttpDelete("matchs/{idMatch}/quitter")]
        public async Task<IActionResult> QuitterMatch(int idMatch, [FromHeader(Name = "X-Matricule")] string matricule)
        {
            if (string.IsNullOrWhiteSpace(matricule))
            {
                return BadRequest("Le matricule du joueur est requis dans l'en-tête X-Matricule.");
            }

            try
            {
                await _participationService.QuitterMatchAsync(matricule, idMatch);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Une erreur inattendue est survenue lors de l'annulation de la participation.");
            }
        }
    }
}