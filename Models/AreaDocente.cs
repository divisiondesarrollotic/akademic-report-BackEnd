using System;
using System.Collections.Generic;

namespace AkademicReport.Models
{
    public partial class AreaDocente
    {
        public int Id { get; set; }
        public int IdDocente { get; set; }
        public int IdArea { get; set; }

        public virtual Area IdAreaNavigation { get; set; } = null!;
        public virtual Docentereal IdDocenteNavigation { get; set; } = null!;
    }
}
