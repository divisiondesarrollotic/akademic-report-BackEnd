using System;
using System.Collections.Generic;

namespace AkademicReport.Models
{
    public partial class NivelUsuario
    {
        public NivelUsuario()
        {
            Usuarios = new HashSet<Usuario>();
        }

        public int Id { get; set; }
        public string Nivel { get; set; } = null!;
        public string Abreviado { get; set; } = null!;

        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}
