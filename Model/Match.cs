using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
    }
}