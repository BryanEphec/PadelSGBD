using System.ComponentModel.DataAnnotations;

namespace Padel.SGBD.Api.Dtos
{
    public class ParticipationCreateDtos
    {
        [Required(ErrorMessage = "Le matricule est requis.")]
        [StringLength(7, MinimumLength = 3, ErrorMessage = "Le matricule doit comporter entre 3 et 7 caractères.")]
        public string Matricule { get; set; } = string.Empty; 
        [Range(1, int.MaxValue, ErrorMessage = "L'ID du match doit être un identifiant valide.")]
        public int IdMatch { get; set; }
        public bool EstOrganisateur { get; set; }
    }
}