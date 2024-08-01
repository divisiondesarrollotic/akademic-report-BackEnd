using AkademicReport.Models;
using System.ComponentModel.DataAnnotations;

namespace AkademicReport.Dto.AsignaturaDto
{
    public class AsignaturaAddDto
    {

       
        public int? id_concepto { get; set; }
        public string? Codigo { get; set; }
        public List<TipoModalidadDto>? Modalidades { get; set; }
        public List<TipoCargaDto>? TiposCargas { get; set; }
        public string? Nombre { get; set; }
        public string? Horas { get; set; }
        public string? Descripcion { get; set; }
        [Required]
        public int IdPrograma { get; set; }
    }
}
