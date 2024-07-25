using System;
using System.Collections.Generic;

namespace AkademicReport.Models
{
    public partial class ProgramasAcademico
    {
        public ProgramasAcademico()
        {
            CargaDocentes = new HashSet<CargaDocente>();
            Conceptos = new HashSet<Concepto>();
            Usuarios = new HashSet<Usuario>();
        }

        public int IdPrograma { get; set; }
        public string? Nombre { get; set; }

        public virtual ICollection<CargaDocente> CargaDocentes { get; set; }
        public virtual ICollection<Concepto> Conceptos { get; set; }
        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}
