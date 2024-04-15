using AkademicReport.Dto.AsignaturaDto;
using System.ComponentModel.DataAnnotations;

namespace AkademicReport.Dto.CargaDto
{
    public class CargaUpdateDto
    {
        public int Id { get; set; }
        public string Periodo { get; set; } = null!;
        public int Recinto { get; set; }
        public string cod_asignatura { get; set; } = null!;
        public string nombre_asignatura { get; set; } = null!;
        public string cod_universitas { get; set; } = null!;
        public int Seccion { get; set; }
        public string Aula { get; set; } = null!;
        public string Modalidad { get; set; } = null!;
        public int dia_id { get; set; }
        public string? hora_inicio { get; set; }
        public string? minuto_inicio { get; set; }
        public string? hora_fin { get; set; }
        public string? minuto_fin { get; set; }
        public double numero_hora { get; set; }
        public int credito { get; set; }
        public string nombre_profesor { get; set; } = null!;
        public string Cedula { get; set; } = null!;
        [Required]
        public TipoCargaDto TiposCargas { get; set; }
    }
}
