using System;
using System.Collections.Generic;

namespace AkademicReport.Models
{
    public partial class CargaDocente
    {
        public CargaDocente()
        {
            CantSemanasMes = new HashSet<CantSemanasMe>();
            LogTransacionals = new HashSet<LogTransacional>();
            NotasCargaIrregulars = new HashSet<NotasCargaIrregular>();
        }

        public int Id { get; set; }
        public int? Curricular { get; set; }
        public string Periodo { get; set; } = null!;
        public int Recinto { get; set; }
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
        public int? IdCodigo { get; set; }
        public int? IdPeriodo { get; set; }
        public bool? HoraContratada { get; set; }
        public bool? Deleted { get; set; }
        public int? CantSemanas { get; set; }
        public int? IdTipoReporte { get; set; }
        public int? IdTipoReporteIrregular { get; set; }
        public int? IdCargaRegular { get; set; }
        public bool? IsAuth { get; set; }

        public virtual TipoCarga? CurricularNavigation { get; set; }
        public virtual Dia DiasNavigation { get; set; } = null!;
        public virtual Codigo? IdCodigoNavigation { get; set; }
        public virtual ConceptoPosgrado? IdConceptoPosgradoNavigation { get; set; }
        public virtual Mese? IdMesNavigation { get; set; }
        public virtual PeriodoAcademico? IdPeriodoNavigation { get; set; }
        public virtual ProgramasAcademico? IdProgramaNavigation { get; set; }
        public virtual TipoReporteIrregular? IdTipoReporteIrregularNavigation { get; set; }
        public virtual TipoReporte? IdTipoReporteNavigation { get; set; }
        public virtual TipoModalidad? ModalidadNavigation { get; set; }
        public virtual Recinto RecintoNavigation { get; set; } = null!;
        public virtual ICollection<CantSemanasMe> CantSemanasMes { get; set; }
        public virtual ICollection<LogTransacional> LogTransacionals { get; set; }
        public virtual ICollection<NotasCargaIrregular> NotasCargaIrregulars { get; set; }
    }
}
