using System.ComponentModel.DataAnnotations;

namespace AkademicReport.Dto.UsuarioDto
{
    public class UsuarioUpdateDto
    {
        public int? Id { get; set; }
        public string? Nombre { get; set; }
        [EmailAddress]
        public string? Correo { get; set; }
        public int? nivel { get; set; }
        public int? IdRecinto { get; set; }

    }
}
