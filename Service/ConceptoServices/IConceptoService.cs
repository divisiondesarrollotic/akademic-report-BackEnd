using AkademicReport.Dto.ConceptoDto;
using AkademicReport.Dto.ConceptoPosgradoDto;
using AkademicReport.Dto.NivelDto;
using AkademicReport.Models;

namespace AkademicReport.Service.ConceptoServices
{
    public interface IConceptoService
    {
        Task<ServiceResponseData<List<ConceptoGetDto>>> GetAll(int idPrograma);
        Task<ServiceResponseData<List<ConceptoGetDto>>> GetById(int id);
        Task<ServicesResponseMessage<string>> Insert(ConceptoAddDto item);
        Task<ServicesResponseMessage<string>> Update(ConceptoGetDto item);
        Task<ServicesResponseMessage<string>> Delete(int id);



        Task<ServiceResponseData<List<ConceptoPosDto>>> GetAllPos();
        Task<ServiceResponseData<List<ConceptoPosDto>>> GetByIdPos(int id);
        Task<ServicesResponseMessage<string>> InsertPos(ConceptoPosDto item);
        Task<ServicesResponseMessage<string>> UpdatePos(ConceptoPosDto item);
        Task<ServicesResponseMessage<string>> DeletePos(int id);
    }
}

