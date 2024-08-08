﻿using AkademicReport.Dto.AsignaturaDto;
using AkademicReport.Dto.ConceptoPosgradoDto;

namespace AkademicReport.Dto.CargaDto
{
    public class CargaPosgradoGet
    {
        public int? Id { get; set; }
        public string Periodo { get; set; } = null!;
        public string Recinto { get; set; } = null!;
        public string RecintoNombreCorto { get; set; }
        public int? IdAsignatura { get; set; }
        public string CodAsignatura { get; set; } = null!;
        public string NombreAsignatura { get; set; } = null!;
        public string CodUniversitas { get; set; } = null!;
        public int? Modalidad { get; set; }
        public TipoModalidadDto? TipoModalidad { get; set; }
        public int Dias { get; set; }
        public string? Anio { get; set; }
        public string? DiaNombre { get; set; }
        public string? HoraInicio { get; set; }
        public string? MinutoInicio { get; set; }
        public string? HoraFin { get; set; }
        public string? MinutoFin { get; set; }
        public int Credito { get; set; }
        public string NombreProfesor { get; set; } = null!;
        public string Cedula { get; set; } = null!;
        public int? DiaMes { get; set; }
        public int? IdMes { get; set; }
        public MesGetDto? Mes { get; set; }
        public int? IdPrograma { get; set; }
        public int? IdConceptoPosgrado { get; set; }
        public ConceptoPosDto? ConceptoPosgrado { get; set; }
        public TipoCargaDto? TipoCarga { get; set; }
    }
}
