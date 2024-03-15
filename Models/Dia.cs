using System;
using System.Collections.Generic;

namespace AkademicReport.Models
{
    public partial class Dia
    {
        public Dia()
        {
            CargaDocentes = new HashSet<CargaDocente>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; } = null!;

        public virtual ICollection<CargaDocente> CargaDocentes { get; set; }
    }
}
