using AkademicReport.Dto.CargaDto;
using AkademicReport.Dto.DocentesDto;

namespace AkademicReport.Dto.ReporteDto
{
    public class CargaIrregularReporteGetDto
    {
        public DocenteReporteDto? Docente { get; set; }
        public NotaCargaIrregularDto? NotaCargaIrregular { get; set; }

        public List<CantSemanaMesDto> MontosTotales { get; set; }
        public List<GetCargaIrregularDto> Carga { get; set; } = new List<GetCargaIrregularDto>();




    }
}
