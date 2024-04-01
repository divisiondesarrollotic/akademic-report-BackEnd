using AkademicReport.Dto.CargaDto;
using AkademicReport.Dto.DocentesDto;
using AkademicReport.Dto.ReporteDto;

namespace AkademicReport.Service.ReposteServices
{
    public interface IReporteService
    {
        Task<ServiceResponseData<DocenteCargaReporteDto>> PorDocente(ReporteDto filtro, List<DocenteGetDto>DocentesAmilca);
        Task<ServiceResponseData<DocenteCargaReporteDto>> PorDocenteCall(ReporteDto filtro);
        Task<ServiceResponseData<List<DocenteCargaReporteDto>>> PorRecinto(ReportePorRecintoDto filtro);
        Task<ServiceResponseReporte<List<ReporteConsolidadoResponseDto>>>ReporteConsolidado(FiltroReporteConsolidado filtro);
    }
}


