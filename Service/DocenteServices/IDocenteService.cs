using AkademicReport.Dto.CargaDto;
using AkademicReport.Dto.DocentesDto;
using AkademicReport.Dto.NivelDto;
using AkademicReport.Dto.NivelesAcademicoDto;

namespace AkademicReport.Service.DocenteServices
{
    public interface IDocenteService
    {
        Task<ServiceResponseData<List<DocenteGetDto>>> GetAll();
        Task<ServiceResponseData<List<DocenteGetDto>>> GetAllFilter(FiltroDocentesDto filtro);
        Task<ServiceResponseDataPaginacion<List<DocenteGetDto>>> GetAllPaginacion(FiltroDocentesDto filtro);
        Task<ServiceResponseData<List<DocenteGetDto>>> GetAllRecinto(FiltroDocentesDto filtro, int idRecinto);
        Task<ServiceResponseDataDocentes<List<DocenteCantidadDto>>> GetDocentexRecinto();
        Task<ServiceResponseData<List<NacionalidadDto>>> GetNacionalidades(FiltroDto filtro);
        Task<ServiceResponseData<List<NivelAcademicoGetDto>>> UpdatePriceNivelAcademico(NivelAcademicoUpdatePrice Precios);
        Task<ServiceResponseData<List<DocenteOtroPrecioDto>>> AddDocenteOtherPrice(List<DocenteOtroPrecioDto> Docentes);
        Task<ServiceResponseData<List<DocenteOtroPrecioDto>>> RemoveDocenteDeTraslado(List<DocenteOtroPrecioDto> Docentes);


        Task<ServiceResponseData<List<NivelAcademicoGetDto>>> GetNivelAcademico(int idPrograma);
       

    }
}
