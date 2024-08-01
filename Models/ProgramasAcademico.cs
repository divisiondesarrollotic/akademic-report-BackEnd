using System;
using System.Collections.Generic;

namespace AkademicReport.Models
{
    public partial class ProgramasAcademico
    {
        public ProgramasAcademico()
        {
            CargaDocentes = new HashSet<CargaDocente>();
            Codigos = new HashSet<Codigo>();
            Conceptos = new HashSet<Concepto>();
            NivelAcademicos = new HashSet<NivelAcademico>();
            TipoCargas = new HashSet<TipoCarga>();
            Usuarios = new HashSet<Usuario>();
        }

        public int IdPrograma { get; set; }
        public string? Nombre { get; set; }

        public virtual ICollection<CargaDocente> CargaDocentes { get; set; }
        public virtual ICollection<Codigo> Codigos { get; set; }
        public virtual ICollection<Concepto> Conceptos { get; set; }
        public virtual ICollection<NivelAcademico> NivelAcademicos { get; set; }
        public virtual ICollection<TipoCarga> TipoCargas { get; set; }
        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}
