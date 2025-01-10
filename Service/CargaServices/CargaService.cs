using AkademicReport.Data;
using AkademicReport.Dto;
using AkademicReport.Dto.AsignaturaDto;
using AkademicReport.Dto.CargaDto;
using AkademicReport.Dto.ConceptoDto;
using AkademicReport.Dto.ConceptoPosgradoDto;
using AkademicReport.Dto.DocentesDto;
using AkademicReport.Dto.ReporteDto;
using AkademicReport.Dto.UsuarioDto;
using AkademicReport.Models;
using AkademicReport.Service.DocenteServices;
using AkademicReport.Utilities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.VisualBasic;

namespace AkademicReport.Service.CargaServices
{
    public class CargaService : ICargaDocenteService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _dataContext;
        private readonly IDocenteService _docenteService;
        public List<string> CodigosIngles = new List<string> { "ING-201", "ING-302", "ING-403", "ING-504", "ING-605", "IOP-01", "IOP-02", "IOP-03", "ING-220", "ING-100", "ING-110", "ING-200", "ING-210", "FRP-201", "FRP-301", "FRP-601", "FRP-701", "FRP-801", "PVS-300", "PVS-305" };
        public int contado = 0;

        //  private readonly ICargaDocenteService _cargaDocenteService;

        public CargaService(IMapper mapper, DataContext dataContext, IDocenteService docenteService)
        {
            _mapper = mapper;
            _dataContext = dataContext;
            _docenteService = docenteService;
           
        }
        public async Task<ServicesResponseMessage<string>> Delete(int id, int idUsuario)
        {
            try
            {
                var carga = await _dataContext.CargaDocentes.FirstOrDefaultAsync(c => c.Id == id);
                if (carga == null)
                    return new ServicesResponseMessage<string>() { Status = 204, Message = Msj.MsjNoRegistros };
                carga.Deleted = true;
                _dataContext.Entry(carga).State =EntityState.Modified;
                await _dataContext.SaveChangesAsync();
                await SaveLogTransaction(new LogTransDto() { Accion = "DELETE", Fecha = DateTime.Now, IdCarga = id, IdUsuario = idUsuario, Cedula = carga.Cedula });
                return new ServicesResponseMessage<string>() { Status = 200, Message = Msj.MsjDelete };
            }
            catch (Exception ex)
            {
                return new ServicesResponseMessage<string>() { Status = 500, Message = Msj.MsjError + ex.ToString };
            }
        }

        public async Task<ServiceResponseCarga<DocenteCargaDto, string>> GetCarga(string Cedula, string Periodo, int idPrograma, List<DocenteGetDto> Docentes)
        {
            try
            {
   
                var ResulData = new DocenteCargaDto();
                var carga = await _dataContext.CargaDocentes.Where(c => c.Cedula==Cedula && c.Periodo == Periodo && c.IdPrograma==idPrograma && c.Deleted==false)
                    .Include(c => c.DiasNavigation)
                    .Include(c => c.CurricularNavigation)
                    .Include(c=>c.ModalidadNavigation)
                    .Include(c=>c.IdConceptoPosgradoNavigation)
                    .Include(c=>c.IdMesNavigation)
                    .Include(c=>c.IdCodigoNavigation)
                    .Include(c=>c.IdPeriodoNavigation).ToListAsync();
                var docentes = Docentes;
                int Creditos = 0;
                var DocenteFilter = docentes.Where(c => c.identificacion.ToString().Contains(Cedula)).FirstOrDefault();

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
                    if (i.IdCodigo != null)
                        i.nombre_asignatura = carga.First(c => c.Id == i.Id).IdCodigoNavigation.Nombre;

                    var tiposModalidad = carga.Where(c => c.Id == i.Id).ToList();
                    foreach (var o in tiposModalidad)
                    {
                        var TipoModalida = new TipoModalidadDto();
                        TipoModalida = _mapper.Map<TipoModalidadDto>(o.ModalidadNavigation);
                        i.TipoModalidad = TipoModalida;
                    }
                    var recinto = await _dataContext.Recintos.FirstOrDefaultAsync(c => c.Id == int.Parse(i.Recinto));
                    if(recinto!=null)
                    {
                        i.RecintoNombre = recinto.NombreCorto;
                    }

                    var existe = CargaLista.Where(c => c.cod_asignatura == i.cod_asignatura && c.Seccion == i.Seccion && i.cod_universitas==i.cod_universitas).FirstOrDefault();
                    var concepto = await _dataContext.Codigos.Where(c => c.Codigo1 == i.cod_asignatura )
                        .Include(c=>c.IdConceptoNavigation).FirstOrDefaultAsync();
                    
                    if(concepto!=null)
                    {
                        i.Concepto = new Dto.ConceptoDto.ConceptoGetDto() { Id = concepto.IdConceptoNavigation.Id, Nombre = concepto.IdConceptoNavigation.Nombre };
                    }
                    else
                    {
                        var Docencia = await _dataContext.Conceptos.FirstAsync(c => c.Nombre.Contains("Docencia"));
                        i.Concepto =  new Dto.ConceptoDto.ConceptoGetDto() { Id = Docencia.Id, Nombre = Docencia.Nombre };
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
                }
      
                foreach (var codigo in CodigosIngles)
                {
                    foreach (var cargaDiplomado in CargaLista)
                    {
                        if(cargaDiplomado.cod_asignatura.Trim().Contains(codigo.Trim()))
                        {
                            var tipoCarga = await _dataContext.TipoCargas.FirstOrDefaultAsync(c => c.Nombre!.Contains("Diplomado"));
                            if(tipoCarga!=null)
                            {
                                cargaDiplomado.TiposCarga = _mapper.Map<TipoCargaDto>(tipoCarga);
                            } 
                        } 
                    }
                }
                ResulData.Carga = CargaLista.OrderBy(c => c.dia_id).ThenBy(c => int.Parse(c.hora_inicio)).ToList();
                ResulData.Docente = DocenteFilter;
                ResulData.CantCredito = Creditos;
                if(ResulData.Carga.Count>0)
                    ResulData.Anio = ResulData.Carga[0].PeriodoObj!.Anio!.Value;
                return new ServiceResponseCarga<DocenteCargaDto, string> { Data = (ResulData, ""), Status = 200 };
            }
            catch (Exception ex)
             {
                string error = ex.ToString();
                throw;
            }

        }


