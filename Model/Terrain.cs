using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Padel.SGBD.Api.Model
{
    public class Terrain
    {
        [Key]
        public int IdTerrain { get; set; }
        public string Nom_Terrain { get; set; } = String.Empty;
        public int IdSite { get; set; } 

        [ForeignKey("IdSite")]
        public Site? Site { get; set; }
        
    }
}