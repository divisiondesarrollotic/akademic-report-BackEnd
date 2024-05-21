using System;
using System.Collections.Generic;

namespace AkademicReport.Models
{
    public partial class TipoModalidadCodigo
    {
        public int Id { get; set; }
        public int? IdTipoModalidad { get; set; }
        public int? Idcodigo { get; set; }

        public virtual TipoModalidad? IdTipoModalidadNavigation { get; set; }
        public virtual Codigo? IdcodigoNavigation { get; set; }
    }
}
