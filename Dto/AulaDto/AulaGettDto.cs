using AkademicReport.Dto.RecintoDto;

namespace AkademicReport.Dto.AulaDto
{
    public class AulaGettDto
    {
        public string? Id { get; set; }
        public string? Nombre { get; set; }
        public string? Edificio { get; set; }
        public string? idRecinto { get; set; }
        public RecintoGetDto? Recinto { get; set; }



    }
}
