using System.Runtime.CompilerServices;

namespace AkademicReport.Dto.ReporteDto
{
    public class ReporteConsolidadoResponseDto
    {
        public int idRecinto { get; set; }
        public string? nombreRecinto { get; set; }
        public string? periodo { get; set; }
        public string? ano { get; set; }
        public int monto { get; set; }
    }
}
