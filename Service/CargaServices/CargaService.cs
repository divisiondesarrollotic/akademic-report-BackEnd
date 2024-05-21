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

        //  private readonly ICargaDocenteService _cargaDocenteService;

        public CargaService(IMapper mapper, DataContext dataContext, IDocenteService docenteService)
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
                    .Include(c => c.DiasNavigation).Include(c => c.CurricularNavigation).Include(c=>c.ModalidadNavigation).ToListAsync();
                var docentes = Docentes;
                int Creditos = 0;
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
                var CargaLista = new List<CargaGetDto>();
                foreach (var i in CargaMap)
                {
                    var tiposModalidad = carga.Where(c => c.Id == i.Id).ToList();
                    foreach (var o in tiposModalidad)
                    {
                        var TipoModalida = new TipoModalidadDto();
                        TipoModalida = _mapper.Map<TipoModalidadDto>(o.ModalidadNavigation);
                        i.TipoModalidad = TipoModalida;
                    }

                    var existe = CargaLista.Where(c => c.cod_asignatura == i.cod_asignatura && c.Seccion == i.Seccion && i.cod_universitas==i.cod_universitas).FirstOrDefault();
                    var concepto = await _dataContext.Codigos.Where(c => c.Codigo1 == i.cod_asignatura ).Include(c=>c.IdConceptoNavigation).FirstOrDefaultAsync();
                    
                    if(concepto!=null)
                    {
                        i.Concepto = new Dto.ConceptoDto.ConceptoGetDto() { Id = concepto.IdConceptoNavigation.Id, Nombre = concepto.IdConceptoNavigation.Nombre };
                    }
                    else
                    {
                        i.Concepto = new Dto.ConceptoDto.ConceptoGetDto() { Id = 0, Nombre = "" };
                    }
                   
                    i.TiposCarga = new TipoCargaDto();
                    i.TiposCarga.Id = carga.FirstOrDefault(c => c.Id == i.Id).CurricularNavigation.Id;
                    i.TiposCarga.Nombre = carga.FirstOrDefault(c => c.Id == i.Id).CurricularNavigation.Nombre;
                    var codigo = await _dataContext.Codigos.Where(c => c.Codigo1.Contains(i.cod_asignatura)).FirstOrDefaultAsync();
                    if (codigo != null)
                    {
                        i.id_asignatura = codigo.Id;
                        i.id_concepto = codigo.IdConcepto;
                    }
                  

                        decimal Horas = CalculoTiempoHoras.Calcular(int.Parse(i.hora_inicio), int.Parse(i.minuto_inicio), int.Parse(i.hora_fin), int.Parse(i.minuto_fin));
                        i.credito = Convert.ToInt32(Horas);
                        Creditos += i.credito;
                        CargaLista.Add(i);
                    
                    //else
                    //{
                    //    if (existe==null)
                    //    {

                    //        decimal Horas = CalculoTiempoHoras.Calcular(int.Parse(i.hora_inicio), int.Parse(i.minuto_inicio), int.Parse(i.hora_fin), int.Parse(i.minuto_fin));
                    //        i.credito = Convert.ToInt32(Horas);
                    //        Creditos += i.credito;
                    //        CargaLista.Add(i);
                    //    }
                    //    //else if (existe.cod_asignatura[0].ToString()=="P" && existe.cod_asignatura[1].ToString() == "D")
                    //    //{
                    //    //    if(CargaLista.Where(c => c.cod_asignatura == i.cod_asignatura && c.Seccion == i.Seccion && c.dia_id == i.dia_id && c.hora_inicio == i.hora_inicio && c.minuto_inicio == i.minuto_inicio && c.hora_fin == i.hora_fin && c.minuto_fin == i.minuto_fin && c.Aula==i.Aula).FirstOrDefault()==null)
                    //    //    {

                    //    //        decimal Horas = CalculoTiempoHoras.Ca lcular(int.Parse(i.hora_inicio), int.Parse(i.minuto_inicio), int.Parse(i.hora_fin), int.Parse(i.minuto_fin));
                    //    //        i.credito = Convert.ToInt32(Horas);
                    //    //        Creditos += i.credito;
                    //    //        CargaLista.Add(i);
                    //    //    }
                    //    //    else
                    //    //    {
                    //    //        i.credito = 0;
                    //    //        CargaLista.Add(i);

                    //    //    }        
                    //    //}
                    //    //else
                    //    //{
                    //    //    i.credito = 0;
                    //    //    CargaLista.Add(i);
                    //    //}
                     
                    //}

                }
                ResulData.Carga = CargaLista.OrderBy(c => c.dia_id).ThenBy(c => int.Parse(c.hora_inicio)).ToList();
                ResulData.Docente = DocenteFilter;
                ResulData.CantCredito = Creditos;
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
                //// Validacion el calculo de las horas no puede dar decimal
                //decimal Horas = CalculoTiempoHoras.Calcular(int.Parse(item.hora_inicio), int.Parse(item.minuto_inicio), int.Parse(item.hora_fin), int.Parse(item.minuto_fin));
                //if(int.Parse(Horas.ToString().Split('.')[1])<1) return new ServicesResponseMessage<string>() { Status = 400, Message = Msj.MsjHorarioIncorrecto };

                //var cargaDocente = await _cargaDocenteService.GetCargaCall(item.Cedula, item.periodo);
                //if(cargaDocente.Data.Value.Item1.CantCredito+item.credito>40) return new ServicesResponseMessage<string>() { Status = 400, Message = (cargaDocente.Data.Value.Item1.Docente.tiempoDedicacion=="TC" 
                //  || cargaDocente.Data.Value.Item1.Docente.tiempoDedicacion == "A" || cargaDocente.Data.Value.Item1.Docente.tiempoDedicacion == "F" 
                //  || cargaDocente.Data.Value.Item1.Docente.tiempoDedicacion == "M") ?  Msj.MsjPasoDeCredito : Msj.MsjPasoDeCreditoMedioTimepo};

                //if (cargaDocente.Data.Value.Item1.Docente.tiempoDedicacion == "MT" && cargaDocente.Data.Value.Item1.CantCredito + item.credito > 32) return new ServicesResponseMessage<string>() { Status = 400, Message = Msj.MsjPasoDeCreditoMedioTimepo};
      
                CargaDocente carga = new CargaDocente();
                carga.Curricular = item.idTipoCarga;
                carga.Periodo = item.periodo;
                carga.Recinto = item.recinto.ToString();
                carga.CodAsignatura = item.cod_asignatura;
                carga.NombreAsignatura = item.nombre_asignatura;
                carga.CodUniversitas = item.cod_universitas;
                carga.Seccion = item.seccion.ToString();
                carga.Aula = item.aula;
                carga.Modalidad = item.idModalidad;
                carga.Dias = item.dia_id;
                carga.HoraInicio = item.hora_inicio;
                carga.MinutoInicio = item.minuto_inicio;
                carga.HoraFin = item.hora_fin;
                carga.MinutoFin = item.minuto_fin;
                carga.NumeroHora = item.numero_hora;
                carga.Credito = item.credito;
                carga.NombreProfesor = item.nombre_profesor;
                carga.Cedula = item.Cedula;
                _dataContext.CargaDocentes.Add(carga);
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
                carga.Curricular = item.idTipoCarga;
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
