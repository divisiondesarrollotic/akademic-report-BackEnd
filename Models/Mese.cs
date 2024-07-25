using System;
using System.Collections.Generic;

namespace AkademicReport.Models
{
    public partial class Mese
    {
        public Mese()
        {
            CargaDocentes = new HashSet<CargaDocente>();
        }

        public int IdMes { get; set; }
        public string Nombre { get; set; } = null!;

        public virtual ICollection<CargaDocente> CargaDocentes { get; set; }
    }
}
