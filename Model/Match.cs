using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Padel.SGBD.Api.Model
{
    public class Match
    {
        [Key]
        public int IdMatch { get; set; }

        public bool EstPrive { get; set; }

        public int IdTerrain { get; set; }
        public DateTime DateHeure { get; set; }

        [ForeignKey("IdTerrain")]
        public Terrain? Terrain { get; set; }
    }
}