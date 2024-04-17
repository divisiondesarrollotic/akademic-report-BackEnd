using AkademicReport.Data;
using AkademicReport.Dto.AsignaturaDto;
using AkademicReport.Dto.CargaDto;
using AkademicReport.Dto.DocentesDto;
using AkademicReport.Dto.UsuarioDto;
using AkademicReport.Models;
using AkademicReport.Service.DocenteServices;
using AkademicReport.Utilities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace AkademicReport.Service.CargaServices
{
    public class CargaService : ICargaDocenteService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _dataContext;
        private readonly IDocenteService _docenteService;

        public CargaService(IMapper mapper, DataContext dataContext, IDocenteService docenteService )
        {
            _mapper = mapper;
            _dataContext = dataContext;
            _docenteService = docenteService;
        }

        public async Task<ServicesResponseMessage<string>> Delete(int id)
        {
            try
            {
                var carga = await _dataContext.CargaDocentes.FirstOrDefaultAsync(c => c.Id == id);
                if (carga == null)
                    return new ServicesResponseMessage<string>() { Status = 204, Message = Msj.MsjNoRegistros };


                _dataContext.CargaDocentes.Remove(carga);
                await _dataContext.SaveChangesAsync();
                return new ServicesResponseMessage<string>() { Status = 200, Message = Msj.MsjDelete };
            }
            catch (Exception ex)
            {
                return new ServicesResponseMessage<string>() { Status = 500, Message = Msj.MsjError + ex.ToString };
            }
        }

        public async Task<ServiceResponseCarga<DocenteCargaDto, string>> GetCarga(string Cedula, string Periodo, List<DocenteGetDto> Docentes)
        {
            try
            {
                var ResulData = new DocenteCargaDto();
                var carga = await _dataContext.CargaDocentes.Where(c => c.Cedula.Contains(Cedula) && c.Periodo == Periodo)
                    .Include(c => c.DiasNavigation).Include(c => c.CurricularNavigation).OrderBy(c => c.DiasNavigation.Id).OrderBy(c => c.HoraInicio).ToListAsync();
                var docentes = Docentes;
                var DocenteFilter = docentes.Where(c => c.identificacion == Cedula).FirstOrDefault();

                if (DocenteFilter == null)
                {
                    ResulData.Carga = null;
                    return new ServiceResponseCarga<DocenteCargaDto, string>() { Status = 204, Data = (ResulData, "Docente no existe") };
                }
                if (carga == null)
                {
                    ResulData.Carga = new List<CargaGetDto>();
                    ResulData.Docente = DocenteFilter;
                    return new ServiceResponseCarga<DocenteCargaDto, string>() { Status = 200, Data = (ResulData, "No hay carga") };
                }
                var CargaMap = _mapper.Map<List<CargaGetDto>>(carga);
                foreach (var i in CargaMap)
                {
                    i.TiposCarga = new TipoCargaDto();
                    i.TiposCarga.Id = carga.FirstOrDefault(c => c.Id == i.Id).CurricularNavigation.Id;
                    i.TiposCarga.Nombre = carga.FirstOrDefault(c => c.Id == i.Id).CurricularNavigation.Nombre;
                    var codigo = await _dataContext.Codigos.Where(c => c.Codigo1.Contains(i.cod_asignatura)).FirstOrDefaultAsync();
                    if (codigo != null)
                    {
                        i.id_asignatura = codigo.Id;
                        i.id_concepto = codigo.IdConcepto;
                    }
                }
                ResulData.Carga = CargaMap.OrderBy(c => c.dia_id).ThenBy(c => c.hora_inicio).ToList();
                ResulData.Docente = DocenteFilter;
                return new ServiceResponseCarga<DocenteCargaDto, string> { Data = (ResulData, ""), Status = 200 };
            }
            catch (Exception)
            {

                throw;
            }

          
        }

        public async Task<ServiceResponseCarga<DocenteCargaDto, string>> GetCargaCall(string cedula, string periodo)
        {
            var Docentes = await _docenteService.GetAll();
            var Result = await GetCarga(cedula, periodo, Docentes.Data);
            return Result;

        }

        public async Task<ServiceResponseData<List<TipoDeCargaDto>>> GetTipoCarga()
        {

            var TipoCargas = await _dataContext.TipoCargas.ProjectTo<TipoDeCargaDto>(_mapper.ConfigurationProvider).ToListAsync();     
            return new ServiceResponseData<List<TipoDeCargaDto>>() { Status = 200, Data = TipoCargas};
           
           
        }

        public async Task<ServicesResponseMessage<string>> Insert(CargaAddDto item)
        {
            try
            {

                CargaDocente carga = new CargaDocente();
                carga.Curricular = item.TiposCargas.Id;
                carga.Periodo = item.periodo;
                carga.Recinto = item.recinto.ToString();
                carga.CodAsignatura = item.cod_asignatura;
                carga.NombreAsignatura = item.nombre_asignatura;
                carga.CodUniversitas = item.cod_universitas;
                carga.Seccion = item.seccion.ToString();
                carga.Aula = item.aula;
                carga.Modalidad = item.modalida;
                carga.Dias = item.dia_id;
                carga.HoraInicio = item.hora_inicio;
                carga.MinutoInicio = item.minuto_inicio;
                carga.HoraFin = item.hora_fin;
                carga.MinutoFin = item.minuto_fin;
                carga.NumeroHora = item.numero_hora;
                carga.Credito = item.credito;
                carga.NombreProfesor = item.nombre_profesor;
                carga.Cedula = item.Cedula;
                EntityEntry<CargaDocente> result =  _dataContext.CargaDocentes.Add(carga);
                await _dataContext.SaveChangesAsync();
               
                
                await _dataContext.SaveChangesAsync();
                return new ServicesResponseMessage<string>() { Status = 200, Message = Msj.MsjInsert };
            }
            catch (Exception ex)
            {
                return new ServicesResponseMessage<string>() { Status = 500, Message = Msj.MsjError + ex.ToString() };
            }
        }

        public async Task<ServicesResponseMessage<string>> Update(CargaUpdateDto item)
        {
            try
            {
                var carga = await _dataContext.CargaDocentes.AsNoTracking().FirstOrDefaultAsync(c => c.Id == Convert.ToInt32(item.Id));
                carga = _mapper.Map<CargaDocente>(item);
                carga.Curricular = item.TiposCargas.Id;
                _dataContext.Entry(carga).State = EntityState.Modified;

                await _dataContext.SaveChangesAsync();
                return new ServicesResponseMessage<string>() { Status = 200, Message = Msj.MsjUpdate };
            }
            catch (Exception ex)
            {
                return new ServicesResponseMessage<string>() { Status = 500, Message = Msj.MsjError + ex.ToString() };
            }
        }

    }
}
