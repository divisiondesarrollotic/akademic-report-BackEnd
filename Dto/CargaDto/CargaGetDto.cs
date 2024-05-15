using AkademicReport.Dto.AsignaturaDto;
using AkademicReport.Dto.ConceptoDto;
using System.ComponentModel.DataAnnotations;

namespace AkademicReport.Dto.CargaDto
{
    public class CargaGetDto
    {
        public int Id { get; set; }
        public string Periodo { get; set; } = null!;
        public string Recinto { get; set; } = null!;
        public int? id_asignatura { get; set; } = null!;
        public string cod_asignatura { get; set; } = null!;
        public string nombre_asignatura { get; set; } = null!;
        public string cod_universitas { get; set; } = null!;
        public int Seccion { get; set; }
        public string Aula { get; set; } = null!;
        public string Modalida { get; set; } = null!;
        public int dia_id { get; set; }
        public string dia_nombre { get; set; }=null!;
        public string hora_inicio { get; set; } = null!;
        public string minuto_inicio { get; set; } = null!;
        public string hora_fin { get; set; } = null!;
        public string minuto_fin { get; set; } = null!;
        public double numero_hora { get; set; }
        public int credito { get; set; }
        public string nombre_profesor { get; set; } = null!;
        public string Cedula { get; set; } = null!;
        public int?  id_concepto { get; set; }
        public ConceptoGetDto? Concepto { get; set; }
        [Required]
        public TipoCargaDto TiposCarga { get; set; }




    }
}
