using AkademicReport.Dto.ProgramaDto;

namespace AkademicReport.Dto.UsuarioDto
{
    public class UsuarioGetDto
    {
        public int id { get; set; }
        public string? nombre { get; set; }
        public string? correo { get; set; }
        public string? nivel { get; set; }
        public string? nivelNombre { get; set; }
        public string? recinto { get; set; }
        public string? id_recinto { get; set; }
        public string? nombre_corto { get; set; }
        public int? IdPrograma { get; set; }
        public ProgramaGetDto Programa { get; set; }
    }
}
