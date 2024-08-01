using System;
using System.Collections.Generic;

namespace AkademicReport.Models
{
    public partial class ConceptoPosgrado
    {
        public ConceptoPosgrado()
        {
            CargaDocentes = new HashSet<CargaDocente>();
        }

        public int IdConceptoPosgrado { get; set; }
        public string? Nombre { get; set; }

        public virtual ICollection<CargaDocente> CargaDocentes { get; set; }
    }
}
