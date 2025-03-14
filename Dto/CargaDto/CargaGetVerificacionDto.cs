using AkademicReport.Dto.DocentesDto;
using AkademicReport.Dto.ReporteDto;

namespace AkademicReport.Dto.CargaDto
{
    public class CargaGetVerificacionDto
    {
        public DocenteGetDto? Docente { get; set; }
        public List<CargaGetDto>? CargaUniversitasAkadeimc { get; set; }
        public List<CargaGetDto>? CargaUniversitas { get; set; }


    }
}
