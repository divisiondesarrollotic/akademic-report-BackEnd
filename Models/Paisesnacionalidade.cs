using System;
using System.Collections.Generic;

namespace AkademicReport.Models
{
    public partial class Paisesnacionalidade
    {
        public int Id { get; set; }
        public string Pais { get; set; } = null!;
        public string Nacionalidad { get; set; } = null!;
    }
}