        public async Task<ServiceResponseCarga<ReportCargaPosgradoDto, string>> GetCargaPosgrado(string Cedula, string Periodo, int idPrograma, List<DocenteGetDto> Docentes, int idRecinto)
        {
            try
            {
                var ResulData = new ReportCargaPosgradoDto();
                var carga = await _dataContext.CargaDocentes.Where(c => c.Cedula.Replace("-","")==Cedula.Replace("-","").ToString() && c.Periodo == Periodo && c.IdPrograma == idPrograma  && c.Deleted==false)
                    .Include(c => c.DiasNavigation)
                    .Include(c => c.CurricularNavigation)
                    .Include(c => c.ModalidadNavigation)
                    .Include(c => c.IdConceptoPosgradoNavigation)
                    .Include(c => c.IdMesNavigation)
                    .Include(c=>c.IdCodigoNavigation) 
                    .Include(c=>c.IdPeriodoNavigation)
                    .Include(c=>c.RecintoNavigation)
                    .Include(c=>c.IdCodigoNavigation).ThenInclude(c=>c.IdConceptoNavigation).ToListAsync();
                var docentes = Docentes;
                int Creditos = 0;
                int pagoPorHora = 0;
                var DocenteFilter = docentes.Where(c => c.identificacion.ToString().Contains(Cedula)).FirstOrDefault();

                if (DocenteFilter == null)
                {
                    ResulData.Cargas = null;
                    return new ServiceResponseCarga<ReportCargaPosgradoDto, string>() { Status = 204, Data = (ResulData, "Docente no existe") };
                }
                if (carga == null)
                {
                    ResulData.Cargas = new List<CargaPosgradoGet>();
                    ResulData.Docente =_mapper.Map<DocenteReporteDto>(DocenteFilter);
                    return new ServiceResponseCarga<ReportCargaPosgradoDto, string>() { Status = 200, Data = (ResulData, "No hay carga") };
                }
                var CargaMap = _mapper.Map<List<CargaPosgradoGet>>(carga);
                var nivelAcademico = await _dataContext.NivelAcademicos.Where(c => c.IdPrograma == 2).ToListAsync();

                foreach (var item in CargaMap)
                {
                    if (item.IdCodigo != null)
                        item.NombreAsignatura = carga.First(c => c.Id == item.Id).IdCodigoNavigation.Nombre;
                    item.ConceptoAsignatura??= _mapper.Map<ConceptoGetDto>(carga.First(c => c.Id == item.Id).IdCodigoNavigation.IdConceptoNavigation);
                    item.RecintoNombreCorto = item.RecintoObj.nombre_corto;
                    item.IdAsignatura = item.Codigo.Id;
                    Creditos += item.Credito;
                }
                if(nivelAcademico.FirstOrDefault(c => c.Nivel == DocenteFilter.nivel)!=null)
                {
                    pagoPorHora= nivelAcademico.FirstOrDefault(c => c.Nivel == DocenteFilter.nivel).PagoHora;
                }
                
                ResulData.Cargas = CargaMap.OrderBy(c => c.IdMes).ToList();
                ResulData.Docente = _mapper.Map<DocenteReporteDto>(DocenteFilter);
                ResulData.Docente.Monto = pagoPorHora.ToString();
                ResulData.CantCreditos =Creditos;   
                return new ServiceResponseCarga<ReportCargaPosgradoDto, string> { Data = (ResulData, ""), Status = 200 };
            }
            catch (Exception ex)
            {
                string error = ex.ToString();
                throw;
            }


        }


