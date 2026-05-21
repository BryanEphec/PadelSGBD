using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Padel.SGBD.Api.Model
{
    [Table("Fermeture")]
    public class Fermeture
    {
        [Key]
        public int IdFermeture { get; set; }
        public DateOnly DateFermeture { get; set; }
        public int? IdSite { get; set; }
        [ForeignKey("IdSite")]
        public Site? Site { get; set; }
    }
}