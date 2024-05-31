using System;
using System.Collections.Generic;

namespace AkademicReport.Models
{
    public partial class PeriodoAcademico
    {
        public int Id { get; set; }
        public string Periodo { get; set; } = null!;
        public bool? Estado { get; set; }
    }
}
