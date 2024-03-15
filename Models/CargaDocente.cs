using System;
using System.Collections.Generic;

namespace AkademicReport.Models
{
    public partial class CargaDocente
    {
        public int Id { get; set; }
        public int? Curricular { get; set; }
        public string Periodo { get; set; } = null!;
        public string Recinto { get; set; } = null!;
        public string CodAsignatura { get; set; } = null!;
        public string NombreAsignatura { get; set; } = null!;
        public string CodUniversitas { get; set; } = null!;
        public string Seccion { get; set; } = null!;
        public string Aula { get; set; } = null!;
        public string Modalidad { get; set; } = null!;
        public int Dias { get; set; }
        public int HoraInicio { get; set; }
        public int MinutoInicio { get; set; }
        public int HoraFin { get; set; }
        public int MinutoFin { get; set; }
        public double NumeroHora { get; set; }
        public int Credito { get; set; }
        public string NombreProfesor { get; set; } = null!;
        public string Cedula { get; set; } = null!;

        public virtual Dia DiasNavigation { get; set; } = null!;
    }
}
