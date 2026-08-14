using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Padel.SGBD.Api.Model
{
    [Table("Participation")]
    public class Participations
    {
        [Required]
        [MaxLength(10)]
        public string Matricule { get; set; } = string.Empty;
        public int IdMatch { get; set; }
        public bool EstOrganisateur { get; set; }
        public bool APaye { get; set; }
        
        [ForeignKey("Matricule")]
        public Membre? Membre { get; set; }
        
        [ForeignKey("IdMatch")]
        public Match? Match { get; set; }

        public DateTime? DatePaiement { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal MontantPaye { get; set; } = 0m;
    }
}