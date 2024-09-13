using AkademicReport.Dto.RecintoDto;

namespace AkademicReport.Dto.FirmaDto
{
    public class FirmaGetDto
    {
        public int IdFirma { get; set; }
        public int? IdRecinto { get; set; }
        public virtual RecintoGetDto? RecintoObj { get; set; }
        public string? Nombre { get; set; }
        public string? Cargo { get; set; }
    }
}
