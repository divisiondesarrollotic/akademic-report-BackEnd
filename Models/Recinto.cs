using System;
using System.Collections.Generic;

namespace AkademicReport.Models
{
    public partial class Recinto
    {
        public Recinto()
        {
            Usuarios = new HashSet<Usuario>();
        }

        public int Id { get; set; }
        public string Recinto1 { get; set; } = null!;
        public string NombreCorto { get; set; } = null!;

        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}
