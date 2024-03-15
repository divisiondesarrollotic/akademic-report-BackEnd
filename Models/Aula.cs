using System;
using System.Collections.Generic;

namespace AkademicReport.Models
{
    public partial class Aula
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Edificio { get; set; }
        public int? Idrecinto { get; set; }
    }
}
