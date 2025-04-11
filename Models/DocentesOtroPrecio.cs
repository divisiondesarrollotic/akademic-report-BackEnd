using System;
using System.Collections.Generic;

namespace AkademicReport.Models
{
    public partial class DocentesOtroPrecio
    {
        public int Id { get; set; }
        public string? Cedula { get; set; }
        public string? Nombre { get; set; }
        public int? IdNivelAcademico { get; set; }

        public virtual NivelAcademico? IdNivelAcademicoNavigation { get; set; }
    }
}
