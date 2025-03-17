using AkademicReport.Dto.DocentesDto;

namespace AkademicReport.Dto.CargaDto
{
    public class DocenteCargaDto
    {
        public DocenteGetDto? Docente { get; set; }
        public NotaCargaIrregularDto? NotaCargaIrregular { get; set; }
        public List<CargaGetDto>? Carga { get; set; }
        public int? CantCredito { get; set; }
        public int?  Anio { get; set; }
    }
}
