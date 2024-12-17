using System;
using System.Collections.Generic;

namespace AkademicReport.Models
{
    public partial class LogTransacional
    {
        public int Id { get; set; }
        public int? IdUsuario { get; set; }
        public string? Accion { get; set; }
        public DateTime? Fecha { get; set; }
        public int? IdCarga { get; set; }
        public string? Cedula { get; set; }

        public virtual CargaDocente? IdCargaNavigation { get; set; }
        public virtual Usuario? IdUsuarioNavigation { get; set; }
    }
}
