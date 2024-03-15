using System;
using System.Collections.Generic;

namespace AkademicReport.Models
{
    public partial class Asignatura
    {
        public int Id { get; set; }
        public string CodPem { get; set; } = null!;
        public string Titulo { get; set; } = null!;
        public int Creditos { get; set; }
        public int CodUni { get; set; }
        public string Horario { get; set; } = null!;
        public string Seccion { get; set; } = null!;
    }
}
