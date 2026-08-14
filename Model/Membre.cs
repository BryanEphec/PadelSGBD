using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Padel.SGBD.Api.Model
{
    [Table("Membre")]
    public class Membre
    {
        [Key]
        [MaxLength(10)]
        public string Matricule { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Nom { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Prenom { get; set; } = string.Empty;

        /// <summary>
        /// Type de membre : 'G' (Global), 'S' (Site), 'L' (Libre)
        /// </summary>
        [Required]
        [MaxLength(1)]
        public string Type { get; set; } = string.Empty;

        public int? IdSiteRatt { get; set; }

        [ForeignKey("IdSiteRatt")]
        public Site? SiteRattachement { get; set; }

        
        public DateTime? DateFinPenalite { get; set; }

        
        [Column(TypeName = "decimal(10,2)")]
        public decimal SoldeDu { get; set; } = 0m;

        [JsonIgnore]
        public virtual ICollection<Participations> Participations { get; set; } = new List<Participations>();
    }
}