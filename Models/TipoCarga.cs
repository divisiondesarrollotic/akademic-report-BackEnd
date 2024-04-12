using System;
using System.Collections.Generic;

namespace AkademicReport.Models
{
    public partial class TipoCarga
    {
        public TipoCarga()
        {
            CargaDocentes = new HashSet<CargaDocente>();
            TipoCargaCodigoIdCodigo1s = new HashSet<TipoCargaCodigo>();
            TipoCargaCodigoIdTipoCargaNavigations = new HashSet<TipoCargaCodigo>();
        }

        public int Id { get; set; }
        public string? Nombre { get; set; }

        public virtual ICollection<CargaDocente> CargaDocentes { get; set; }
        public virtual ICollection<TipoCargaCodigo> TipoCargaCodigoIdCodigo1s { get; set; }
        public virtual ICollection<TipoCargaCodigo> TipoCargaCodigoIdTipoCargaNavigations { get; set; }
    }
}
