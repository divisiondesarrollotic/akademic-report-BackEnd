using AkademicReport.Dto.CargaDto;
using AkademicReport.Dto.DocentesDto;

namespace AkademicReport.Dto.ReporteDto
{
    public class ReportCargaPosgradoDto
    {
        public int? CantCreditos { get; set; }
        public int MontoVinculacion { get; set; }
        public int MontoSemanal { get; set; }
        public int MontoMensual { get; set; }

        public DocenteReporteDto? Docente { get; set; }
        public List<CargaPosgradoGet>? Cargas { get; set; }
    }
}
