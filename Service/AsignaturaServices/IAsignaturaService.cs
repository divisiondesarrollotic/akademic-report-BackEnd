using AkademicReport.Dto.AsignaturaDto;
using AkademicReport.Dto.DocentesDto;
using AkademicReport.Dto.NivelDto;

namespace AkademicReport.Service.AsignaturaServices
{
    public interface IAsignaturaService
    {
        Task<ServiceResponseData<List<AsignaturaGetDto>>> GetAll();
        Task<ServiceResponseData<List<AsignaturaGetDto>>> GetAllFilter( string filtro );
        Task<ServiceResponseDataPaginacion<List<AsignaturaGetDto>>> GetAllPaginacion(FiltroDocentesDto filtro);
        Task<ServiceResponseData<List<AsignaturaGetDto>>> GetAllByIdConcepto(int idConcepto);
        Task<ServiceResponseData<List<AsignaturaGetDto>>> GetById(int id);
        Task<ServicesResponseMessage<string>> Insert(AsignaturaAddDto item);
        Task<ServicesResponseMessage<string>> Update(AsignaturaUpdateDto item);
        Task<ServicesResponseMessage<string>> Delete(int id);
        Task<ServiceResponseData<List<TipoModalidadDto>>> GetAllTipoModalid();
    }
}
