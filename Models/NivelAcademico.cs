using System;
using System.Collections.Generic;

namespace AkademicReport.Models
{
    public partial class NivelAcademico
    {
        public int Id { get; set; }
        public string Nivel { get; set; } = null!;
        public int PagoHora { get; set; }
    }
}
