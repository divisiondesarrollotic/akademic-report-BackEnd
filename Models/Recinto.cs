using System;
using System.Collections.Generic;

namespace AkademicReport.Models
{
    public partial class Recinto
    {
        public Recinto()
        {
            Docentereals = new HashSet<Docentereal>();
            Usuarios = new HashSet<Usuario>();
        }

        public int Id { get; set; }
        public string Recinto1 { get; set; } = null!;
        public string NombreCorto { get; set; } = null!;

        public virtual ICollection<Docentereal> Docentereals { get; set; }
        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}
