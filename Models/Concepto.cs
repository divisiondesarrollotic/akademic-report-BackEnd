using System;
using System.Collections.Generic;

namespace AkademicReport.Models
{
    public partial class Concepto
    {
        public Concepto()
        {
            Codigos = new HashSet<Codigo>();
        }

        public int Id { get; set; }
        public string? Nombre { get; set; }
        public int? IdPrograma { get; set; }

        public virtual ProgramasAcademico? IdProgramaNavigation { get; set; }
        public virtual ICollection<Codigo> Codigos { get; set; }
    }
}
