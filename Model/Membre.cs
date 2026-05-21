using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Padel.SGBD.Api.Model
{
    [Table("Membre")]
    public class Membre
    {
        [Key]
        public String Matricule { get; set; } = String.Empty;
        public String Nom { get; set; } = String.Empty;
        public String Prenom { get; set; } = String.Empty;
        public String Type { get; set; } = String.Empty;
        public int? IdSiteRatt { get; set; }
        public bool SousPenalite { get; set; }
        
        [ForeignKey("IdSiteRatt")]
        public Site? Site { get; set; }
    }
}