namespace Padel.SGBD.Api.Dtos
{
    public class MembreCreateDtos
    {
        public String Nom { get; set; } = String.Empty;
        public String Prenom { get; set; } = String.Empty;
        public String Type { get; set; } = String.Empty;
        public int? IdSiteRatt { get; set; }
        public bool SousPenalite { get; set; }
    }
}