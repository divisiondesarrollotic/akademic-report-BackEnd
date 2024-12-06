using AkademicReport.Dto.CargaDto;
using AkademicReport.Dto.DocentesDto;
using AkademicReport.Dto.PeriodoDto;
using AkademicReport.Dto.ReporteDto;
using AkademicReport.Models;

namespace AkademicReport.Service.CargaServices
{
    public interface ICargaDocenteService
    {
        Task<ServiceResponseCarga<DocenteCargaDto, string>> GetCarga(string cedula, string periodo, int idPrograma, List<DocenteGetDto> DocentesAmilca);
        Task<ServiceResponseCarga<DocenteCargaDto, string>> GetCargaCall(string cedula, string periodo, int idPrograma);
        Task<ServiceResponseCarga<ReportCargaPosgradoDto, string>> GetCargaCallPosgrado(string cedula, string periodo, int idPrograma, List<DocenteGetDto>? DocentesAmilca);
        Task<ServicesResponseMessage<string>> Insert(CargaAddDto item);
        Task<ServicesResponseMessage<string>> InsertCargaPosgrado(CargaPosgradroDto item);
        Task<ServicesResponseMessage<string>> UpdateCargaPosgrado(CargaPosgradroDto item);
        Task<ServicesResponseMessage<string>> Update(CargaUpdateDto item);
        Task<ServicesResponseMessage<string>> Delete(int id);
        Task<ServiceResponseData<List<TipoDeCargaDto>>> GetTipoCarga(int IdPrograma);
        Task<ServiceResponseData<List<MesGetDto>>> GetMeses();
        Task<ServiceResponseData<List<Dia>>> GetDias();
        Task<ServicesResponseMessage<string>> UpdateHorasContratadas(int idCarga);

        Task<bool> ValidateNivelPosgrado(string nivel);
    }
}
