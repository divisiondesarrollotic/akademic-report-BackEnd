using AkademicReport.Dto.ConceptoDto;
using AkademicReport.Dto.FirmaDto;
using AkademicReport.Dto.FirmasDto;

namespace AkademicReport.Service.FirmaServices
{
    public interface IFirmaService
    {
        Task<ServiceResponseData<List<FirmaGetDto>>> GetAll(int idPrograma);
        Task<ServiceResponseData<List<FirmaGetDto>>> GetByIdRecinto(int idRecinto, int idPrograma);
        Task<ServicesResponseMessage<string>> Insert(FirmaDto item);
        Task<ServicesResponseMessage<string>> Update(FirmaDto item);
        Task<ServicesResponseMessage<string>> Delete(int id);
    }
}
