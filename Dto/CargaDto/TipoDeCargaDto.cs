using AkademicReport.Dto.ProgramaDto;

namespace AkademicReport.Dto.CargaDto
{
    public class TipoDeCargaDto
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public int? IdPrograma { get; set; }
        public ProgramaGetDto? Programa { get; set; }

    }
}
