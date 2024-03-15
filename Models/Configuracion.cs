using System;
using System.Collections.Generic;

namespace AkademicReport.Models
{
    public partial class Configuracion
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Valor { get; set; } = null!;
    }
}
