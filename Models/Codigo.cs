using System;
using System.Collections.Generic;

namespace AkademicReport.Models
{
    public partial class Codigo
    {
        public Codigo()
        {
            TipoCargaCodigos = new HashSet<TipoCargaCodigo>();
        }

        public int Id { get; set; }
        public int? IdConcepto { get; set; }
        public string? Codigo1 { get; set; }
        public string? Modalida { get; set; }
        public string? Nombre { get; set; }
        public string? Horas { get; set; }
        public string? Descripcion { get; set; }

        public virtual Concepto? IdConceptoNavigation { get; set; }
        public virtual ICollection<TipoCargaCodigo> TipoCargaCodigos { get; set; }
    }
}
