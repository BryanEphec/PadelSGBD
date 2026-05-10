using System.ComponentModel.DataAnnotations.Schema;

namespace Padel.SGBD.Api.Model
{
    [Table("Participation")]
    public class Participations
    {
        public String Matricule { get; set; } = String.Empty;
        public int IdMatch { get; set; }
        public bool EstOrganisateur { get; set; }
        public bool APaye { get; set; }
        [ForeignKey("Matricule")]
        public Membre? Membre { get; set; }
        [ForeignKey("IdMatch")]
        public Match? Match { get; set; }
    }
}