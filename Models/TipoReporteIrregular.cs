using System;
using System.Collections.Generic;

namespace AkademicReport.Models
{
    public partial class TipoReporteIrregular
    {
        public TipoReporteIrregular()
        {
            CargaDocentes = new HashSet<CargaDocente>();
            NotasCargaIrregulars = new HashSet<NotasCargaIrregular>();
        }

        public int Id { get; set; }
        public string? Nombre { get; set; }

        public virtual ICollection<CargaDocente> CargaDocentes { get; set; }
        public virtual ICollection<NotasCargaIrregular> NotasCargaIrregulars { get; set; }
    }
}
