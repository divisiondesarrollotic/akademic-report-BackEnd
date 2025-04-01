using System.ComponentModel.DataAnnotations;

namespace AkademicReport.Dto.PeriodoDto
{
    public class PeriodoUpdate
    {
        public string? Periodo { get; set; }
        public string? Descripcion { get; set; }
        [Required]
        public int? Anio { get; set; }
        public int? Cuatrimestre { get; set; }



    }
}
