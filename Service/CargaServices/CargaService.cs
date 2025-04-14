using AkademicReport.Data;
using AkademicReport.Dto;
using AkademicReport.Dto.AsignaturaDto;
using AkademicReport.Dto.CargaDto;
using AkademicReport.Dto.ConceptoDto;
using AkademicReport.Dto.ConceptoPosgradoDto;
using AkademicReport.Dto.DocentesDto;
using AkademicReport.Dto.ReporteDto;
using AkademicReport.Dto.TiposReporteDto;
using AkademicReport.Dto.UsuarioDto;
using AkademicReport.Models;
using AkademicReport.Service.DocenteServices;
using AkademicReport.Utilities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using static AkademicReport.Dto.CargaDto.ResponseApiUniversitas;

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
                _dataContext.Entry(carga).State = EntityState.Modified;
                await _dataContext.SaveChangesAsync();
                await SaveLogTransaction(new LogTransDto() { Accion = "DELETE", Fecha = DateTime.Now, IdCarga = id, IdUsuario = idUsuario, Cedula = carga.Cedula });
                return new ServicesResponseMessage<string>() { Status = 200, Message = Msj.MsjDelete };
            }
            catch (Exception ex)
            {
                return new ServicesResponseMessage<string>() { Status = 500, Message = Msj.MsjError + ex.ToString };
            }
        }

        public async Task<NotaCargaIrregularDto> GetByCdulaPeriodoNotaIrregular(string cedula, string periodo)
        {

            var periodos = await _dataContext.PeriodoAcademicos.ToListAsync();
            int idPeriodo = periodos.FirstOrDefault(c => c.Periodo.Contains(periodo)).Id;
            var notaDb = await _dataContext.NotasCargaIrregulars.FirstOrDefaultAsync(c => c.Cedula == cedula && c.IdPeriodo == idPeriodo);
            return _mapper.Map<NotaCargaIrregularDto>(notaDb);
        }

        public async Task<ServiceResponseCarga<DocenteCargaDto, string>> GetCarga(string Cedula, string Periodo, int idPrograma, List<DocenteGetDto> Docentes)
        {
            try
            {
                var ResulData = new DocenteCargaDto();
                var NotaIrregular = await GetByCdulaPeriodoNotaIrregular(Cedula, Periodo);
                ResulData.NotaCargaIrregular = NotaIrregular != null ? NotaIrregular : null;
                var carga = await _dataContext.CargaDocentes.Where(c => c.Cedula == Cedula && c.Periodo == Periodo && c.IdPrograma == idPrograma && c.Deleted == false && c.IsAuth == true)
                    .Include(c => c.DiasNavigation)
                    .Include(c => c.CurricularNavigation)
                    .Include(c => c.ModalidadNavigation)
                    .Include(c => c.IdConceptoPosgradoNavigation)
                    .Include(c => c.IdMesNavigation)
                    .Include(c => c.IdCodigoNavigation)
                    .Include(c => c.IdPeriodoNavigation)
                    .Include(c => c.IdTipoReporteIrregularNavigation)
                    .Include(c => c.IdTipoReporteNavigation)
                    .Include(c => c.CantSemanasMes)
                    .ToListAsync();
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

                var CargaMap = new List<CargaGetDto>();
                CargaMap = _mapper.Map<List<CargaGetDto>>(carga);
                //Ahora a esa carga le agregamos la carga de univeritas
                //var CargaUniversita = await GetCargaUniversitas(Periodo, Cedula);
                //CargaMap.AddRange(CargaUniversita.Data);

                var CargaLista = new List<CargaGetDto>();
                foreach (var i in CargaMap)
                {
                    i.DataDocente = DocenteFilter;
                    if (i.IdCodigo != null)
                    {
                        i.nombre_asignatura = carga.First(c => c.Id == i.Id).IdCodigoNavigation.Nombre;

                    }
                    decimal CreditosCalculado =CalculoTiempoHoras.Calcular(Convert.ToInt32(i.hora_inicio), Convert.ToInt32(i.minuto_inicio), Convert.ToInt32(i.hora_fin), Convert.ToInt32(i.minuto_fin));
                    i.credito = Convert.ToInt32(CreditosCalculado);
                    var tiposModalidad = await _dataContext.TipoModalidads.FirstAsync(c => c.Id == i.TipoModalidad.Id);
                    var TipoModalida = new TipoModalidadDto();
                    TipoModalida = _mapper.Map<TipoModalidadDto>(tiposModalidad);
                    i.TipoModalidad = TipoModalida;
                    var cantSemanasMes = carga.FirstOrDefault(c => c.Id == i.Id);
                    if (cantSemanasMes != null)
                        i.cantSemanaMes = _mapper.Map<List<DayCantSemanasDto>>(cantSemanasMes.CantSemanasMes);


                    var recinto = await _dataContext.Recintos.FirstOrDefaultAsync(c => c.Id == int.Parse(i.Recinto));
                    if (recinto != null)
                    {
                        i.RecintoNombre = recinto.NombreCorto;
                    }

                    var existe = CargaLista.Where(c => c.cod_asignatura == i.cod_asignatura && c.Seccion == i.Seccion && i.cod_universitas == i.cod_universitas).FirstOrDefault();
                    var concepto = await _dataContext.Codigos.Where(c => c.Codigo1 == i.cod_asignatura)
                        .Include(c => c.IdConceptoNavigation).FirstOrDefaultAsync();

                    if (concepto != null)
                    {
                        i.Concepto = new Dto.ConceptoDto.ConceptoGetDto() { Id = concepto.IdConceptoNavigation.Id, Nombre = concepto.IdConceptoNavigation.Nombre };
                    }
                    else
                    {
                        var Docencia = await _dataContext.Conceptos.FirstAsync(c => c.Nombre.Contains("Docencia"));
                        i.Concepto = new Dto.ConceptoDto.ConceptoGetDto() { Id = Docencia.Id, Nombre = Docencia.Nombre };
                    }

                    i.TiposCarga = new TipoCargaDto();
                    i.TiposCarga = _mapper.Map<TipoCargaDto>(await _dataContext.TipoCargas.FirstAsync(c => c.Id == i.Curricular));

                    var codigo = await _dataContext.Codigos.Where(c => c.Codigo1.Contains(i.cod_asignatura)).FirstOrDefaultAsync();
                    if (codigo != null)
                    {
                        i.id_asignatura = codigo.Id;
                        i.id_concepto = codigo.IdConcepto;

                    }
                    //decimal Horas = CalculoTiempoHoras.Calcular(int.Parse(i.hora_inicio), int.Parse(i.minuto_inicio), int.Parse(i.hora_fin), int.Parse(i.minuto_fin));
                    //i.credito = Convert.ToInt32(Horas);
                    //Creditos += i.credito;
                    CargaLista.Add(i);
                }

                foreach (var codigo in CodigosIngles)
                {
                    foreach (var cargaDiplomado in CargaLista)
                    {
                        if (cargaDiplomado.cod_asignatura.Trim().Contains(codigo.Trim()))
                        {
                            var tipoCarga = await _dataContext.TipoCargas.FirstOrDefaultAsync(c => c.Nombre!.Contains("Diplomado"));
                            if (tipoCarga != null)
                            {
                                cargaDiplomado.TiposCarga = _mapper.Map<TipoCargaDto>(tipoCarga);
                            }
                        }
                    }
                }
                ResulData.Carga = CargaLista.OrderBy(c => c.dia_id).ThenBy(c => int.Parse(c.hora_inicio)).ToList();
                ResulData.Docente = DocenteFilter;
                ResulData.CantCredito = Creditos;
                if (ResulData.Carga.Count > 0)
                    ResulData.Anio = CargaLista[0].PeriodoObj!.Anio!.Value;
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
                var carga = await _dataContext.CargaDocentes.Where(c => c.Cedula.Replace("-", "") == Cedula.Replace("-", "").ToString() && c.Periodo == Periodo && c.IdPrograma == idPrograma && c.Deleted == false)
                    .Include(c => c.DiasNavigation)
                    .Include(c => c.CurricularNavigation)
                    .Include(c => c.ModalidadNavigation)
                    .Include(c => c.IdConceptoPosgradoNavigation)
                    .Include(c => c.IdMesNavigation)
                    .Include(c => c.IdCodigoNavigation)
                    .Include(c => c.IdPeriodoNavigation)
                    .Include(c => c.RecintoNavigation)
                    .Include(c => c.IdCodigoNavigation).ThenInclude(c => c.IdConceptoNavigation).ToListAsync();
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
                    ResulData.Docente = _mapper.Map<DocenteReporteDto>(DocenteFilter);
                    return new ServiceResponseCarga<ReportCargaPosgradoDto, string>() { Status = 200, Data = (ResulData, "No hay carga") };
                }
                var CargaMap = _mapper.Map<List<CargaPosgradoGet>>(carga);
                var nivelAcademico = await _dataContext.NivelAcademicos.Where(c => c.IdPrograma == 2).ToListAsync();

                foreach (var item in CargaMap)
                {
                    if (item.IdCodigo != null)
                        item.NombreAsignatura = carga.First(c => c.Id == item.Id).IdCodigoNavigation.Nombre;
                    item.ConceptoAsignatura ??= _mapper.Map<ConceptoGetDto>(carga.First(c => c.Id == item.Id).IdCodigoNavigation.IdConceptoNavigation);
                    item.RecintoNombreCorto = item.RecintoObj.nombre_corto;
                    item.IdAsignatura = item.Codigo.Id;
                    Creditos += item.Credito;
                }
                if (nivelAcademico.FirstOrDefault(c => c.Nivel == DocenteFilter.nivel) != null)
                {
                    pagoPorHora = nivelAcademico.FirstOrDefault(c => c.Nivel == DocenteFilter.nivel).PagoHora;
                }

                ResulData.Cargas = CargaMap.OrderBy(c => c.IdMes).ToList();
                ResulData.Docente = _mapper.Map<DocenteReporteDto>(DocenteFilter);
                ResulData.Docente.Monto = pagoPorHora.ToString();
                ResulData.CantCreditos = Creditos;
                return new ServiceResponseCarga<ReportCargaPosgradoDto, string> { Data = (ResulData, ""), Status = 200 };
            }
            catch (Exception ex)
            {
                string error = ex.ToString();
                throw;
            }
        }


        public async Task<ServiceResponseCarga<DocenteCargaDto, string>> GetCargaCall(string cedula, string periodo, int idPrograma, int idTipoReporte, int idTipoReporteI)
        {
            FiltroDocentesDto filtro = new FiltroDocentesDto();
            filtro.Filtro = cedula;
            var Docentes = await _docenteService.GetAllFilter(filtro);
            var Result = await GetCarga(cedula, periodo, idPrograma, Docentes.Data);
            if (Result.Data != null && Result.Data.Value.Item1 != null && Result.Data.Value.Item1.Carga != null)
            {
                Result.Data.Value.Item1.CargaRegular = new List<CargaGetDto>();
                if (idTipoReporteI == 2 || idTipoReporteI == 9 || idTipoReporteI == 10 || idTipoReporteI == 11 || idTipoReporteI == 12 || idTipoReporteI == 13)
                {
                    var notaCargaIrregular = await _dataContext.NotasCargaIrregulars.Include(c => c.IdPeriodoNavigation)
                        .FirstOrDefaultAsync(c => c.IdPeriodoNavigation.Periodo == periodo && c.Cedula == cedula && c.IdTipoReporte == idTipoReporte && c.IdTipoReporteIrregular == idTipoReporteI);
                    Result.Data.Value.Item1.NotaCargaIrregular = notaCargaIrregular != null ? _mapper.Map<NotaCargaIrregularDto>(notaCargaIrregular) : null;

                    Result.Data.Value.Item1.CargaRegular = Result.Data.Value.Item1.Carga.Where(c => c.IdTipoReporte == 1
                && c.IdTipoReporteIrregular == 4).ToList();
                }
                Result.Data.Value.Item1.Carga = Result.Data.Value.Item1.Carga.Where(c => c.IdTipoReporte == idTipoReporte && c.IdTipoReporteIrregular == idTipoReporteI).ToList();

            }
            return Result;

        }
        public async Task<ServiceResponseCarga<ReportCargaPosgradoDto, string>> GetCargaCallPosgrado(string cedula, string periodo, int idPrograma, List<DocenteGetDto> DocentesAmilca, int idRecinto)
        {
            FiltroDocentesDto filtro = new FiltroDocentesDto();
            filtro.Filtro = cedula;
            var Docente = DocentesAmilca.Where(c => c.identificacion == cedula).ToList();
            if (Docente!.Count > 0 && await ValidateNivelPosgrado(Docente[0].nivel) == false)
                return new ServiceResponseCarga<ReportCargaPosgradoDto, string>() { Status = 204, Data = (null, $"Para un docente poder impartir carga en posgrado su nivel tiene que ser Maestría o Doctorado. Nivel del docente : {Docente[0].nivel}") };
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
            var TipoCargas = await _dataContext.TipoCargas.ProjectTo<TipoDeCargaDto>(_mapper.ConfigurationProvider).Where(c => c.IdPrograma == IdPrograma).ToListAsync();
            return new ServiceResponseData<List<TipoDeCargaDto>>() { Status = 200, Data = TipoCargas };
        }

        public async Task<ServicesResponseMessage<string>> Insert(CargaAddDto item)
        {
            try
            {
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
                carga.IsAuth = item.cod_universitas == "N/A" ? true : false;
                carga.IdTipoReporte = item.IdTipoReporte == 0 || item.IdTipoReporte == null ? 1 : item.IdTipoReporte;
                carga.IdTipoReporteIrregular = item.IdTipoReporteIrregular == 0 || item.IdTipoReporteIrregular == null ? 4 : item.IdTipoReporteIrregular;
                carga.CantSemanas = item.CantSemanas;
                var periodoDb = await _dataContext.PeriodoAcademicos.Where(c => c.Periodo == item.periodo).FirstAsync();
                carga.IdPeriodo = periodoDb.Id;
                EntityEntry<CargaDocente> cargaSave = _dataContext.CargaDocentes.Add(carga);
                await _dataContext.SaveChangesAsync();

                if (item.CantSemanaMes != null && item.CantSemanaMes.Count > 0)
                {
                    foreach (var c in item.CantSemanaMes)
                    {
                        var mesSeamanas = _mapper.Map<CantSemanasMe>(c);
                        mesSeamanas.IdCarga = cargaSave.Entity.Id;
                        _dataContext.CantSemanasMes.Add(mesSeamanas);
                    }
                }
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
                carga.IsAuth = true;
                carga.Curricular = item.idTipoCarga;
                carga.HoraContratada = true;
                carga.IdConceptoPosgrado = item.IdConceptoPosgrado;
                var periodo = await _dataContext.PeriodoAcademicos.Where(c => c.Periodo == item.Periodo).FirstAsync();
                carga.IdPeriodo = periodo.Id;
                EntityEntry<CargaDocente> cargaSave = _dataContext.CargaDocentes.Add(carga);
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
                    carga.IsAuth = true;
                    carga.HoraContratada = true;
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

        public async Task<ServicesResponseMessage<string>> Update(CargaUpdateDto item)
        {
            try
            {
                var carga = await _dataContext.CargaDocentes.AsNoTracking().FirstOrDefaultAsync(c => c.Id == Convert.ToInt32(item.Id));
                if (carga != null)
                {
                    carga = _mapper.Map<CargaDocente>(item);
                    carga.Curricular = item.idTipoCarga;
                    carga.HoraContratada = true;
                    carga.IdPrograma = 1;
                    carga.Deleted = false;
                    var periodo = await _dataContext.PeriodoAcademicos.Where(c => c.Periodo == item.Periodo).FirstAsync();
                    carga.CantSemanas = item.CantSemanas;
                    carga.IdPeriodo = periodo.Id;
                    carga.IsAuth = item.cod_universitas == "N/A" ? true : false;
                    carga.IdTipoReporte = item.IdTipoReporte == 0 || item.IdTipoReporte == null ? 1 : item.IdTipoReporte;
                    carga.IdTipoReporteIrregular = item.IdTipoReporteIrregular == 0 || item.IdTipoReporteIrregular == null ? 4 : item.IdTipoReporteIrregular;
                    _dataContext.Entry(carga).State = EntityState.Modified;
                    await _dataContext.SaveChangesAsync();

                    //Eliminamos los horario senabales para luego
                    var semanas = await _dataContext.CantSemanasMes.Where(c => c.IdCarga == item.Id).ToListAsync();
                    if (semanas.Any())
                    {
                        _dataContext.CantSemanasMes.RemoveRange(semanas);
                    }

                    if (item.CantSemanaMes?.Any() == true)
                    {
                        var nuevasSemanasa = item.CantSemanaMes.Select(c => _mapper.Map<CantSemanasMe>(c)).ToList();
                        foreach (var i in nuevasSemanasa)
                        {
                            i.IdCarga = item.Id;
                            _dataContext.CantSemanasMes.Add(i);

                        }
                    }
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


        public async Task<bool> ValidateNivelPosgrado(string nivel)
        {
            var nivelAcademicoDb = await _dataContext.NivelAcademicos.Where(c => c.IdPrograma == 2).ToListAsync();
            var nivelAcademico = nivelAcademicoDb.Find(c => c.Nivel.Contains(nivel));
            bool resultado = false;
            resultado = nivelAcademico == null ? false : true;
            return resultado;
        }

        public async Task<ServicesResponseMessage<string>> UpdateHorasContratadas(int idCarga)
        {
            try
            {
                var carga = await _dataContext.CargaDocentes.AsNoTracking().FirstOrDefaultAsync(c => c.Id == idCarga);
                if (carga != null)
                {

                    carga.HoraContratada = carga.HoraContratada == null || carga.HoraContratada == false ? true : false;
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

        public async Task<ServiceResponseData<List<CargaGetDto>>> GetCargaUniversitas(string periodo, int recinto)
        {
            try
            {
                // Datos de autenticación
                string clientId = "1lHZgaHDvpD8rmctuJnn9w..";
                string clientSecret = "6YlofIMT6TqmQ6TOL_z_BQ..";
                ResponseTokenUniversitasDto reponseToken = new ResponseTokenUniversitasDto();
                // Codificar client_id y client_secret en Base64
                var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));

                // URL del endpoint de la API
                string tokenUrl = "https://uxxi.isfodosu.edu.do/ac_api_sql/uxxiac_api_sql/oauth/token";
                string consultaUrl = "https://uxxi.isfodosu.edu.do/ac_api_sql/uxxiac_api_sql/_/sql";
                HttpClient client = new HttpClient();

                // Crear HttpClient

                // Establecer los encabezados de la solicitud
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("Authorization", $"Basic {credentials}");

                // Crear el contenido del cuerpo de la solicitud (application/x-www-form-urlencoded)
                var contenta = new FormUrlEncodedContent(new[]
                {
                // Aquí puedes agregar más parámetros si la API los requiere, como `grant_type` o `scope`
                new KeyValuePair<string, string>("grant_type", "client_credentials")
                });

                // Hacer la solicitud POST
                HttpResponseMessage firstReponse = await client.PostAsync(tokenUrl, contenta);

                // Comprobar si la solicitud fue exitosa
                if (firstReponse.IsSuccessStatusCode)
                {
                    // Leer y mostrar la respuesta
                    string responseBody = await firstReponse.Content.ReadAsStringAsync();
                    Console.WriteLine("Respuesta exitosa: " + responseBody);
                    if (responseBody != null)
                    {
                        var r = JsonConvert.DeserializeObject<ResponseTokenUniversitasDto>(responseBody);
                        reponseToken = r;
                    }
                }
                else
                {
                    Console.WriteLine($"Error: {firstReponse.StatusCode}");
                    string errorResponse = await firstReponse.Content.ReadAsStringAsync();
                    Console.WriteLine($"Detalles del error: {errorResponse}");
                    return new ServiceResponseData<List<CargaGetDto>>() { Status = 401, Message = errorResponse, Data = new List<CargaGetDto>() };

                }

                string query = $@"select  t1.vac_codnum, t1.ass_codnum, t1. id_grp_activ, t1.desid1,  t1.cpcgrp,
        t1.numcre, t1.id_assignatura, ta.nomid1, t1.id_activ secc,
        t1.any_anyaca,
        t2.dsm_codnum, tc.nomid1 recinto,
        t2.codnum, t2.horini, t2.minini, t2.horfin, t2.minfin, t2.numhor,
        tal.DESID1 AUL_DESC , t2.identificador, t2.descripcion
from (  select  a.vac_codnum, a.id_activ, a.nomact, a.ass_codnum,
                a.eje_codnum, a.numcre, a.flgges, a.flgcom, a.id_assignatura,
                b.codnum gac_codnum, b.id_grp_activ, b.any_anyaca, b.desid1, b.cpcgrp
        from (  SELECT  vac.codnum vac_codnum,
                        vac.id_activ,
                        vac.desvac nomact,
                        seg.ass_codnum,
                        vac.flgges,
                        vac.moa_codnum,
                        vac.cac_codnum,
                        vac.eje_codnum,
                        vac.numcre,
                        vac.maxpct,
                        vac.flgcom,
                        seg.flgassres,
                        vac.cen_codnumseg,
                        vac.dep_codalfseg,
                        seg.id_assignatura,
                        seg.flgpal
                FROM (  SELECT  aas.ASS_CODNUM,
                                vac.CODNUM VAC_CODNUM,
                                aas.CEN_CODNUM,
                                NVL(aas.FLGPAL,    'N' ) FLGPAL,
                                NVL(aas.FLGASSRES, 'N' ) FLGASSRES,
                                eje.CODNUM               EJE_CODNUM,
                                eje.ANY_ANYACA,
                                vac.CEN_CODNUMSEG,
                                vac.DEP_CODALFSEG,
                                aas.ID_ASSIGNATURA
                        FROM    TACA_EJERCICIO eje,
                                TACA_VIG_ACTIV vac,
                                (  SELECT A1.*,A2.ID_ASSIGNATURA
                                    FROM TACA_ACTIV_ASIG A1,TALU_ASSIGNATURA A2
                                    WHERE A2.CODNUM=A1.ASS_CODNUM )aas
                        WHERE vac.eje_codnum = eje.codnum
                        AND vac.eje_codnum = aas.eje_codnum(+)
                        AND vac.codnum     = aas.vac_codnum(+)
                     ) seg,
                     (  SELECT  vac.eje_codnum,
                                vac.codnum,
                                vac.id_activ,
                                vac.desid1,
                                vac.desid2,
                                vac.desid3,
                                DECODE (1, 1, vac.desid1,
                                                            2, vac.desid2,
                                                            3, vac.desid3) desvac,
                                vac.flgges,
                                vac.moa_codnum,
                                vac.cac_codnum,
                                vac.numcre,
                                vac.maxpct,
                                DECODE (    (SELECT COUNT(1) FROM TACA_ACTIV_ASIG WHERE EJE_CODNUM = vac.eje_codnum AND VAC_CODNUM = vac.codnum),
                                                            0, 'N',
                                                            1, 'N',
                                                            'S') FLGCOM,
                                vac.tqu_codalf,
                                vac.txtid1,
                                vac.txtid2,
                                vac.txtid3,
                                DECODE (1, 1, vac.txtid1,
                                                            2, vac.txtid2,
                                                            3, vac.txtid3) txtvac,
                                vac.texto,
                                vac.resumen,
                                vac.cen_codnumseg,
                                vac.dep_codalfseg,
                                vac.factordes,
                                vac.cpcmingrp
                        FROM   taca_vig_activ vac
                    ) vac
                WHERE vac.codnum    = seg.vac_codnum
                AND vac.eje_codnum  = seg.eje_codnum
             ) a
        join TACA_GRP_ACTIV b
        on (a. vac_codnum = b.vac_codnum and a.eje_codnum= b.eje_codnum)
    ) t1,
    (   select  tfc.hor_codnum, tfc.eje_codnum, tfc.per_codnum, tfc.dsm_codnum, tfc.codnum, tfc.horini,
                tfc.minini, tfc.horfin, tfc.minfin, tfc.numhor, tfc.frn_codnum, tfc.aul_codnum,
                vlp.identificador, vlp.descripcion
        from (  select  fra.hor_codnum, fra.eje_codnum, fra.per_codnum, fra.dsm_codnum, dias.id_dia_semana, dias.desid1,
                        fra.codnum, fra.horini, fra.minini, fra.horfin, fra.minfin, fra.numhor,
                        fra.frn_codnum, fra.aul_codnum
                from (  select  tf.hor_codnum, tf.eje_codnum, tf.per_codnum, tf.dsm_codnum, tf.codnum,
                                tf.horini, tf.minini, tf.horfin, tf.minfin, tf.numhor,tc.frn_codnum, tc.aul_codnum
                        from TACA_FRANJA tf
                        left join TACA_CPSFRNAUL tc
                        on (tf.hor_codnum = tc.hor_codnum and tf.eje_codnum = tc.eje_codnum
                             and tf.per_codnum = tc.per_codnum and tf.dsm_codnum = tc.dsm_codnum)
                     ) fra,
                     taca_dias_semana dias
                where fra.dsm_codnum = dias.codnum
             ) tfc,
             (  SELECT  I.HOR_CODNUM, I.PER_CODNUM, I.DSM_CODNUM, I.FRN_CODNUM,
                        L.CODNUM, L.IDENTIFICADOR, L.DESCRIPCION, L.FLGPRF, L.FLGEXT,
                        L.NOMPRS, L.EJE_CODNUM, L.LL1PRS, L.LL2PRS, MIN(L.DATINI), MAX(L.DATFIN),
                        L.PLZ_CODNUM,I.PCTREP, I.FLGPAL, PI.DATINI, PI.DATFIN, PI.CODNUM, I.CODNUM
                FROM (  SELECT  prs.codnum CODNUM,
                                emp.codnum epe_codnum,
                                prs.dniprs IDENTIFICADOR,
                                prs.ll1prs || ' ' || prs.ll2prs|| ', ' || prs.nomprs DESCRIPCION,
                                'S' flgprf,
                                prf.flgext,
                                prs.nomprs nomprs,
                                emp.eje_codnum eje_codnum,
                                prs.ll1prs ll1prs,
                                prs.ll2prs ll2prs,
                                ads.datini,
                                ads.datfin,
                                flgdir,
                                TO_NUMBER( NULL ) PLZ_CODNUM
                        FROM (  SELECT  epe_codnum, plp_codnum, 
                                        MIN(datini) datini, 
                                        MAX(NVL(datfin, TO_DATE('01-01-3000','dd-mm-yyyy'))) datfin 
                                FROM TACA_ADSCRIPCION_PER 
                                GROUP BY epe_codnum, plp_codnum) ads,
                            TACA_EMPLEADO_PER emp,
                            TACA_PLAZA_PER plz,
                            TUIB_PERSONA prs,
                            TUIB_PROFESSOR prf,
                            TACA_EJERCICIO eje
                        WHERE ( eje.codnum=emp.eje_codnum 
                               AND ( emp.codnum = ads.epe_codnum )
                               AND ( plz.codnum = ads.plp_codnum )
                               AND ( prs.dniprs = prf.prs_dniprs )
                               AND ( prs.codnum = prf.prs_codnum )
                               AND ( prf.prs_codnum = emp.prf_codnum )
                               AND prf.flgext='N')
                        AND EXISTS (    SELECT 'X'
                                        FROM TACA_VAD_PLZ_PER vad
                                        WHERE eje.datini <= NVL(vad.datfin, TO_DATE('01-01-3000', 'dd-mm-yyyy'))
                                        AND eje.datfin >= vad.datini
                                        AND plz.codnum = vad.plp_codnum
                                        AND eje.codnum= vad.eje_codnum )
                        UNION ALL
                        SELECT  TO_NUMBER( NULL ) CODNUM,
                                TO_NUMBER( NULL ),
                                plz.id_plaza IDENTIFICADOR,
                                DECODE( 1, 1, plz.desid1, 2, plz.desid2, 3, plz.desid3 ) DESCRIPCION,
                                'N' flgprf,
                                'N' flgext,
                                NULL,
                                plz.eje_codnum,
                                NULL,
                                NULL,
                                plz.datalt datalt,
                                plz.datamt datamt,
                                NULL flgdir,
                                plz.codnum PLZ_CODNUM
                        FROM TACA_PLAZA_PER plz
                        WHERE NOT EXISTS( SELECT 'x'
                                        FROM TACA_ADSCRIPCION_PER ads
                                        WHERE ads.plp_codnum = plz.codnum
                                        and ads.eje_codnum = plz.eje_codnum)
                        UNION ALL
                        SELECT  prs.codnum CODNUM,
                                TO_NUMBER( NULL ),
                                prs.dniprs IDENTIFICADOR,
                                prs.ll1prs || ' ' || prs.ll2prs|| ', ' || prs.nomprs DESCRIPCION,
                                'S' flgprf,
                                prf.flgext,
                                prs.nomprs nomprs,
                                (   SELECT e.codnum
                                    FROM talu_anyacademic a, taca_ejercicio e
                                    WHERE a.anyaca = e.any_anyaca
                                    AND tipeje = 'E'
                                    AND flgatimat = 'S') eje_codnum,
                                prs.ll1prs ll1prs,
                                prs.ll2prs ll2prs,
                                TO_DATE(NULL) ,
                                PRF.DATFIN ,
                                flgdir,
                                TO_NUMBER( NULL ) PLZ_CODNUM
                        FROM TUIB_PERSONA prs, TUIB_PROFESSOR prf
                        WHERE ( prs.dniprs = prf.prs_dniprs )
                        AND ( prs.codnum = prf.prs_codnum )
                        AND PRF.FLGEXT='S'
                        --
                        -- para incluir profesores que no tengan marcada la casilla de externo, que no tengan contrato en el ejercicio 
                        -- y que tengan contrato en un ejercicio posterior
                        --
                        UNION ALL
                        SELECT  prs.codnum CODNUM,
                                TO_NUMBER(NULL) epe_codnum,
                                prs.dniprs IDENTIFICADOR,
                                prs.ll1prs || ' ' || prs.ll2prs|| ', ' || prs.nomprs DESCRIPCION,
                                'N' flgprf,
                                prf.flgext,
                                prs.nomprs nomprs,
                                (   SELECT e.codnum
                                    FROM talu_anyacademic a, taca_ejercicio e
                                    WHERE a.anyaca = e.any_anyaca
                                    AND tipeje = 'E'
                                    AND flgatimat = 'S') eje_codnum,
                                prs.ll1prs ll1prs,
                                prs.ll2prs ll2prs,
                                TO_DATE(NULL) datalt,
                                PRF.DATFIN datamt,
                                flgdir flgdir,
                                TO_NUMBER(NULL) PLZ_CODNUM
                        FROM tuib_persona prs, tuib_professor prf                  
                        WHERE prs.dniprs = prf.prs_dniprs
                        AND prs.codnum = prf.prs_codnum    
                        AND prf.flgext='N'     
                        AND NOT EXISTS (    SELECT 1
                                            FROM taca_adscripcion_per ad1, taca_empleado_per em1
                                            WHERE ad1.epe_codnum = em1.codnum
                                            AND em1.prf_codnum = prf.prs_codnum
                                            AND em1.eje_codnum = (  SELECT e.codnum
                                                                    FROM talu_anyacademic a, taca_ejercicio e
                                                                    WHERE a.anyaca = e.any_anyaca
                                                                    AND tipeje = 'E'
                                                                    AND flgatimat = 'S')
                                        )
                        AND  EXISTS (   SELECT 1
                                        FROM taca_adscripcion_per ad1, taca_empleado_per em1
                                        WHERE ad1.epe_codnum = em1.codnum
                                        AND em1.prf_codnum = prf.prs_codnum
                                        AND em1.eje_codnum >= 	(   SELECT e.codnum
                                                                    FROM talu_anyacademic a, taca_ejercicio e
                                                                    WHERE a.anyaca = e.any_anyaca
                                                                    AND tipeje = 'E'
                                                                    AND flgatimat = 'S')
                                    )
                     ) L,
                    TACA_PERIODO_IMPARTICION PI,
                    TACA_IMPARTICION I
                WHERE   I.EJE_CODNUM = PI.EJE_CODNUM AND
                        I.HOR_CODNUM = PI.HOR_CODNUM AND
                        I.PER_CODNUM = PI.PER_CODNUM AND
                        I.PEI_CODNUM = PI.CODNUM AND
                        -- PROFESORES
                        (L.CODNUM = I.PRF_CODNUM OR
                        -- PLAZAS
                        L.PLZ_CODNUM = I.PLP_CODNUM) AND
                        L.EJE_CODNUM = I.EJE_CODNUM
                GROUP BY I.HOR_CODNUM, I.PER_CODNUM, I.DSM_CODNUM, I.FRN_CODNUM,
                         L.CODNUM, L.IDENTIFICADOR, L.DESCRIPCION, L.FLGPRF, L.FLGEXT,
                         L.NOMPRS, L.EJE_CODNUM, L.LL1PRS, L.LL2PRS, L.PLZ_CODNUM,I.PCTREP,
                         I.FLGPAL, PI.DATINI, PI.DATFIN, PI.CODNUM, I.CODNUM
             ) vlp
        where tfc.hor_codnum = vlp.hor_codnum
        and tfc.eje_codnum = vlp.eje_codnum
        and tfc.per_codnum = vlp.per_codnum
        and tfc.dsm_codnum = vlp.dsm_codnum
        and tfc.codnum = vlp.frn_codnum
    ) t2,
    (   SELECT  g.codnum GAC_CODNUM,
                ac.codnum VAC_CODNUM,
                p.eje_codnum EJE_CODNUM,
                p.hor_codnum HOR_CODNUM,
                p.codnum PER_CODNUM,
                p.datini DATINI,
                p.datfin DATFIN,
                p.observ NOMPER
        FROM taca_grp_activ g, taca_vig_activ ac, taca_cpsgachor gh, taca_periodo p
        WHERE g.eje_codnum = gh.eje_codnum
        AND g.vac_codnum = gh.vac_codnum
        AND g.codnum = gh.gac_codnum
        AND g.eje_codnum = ac.eje_codnum
        AND g.vac_codnum = ac.codnum
        AND gh.vac_codnum = ac.codnum
        AND gh.eje_codnum = ac.eje_codnum
        AND gh.hor_codnum = p.hor_codnum
        AND gh.eje_codnum = p.eje_codnum
    ) c,
    talu_assignatura ta, TUIB_AULA tal, tuib_edifici edi, talu_centre tc
where t1.vac_codnum = c.vac_codnum
and t1.eje_codnum= c.eje_codnum
and t1.gac_codnum = c.gac_codnum
and c.eje_codnum = t2.eje_codnum
and t1.eje_codnum = t2.eje_codnum
and c.hor_codnum = t2.hor_codnum
and t1.id_assignatura = ta.id_assignatura
and tal.codnum(+) = t2.aul_codnum
and tal.edi_codnum = edi.codnum(+)
and edi.cpu_codalf = tc.codnum(+)
and  t1.any_anyaca= '{periodo}'
AND SUBSTR(t1.id_grp_activ, 1, 1) = '{recinto}'";
                List<CargaGetDto> cargaLista = new List<CargaGetDto>();
                var content = new StringContent(query, Encoding.UTF8, "application/sql");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", reponseToken.access_token);
                HttpResponseMessage response = await client.PostAsync(consultaUrl, content);
                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    ResponseApiUniversitas.Application cargaOfDocente = JsonConvert.DeserializeObject<ResponseApiUniversitas.Application>(jsonResponse);
                    var codigosPosGrado = await _dataContext.Codigos.Where(c => c.IdPrograma == 2).ToListAsync();

                    foreach (var carga in cargaOfDocente.items)
                    {
                        foreach (var item in carga.resultSet.items)
                        {
                            //Valida que el codigo sea de posgrado
                            if (codigosPosGrado.FirstOrDefault(c => c.Codigo1 == (item.nomid1.Substring(0, 7).ToString())) == null)
                            {
                                var cargaMap = _mapper.Map<CargaGetDto>(item);
                                cargaMap.Seccion = int.Parse(item.id_grp_activ.ToString()[1].ToString());
                                // validacion para que haya carga duplicada
                                if (cargaLista.Where(c => c.dia_id == cargaMap.dia_id
                                && c.hora_inicio == cargaMap.hora_inicio
                                && c.minuto_inicio == cargaMap.minuto_inicio
                                && c.hora_fin == cargaMap.hora_fin
                                && c.minuto_fin == cargaMap.minuto_fin
                                && c.Cedula == cargaMap.Cedula
                                && c.Periodo == cargaMap.Periodo
                                ).FirstOrDefault() == null)
                                {
                                    cargaLista.Add(cargaMap);

                                }
                            }
                        }
                    }
                    //var DocentesLimpio = await CleanData(docentesApi, 1);
                    return new ServiceResponseData<List<CargaGetDto>>() { Data = cargaLista, Status = 200 };
                }
                return new ServiceResponseData<List<CargaGetDto>>() { Status = 500, Data = new List<CargaGetDto>() };
            }
            catch (Exception ex)
            {
                return new ServiceResponseData<List<CargaGetDto>>() { Status = 500, Message = ex.ToString() };
            }
        }

        public async Task<ServiceResponseData<List<CargaGetDto>>> SincronizarCarga(string periodo, int recinto)
        {
            try
            {
                var docenteRecinto = await _docenteService.GetAllRecinto(new FiltroDocentesDto(), recinto);
                var cargUniversitas = await GetCargaUniversitas(periodo, recinto);
                var periodoDb = await _dataContext.PeriodoAcademicos.Where(c => c.Periodo == periodo).FirstOrDefaultAsync();
                // Buscamos la carga de este periodo y la eliminamos para insertar la nueva
                var cargaUniversitasEnAkademick = await _dataContext.CargaDocentes.Where(c => c.Periodo == periodo && c.CodUniversitas != "N/A" && c.Recinto == recinto).ToListAsync();
                if (cargaUniversitasEnAkademick != null)
                {
                    _dataContext.CargaDocentes.RemoveRange(cargaUniversitasEnAkademick);
                    await _dataContext.SaveChangesAsync();
                }
                if (cargUniversitas.Status != 200)
                    return new ServiceResponseData<List<CargaGetDto>>() { Status = cargUniversitas.Status, Message = cargUniversitas.Message };
                if (cargUniversitas.Data != null)
                {

                    foreach (var item in cargUniversitas.Data)
                    {
                        // Este codigo agrega la carga nueva

                        var cargaMap = _mapper.Map<CargaDocente>(item);
                        cargaMap.HoraContratada = docenteRecinto.Data.Where(c => c.identificacion == item.Cedula).FirstOrDefault() != null
                        && docenteRecinto.Data.Where(c => c.identificacion == item.Cedula).FirstOrDefault().tiempoDedicacion == "MT" ? false : true;
                        cargaMap.Periodo = periodoDb.Periodo;
                        cargaMap.IdPeriodo = periodoDb.Id;
                        cargaMap.Aula = cargaMap.Aula == null ? "" : cargaMap.Aula.ToString();
                        cargaMap.IdTipoReporte = 1;
                        cargaMap.IdTipoReporteIrregular = 4;
                        cargaMap.IsAuth = true;
                        _dataContext.CargaDocentes.Add(cargaMap);
                    }
                    await _dataContext.SaveChangesAsync();
                }
                return new ServiceResponseData<List<CargaGetDto>>() { Data = cargUniversitas.Data, Status = 200 };

            }
            catch (Exception ex)
            {

                return new ServiceResponseData<List<CargaGetDto>>() { Status = 500, Message = ex.ToString() };

            }
        }

        public async Task<ServicesResponseMessage<string>> ChangeCarga(string cedula, string profesor, int idCaga)
        {
            try
            {
                var carga = await _dataContext.CargaDocentes.AsNoTracking().FirstOrDefaultAsync(c => c.Id == idCaga);
                if (carga != null)
                {
                    carga.NombreProfesor = profesor;
                    carga.Cedula = cedula;
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

        public async Task<ServiceResponseData<List<CargaGetVerificacionDto>>> GetCargaAkadeicWithUniversitas(string periodo, int recinto)
        {
            try
            {
                var docenteRecinto = await _docenteService.GetAllRecinto(new FiltroDocentesDto(), recinto);
                var listCargaWithDocenteDone = new List<CargaGetVerificacionDto>();
                var cargaUniversitas = await GetCargaUniversitas(periodo, recinto);
                var recintoDb = await _dataContext.Recintos.ToListAsync();
                var modalidadesDb = await _dataContext.TipoModalidads.ToListAsync();
                var dias = await _dataContext.Dias.ToListAsync();

                foreach (var docente in docenteRecinto.Data)
                {
                    var cargaUniversitasAkedemic = await GetCarga(docente.identificacion, periodo, 1, docenteRecinto.Data);
                    if (cargaUniversitas == null || cargaUniversitasAkedemic == null || docenteRecinto == null)
                        return new ServiceResponseData<List<CargaGetVerificacionDto>>() { Status = 400, Message = Msj.ServicesDawm };

                    var docenteDone = new CargaGetVerificacionDto { CargaUniversitas = new List<CargaGetDto>(), CargaUniversitasAkadeimc = new List<CargaGetDto>() };
                    docenteDone.Docente = docente;
                    docenteDone.CargaUniversitasAkadeimc = cargaUniversitasAkedemic.Data.Value.Item1.Carga.Where(c => c.CodUniversitas != "N/A").ToList();

                    var cargaDocenteByCedula = cargaUniversitas.Data.Where(c => c.Cedula == docente.identificacion).ToList();
                    foreach (var item in cargaDocenteByCedula)
                    {
                        item.Aula = item.Aula == null ? "" : item.Aula;
                        item.TipoModalidad = new TipoModalidadDto();
                        item.dia_nombre = dias.Where(c => c.Id == item.dia_id).First().Nombre;
                        item.TipoModalidad = _mapper.Map<TipoModalidadDto>(modalidadesDb.First(c => c.Id == item.Modalidad));
                        item.RecintoNombre = recintoDb.First(c => c.Id == int.Parse(item.Recinto)).NombreCorto;
                        item.isEqual = docenteDone.CargaUniversitasAkadeimc.Any(c => c.Recinto == item.Recinto
                        && c.cod_asignatura == item.cod_asignatura && c.Aula == item.Aula
                        && c.Seccion == item.Seccion
                        && c.Modalidad == item.Modalidad
                        && c.dia_id == item.dia_id
                        && c.hora_inicio == item.hora_inicio
                        && c.minuto_inicio == item.minuto_inicio
                        && c.hora_fin == item.hora_fin
                        && c.minuto_fin == item.minuto_fin);
                        docenteDone.CargaUniversitas.Add(item);
                    }

                    if (docenteDone.CargaUniversitas != null && docenteDone.CargaUniversitas.Count > 0)
                    {
                        docenteDone.CargaUniversitas = docenteDone.CargaUniversitas.OrderBy(c => c.dia_id).ThenBy(c => int.Parse(c.hora_inicio)).ToList();
                        docenteDone.CargaUniversitasAkadeimc = docenteDone.CargaUniversitasAkadeimc.OrderBy(c => c.dia_id).ThenBy(c => int.Parse(c.hora_inicio)).ToList();
                        listCargaWithDocenteDone.Add(docenteDone);
                    }
                }
                return new ServiceResponseData<List<CargaGetVerificacionDto>>() { Status = 200, Message = Msj.MsjUpdate, Data = listCargaWithDocenteDone };
            }
            catch (Exception ex)
            {
                return new ServiceResponseData<List<CargaGetVerificacionDto>>() { Status = 500, Message = Msj.MsjError + ex.ToString() };
            }
        }

        public async Task<ServiceResponseData<List<TipoReporteGetDto>>> GetTipoReporte()
        {
            try
            {
                var tipoReporteDb = await _dataContext.TipoReportes.ProjectTo<TipoReporteGetDto>(_mapper.ConfigurationProvider).ToListAsync();
                return new ServiceResponseData<List<TipoReporteGetDto>>() { Data = tipoReporteDb.OrderBy(c => c.Nombre).ToList(), Status = 200, Message = Msj.MsjSucces };
            }
            catch (Exception ex)
            {
                return new ServiceResponseData<List<TipoReporteGetDto>>() { Status = 500, Message = Msj.MsjError + ex.ToString() };
            }
        }

        public async Task<ServiceResponseData<List<TipoReporteIrregularGetDto>>> GetTipoReporteIrregular()
        {
            try
            {
                var dipoReporteIrregular = await _dataContext.TipoReporteIrregulars.ProjectTo<TipoReporteIrregularGetDto>(_mapper.ConfigurationProvider).ToListAsync();
                return new ServiceResponseData<List<TipoReporteIrregularGetDto>>() { Data = dipoReporteIrregular.OrderBy(c => c.Nombre).ToList(), Status = 200, Message = Msj.MsjSucces };
            }
            catch (Exception ex)
            {
                return new ServiceResponseData<List<TipoReporteIrregularGetDto>>() { Status = 500, Message = Msj.MsjError + ex.ToString() };
            }
        }

        public async Task<ServiceResponseData<NotaCargaIrregularDto>> InsertNotaCargaIrregular(NotaCargaIrregularDto nota)
        {
            try
            {
                EntityEntry<NotasCargaIrregular> entity = _dataContext.NotasCargaIrregulars.Add(_mapper.Map<NotasCargaIrregular>(nota));
                await _dataContext.SaveChangesAsync();

                return new ServiceResponseData<NotaCargaIrregularDto>() { Data = _mapper.Map<NotaCargaIrregularDto>(entity.Entity), Status = 200, Message = Msj.MsjSucces };
            }
            catch (Exception ex)
            {
                return new ServiceResponseData<NotaCargaIrregularDto>() { Status = 500, Message = Msj.MsjError + ex.ToString() };
            }
        }

        public async Task<ServiceResponseData<NotaCargaIrregularDto>> UpdateNotaCargaIrregular(NotaCargaIrregularDto nota)
        {
            try
            {
                var notaDb = await _dataContext.NotasCargaIrregulars.AsNoTracking().FirstOrDefaultAsync(c => c.Id == nota.Id);
                _dataContext.Entry(_mapper.Map<NotasCargaIrregular>(nota)).State = EntityState.Modified;
                await _dataContext.SaveChangesAsync();
                return new ServiceResponseData<NotaCargaIrregularDto>() { Data = _mapper.Map<NotaCargaIrregularDto>(nota), Status = 200, Message = Msj.MsjUpdate };
            }
            catch (Exception ex)
            {
                return new ServiceResponseData<NotaCargaIrregularDto>() { Status = 500, Message = Msj.MsjError + ex.ToString() };
            }
        }

        public async Task<ServiceResponseData<List<MesGetDto>>> GetMesesByCuatrimestre(int cuatrimestre)
        {
            try
            {
                var meses = await _dataContext.Meses.Where(c => c.Cuatrimestre == cuatrimestre).ToListAsync();

                return new ServiceResponseData<List<MesGetDto>>() { Data = _mapper.Map<List<MesGetDto>>(meses), Status = 200, Message = Msj.MsjSucces };
            }
            catch (Exception ex)
            {
                return new ServiceResponseData<List<MesGetDto>>() { Status = 500, Message = Msj.MsjError + ex.ToString() };
            }
        }

        public async Task<ServiceResponseData<CargaGetDto>> AutorizarCarga(List<AuthCargaDto> Cargas)
        {
            try
            {
                foreach (var carga in Cargas)
                {
                    var cargaDb = await _dataContext.CargaDocentes.AsNoTracking().FirstAsync(c => c.Id == carga.IdCarga);
                    cargaDb.IsAuth = cargaDb.IsAuth == true ? false : true;
                    _dataContext.Entry(cargaDb).State = EntityState.Modified;
                    await _dataContext.SaveChangesAsync();
                }
                return new ServiceResponseData<CargaGetDto>() { Status = 200, Message = Msj.MsjUpdate };

            }
            catch (Exception ex)
            {

                return new ServiceResponseData<CargaGetDto>() { Status = 500, Message = Msj.MsjError + ex.ToString() };

            }
        }
    }
}
