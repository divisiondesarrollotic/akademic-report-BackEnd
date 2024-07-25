using AkademicReport.Dto.ConceptoDto;
using AkademicReport.Dto.NivelDto;

namespace AkademicReport.Service.ConceptoServices
{
    public interface IConceptoService
    {
        Task<ServiceResponseData<List<ConceptoGetDto>>> GetAll(int idPrograma);
        Task<ServiceResponseData<List<ConceptoGetDto>>> GetById(int id);
        Task<ServicesResponseMessage<string>> Insert(ConceptoAddDto item);
        Task<ServicesResponseMessage<string>> Update(ConceptoGetDto item);
        Task<ServicesResponseMessage<string>> Delete(int id);
    }
}

