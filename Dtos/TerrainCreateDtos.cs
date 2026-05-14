namespace Padel.SGBD.Api.Dtos
{
    public class TerrainCreateDtos
    {
        public string Nom_Terrain { get; set; } = String.Empty;
        public int IdSite { get; set; }
        public String Type { get; set; } = String.Empty;
    }
}