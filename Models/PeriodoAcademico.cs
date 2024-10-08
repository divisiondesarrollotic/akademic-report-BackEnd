using System;
using System.Collections.Generic;

namespace AkademicReport.Models
{
    public partial class PeriodoAcademico
    {
        public PeriodoAcademico()
        {
            CargaDocentes = new HashSet<CargaDocente>();
        }

        public int Id { get; set; }
        public string? Periodo { get; set; }
        public bool? Estado { get; set; }
        public bool? EstadoPosgrado { get; set; }
        public string? Descripcion { get; set; }
        public int? Anio { get; set; }

        public virtual ICollection<CargaDocente> CargaDocentes { get; set; }
    }
}
