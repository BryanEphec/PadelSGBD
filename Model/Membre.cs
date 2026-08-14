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

        [Required]
        [MaxLength(1)]
        public string Type { get; set; } = string.Empty;

        public int? IdSiteRatt { get; set; }

        [ForeignKey("IdSiteRatt")]
        public Site? Site { get; set; }

        public DateTime? DateFinPenalite { get; set; }

       [NotMapped]
        public bool SousPenalite
        {
            get => DateFinPenalite.HasValue && DateFinPenalite.Value > DateTime.Now;
            set
        {
            if (value)
            {
            DateFinPenalite = DateTime.Now.AddDays(7);
            }
            else
            {
            DateFinPenalite = null;
        }
    }
}

        [Column(TypeName = "decimal(10,2)")]
        public decimal SoldeDu { get; set; } = 0m;

        [JsonIgnore]
        public virtual ICollection<Participations> Participations { get; set; } = new List<Participations>();
    }
}