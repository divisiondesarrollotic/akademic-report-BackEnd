using System;
using System.Collections.Generic;

namespace AkademicReport.Models
{
    public partial class Recinto
    {
        public Recinto()
        {
            Aulas = new HashSet<Aula>();
            Docentereals = new HashSet<Docentereal>();
            Firmas = new HashSet<Firma>();
            Usuarios = new HashSet<Usuario>();
        }

        public int Id { get; set; }
        public string Recinto1 { get; set; } = null!;
        public string NombreCorto { get; set; } = null!;

        public virtual ICollection<Aula> Aulas { get; set; }
        public virtual ICollection<Docentereal> Docentereals { get; set; }
        public virtual ICollection<Firma> Firmas { get; set; }
        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}
