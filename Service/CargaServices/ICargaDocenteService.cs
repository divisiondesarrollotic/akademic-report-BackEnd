using AkademicReport.Dto.CargaDto;
using AkademicReport.Dto.DocentesDto;
using AkademicReport.Dto.PeriodoDto;

namespace AkademicReport.Service.CargaServices
{
    public interface ICargaDocenteService
    {
        Task<ServiceResponseCarga<DocenteCargaDto, string>> GetCarga(string cedula, string periodo, List<DocenteGetDto> DocentesAmilca);
        Task<ServiceResponseCarga<DocenteCargaDto, string>> GetCargaCall(string cedula, string periodo);
        Task<ServicesResponseMessage<string>> Insert(CargaAddDto item);
        Task<ServicesResponseMessage<string>> Update(CargaUpdateDto item);
        Task<ServicesResponseMessage<string>> Delete(int id);
        Task<ServiceResponseData<List<TipoDeCargaDto>>> GetTipoCarga();
    }
}
