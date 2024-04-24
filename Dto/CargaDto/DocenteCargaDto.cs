using AkademicReport.Dto.DocentesDto;

namespace AkademicReport.Dto.CargaDto
{
    public class DocenteCargaDto
    {
        public DocenteGetDto? Docente { get; set; }
        public List<CargaGetDto>? Carga { get; set; }
        public int? CantCredito { get; set; }
    }
}
