using System;
using System.Collections.Generic;

namespace AkademicReport.Models
{
    public partial class TipoModalidad
    {
        public TipoModalidad()
        {
            CargaDocentes = new HashSet<CargaDocente>();
            TipoModalidadCodigos = new HashSet<TipoModalidadCodigo>();
        }

        public int Id { get; set; }
        public string? Nombre { get; set; }

        public virtual ICollection<CargaDocente> CargaDocentes { get; set; }
        public virtual ICollection<TipoModalidadCodigo> TipoModalidadCodigos { get; set; }
    }
}
