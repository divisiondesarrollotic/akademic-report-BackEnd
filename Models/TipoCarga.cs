using System;
using System.Collections.Generic;

namespace AkademicReport.Models
{
    public partial class TipoCarga
    {
        public TipoCarga()
        {
            CargaDocentes = new HashSet<CargaDocente>();
            TipoCargaCodigos = new HashSet<TipoCargaCodigo>();
        }

        public int Id { get; set; }
        public string? Nombre { get; set; }
        public int? IdPrograma { get; set; }

        public virtual ProgramasAcademico? IdProgramaNavigation { get; set; }
        public virtual ICollection<CargaDocente> CargaDocentes { get; set; }
        public virtual ICollection<TipoCargaCodigo> TipoCargaCodigos { get; set; }
    }
}
