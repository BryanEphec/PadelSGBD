using System;

namespace Padel.SGBD.Api.Dtos
{
    public class ParticipationCreateDto
    {
        public string Matricule { get; set; } = string.Empty; 
        public int IdMatch { get; set; }
        public bool EstOrganisateur { get; set; }
    }
}