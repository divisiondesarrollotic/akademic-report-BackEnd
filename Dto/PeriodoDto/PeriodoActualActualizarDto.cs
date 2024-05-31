using System.ComponentModel.DataAnnotations;

namespace AkademicReport.Dto.PeriodoDto
{
    public class PeriodoActualActualizarDto
    {
        [Required]
        public PeriodoGetDto? Periodo { get; set; }
    
    }
}
