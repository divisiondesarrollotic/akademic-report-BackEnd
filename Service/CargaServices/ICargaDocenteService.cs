using AkademicReport.Dto.CargaDto;
using AkademicReport.Dto.DocentesDto;
using AkademicReport.Dto.PeriodoDto;
using AkademicReport.Dto.ReporteDto;
using AkademicReport.Dto.TiposReporteDto;
using AkademicReport.Models;

namespace AkademicReport.Service.CargaServices
{
    public interface ICargaDocenteService
    {
        Task<ServiceResponseCarga<DocenteCargaDto, string>> GetCarga(string cedula, string periodo, int idPrograma, List<DocenteGetDto> DocentesAmilca);
        Task<ServiceResponseCarga<DocenteCargaDto, string>> GetCargaCall(string cedula, string periodo, int idPrograma, int idTipoReporte, int idTipoReporteI);
        Task<ServiceResponseCarga<ReportCargaPosgradoDto, string>> GetCargaCallPosgrado(string cedula, string periodo, int idPrograma, List<DocenteGetDto>? DocentesAmilca, int idRecinto);
        Task<ServicesResponseMessage<string>> Insert(CargaAddDto item);
        Task<ServicesResponseMessage<string>> InsertCargaPosgrado(CargaPosgradroDto item);
        Task<ServicesResponseMessage<string>> UpdateCargaPosgrado(CargaPosgradroDto item);
        Task<ServicesResponseMessage<string>> Update(CargaUpdateDto item);
        Task<ServicesResponseMessage<string>> Delete(int id, int idUsuario);
        Task<ServiceResponseData<List<TipoDeCargaDto>>> GetTipoCarga(int IdPrograma);
        Task<ServiceResponseData<List<MesGetDto>>> GetMesesByCuatrimestre(int cuatrimestre);
        Task<ServiceResponseData<List<MesGetDto>>> GetMeses();
        Task<ServiceResponseData<List<Dia>>> GetDias();
        Task<ServicesResponseMessage<string>> UpdateHorasContratadas(int idCarga);
        Task<bool> ValidateNivelPosgrado(string nivel);

        Task<ServiceResponseData<List<CargaGetDto>>> GetCargaUniversitas(string periodo, int recinto);
        Task<ServiceResponseData<List<CargaGetDto>>> SincronizarCarga(string periodo, int recinto);
        Task<ServiceResponseData<List<CargaGetVerificacionDto>>> GetCargaAkadeicWithUniversitas(string periodo, int recinto);

        Task<ServicesResponseMessage<string>> ChangeCarga(string cedula, string profesor, int idCaga);

        Task<ServiceResponseData<List<TipoReporteGetDto>>> GetTipoReporte();
        Task<ServiceResponseData<List<TipoReporteIrregularGetDto>>> GetTipoReporteIrregular();
        Task<ServiceResponseData<NotaCargaIrregularDto>> InsertNotaCargaIrregular(NotaCargaIrregularDto nota);
        Task<ServiceResponseData<NotaCargaIrregularDto>> UpdateNotaCargaIrregular(NotaCargaIrregularDto nota);
        Task<ServiceResponseData<CargaGetDto>> AutorizarCarga(List<AuthCargaDto> Cargas);













    }
}
