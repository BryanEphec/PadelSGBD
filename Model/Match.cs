using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Padel.SGBD.Api.Model
{
    [Table("Match")]
    public class Match
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdMatch { get; set; }

        public bool EstPrive { get; set; }

        public int IdTerrain { get; set; }
        public DateTime DateHeure { get; set; }

        [ForeignKey("IdTerrain")]
        public Terrain? Terrain { get; set; }

        /// <summary>
        /// Cout total fixe de 60€
        /// </summary>
        [Column(TypeName = "decimal(10,2)")]
        public  decimal TarifTotal { get; set; } = 60.00m;

        public virtual ICollection<Participations> Participations { get; set; } = new List<Participations>();
    }
}