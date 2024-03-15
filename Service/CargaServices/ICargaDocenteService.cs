using AkademicReport.Dto.CargaDto;
using AkademicReport.Dto.PeriodoDto;

namespace AkademicReport.Service.CargaServices
{
    public interface ICargaDocenteService
    {
        Task<ServiceResponseCarga<DocenteCargaDto, string>> GetCarga(string cedula, string periodo);
        Task<ServicesResponseMessage<string>> Insert(CargaAddDto item);
        Task<ServicesResponseMessage<string>> Update(CargaUpdateDto item);
        Task<ServicesResponseMessage<string>> Delete(int id);
    }
}
