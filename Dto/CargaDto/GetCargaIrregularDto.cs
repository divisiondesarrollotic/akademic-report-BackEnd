using AkademicReport.Dto.AsignaturaDto;
using AkademicReport.Dto.AulaDto;
using AkademicReport.Dto.ConceptoDto;
using AkademicReport.Dto.RecintoDto;
using AkademicReport.Dto.ReporteDto;
using AkademicReport.Models;

namespace AkademicReport.Dto.CargaDto
{
    public class GetCargaIrregularDto   
    {
        public string Recinto { get; set; } = null!;
        public TipoCargaDto? TipoCarga { get; set; }
        public int Seccion { get; set; }
        public string Aula { get; set; }
        public string Horario_dia { get; set; } = null!;
        public string Horario_inicio { get; set; } = null!;
        public string Horario_final { get; set; } = null!;
        public int? Credito { get; set; }
        public int? Precio_hora { get; set; }
        public string codigo_asignatura { get; set; } = null!;
        public string nombre_asignatura { get; set; } = null!;
        public string modalidad { get; set; }
        public ConceptoGetDto? Concepto { get; set; }
        public TipoCargaDto TiposCarga { get; set; }
        public List<CantSemanaMesDto> MontosMesObj { get; set; }
        public bool? IsAuth { get; set; }




    }
}
