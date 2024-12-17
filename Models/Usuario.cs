using System;
using System.Collections.Generic;

namespace AkademicReport.Models
{
    public partial class Usuario
    {
        public Usuario()
        {
            LogTransacionals = new HashSet<LogTransacional>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Correo { get; set; } = null!;
        public string Contra { get; set; } = null!;
        public int Nivel { get; set; }
        public int IdRecinto { get; set; }
        public int SoftDelete { get; set; }
        public int? IdPrograma { get; set; }

        public virtual ProgramasAcademico? IdProgramaNavigation { get; set; }
        public virtual Recinto IdRecintoNavigation { get; set; } = null!;
        public virtual NivelUsuario NivelNavigation { get; set; } = null!;
        public virtual ICollection<LogTransacional> LogTransacionals { get; set; }
    }
}
