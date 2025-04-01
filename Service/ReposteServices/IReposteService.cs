using AkademicReport.Dto.CargaDto;
using AkademicReport.Dto.DocentesDto;
using AkademicReport.Dto.ReporteDto;

namespace AkademicReport.Service.ReposteServices
{
    public interface IReporteService
    {
        Task<ServiceResponseData<DocenteCargaReporteDto>> PorDocente(ReporteDto filtro, List<DocenteGetDto>DocentesAmilca);
        Task<ServiceResponseData<DocenteCargaReporteDto>> PorDocenteCall(ReporteDto filtro);
        Task<ServiceResponseReporte<List<DocenteCargaReporteDto>>> PorRecinto(ReportePorRecintoDto filtro);
        Task<ServiceResponseReporte<List<DocenteCargaReporteDto>>> ReporteDiplomado(ReportePorRecintoDto filtro);
        Task<ServiceResponseReporte<List<ReporteConsolidadoResponseDto>>>ReporteConsolidado(FiltroReporteConsolidado filtro);
        // Reporte para la carga de grado cuando sea irregular
        Task<ServiceResponseReporte<List<CargaIrregularReporteGetDto>>> GetCargaIrregularReport(ReportePorRecintoDto filtro);

        // Repostes de posgrado'
        //Reporte por el id concepto de la carga
        Task<ServiceResponseReporte<List<ReportCargaPosgradoDto>>> ReporteByIdProgramCargaPosgrado(int idConcepto, string periodo, int idRecinto);
        Task<ServiceResponseReporte<List<ReportCargaPosgradoDto>>> ReporteByPeriodoPosgrado(string periodo, int idRecinto);
        Task<ServiceResponseReporte<List<ReportCargaPosgradoDto>>> ReporteByIdRecintoPosgrado(int idRecinto, string periodo);
        Task<ServiceResponseReporte<List<ReportCargaPosgradoDto>>> ReporteByDocentePosgrado(string cedula, string periodo, int idRecinto);

        // 
   

    }
}


