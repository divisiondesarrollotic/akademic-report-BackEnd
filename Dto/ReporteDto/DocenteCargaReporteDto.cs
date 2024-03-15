using AkademicReport.Dto.CargaDto;
using AkademicReport.Dto.DocentesDto;

namespace AkademicReport.Dto.ReporteDto
{
    public class DocenteCargaReporteDto
    {
        public DocenteReporteDto? Docente { get; set; }
        public List<CargaReporteDto>? Carga { get; set; }
        public int Monto { get; set; }
    }
}
