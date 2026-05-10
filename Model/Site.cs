using System.ComponentModel.DataAnnotations;

namespace Padel.SGBD.Api.Model
{
    public class Site
    {
        [Key]
        public int IdSite { get; set; }
        public string Nom { get; set; } = String.Empty;
        public string Ville { get; set; } = String.Empty;

        public TimeOnly HOuverture{ get; set; }
        public TimeOnly HFermeture { get; set; }
           }
}