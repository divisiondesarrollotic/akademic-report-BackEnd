using AkademicReport.Models;
using System.ComponentModel.DataAnnotations;

namespace AkademicReport.Dto.AsignaturaDto
{
    public class AsignaturaUpdateDto
    {
        public int Id { get; set; }
       
        public int? id_concepto { get; set; }
        public string? Codigo { get; set; }
        public List<int>? Modalidades { get; set; }
        [Required]
        public List<int> TiposCargas { get; set; }
        public string? Nombre { get; set; }
        public string? Horas { get; set; }
        public string? Descripcion { get; set; }
        [Required]
        public int IdPrograma { get; set; }
        public bool? IsGiaCarga { get; set; }

    }
}
