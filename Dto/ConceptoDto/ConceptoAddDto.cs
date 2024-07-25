using System.ComponentModel.DataAnnotations;

namespace AkademicReport.Dto.ConceptoDto
{
    public class ConceptoAddDto
    {
        public string Nombre { get; set; } = null!;
        [Required]
        public int? IdPrograma { get; set; }

    }
}
