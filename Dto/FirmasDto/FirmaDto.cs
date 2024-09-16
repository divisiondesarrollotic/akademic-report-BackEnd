using System.ComponentModel.DataAnnotations;

namespace AkademicReport.Dto.FirmasDto
{
    public class FirmaDto
    {
        public int? IdFirma { get; set; }
        [Required]
        public int? IdRecinto { get; set; }
        [Required]
        public string? Nombre { get; set; }
        [Required]
        public string? Cargo { get; set; }
        [Required]
        public int? IdPrograma { get; set; }

    }
}
