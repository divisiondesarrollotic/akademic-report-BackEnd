using AkademicReport.Dto.CargaDto;
using AkademicReport.Dto.ReporteDto;

namespace AkademicReport.Service.ReposteServices
{
    public interface IReposteService
    {
        Task<ServiceResponseData<DocenteCargaReporteDto>> PorDocente(ReporteDto filtro);
        Task<ServiceResponseData<List<DocenteCargaReporteDto>>> PorRecinto(ReportePorRecintoDto filtro);
    }
}


