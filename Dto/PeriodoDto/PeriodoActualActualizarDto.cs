using System.ComponentModel.DataAnnotations;

namespace AkademicReport.Dto.PeriodoDto
{
    public class PeriodoActualActualizarDto
    {
        [Required]
        public string? periodo { get; set; }
    }
}
