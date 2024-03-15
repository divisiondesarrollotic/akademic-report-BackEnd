using AkademicReport.Dto.AulaDto;
using AkademicReport.Dto.NivelDto;
using AkademicReport.Models;

namespace AkademicReport.Service.AulaServices
{
    public interface IAulaService
    {
        Task<ServiceResponseData<List<AulaGettDto>>> GetAllByIdRecinto(int id);
    }
}