        public async Task<ServiceResponseCarga<DocenteCargaDto, string>> GetCargaCall(string cedula, string periodo, int idPrograma)
        {
            FiltroDocentesDto filtro = new FiltroDocentesDto();
            filtro.Filtro= cedula;
            var Docentes = await _docenteService.GetAllFilter(filtro);
            var Result = await GetCarga(cedula, periodo, idPrograma, Docentes.Data);       
            return Result;

        }
        public async Task<ServiceResponseCarga<ReportCargaPosgradoDto, string>> GetCargaCallPosgrado(string cedula, string periodo, int idPrograma, List<DocenteGetDto> DocentesAmilca, int idRecinto)
        {
            FiltroDocentesDto filtro = new FiltroDocentesDto();
            filtro.Filtro = cedula;
            var Docente = DocentesAmilca.Where(c=>c.identificacion==cedula).ToList();
            if(Docente!.Count>0 && await ValidateNivelPosgrado(Docente[0].nivel)==false)
                return new ServiceResponseCarga<ReportCargaPosgradoDto, string>() { Status = 204, Data =(null,$"Para un docente poder impartir carga en posgrado su nivel tiene que ser Maestría o Doctorado. Nivel del docente : {Docente[0].nivel}") };
            var Result = await GetCargaPosgrado(cedula, periodo, idPrograma, Docente, idRecinto);
            return Result;
        }

        public async Task<ServiceResponseData<List<Dia>>> GetDias()
        {
            try
            {
                var diasDb = await _dataContext.Dias.ToListAsync();

                return new ServiceResponseData<List<Dia>>() { Data = diasDb, Status = 200, Message = Msj.MsjSucces };
            }
            catch (Exception ex)
            {
                return new ServiceResponseData<List<Dia>>() { Status = 500, Message = Msj.MsjError + ex.ToString() };
            }
        }

        public async Task<ServiceResponseData<List<MesGetDto>>> GetMeses()
        {
            try
            {
                var meses = await _dataContext.Meses.ToListAsync();
                return new ServiceResponseData<List<MesGetDto>>() { Data = _mapper.Map<List<MesGetDto>>(meses), Status = 200, Message = Msj.MsjSucces };
            }
            catch (Exception ex)
            {
                return new ServiceResponseData<List<MesGetDto>>() { Status = 500, Message = Msj.MsjError + ex.ToString() };
            }
        }


