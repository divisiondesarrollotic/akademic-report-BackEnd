using AkademicReport.Dto.AulaDto;
using AkademicReport.Dto.NivelDto;
using AkademicReport.Dto.PeriodoDto;
using AkademicReport.Models;

namespace AkademicReport.Service.AulaServices
{
    public interface IAulaService
    {
        Task<ServiceResponseData<List<AulaGettDto>>> GetAllByIdRecinto(int id);

        Task<ServiceResponseData<AulaGettDto>> GetById(int id);
        Task<ServicesResponseMessage<string>> Insert(AulaDto item);
        Task<ServicesResponseMessage<string>> Update(AulaDto item);
        Task<ServicesResponseMessage<string>> Delete(int id);
    }
}
