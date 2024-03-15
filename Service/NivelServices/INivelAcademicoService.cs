using AkademicReport.Dto.NivelDto;
using AkademicReport.Dto.PeriodoDto;

namespace AkademicReport.Service.NivelServices
{
    public interface INivelAcademicoService
    {
        Task<ServiceResponseData<List<NivelGetDto>>> GetAll();
        Task<ServiceResponseData<List<NivelGetDto>>> GetById(int id);
        Task<ServicesResponseMessage<string>> Insert(NivelAddDto item);
        Task<ServicesResponseMessage<string>> Update(NivelUpdateDto item);
        Task<ServicesResponseMessage<string>> Delete(int id);
    }
}
