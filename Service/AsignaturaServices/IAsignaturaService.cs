using AkademicReport.Dto.AsignaturaDto;
using AkademicReport.Dto.NivelDto;

namespace AkademicReport.Service.AsignaturaServices
{
    public interface IAsignaturaService
    {
        Task<ServiceResponseData<List<AsignaturaGetDto>>> GetAll();
        Task<ServiceResponseDataPaginacion<List<AsignaturaGetDto>>> GetAllPaginacion(AsignaturaPaginacionDto paginacion);
        Task<ServiceResponseData<List<AsignaturaGetDto>>> GetAllByIdConcepto(int idConcepto);
        Task<ServiceResponseData<List<AsignaturaGetDto>>> GetById(int id);
        Task<ServicesResponseMessage<string>> Insert(AsignaturaAddDto item);
        Task<ServicesResponseMessage<string>> Update(AsignaturaUpdateDto item);
        Task<ServicesResponseMessage<string>> Delete(int id);
    }
}
