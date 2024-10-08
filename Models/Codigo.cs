using System;
using System.Collections.Generic;

namespace AkademicReport.Models
{
    public partial class Codigo
    {
        public Codigo()
        {
            CargaDocentes = new HashSet<CargaDocente>();
            TipoCargaCodigos = new HashSet<TipoCargaCodigo>();
            TipoModalidadCodigos = new HashSet<TipoModalidadCodigo>();
        }

        public int Id { get; set; }
        public int? IdConcepto { get; set; }
        public string? Codigo1 { get; set; }
        public string? Modalida { get; set; }
        public string? Nombre { get; set; }
        public string? Horas { get; set; }
        public string? Descripcion { get; set; }
        public int? IdPrograma { get; set; }

        public virtual Concepto? IdConceptoNavigation { get; set; }
        public virtual ProgramasAcademico? IdProgramaNavigation { get; set; }
        public virtual ICollection<CargaDocente> CargaDocentes { get; set; }
        public virtual ICollection<TipoCargaCodigo> TipoCargaCodigos { get; set; }
        public virtual ICollection<TipoModalidadCodigo> TipoModalidadCodigos { get; set; }
    }
}
