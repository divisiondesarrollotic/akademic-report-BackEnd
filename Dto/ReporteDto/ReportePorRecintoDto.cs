namespace AkademicReport.Dto.ReporteDto
{
    public class ReportePorRecintoDto
    {
      
        public int idRecinto { get; set; }
        public string? Periodo { get; set; }
        public string? Curricular { get; set; }
        public string? TipoDocente { get; set; } = null;
        public int? IdTipoReporte { get; set; } = 0;
        public int? IdTipoReporteIrregular { get; set; } = 0;

    }
}
