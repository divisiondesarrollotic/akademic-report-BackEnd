using System.ComponentModel.DataAnnotations;

namespace AkademicReport.Dto.UsuarioDto
{
    public class UsuarioAddDto
    {
       
        public string? Nombre { get; set; }
        [EmailAddress]
        public string? Correo { get; set; }
        public int? nivel { get; set; }
        public int? IdRecinto { get; set; }
     
    }
}
