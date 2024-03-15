using System;
using System.Collections.Generic;

namespace AkademicReport.Models
{
    public partial class Vinculo
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Corto { get; set; } = null!;
        public int Monto { get; set; }
    }
}
