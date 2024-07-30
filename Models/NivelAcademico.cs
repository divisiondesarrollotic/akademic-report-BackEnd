using System;
using System.Collections.Generic;

namespace AkademicReport.Models
{
    public partial class NivelAcademico
    {
        public NivelAcademico()
        {
            Docentereals = new HashSet<Docentereal>();
        }

        public int Id { get; set; }
        public string Nivel { get; set; } = null!;
        public int PagoHora { get; set; }
        public int? IdPrograma { get; set; }

        public virtual ProgramasAcademico? IdProgramaNavigation { get; set; }
        public virtual ICollection<Docentereal> Docentereals { get; set; }
    }
}
