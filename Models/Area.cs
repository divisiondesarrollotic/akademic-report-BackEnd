using System;
using System.Collections.Generic;

namespace AkademicReport.Models
{
    public partial class Area
    {
        public Area()
        {
            AreaDocentes = new HashSet<AreaDocente>();
        }

        public int Id { get; set; }
        public string Area1 { get; set; } = null!;

        public virtual ICollection<AreaDocente> AreaDocentes { get; set; }
    }
}
