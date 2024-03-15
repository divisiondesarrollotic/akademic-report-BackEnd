using System.ComponentModel.DataAnnotations;

namespace AkademicReport.Dto.CargaDto
{
    public class DtoCarga
    {
        [Required]
        public string? Cedula { get; set; }
        [Required]
        public string? Periodo { get; set; }
    }
}
