using System;
using System.Collections.Generic;

namespace AkademicReport.Models
{
    public partial class Firma
    {
        public int IdFirma { get; set; }
        public int? IdRecinto { get; set; }
        public string? Nombre { get; set; }
        public string? Cargo { get; set; }
        public int? IdPrograma { get; set; }

        public virtual ProgramasAcademico? IdProgramaNavigation { get; set; }
        public virtual Recinto? IdRecintoNavigation { get; set; }
    }
}
