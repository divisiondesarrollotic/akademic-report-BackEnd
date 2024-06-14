using AkademicReport.Dto.ReporteDto;

namespace AkademicReport.Dto.DocentesDto
{
    public class DocenteCargaReporteDtoPorDocente
    {
        public DocenteReporteDto? Docente { get; set; }
        public List<CargaReporteDto>? Carga { get; set; }

        public int? CantCreditos { get; set; }
        public int MontoVinculacion { get; set; }
        public int MontoSemanal { get; set; }
        public int MontoMensual { get; set; }

    }
}
