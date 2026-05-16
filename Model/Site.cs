using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Padel.SGBD.Api.Model
{
    public class Site
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdSite { get; set; }
        public string Nom { get; set; } = String.Empty;
        public string Ville { get; set; } = String.Empty;

        public TimeOnly HOuverture{ get; set; }
        public TimeOnly HFermeture { get; set; }

        public virtual ICollection<Terrain> Terrains { get; set; } = new List<Terrain>();
           }

}