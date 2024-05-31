using System.ComponentModel.DataAnnotations;

namespace AkademicReport.Dto.PeriodoDto
{
    public class PeriodoAddDto
    {
        [Required]
        public string Periodo { get; set; } = null!;
        public bool estado { get; set; }
    }
}
