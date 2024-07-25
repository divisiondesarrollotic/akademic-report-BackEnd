using AkademicReport.Dto.ProgramaDto;

namespace AkademicReport.Dto.ConceptoDto
{
    public class ConceptoGetDto
    {
        public int? Id { get; set; }
        public string Nombre { get; set; } = null!;
        public ProgramaGetDto? Programa { get; set; }
    }
}
