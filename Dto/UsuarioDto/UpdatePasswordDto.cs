namespace AkademicReport.Dto.UsuarioDto
{
    public class UpdatePasswordDto
    {
        public int IdUusario { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
