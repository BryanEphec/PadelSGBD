using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Padel.SGBD.Api.Model
{
    public class Terrain
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdTerrain { get; set; }
        [JsonPropertyName("nom_terrain")]
        public string Nom_Terrain { get; set; } = String.Empty;
        public int IdSite { get; set; } 
        [JsonPropertyName("type")]
        public String Type { get; set; } = String.Empty;

        [ForeignKey("IdSite")]
        [JsonIgnore]
        public Site? Site { get; set; }
        
    }
}