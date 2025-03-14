using System.ComponentModel.DataAnnotations;

namespace AkademicReport.Dto.CargaDto
{
    public class DtoCarga
    {
        [Required]
        public string? Cedula { get; set; }
        [Required]
        public string? Periodo { get; set; }
        [Required]
        public int idPrograma { get; set; }
        public int? IdTipoReporte { get; set; } = 0;
        public int? IdTipoReporteI { get; set; } = 0;
    }
}
