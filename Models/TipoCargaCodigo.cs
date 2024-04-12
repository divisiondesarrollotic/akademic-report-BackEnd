using System;
using System.Collections.Generic;

namespace AkademicReport.Models
{
    public partial class TipoCargaCodigo
    {
        public int Id { get; set; }
        public int? IdCodigo { get; set; }
        public int? IdTipoCarga { get; set; }

        public virtual TipoCarga? IdCodigo1 { get; set; }
        public virtual Codigo? IdCodigoNavigation { get; set; }
        public virtual TipoCarga? IdTipoCargaNavigation { get; set; }
    }
}
