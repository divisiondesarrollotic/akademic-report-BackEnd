using System;
using System.Collections.Generic;

namespace AkademicReport.Models
{
    public partial class CantSemanasMe
    {
        public int Id { get; set; }
        public int? IdCarga { get; set; }
        public int? Mes { get; set; }
        public int? CantSemanas { get; set; }

        public virtual CargaDocente? IdCargaNavigation { get; set; }
        public virtual Mese? MesNavigation { get; set; }
    }
}
