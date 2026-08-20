using Microsoft.AspNetCore.Mvc;
using Padel.SGBD.Api.Dtos;
using Padel.SGBD.Api.Services.Interfaces;

namespace Padel.SGBD.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatchController : ControllerBase
    {
        private readonly IMatchService _matchService;

        public MatchController(IMatchService matchService)
        {
            _matchService = matchService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMatches()
        {
            var matches = await _matchService.GetAllMatchesAsync();
            return Ok(matches);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMatchById(int id)
        {
            var match = await _matchService.GetMatchByIdAsync(id);
            if (match == null)
            {
                return NotFound($"Match avec l'identifiant {id} introuvable.");
            }
            return Ok(match);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMatch([FromHeader(Name = "X-Matricule")] string matriculeOrganisateur, [FromBody] MatchCreateDtos matchDto)
        {
            if (string.IsNullOrWhiteSpace(matriculeOrganisateur))
            {
                return BadRequest("Le matricule de l'organisateur est requis dans l'en-tête X-Matricule.");
            }

            try
            {
                var matchCree = await _matchService.CreerReservationAsync(matriculeOrganisateur, matchDto);
                return CreatedAtAction(nameof(GetMatchById), new { id = matchCree.IdMatch }, matchCree);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERREUR CRÉATION MATCH] : {ex.Message}");
                Console.WriteLine($"[STACKTRACE] : {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"[INNER EXCEPTION] : {ex.InnerException.Message}");
                }

                return StatusCode(500, $"Erreur interne : {ex.Message} -> {ex.InnerException?.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMatch(int id, [FromHeader(Name = "X-Matricule")] string matriculeDemandeur)
        {
            if (string.IsNullOrWhiteSpace(matriculeDemandeur))
            {
                return BadRequest("Le matricule du demandeur est requis dans l'en-tête X-Matricule.");
            }

            try
            {
                await _matchService.AnnulerMatchAsync(id, matriculeDemandeur);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERREUR SUPPRESSION MATCH] : {ex.Message}");
                return StatusCode(500, "Une erreur inattendue est survenue lors de l'annulation du match.");
            }
        }
    }
}