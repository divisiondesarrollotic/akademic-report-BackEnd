﻿using AkademicReport.Dto.AsignaturaDto;
using AkademicReport.Dto.CargaDto;
using AkademicReport.Dto.ConceptoDto;
using AkademicReport.Dto.DocentesDto;
using AkademicReport.Dto.PeriodoDto;

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
        public AsignaturaGetDto? AsignaturaObj { get; set; }
        public string modalidad { get; set; }
        public int id { get; set; }
        public int seccion { get; set; }
        public string? CodUniversitas { get; set; }
        public DiaGetDto? DiaObj { get; set; }
        public string Horario_dia { get; set; } = null!;
        public string Horario_inicio { get; set; } = null!;
        public string Horario_final { get; set; } = null!;
        public int credito { get; set; }
        public int precio_hora { get; set; }
        public int pago_asignatura { get; set; }
        public string Aula { get; set; }
        public int pago_asignaturaMensual { get; set; }

        public string recinto { get; set; } = null!;
        public int? IdPeriodo { get; set; }
        public bool? HoraContratada { get; set; }
        public PeriodoGetDto? PeriodoObj { get; set; }
        public int? IdTipoReporte { get; set; }
        public int? IdTipoReporteIrregular { get; set; }
        public bool? IsAuth { get; set; }
        public DocenteReporteDto? DataDocente { get; set; }
    }
}
