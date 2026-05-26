using System;
using System.ComponentModel.DataAnnotations;

namespace Padel.SGBD.Api.Dtos
{
    public class MatchCreateDtos
    {
        [Required(ErrorMessage = "L'ID du terrain est obligatoire.")]
        [Range(1, int.MaxValue, ErrorMessage = "Veuillez sélectionner un terrain valide.")]
        public int IdTerrain { get; set; }

        [Required(ErrorMessage = "La date et l'heure du match sont obligatoires.")]
        public DateTime DateHeure { get; set; }

        public bool EstPrive { get; set; }
    }
}