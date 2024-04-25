using System;
using System.Collections.Generic;

namespace AkademicReport.Models
{
    public partial class Docentereal
    {
        public Docentereal()
        {
            AreaDocentes = new HashSet<AreaDocente>();
        }

        public int Id { get; set; }
        public string? TiempoDedicacion { get; set; }
        public string? Identificacion { get; set; }
        public string? Nombre { get; set; }
        public string? Nacionalidad { get; set; }
        public string? Sexo { get; set; }
        public int? IdVinculo { get; set; }
        public int? IdRecinto { get; set; }
        public int? IdNivelAcademico { get; set; }
        public string? TipoIdentificacion { get; set; }

        public virtual NivelAcademico? IdNivelAcademicoNavigation { get; set; }
        public virtual Recinto? IdRecintoNavigation { get; set; }
        public virtual Vinculo? IdVinculoNavigation { get; set; }
        public virtual ICollection<AreaDocente> AreaDocentes { get; set; }
    }
}
