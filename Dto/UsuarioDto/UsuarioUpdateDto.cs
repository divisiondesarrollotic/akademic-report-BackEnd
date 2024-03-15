using System.ComponentModel.DataAnnotations;

namespace AkademicReport.Dto.UsuarioDto
{
    public class UsuarioUpdateDto
    {
        public string? Id { get; set; }
        public string? Nombre { get; set; }
        [EmailAddress]
        public string? Correo { get; set; }
        public string? Contra { get; set; }
        public string? nivel { get; set; }

    }
}
