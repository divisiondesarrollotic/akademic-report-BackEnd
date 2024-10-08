using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace AkademicReport.Dto.ReporteDto
{
    public class ReporteDto
    {
        public string? Cedula { get; set; }
        public string? idRecinto { get; set; }
        public string? Periodo { get; set; }
        [Required]
        public int idPrograma { get; set; }
        public int Anio { get; set; }


    }
}