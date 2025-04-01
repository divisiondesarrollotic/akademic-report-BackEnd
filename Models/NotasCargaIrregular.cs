using System;
using System.Collections.Generic;

namespace AkademicReport.Models
{
    public partial class NotasCargaIrregular
    {
        public int Id { get; set; }
        public int? IdPeriodo { get; set; }
        public string? Nota { get; set; }
        public string? Cedula { get; set; }
        public int? IdCarga { get; set; }
        public int? IdTipoReporte { get; set; }
        public int? IdTipoReporteIrregular { get; set; }

        public virtual CargaDocente? IdCargaNavigation { get; set; }
        public virtual PeriodoAcademico? IdPeriodoNavigation { get; set; }
        public virtual TipoReporteIrregular? IdTipoReporteIrregularNavigation { get; set; }
        public virtual TipoReporte? IdTipoReporteNavigation { get; set; }
    }
}
