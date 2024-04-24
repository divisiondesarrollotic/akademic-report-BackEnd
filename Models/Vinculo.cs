using System;
using System.Collections.Generic;

namespace AkademicReport.Models
{
    public partial class Vinculo
    {
        public Vinculo()
        {
            Docentereals = new HashSet<Docentereal>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Corto { get; set; } = null!;
        public int Monto { get; set; }

        public virtual ICollection<Docentereal> Docentereals { get; set; }
    }
}
