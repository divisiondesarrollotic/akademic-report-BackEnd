using System;
using System.Collections.Generic;

namespace AkademicReport.Models
{
    public partial class PagoFijo
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public int PagoHora { get; set; }
    }
}
