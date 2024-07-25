using System.ComponentModel.DataAnnotations;

namespace AkademicReport.Dto.UsuarioDto
{
    public class UsuarioUpdateDto
    {
        public int? Id { get; set; }
        public string? Nombre { get; set; }
        [EmailAddress]
        public string? Correo { get; set; }
        [Required]
        public int? nivel { get; set; }
        public int? IdRecinto { get; set; }
        [Required]
        public int? IdPrograma { get; set; }
    }
}
