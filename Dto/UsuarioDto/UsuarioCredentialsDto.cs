using System.ComponentModel.DataAnnotations;

namespace AkademicReport.Dto.UsuarioDto
{
    public class UsuarioCredentialsDto
    {
        [Required]
        public string? correo { get; set; }
        [Required]
        public string? contra { get; set; }
    }
}
