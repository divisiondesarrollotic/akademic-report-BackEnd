namespace AkademicReport.Dto.UsuarioDto
{
    public class UpdatePasswordDto
    {
        public int IdUsuario { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