        public async Task<ServiceResponseData<List<TipoDeCargaDto>>> GetTipoCarga(int IdPrograma)
        {
            var TipoCargas = await _dataContext.TipoCargas.ProjectTo<TipoDeCargaDto>(_mapper.ConfigurationProvider).Where(c=>c.IdPrograma==IdPrograma).ToListAsync();     
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
                carga.HoraContratada = true;
                carga.Curricular = item.idTipoCarga;
                carga.Periodo = item.periodo;
                carga.Recinto = item.recinto;
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
                carga.DiaMes = null;
                carga.DiaMes = null;
                carga.IdPrograma = 1;
                carga.Deleted = false;
                var periodo = await _dataContext.PeriodoAcademicos.Where(c => c.Periodo == item.periodo).FirstAsync();
                carga.IdPeriodo = periodo.Id;
                EntityEntry<CargaDocente> cargaSave = _dataContext.CargaDocentes.Add(carga);
                await _dataContext.SaveChangesAsync();
                await SaveLogTransaction(new LogTransDto() { Accion = "CREATE", Fecha = DateTime.Now, IdCarga = cargaSave.Entity.Id, IdUsuario = item.idUsuario, Cedula = carga.Cedula });
                return new ServicesResponseMessage<string>() { Status = 200, Message = Msj.MsjInsert };
            }
            catch (Exception ex)
            {
                return new ServicesResponseMessage<string>() { Status = 500, Message = Msj.MsjError + ex.ToString() };
            }
        }

        public async Task<ServicesResponseMessage<string>> InsertCargaPosgrado(CargaPosgradroDto item)
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

                var carga = _mapper.Map<CargaDocente>(item);
                carga.Seccion = "0";
                carga.Aula = "N/A";
                carga.IdPrograma = 2;
                carga.NumeroHora = 0;
                carga.Deleted = false;
                carga.Curricular = item.idTipoCarga;
                carga.HoraContratada = true;
                carga.IdConceptoPosgrado = item.IdConceptoPosgrado;
                var periodo = await _dataContext.PeriodoAcademicos.Where(c => c.Periodo == item.Periodo).FirstAsync();
                carga.IdPeriodo = periodo.Id;
                EntityEntry<CargaDocente> cargaSave  = _dataContext.CargaDocentes.Add(carga);
                await _dataContext.SaveChangesAsync();
                await SaveLogTransaction(new LogTransDto() { Accion = "CREATE", Fecha = DateTime.Now, IdCarga = cargaSave.Entity.Id, IdUsuario = item.idUsuario });

                return new ServicesResponseMessage<string>() { Status = 200, Message = Msj.MsjInsert };
            }
            catch (Exception ex)
            {
                return new ServicesResponseMessage<string>() { Status = 500, Message = Msj.MsjError + ex.ToString() };
            }
        }
        public async Task<ServicesResponseMessage<string>> UpdateCargaPosgrado(CargaPosgradroDto item)
        {
            try
            {
                var carga = await _dataContext.CargaDocentes.AsNoTracking().FirstOrDefaultAsync(c => c.Id == Convert.ToInt32(item.Id));
                if (carga != null)
                {
                    carga = _mapper.Map<CargaDocente>(item);
                    carga.Seccion = "N/A";
                    carga.Aula = "N/A";
                    carga.IdPrograma = 2;
                    carga.Curricular = null;
                    carga.NumeroHora = 0;
                    carga.Curricular = 5;
                    carga.HoraContratada = true;
                    carga.Deleted = false;
                    var periodo = await _dataContext.PeriodoAcademicos.Where(c => c.Periodo == item.Periodo).FirstAsync();
                    carga.IdPeriodo = periodo.Id;
                    _dataContext.Entry(carga).State = EntityState.Modified;
                    await _dataContext.SaveChangesAsync();
                    await SaveLogTransaction(new LogTransDto() { Accion = "UPDATE", Fecha = DateTime.Now, IdCarga = item.Id, IdUsuario = item.idUsuario, Cedula= item.Cedula });

                }
                return new ServicesResponseMessage<string>() { Status = 200, Message = Msj.MsjUpdate };
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
                if(carga!=null)
                {
                    carga = _mapper.Map<CargaDocente>(item);
                    carga.Curricular = item.idTipoCarga;
                    carga.HoraContratada = true;
                    carga.IdPrograma = 1;
                    carga.Deleted = false;
                    var periodo = await _dataContext.PeriodoAcademicos.Where(c => c.Periodo == item.Periodo).FirstAsync();
                    carga.IdPeriodo = periodo.Id;
                    _dataContext.Entry(carga).State = EntityState.Modified;
                    await _dataContext.SaveChangesAsync();
                    await SaveLogTransaction(new LogTransDto() { Accion = "UPDATE", Fecha = DateTime.Now, IdCarga = item.Id, IdUsuario = item.idUsuario, Cedula = item.Cedula });
                }
                return new ServicesResponseMessage<string>() { Status = 200, Message = Msj.MsjUpdate };
            }
            catch (Exception ex)
            {
                return new ServicesResponseMessage<string>() { Status = 500, Message = Msj.MsjError + ex.ToString() };
            }
        }


        public  async Task<bool> ValidateNivelPosgrado(string nivel)
        {
            var nivelAcademicoDb = await _dataContext.NivelAcademicos.Where(c => c.IdPrograma == 2).ToListAsync();
            var nivelAcademico = nivelAcademicoDb.Find(c => c.Nivel.Contains(nivel));
            bool resultado = false;
            resultado = nivelAcademico == null?  false :  true;
            return resultado;
        }

        public async Task<ServicesResponseMessage<string>> UpdateHorasContratadas(int idCarga)
        {
            try
            {
                var carga = await _dataContext.CargaDocentes.AsNoTracking().FirstOrDefaultAsync(c => c.Id == idCarga);
                if (carga != null)
                {
                    carga.HoraContratada = carga.HoraContratada==null || carga.HoraContratada==false? true : false;
                    _dataContext.Entry(carga).State = EntityState.Modified;
                    await _dataContext.SaveChangesAsync();
                }

                return new ServicesResponseMessage<string>() { Status = 200, Message = Msj.MsjUpdate };
            }
            catch (Exception ex)
            {
                return new ServicesResponseMessage<string>() { Status = 500, Message = Msj.MsjError + ex.ToString() };
            }


        }
        public async Task<bool> SaveLogTransaction(LogTransDto log)
        {
            try
            {
                    _dataContext.LogTransacionals.Add(_mapper.Map<LogTransacional>(log));
                    await _dataContext.SaveChangesAsync();
                    return true;
            }
            catch (Exception ex)
            {

                return false;
            }

        }
    }
}
