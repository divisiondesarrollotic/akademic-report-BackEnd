using AkademicReport.Dto.AsignaturaDto;
using AkademicReport.Dto.ConceptoDto;

namespace AkademicReport.Dto.ReporteDto
{
    public class CargaReporteDto
    {
        public int MontoVinculacion { get; set; }
        public string Vinculacion { get; set; }
        public string Periodo { get; set; } = null!;
        public ConceptoGetDto? Concepto { get; set; }
        public TipoCargaDto TiposCarga { get; set; }
        public string codigo_asignatura { get; set; } = null!;
        public string nombre_asignatura { get; set; } = null!;
        public string modalidad { get; set; }
        public int id { get; set; }
        public int seccion { get; set; }
        public string? CodUniversitas { get; set; }
        public string Horario_dia { get; set; } = null!;
        public string Horario_inicio { get; set; } = null!;
        public string Horario_final { get; set; } = null!;
        public int credito { get; set; }
        public int precio_hora { get; set; }
        public int pago_asignatura { get; set; }
        public string Aula { get; set; }
        public int pago_asignaturaMensual { get 
            { 
               if(Vinculacion=="TC")
                {
                    return 0;
                }
               else
                {
                    return this.pago_asignatura * 4;
                }
                
            } }

        public string recinto { get; set; } = null!;
    }
}
