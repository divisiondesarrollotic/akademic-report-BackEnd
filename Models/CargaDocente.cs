using System;
using System.Collections.Generic;

namespace AkademicReport.Models
{
    public partial class CargaDocente
    {
        public int Id { get; set; }
        public int? Curricular { get; set; }
        public string Periodo { get; set; } = null!;
        public string Recinto { get; set; } = null!;
        public string? CodAsignatura { get; set; }
        public string NombreAsignatura { get; set; } = null!;
        public string CodUniversitas { get; set; } = null!;
        public string Seccion { get; set; } = null!;
        public string Aula { get; set; } = null!;
        public int? Modalidad { get; set; }
        public int Dias { get; set; }
        public string? HoraInicio { get; set; }
        public string? MinutoInicio { get; set; }
        public string? HoraFin { get; set; }
        public string? MinutoFin { get; set; }
        public double NumeroHora { get; set; }
        public int Credito { get; set; }
        public string NombreProfesor { get; set; } = null!;
        public string Cedula { get; set; } = null!;
        public int? DiaMes { get; set; }
        public int? IdMes { get; set; }
        public int? IdPrograma { get; set; }
        public int? IdConceptoPosgrado { get; set; }
        public string? Anio { get; set; }

        public virtual TipoCarga? CurricularNavigation { get; set; }
        public virtual Dia DiasNavigation { get; set; } = null!;
        public virtual ConceptoPosgrado? IdConceptoPosgradoNavigation { get; set; }
        public virtual Mese? IdMesNavigation { get; set; }
        public virtual ProgramasAcademico? IdProgramaNavigation { get; set; }
        public virtual TipoModalidad? ModalidadNavigation { get; set; }
    }
}
