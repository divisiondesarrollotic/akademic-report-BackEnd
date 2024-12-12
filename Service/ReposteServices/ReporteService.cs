using AkademicReport.Data;
using AkademicReport.Dto.AsignaturaDto;
using AkademicReport.Dto.CargaDto;
using AkademicReport.Dto.ConceptoDto;
using AkademicReport.Dto.DocentesDto;
using AkademicReport.Dto.PeriodoDto;
using AkademicReport.Dto.ReporteDto;
using AkademicReport.Models;
using AkademicReport.Service.CargaServices;
using AkademicReport.Service.DocenteServices;
using AkademicReport.Utilities;
using AutoMapper;
using AutoMapper.Configuration.Conventions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace AkademicReport.Service.ReposteServices
{
    public class ReporteService : IReporteService
    {
        private readonly ICargaDocenteService _cargaService;
        private readonly IDocenteService _docentesService;
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;
        public List<string> CodigosIngles = new List<string> { "ING-201", "ING-302", "ING-403", "ING-504", "ING-605", "IOP-01", "IOP-02", "IOP-03", "ING-220", "ING-100", "ING-110", "ING-200", "ING-210", "FRP-201", "FRP-301", "FRP-601", "FRP-701", "FRP-801", "PVS-300", "PVS-305" };


        public ReporteService(ICargaDocenteService cargaService, DataContext dataContext, IDocenteService docenteService, IMapper mapper)
        {
            _cargaService = cargaService;
            _dataContext = dataContext;
            _docentesService = docenteService;
            _mapper = mapper;
        }


        public async Task<ServiceResponseData<DocenteCargaReporteDto>> PorDocente(ReporteDto filtro, List<DocenteGetDto> DocentesAmilca)
        {
            var Response = new ServiceResponseData<DocenteCargaReporteDto>();
            try
            {
                if (filtro.Periodo != null)
                {
                    var periodo = await _dataContext.PeriodoAcademicos.Where(c => c.Periodo == filtro.Periodo).FirstAsync();
                    Response.Anio = periodo.Anio!.Value;
                }
                DocenteCargaReporteDto DataResult = new DocenteCargaReporteDto();
                DataResult.Carga = new List<CargaReporteDto>();

                var CargaMapeada = new List<CargaReporteDto>();
                var CargaLista = new List<CargaGetDto>();
                var Docente = new DocenteReporteDto();
                var cargas = await _cargaService.GetCarga(filtro.Cedula!, filtro.Periodo!, filtro.idPrograma, DocentesAmilca);
                if (cargas.Data.Value.Item1.Carga.Count > 0)
                {
                    Response.Anio = cargas.Data.Value.Item1.Carga[0].PeriodoObj.Anio.Value;

                }

                if (cargas.Data!.Value.Item1.Docente != null)
                {

                    Docente.Id = cargas.Data.Value.Item1.Docente.id;
                    Docente.TiempoDedicacion = cargas.Data.Value.Item1.Docente.tiempoDedicacion;
                    Docente.Identificacion = cargas.Data.Value.Item1.Docente.identificacion;
                    Docente.Nombre = cargas.Data.Value.Item1.Docente.nombre;
                    Docente.Nacionalidad = cargas.Data.Value.Item1.Docente.nacionalidad;
                    Docente.Sexo = cargas.Data.Value.Item1.Docente.sexo;
                    Docente.Id_vinculo = cargas.Data.Value.Item1.Docente.id_vinculo;
                    Docente.NombreVinculo = cargas.Data.Value.Item1.Docente.nombreVinculo;
                    Docente.Monto = "0";
                    var recinto = await _dataContext.Recintos.FirstOrDefaultAsync(c => c.Id == int.Parse(cargas.Data.Value.Item1.Docente.id_recinto));
                    Docente.Id_recinto = cargas.Data.Value.Item1.Docente.id_recinto;
                    Docente.Recinto = recinto!.Recinto1;
                    Docente.Nombre_corto = cargas.Data.Value.Item1.Docente.nombre_corto;
                    Docente.Id_nivel_academico = cargas.Data.Value.Item1.Docente.id_nivel_academico;
                    Docente.Nivel = cargas.Data.Value.Item1.Docente.nivel;
                    DataResult.Docente = Docente;
                }
                if (cargas.Data.Value.Item1.Carga == null || cargas.Data.Value.Item1.Carga.Count == 0)
                {
                    DataResult.Docente = Docente;
                    Response.Data = DataResult;
                    Response.Message = "El docente no tiene carga en este periodo";
                    Response.Status = 204;
                    return Response;
                }

                if (cargas.Data.Value.Item1.Carga != null)
                {
                    int Monto = 0;
                    if (DataResult.Docente!.TiempoDedicacion == "TC")
                    {
                        var vinculacion = await _dataContext.Vinculos.FirstOrDefaultAsync(c => c.Corto == "TC");

                        Monto = vinculacion!.Monto;
                        DataResult.MontoMensual = Monto;
                        DataResult.MontoVinculacion = Monto;

                        var nivelAcademico = await _dataContext.NivelAcademicos
                            .Where(n => n.Nivel.ToUpper().Replace("á", "a")
                                                .Replace("é", "e")
                                                .Replace("í", "i")
                                                .Replace("ó", "o")
                                                .Replace("ú", "u")
                                                .Contains(Docente.Nivel.ToUpper().Replace("á", "a")
                                                                    .Replace("é", "e")
                                                                    .Replace("í", "i")
                                                                    .Replace("ó", "o")
                                                                    .Replace("ú", "u")))
                            .FirstOrDefaultAsync();
                        foreach (var item in cargas.Data.Value.Item1.Carga)
                        {

                            decimal Horas = CalculoTiempoHoras.Calcular(int.Parse(item.hora_inicio), int.Parse(item.minuto_inicio), int.Parse(item.hora_fin), int.Parse(item.minuto_fin));
                            item.credito = Convert.ToInt32(Horas);
                            CargaLista.Add(item);
                            var c = new CargaReporteDto();
                            c.Aula = item.Aula;
                            c.TiposCarga = item.TiposCarga;
                            c.Periodo = item.Periodo;
                            c.codigo_asignatura = item.cod_asignatura;
                            c.nombre_asignatura = item.nombre_asignatura;
                            c.id = item.Id;
                            c.modalidad = item.TipoModalidad.Nombre;
                            c.CodUniversitas = item.CodUniversitas;
                            c.seccion = item.Seccion;
                            c.Horario_dia = item.dia_nombre;
                            c.Horario_inicio = $"{item.hora_inicio} : {item.minuto_inicio}";
                            c.Horario_final = $"{item.hora_fin} : {item.minuto_fin}";
                            c.credito = item.credito;
                            var recinto = await _dataContext.Recintos.FirstOrDefaultAsync(c => c.Id == int.Parse(item.Recinto));
                            c.recinto = recinto.NombreCorto;
                            c.precio_hora = 0;
                            c.Concepto = item.Concepto;
                            c.pago_asignatura = 0;
                            var periodo = await _dataContext.PeriodoAcademicos.Where(c => c.Periodo == item.Periodo).FirstAsync();
                            c.IdPeriodo = periodo.Id;
                            c.PeriodoObj = _mapper.Map<PeriodoGetDto>(periodo);
                            c.HoraContratada = item.HoraContratada;
                            DataResult.Carga.Add(c);

                        }
                        int CantCreditos = 0;
                        foreach (var item in DataResult.Carga)
                        {
                            CantCreditos += item.credito;
                        }
                        DataResult.CantCreditos = CantCreditos;
                        Response.Data = DataResult;
                        Response.Status = 200;
                        return Response;
                    }
                    else if (DataResult.Docente.TiempoDedicacion == "MT")
                    {
                        var vinculacion = await _dataContext.Vinculos.FirstOrDefaultAsync(c => c.Corto == "MT");
                        DataResult.MontoVinculacion = vinculacion.Monto;
                        //var nivelAcademico = await _dataContext.NivelAcademicos
                        //     .Where(n => n.Nivel.ToUpper().Trim().Replace("á", "a")
                        //                         .Replace("é", "e")
                        //                         .Replace("í", "i")
                        //                         .Replace("ó", "o")
                        //                         .Replace("ú", "u")
                        //                         .Contains(DataResult.Docente.Nivel.ToUpper().Trim().Replace("á", "a")
                        //                                             .Replace("é", "e")
                        //                                             .Replace("í", "i")
                        //                                             .Replace("ó", "o")
                        //                                             .Replace("ú", "u")))
                        //     .FirstOrDefaultAsync();

                        var nivelAcademico = await _dataContext.NivelAcademicos.Where(n => n.Nivel.ToUpper().Trim().Contains(DataResult.Docente.Nivel.ToUpper().Trim())).FirstOrDefaultAsync();
                        DataResult.Docente.Pago_hora = nivelAcademico.PagoHora.ToString();
                        Monto = vinculacion.Monto;
                        int MontoPorAsignatura = 0;
                        int CantCreditosF = 0;
                        int CantidadPagada = 0;
                        Docente.Pago_hora = nivelAcademico.PagoHora.ToString();
                        DataResult.Docente.Pago_hora = nivelAcademico.PagoHora.ToString();

                        foreach (var item in cargas.Data.Value.Item1.Carga)
                        {


                            //if (item.cod_universitas != "N/A" && item.TiposCarga.Nombre == "Curricular" && CantCreditosF < 21)
                            //{
                            //    MontoPorAsignatura = 0;
                            //}

                            //else
                            //{
                            //    MontoPorAsignatura += (item.credito * nivelAcademico.PagoHora);
                            //}

                            var c = new CargaReporteDto();
                            c.Aula = item.Aula;
                            c.precio_hora = nivelAcademico.PagoHora;
                            c.modalidad = item.TipoModalidad.Nombre;
                            c.TiposCarga = item.TiposCarga;
                            c.MontoVinculacion = vinculacion.Monto;
                            c.Vinculacion = Docente.TiempoDedicacion;
                            c.Periodo = item.Periodo;
                            c.codigo_asignatura = item.cod_asignatura;
                            c.nombre_asignatura = item.nombre_asignatura;
                            c.id = item.Id;
                            c.CodUniversitas = item.CodUniversitas;
                            c.seccion = item.Seccion;
                            c.Horario_dia = item.dia_nombre;
                            c.Horario_inicio = $"{item.hora_inicio} : {item.minuto_inicio}";
                            c.Horario_final = $"{item.hora_fin} : {item.minuto_fin}";
                            c.credito = item.credito;
                            c.Concepto = item.Concepto;
                            var periodo = await _dataContext.PeriodoAcademicos.Where(c => c.Periodo == item.Periodo).FirstAsync();
                            c.IdPeriodo = periodo.Id;
                            c.HoraContratada = item.HoraContratada;
                            c.PeriodoObj = _mapper.Map<PeriodoGetDto>(periodo);
                            var recinto = await _dataContext.Recintos.FirstOrDefaultAsync(c => c.Id == int.Parse(item.Recinto));
                            c.recinto = recinto.NombreCorto;

                            if (CantCreditosF + c.credito > 12)
                            {
                                // verificacion por si el monto resultante de la suma en mayor a 20 que solo tome despues de; va;pr 20
                                CantCreditosF += c.credito;
                                if (CantidadPagada == 0)
                                {
                                    CantidadPagada = CantCreditosF - 12;
                                    c.precio_hora = nivelAcademico.PagoHora;
                                    c.pago_asignatura = c.precio_hora * CantidadPagada;
                                    MontoPorAsignatura += c.pago_asignatura;
                                }
                                else
                                {
                                    c.precio_hora = nivelAcademico.PagoHora;
                                    c.pago_asignatura = c.precio_hora * c.credito;
                                    MontoPorAsignatura += c.pago_asignatura;
                                }
                            }
                            else
                            {
                                CantCreditosF += c.credito;
                                c.precio_hora = 0;
                                c.pago_asignatura = 0;
                            }
                            DataResult.Carga.Add(c);
                        }
                        DataResult.CantCreditos = CantCreditosF;
                        DataResult.MontoSemanal = MontoPorAsignatura;
                        DataResult.MontoMensual = Monto + (MontoPorAsignatura * 4);
                        Response.Data = DataResult;
                        Response.Status = 200;
                        return Response;
                    }
                    else if (DataResult.Docente.TiempoDedicacion == "A")
                    {
                        var nivelAcademico = await _dataContext.NivelAcademicos
                             .Where(n => n.Nivel.Replace("á", "a")
                                                 .Replace("é", "e")
                                                 .Replace("í", "i")
                                                 .Replace("ó", "o")
                                                 .Replace("ú", "u")
                                                 .Contains(DataResult.Docente.Nivel.Replace("á", "a")
                                                                     .Replace("é", "e")
                                                                     .Replace("í", "i")
                                                                     .Replace("ó", "o")
                                                                     .Replace("ú", "u")))
                             .FirstOrDefaultAsync();
                        DataResult.Docente.Pago_hora = nivelAcademico.PagoHora.ToString();
                        foreach (var item in cargas.Data.Value.Item1.Carga)
                        {
                            decimal Horas = CalculoTiempoHoras.Calcular(int.Parse(item.hora_inicio), int.Parse(item.minuto_inicio), int.Parse(item.hora_fin), int.Parse(item.minuto_fin));
                            item.credito = Convert.ToInt32(Horas);
                            CargaLista.Add(item);
                            var c = new CargaReporteDto();
                            c.Aula = item.Aula;
                            c.modalidad = item.TipoModalidad.Nombre;
                            c.MontoVinculacion = 0;
                            c.Periodo = item.Periodo;
                            c.TiposCarga = item.TiposCarga;
                            c.Vinculacion = Docente.TiempoDedicacion;
                            c.codigo_asignatura = item.cod_asignatura;
                            c.nombre_asignatura = item.nombre_asignatura;
                            c.id = item.Id;
                            c.CodUniversitas = item.cod_universitas;
                            c.Concepto = item.Concepto;
                            c.seccion = item.Seccion;
                            c.Horario_dia = item.dia_nombre;
                            c.Horario_inicio = $"{item.hora_inicio} : {item.minuto_inicio}";
                            c.Horario_final = $"{item.hora_fin} : {item.minuto_fin}";
                            c.credito = item.credito;
                            var recinto = await _dataContext.Recintos.FirstOrDefaultAsync(c => c.Id == int.Parse(item.Recinto));
                            c.recinto = recinto.NombreCorto;
                            c.precio_hora = nivelAcademico.PagoHora;
                            c.pago_asignatura = c.precio_hora * c.credito;
                            var periodo = await _dataContext.PeriodoAcademicos.Where(c => c.Periodo == item.Periodo).FirstAsync();
                            c.IdPeriodo = periodo.Id;
                            c.HoraContratada = item.HoraContratada;
                            c.PeriodoObj = _mapper.Map<PeriodoGetDto>(periodo);
                            DataResult.Docente.Pago_hora = c.precio_hora.ToString();
                            DataResult.Carga.Add(c);
                        }

                        foreach (var item in CargaLista)
                        {
                            Monto += (item.credito * nivelAcademico.PagoHora);
                        }

                        int CantCreditos = 0;
                        Response.Data = DataResult;
                        foreach (var item in DataResult.Carga)
                        {
                            CantCreditos += item.credito;
                        }
                        DataResult.CantCreditos = CantCreditos;
                        DataResult.MontoSemanal = Monto;
                        DataResult.MontoMensual = Monto * 4;
                        Response.Data = DataResult;
                        Response.Status = 200;
                        return Response;
                    }
                    else if (DataResult.Docente.TiempoDedicacion == "F")
                    {
                        var vinculacion = await _dataContext.Vinculos.FirstOrDefaultAsync(c => c.Corto == "F");
                        DataResult.Docente.Pago_hora = vinculacion.Monto.ToString();
                        foreach (var item in cargas.Data.Value.Item1.Carga)
                        {

                            decimal Horas = CalculoTiempoHoras.Calcular(int.Parse(item.hora_inicio), int.Parse(item.minuto_inicio), int.Parse(item.hora_fin), int.Parse(item.minuto_fin));
                            item.credito = Convert.ToInt32(Horas);
                            CargaLista.Add(item);
                            var c = new CargaReporteDto();
                            c.Aula = item.Aula;
                            c.modalidad = item.TipoModalidad.Nombre;
                            c.MontoVinculacion = vinculacion.Monto;
                            c.Periodo = item.Periodo;
                            c.TiposCarga = item.TiposCarga;
                            c.Vinculacion = Docente.TiempoDedicacion;
                            c.codigo_asignatura = item.cod_asignatura;
                            c.nombre_asignatura = item.nombre_asignatura;
                            c.id = item.Id;
                            c.CodUniversitas = item.CodUniversitas;
                            c.Concepto = item.Concepto;
                            c.seccion = item.Seccion;
                            c.Horario_dia = item.dia_nombre;
                            c.HoraContratada = item.HoraContratada;
                            c.Horario_inicio = $"{item.hora_inicio} : {item.minuto_inicio}";
                            c.Horario_final = $"{item.hora_fin} : {item.minuto_fin}";
                            c.credito = item.credito;
                            var recinto = await _dataContext.Recintos.FirstOrDefaultAsync(c => c.Id == int.Parse(item.Recinto));
                            c.recinto = recinto.NombreCorto;
                            c.precio_hora = vinculacion.Monto;
                            var periodo = await _dataContext.PeriodoAcademicos.Where(c => c.Periodo == item.Periodo).FirstAsync();
                            c.IdPeriodo = periodo.Id;
                            c.PeriodoObj = _mapper.Map<PeriodoGetDto>(periodo);
                            c.pago_asignatura = c.precio_hora * c.credito;
                            DataResult.Docente.Pago_hora = c.precio_hora.ToString();
                            DataResult.Carga.Add(c);


                        }

                        foreach (var item in CargaLista)
                        {
                            Monto += (item.credito * vinculacion.Monto);
                        }

                        int CantCreditos = 0;
                        Response.Data = DataResult;
                        Response.Data.MontoVinculacion = vinculacion.Monto;
                        foreach (var item in DataResult.Carga)
                        {
                            CantCreditos += item.credito;
                        }

                        DataResult.CantCreditos = CantCreditos;
                        DataResult.MontoSemanal = Monto;
                        DataResult.MontoMensual = Monto * 4;

                        Response.Data = DataResult;
                        Response.Status = 200;
                        return Response;
                    }
                    else if (DataResult.Docente.TiempoDedicacion == "M")
                    {
                        var vinculacion = await _dataContext.Vinculos.FirstOrDefaultAsync(c => c.Corto == "M");


                        DataResult.Docente.Pago_hora = vinculacion.Monto.ToString();
                        foreach (var item in cargas.Data.Value.Item1.Carga)
                        {

                            decimal Horas = CalculoTiempoHoras.Calcular(int.Parse(item.hora_inicio), int.Parse(item.minuto_inicio), int.Parse(item.hora_fin), int.Parse(item.minuto_fin));
                            item.credito = Convert.ToInt32(Horas);
                            CargaLista.Add(item);


                            var c = new CargaReporteDto();
                            c.Aula = item.Aula;
                            c.modalidad = item.TipoModalidad.Nombre;
                            c.MontoVinculacion = vinculacion.Monto;
                            c.Periodo = item.Periodo;
                            c.Vinculacion = Docente.TiempoDedicacion;
                            c.TiposCarga = item.TiposCarga;
                            c.codigo_asignatura = item.cod_asignatura;
                            c.nombre_asignatura = item.nombre_asignatura;
                            c.id = item.Id;
                            c.CodUniversitas = item.CodUniversitas;
                            c.Concepto = item.Concepto;
                            c.seccion = item.Seccion;
                            c.HoraContratada = item.HoraContratada;
                            c.Horario_dia = item.dia_nombre;
                            c.Horario_inicio = $"{item.hora_inicio} : {item.minuto_inicio}";
                            c.Horario_final = $"{item.hora_fin} : {item.minuto_fin}";
                            c.credito = item.credito;
                            var recinto = await _dataContext.Recintos.FirstOrDefaultAsync(c => c.Id == int.Parse(item.Recinto));
                            c.recinto = recinto.NombreCorto;
                            c.precio_hora = vinculacion.Monto;
                            DataResult.Docente.Pago_hora = c.precio_hora.ToString();
                            c.pago_asignatura = c.precio_hora * c.credito;
                            var periodo = await _dataContext.PeriodoAcademicos.Where(c => c.Periodo == item.Periodo).FirstAsync();
                            c.IdPeriodo = periodo.Id;
                            c.PeriodoObj = _mapper.Map<PeriodoGetDto>(periodo);
                            DataResult.Carga.Add(c);
                        }

                        foreach (var item in CargaLista)
                        {
                            Monto += (item.credito * vinculacion.Monto);
                        }

                        int CantCreditos = 0;
                        Response.Data = DataResult;
                        foreach (var item in DataResult.Carga)
                        {
                            CantCreditos += item.credito;
                        }
                        Response.Data.MontoVinculacion = vinculacion.Monto;
                        DataResult.CantCreditos = CantCreditos;
                        DataResult.MontoSemanal = Monto;
                        DataResult.MontoMensual = Monto * 4;
                        Response.Status = 200;
                        return Response;
                    }

                }
                return Response;
            }
            catch (Exception ex)
            {

                Response.Status = 400;
                Response.Message = ex.ToString();
                string error = ex.ToString();
                return Response;

            }


        }

        //public async Task<ServiceResponseData<DocenteCargaReporteDto>> PorDocente(ReporteDto filtro, List<DocenteGetDto> DocentesAmilca)
        //{
        //    DocenteCargaReporteDto DataResult = new DocenteCargaReporteDto();
        //    DataResult.Carga = new List<CargaReporteDto>();
        //    var Response = new ServiceResponseData<DocenteCargaReporteDto>();
        //    var CargaMapeada = new List<CargaReporteDto>();
        //    var CargaLista = new List<CargaGetDto>();
        //    var Docente = new DocenteReporteDto();
        //    var cargas = await _cargaService.GetCarga(filtro.Cedula, filtro.Periodo, DocentesAmilca);
        //    if (cargas.Data.Value.Item1.Docente != null)
        //    {
        //        Docente.Id = cargas.Data.Value.Item1.Docente.id;
        //        Docente.TiempoDedicacion = cargas.Data.Value.Item1.Docente.tiempoDedicacion;
        //        Docente.Identificacion = cargas.Data.Value.Item1.Docente.identificacion;
        //        Docente.Nombre = cargas.Data.Value.Item1.Docente.nombre;
        //        Docente.Nacionalidad = cargas.Data.Value.Item1.Docente.nacionalidad;
        //        Docente.Sexo = cargas.Data.Value.Item1.Docente.sexo;
        //        Docente.Id_vinculo = cargas.Data.Value.Item1.Docente.id_vinculo;
        //        Docente.NombreVinculo = cargas.Data.Value.Item1.Docente.nombreVinculo;
        //        Docente.Monto = "0";
        //        var recinto = await _dataContext.Recintos.FirstOrDefaultAsync(c => c.Id == int.Parse(cargas.Data.Value.Item1.Docente.id_recinto));
        //        Docente.Id_recinto = cargas.Data.Value.Item1.Docente.id_recinto;
        //        Docente.Recinto = recinto.Recinto1;
        //        Docente.Nombre_corto = cargas.Data.Value.Item1.Docente.nombre_corto;
        //        Docente.Id_nivel_academico = cargas.Data.Value.Item1.Docente.id_nivel_academico;
        //        Docente.Nivel = cargas.Data.Value.Item1.Docente.nivel;
        //        DataResult.Docente = Docente;

        //    }
        //    if (cargas.Data.Value.Item1.Carga == null)
        //    {
        //        DataResult.Docente = Docente;
        //        Response.Data = DataResult;
        //        Response.Status = 204;
        //        return Response;
        //    }

        //    if (cargas.Data.Value.Item1.Carga != null)
        //    {
        //        int Monto = 0;
        //        if (DataResult.Docente.TiempoDedicacion == "TC")
        //        {
        //            var vinculacion = await _dataContext.Vinculos.FirstOrDefaultAsync(c => c.Corto == "TC");
        //            Monto = vinculacion.Monto;
        //            DataResult.Monto = Monto;

        //            var nivelAcademico = await _dataContext.NivelAcademicos
        //                .Where(n => n.Nivel.ToUpper().Replace("á", "a")
        //                                    .Replace("é", "e")
        //                                    .Replace("í", "i")
        //                                    .Replace("ó", "o")
        //                                    .Replace("ú", "u")
        //                                    .Contains(Docente.Nivel.ToUpper().Replace("á", "a")
        //                                                        .Replace("é", "e")
        //                                                        .Replace("í", "i")
        //                                                        .Replace("ó", "o")
        //                                                        .Replace("ú", "u")))
        //                .FirstOrDefaultAsync();
        //            DataResult.Docente.Pago_hora = nivelAcademico.PagoHora.ToString();
        //            foreach (var item in cargas.Data.Value.Item1.Carga)
        //            {
        //                var existe = CargaLista.Where(c => c.cod_asignatura == item.cod_asignatura && c.Seccion == item.Seccion).FirstOrDefault();
        //                if (existe == null)
        //                {
        //                    CargaLista.Add(item);
        //                }
        //                else if (existe.cod_asignatura[0].ToString() == "P" && existe.cod_asignatura[1].ToString() == "D")
        //                {
        //                    if (CargaLista.Where(c => c.cod_asignatura == item.cod_asignatura && c.Seccion == item.Seccion && c.dia_id == item.dia_id && c.hora_inicio == item.hora_inicio && c.minuto_inicio == item.minuto_inicio && c.hora_fin == item.hora_fin && c.minuto_fin == item.minuto_fin && c.Aula == item.Aula).FirstOrDefault() == null)
        //                    {
        //                        CargaLista.Add(item);
        //                    }
        //                    else
        //                    {
        //                        item.credito = 0;
        //                        CargaLista.Add(item);
        //                    }
        //                }
        //                else
        //                {
        //                    item.credito = 0;
        //                    CargaLista.Add(item);
        //                }

        //                var c = new CargaReporteDto();
        //                c.TiposCarga = item.TiposCarga;
        //                c.Periodo = item.Periodo;
        //                c.codigo_asignatura = item.cod_asignatura;
        //                c.nombre_asignatura = item.nombre_asignatura;
        //                c.id = item.Id;
        //                c.seccion = item.Seccion;
        //                c.Horario_dia = item.dia_nombre;
        //                c.Horario_inicio = $"{item.hora_inicio} : {item.minuto_inicio}";
        //                c.Horario_final = $"{item.hora_fin} : {item.minuto_fin}";
        //                c.credito = item.credito;
        //                var recinto = await _dataContext.Recintos.FirstOrDefaultAsync(c => c.Id == int.Parse(item.Recinto));
        //                c.recinto = recinto.NombreCorto;
        //                c.precio_hora = 0;
        //                c.pago_asignatura = 0;
        //                DataResult.Carga.Add(c);

        //            }
        //            int CantCreditos = 0;
        //            foreach (var item in DataResult.Carga)
        //            {
        //                CantCreditos += item.credito;
        //            }
        //            DataResult.CantCreditos = CantCreditos;

        //            Response.Data = DataResult;
        //            Response.Status = 200;

        //            return Response;
        //        }
        //        else if (DataResult.Docente.TiempoDedicacion == "MT")
        //        {
        //            var vinculacion = await _dataContext.Vinculos.FirstOrDefaultAsync(c => c.Corto == "MT");
        //            var nivelAcademico = await _dataContext.NivelAcademicos
        //                 .Where(n => n.Nivel.ToUpper().Replace("á", "a")
        //                                     .Replace("é", "e")
        //                                     .Replace("í", "i")
        //                                     .Replace("ó", "o")
        //                                     .Replace("ú", "u")
        //                                     .Contains(DataResult.Docente.Nivel.ToUpper().Replace("á", "a")
        //                                                         .Replace("é", "e")
        //                                                         .Replace("í", "i")
        //                                                         .Replace("ó", "o")
        //                                                         .Replace("ú", "u")))
        //                 .FirstOrDefaultAsync();
        //            DataResult.Docente.Pago_hora = nivelAcademico.PagoHora.ToString();

        //            Monto = vinculacion.Monto;
        //            int MontoPorAsignatura = 0;
        //            int CantCreditosF = 0;
        //            cargas.Data.Value.Item1.Carga.ForEach(c => CantCreditosF += c.credito);
        //            foreach (var item in cargas.Data.Value.Item1.Carga)
        //            {
        //                var CargaExistente = cargas.Data.Value.Item1.Carga.Where(c => c.cod_asignatura == item.cod_asignatura && c.Seccion == item.Seccion && c.cod_universitas == item.cod_universitas).ToList();
        //                if (CargaExistente.Count > 1)
        //                {
        //                    if (item.cod_universitas != "N/A" || item.TiposCarga.Nombre == "Curricular" && CantCreditosF < 21)
        //                    {
        //                        MontoPorAsignatura = 0;
        //                    }
        //                    else
        //                    {
        //                        MontoPorAsignatura += (CargaExistente[0].credito * nivelAcademico.PagoHora);
        //                    }
        //                }
        //                else
        //                {
        //                    if (item.cod_universitas != "N/A" || item.TiposCarga.Nombre == "Curricular" && CantCreditosF < 21)
        //                    {
        //                        MontoPorAsignatura = 0;
        //                    }
        //                    else
        //                    {
        //                        MontoPorAsignatura += (item.credito * nivelAcademico.PagoHora);
        //                    }

        //                }
        //                var c = new CargaReporteDto();
        //                c.TiposCarga = item.TiposCarga;
        //                c.Periodo = item.Periodo;
        //                c.codigo_asignatura = item.cod_asignatura;
        //                c.nombre_asignatura = item.nombre_asignatura;
        //                c.id = item.Id;
        //                c.seccion = item.Seccion;
        //                c.Horario_dia = item.dia_nombre;
        //                c.Horario_inicio = $"{item.hora_inicio} : {item.minuto_inicio}";
        //                c.Horario_final = $"{item.hora_fin} : {item.minuto_fin}";
        //                c.credito = item.credito;
        //                var recinto = await _dataContext.Recintos.FirstOrDefaultAsync(c => c.Id == int.Parse(item.Recinto));
        //                c.recinto = recinto.NombreCorto;
        //                if (item.cod_universitas != "N/A" || item.TiposCarga.Nombre == "Curricular")
        //                {
        //                    c.precio_hora = 0;
        //                    c.pago_asignatura = 0;
        //                }
        //                else
        //                {
        //                    c.precio_hora = nivelAcademico.PagoHora;
        //                    c.pago_asignatura = c.precio_hora * c.credito;
        //                }

        //                DataResult.Carga.Add(c);

        //            }
        //            int CantCreditos = 0;
        //            foreach (var item in DataResult.Carga)
        //            {
        //                CantCreditos += item.credito;
        //            }
        //            DataResult.CantCreditos = CantCreditos;
        //            DataResult.Monto = Monto + (MontoPorAsignatura * 4);
        //            Response.Data = DataResult;

        //            Response.Status = 200;
        //            return Response;
        //        }
        //        else if (DataResult.Docente.TiempoDedicacion == "A")
        //        {

        //            var nivelAcademico = await _dataContext.NivelAcademicos
        //                 .Where(n => n.Nivel.Replace("á", "a")
        //                                     .Replace("é", "e")
        //                                     .Replace("í", "i")
        //                                     .Replace("ó", "o")
        //                                     .Replace("ú", "u")
        //                                     .Contains(DataResult.Docente.Nivel.Replace("á", "a")
        //                                                         .Replace("é", "e")
        //                                                         .Replace("í", "i")
        //                                                         .Replace("ó", "o")
        //                                                         .Replace("ú", "u")))
        //                 .FirstOrDefaultAsync();

        //            DataResult.Docente.Pago_hora = nivelAcademico.PagoHora.ToString();
        //            foreach (var item in cargas.Data.Value.Item1.Carga)
        //            {
        //                var existe = CargaLista.Where(c => c.cod_asignatura == item.cod_asignatura && c.Seccion == item.Seccion && c.cod_universitas == item.cod_universitas).FirstOrDefault();
        //                if (existe == null)
        //                {
        //                    CargaLista.Add(item);
        //                }
        //                else if (existe.cod_asignatura[0].ToString() == "P" && existe.cod_asignatura[1].ToString() == "D")
        //                {
        //                    if (CargaLista.Where(c => c.cod_asignatura == item.cod_asignatura && c.Seccion == item.Seccion && c.dia_id == item.dia_id && c.hora_inicio == item.hora_inicio && c.minuto_inicio == item.minuto_inicio && c.hora_fin == item.hora_fin && c.minuto_fin == item.minuto_fin && c.Aula == item.Aula).FirstOrDefault() == null)
        //                    {
        //                        CargaLista.Add(item);
        //                    }
        //                    else
        //                    {
        //                        item.credito = 0;
        //                        CargaLista.Add(item);

        //                    }

        //                }
        //                else
        //                {
        //                    item.credito = 0;
        //                    CargaLista.Add(item);
        //                }

        //                var c = new CargaReporteDto();
        //                c.Periodo = item.Periodo;
        //                c.TiposCarga = item.TiposCarga;
        //                c.codigo_asignatura = item.cod_asignatura;
        //                c.nombre_asignatura = item.nombre_asignatura;
        //                c.id = item.Id;
        //                c.seccion = item.Seccion;
        //                c.Horario_dia = item.dia_nombre;
        //                c.Horario_inicio = $"{item.hora_inicio} : {item.minuto_inicio}";
        //                c.Horario_final = $"{item.hora_fin} : {item.minuto_fin}";
        //                c.credito = item.credito;
        //                var recinto = await _dataContext.Recintos.FirstOrDefaultAsync(c => c.Id == int.Parse(item.Recinto));
        //                c.recinto = recinto.NombreCorto;
        //                c.precio_hora = nivelAcademico.PagoHora;
        //                c.pago_asignatura = c.precio_hora * c.credito;
        //                DataResult.Carga.Add(c);


        //            }

        //            foreach (var item in CargaLista)
        //            {
        //                Monto += (item.credito * nivelAcademico.PagoHora);
        //            }

        //            int CantCreditos = 0;
        //            Response.Data = DataResult;
        //            foreach (var item in DataResult.Carga)
        //            {
        //                CantCreditos += item.credito;
        //            }

        //            DataResult.CantCreditos = CantCreditos;
        //            DataResult.Monto = Monto;

        //            Response.Data = DataResult;
        //            Response.Status = 200;
        //            return Response;
        //        }
        //        else if (DataResult.Docente.TiempoDedicacion == "F")
        //        {
        //            var vinculacion = await _dataContext.Vinculos.FirstOrDefaultAsync(c => c.Corto == "F");


        //            DataResult.Docente.Pago_hora = vinculacion.Monto.ToString();
        //            foreach (var item in cargas.Data.Value.Item1.Carga)
        //            {
        //                var existe = CargaLista.Where(c => c.cod_asignatura == item.cod_asignatura && c.Seccion == item.Seccion && c.cod_universitas == item.cod_universitas).FirstOrDefault();
        //                if (existe == null)
        //                {
        //                    CargaLista.Add(item);
        //                }
        //                else if (existe.cod_asignatura[0].ToString() == "P" && existe.cod_asignatura[1].ToString() == "D")
        //                {
        //                    if (CargaLista.Where(c => c.cod_asignatura == item.cod_asignatura && c.Seccion == item.Seccion && c.dia_id == item.dia_id && c.hora_inicio == item.hora_inicio && c.minuto_inicio == item.minuto_inicio && c.hora_fin == item.hora_fin && c.minuto_fin == item.minuto_fin && c.Aula == item.Aula).FirstOrDefault() == null)
        //                    {
        //                        CargaLista.Add(item);
        //                    }
        //                    else
        //                    {
        //                        item.credito = 0;
        //                        CargaLista.Add(item);

        //                    }

        //                }
        //                else
        //                {
        //                    item.credito = 0;
        //                    CargaLista.Add(item);
        //                }

        //                var c = new CargaReporteDto();
        //                c.Periodo = item.Periodo;
        //                c.TiposCarga = item.TiposCarga;
        //                c.codigo_asignatura = item.cod_asignatura;
        //                c.nombre_asignatura = item.nombre_asignatura;
        //                c.id = item.Id;
        //                c.seccion = item.Seccion;
        //                c.Horario_dia = item.dia_nombre;
        //                c.Horario_inicio = $"{item.hora_inicio} : {item.minuto_inicio}";
        //                c.Horario_final = $"{item.hora_fin} : {item.minuto_fin}";
        //                c.credito = item.credito;
        //                var recinto = await _dataContext.Recintos.FirstOrDefaultAsync(c => c.Id == int.Parse(item.Recinto));
        //                c.recinto = recinto.NombreCorto;
        //                c.precio_hora = vinculacion.Monto;
        //                c.pago_asignatura = c.precio_hora * c.credito;
        //                DataResult.Carga.Add(c);


        //            }

        //            foreach (var item in CargaLista)
        //            {
        //                Monto += (item.credito * vinculacion.Monto);
        //            }

        //            int CantCreditos = 0;
        //            Response.Data = DataResult;
        //            foreach (var item in DataResult.Carga)
        //            {
        //                CantCreditos += item.credito;
        //            }

        //            DataResult.CantCreditos = CantCreditos;
        //            DataResult.Monto = Monto;

        //            Response.Data = DataResult;
        //            Response.Status = 200;
        //            return Response;
        //        }
        //        else if (DataResult.Docente.TiempoDedicacion == "M")
        //        {
        //            var vinculacion = await _dataContext.Vinculos.FirstOrDefaultAsync(c => c.Corto == "M");


        //            DataResult.Docente.Pago_hora = vinculacion.Monto.ToString();
        //            foreach (var item in cargas.Data.Value.Item1.Carga)
        //            {
        //                var existe = CargaLista.Where(c => c.cod_asignatura == item.cod_asignatura && c.Seccion == item.Seccion && c.cod_universitas == item.cod_universitas).FirstOrDefault();
        //                if (existe == null)
        //                {
        //                    CargaLista.Add(item);
        //                }
        //                else if (existe.cod_asignatura[0].ToString() == "P" && existe.cod_asignatura[1].ToString() == "D")
        //                {
        //                    if (CargaLista.Where(c => c.cod_asignatura == item.cod_asignatura && c.Seccion == item.Seccion && c.dia_id == item.dia_id && c.hora_inicio == item.hora_inicio && c.minuto_inicio == item.minuto_inicio && c.hora_fin == item.hora_fin && c.minuto_fin == item.minuto_fin && c.Aula == item.Aula).FirstOrDefault() == null)
        //                    {
        //                        CargaLista.Add(item);
        //                    }
        //                    else
        //                    {
        //                        item.credito = 0;
        //                        CargaLista.Add(item);

        //                    }

        //                }
        //                else
        //                {
        //                    item.credito = 0;
        //                    CargaLista.Add(item);
        //                }

        //                var c = new CargaReporteDto();
        //                c.Periodo = item.Periodo;
        //                c.TiposCarga = item.TiposCarga;
        //                c.codigo_asignatura = item.cod_asignatura;
        //                c.nombre_asignatura = item.nombre_asignatura;
        //                c.id = item.Id;
        //                c.seccion = item.Seccion;
        //                c.Horario_dia = item.dia_nombre;
        //                c.Horario_inicio = $"{item.hora_inicio} : {item.minuto_inicio}";
        //                c.Horario_final = $"{item.hora_fin} : {item.minuto_fin}";
        //                c.credito = item.credito;
        //                var recinto = await _dataContext.Recintos.FirstOrDefaultAsync(c => c.Id == int.Parse(item.Recinto));
        //                c.recinto = recinto.NombreCorto;
        //                c.precio_hora = vinculacion.Monto;
        //                c.pago_asignatura = c.precio_hora * c.credito;
        //                DataResult.Carga.Add(c);
        //            }

        //            foreach (var item in CargaLista)
        //            {
        //                Monto += (item.credito * vinculacion.Monto);
        //            }

        //            int CantCreditos = 0;
        //            Response.Data = DataResult;
        //            foreach (var item in DataResult.Carga)
        //            {
        //                CantCreditos += item.credito;
        //            }

        //            DataResult.CantCreditos = CantCreditos;
        //            DataResult.Monto = Monto;

        //            Response.Data = DataResult;
        //            Response.Status = 200;
        //            return Response;
        //        }

        //    }
        //    return Response;
        //}


        public async Task<ServiceResponseData<DocenteCargaReporteDto>> PorDocenteCall(ReporteDto filtro)
        {
            FiltroDocentesDto filtroD = new FiltroDocentesDto();
            filtroD.idPrograma = 1;
            filtroD.Filtro = filtro.Cedula;
            var Docentes = await _docentesService.GetAllFilter(filtroD);
            var Result = await PorDocente(filtro, Docentes.Data);
            return Result;
        }

        public async Task<ServiceResponseReporte<List<DocenteCargaReporteDto>>> PorRecinto(ReportePorRecintoDto filtro)
        {

            var recintoActual = await _dataContext.Recintos.Where(c => c.Id == filtro.idRecinto).FirstOrDefaultAsync();
            var docentes = await _docentesService.GetAllRecinto(new FiltroDocentesDto(), recintoActual.Id);
            List<DocenteGetDto> docentesRecinto = new List<DocenteGetDto>();

            switch (filtro.TipoDocente)
            {
                case "MT":
                    docentesRecinto = docentes.Data!.Where(c =>
                        c.id_recinto == filtro.idRecinto.ToString() &&
                        c.tiempoDedicacion == "MT").ToList();
                    break;

                case "A":
                    docentesRecinto = docentes.Data!.Where(c =>
                        c.id_recinto == filtro.idRecinto.ToString() &&
                        (c.tiempoDedicacion == "MT" || c.tiempoDedicacion == "A")).ToList();
                    break;

                case "TC":
                    docentesRecinto = docentes.Data!.Where(c =>
                        c.id_recinto == filtro.idRecinto.ToString() &&
                        c.tiempoDedicacion == "TC").ToList();
                    break;

                default:
                    docentesRecinto = docentes.Data!.Where(c =>
                        c.id_recinto == filtro.idRecinto.ToString()).ToList();
                    break;
            }

            List<DocenteCargaReporteDto> CargadDocentes = new List<DocenteCargaReporteDto>();
            var vinculacion = await _dataContext.Vinculos.ToListAsync();
            int Monto = 0;
            int MontoPorAsignatura = 0;
            int Total = 0;
            int CantCreditos = 0;
            int CantidadPagada = 0;
            int CantCreditosF = 0;
            if (filtro.Curricular == "3")
            {
                docentesRecinto = docentesRecinto.Where(c => c.tiempoDedicacion == "F" && c.TipoDocente == "Facilitador del DIID").ToList();
            }
            else
            {
                docentesRecinto = docentesRecinto.Where(c => c.TipoDocente != "Facilitador del DIID").ToList();
            }

            foreach (var docente in docentesRecinto)
            {

                if (docente.identificacion != null)
                {

                    var filter = new ReporteDto();
                    filter.Cedula = docente.identificacion;
                    filter.Periodo = filtro.Periodo;
                    filter.idRecinto = filtro.idRecinto.ToString();
                    filter.idPrograma = 1;
                    if (filter.Cedula == "001-1749203-3")
                    {

                    }
                    ServiceResponseData<DocenteCargaReporteDto> DocenteConSuCarga = await PorDocente(filter, docentes.Data);


                    if (DocenteConSuCarga.Data != null && DocenteConSuCarga.Data.Carga != null)
                    {

                        List<CargaReporteDto> CargaFilter = new List<CargaReporteDto>();
                        if (filtro.Curricular != "0" && filtro.Curricular != null)
                        {
                            if (DocenteConSuCarga.Data.Docente!.TiempoDedicacion == "F")
                            {
                                CargaFilter = DocenteConSuCarga.Data.Carga.ToList();
                            }
                            else
                            {
                                CargaFilter = DocenteConSuCarga.Data.Carga.Where(c => c.TiposCarga.Id == Convert.ToInt32(filtro.Curricular)).ToList();
                            }

                            if (DocenteConSuCarga.Data.Docente.TiempoDedicacion == "TC")
                            {
                                Monto = DocenteConSuCarga.Data.MontoVinculacion;
                                foreach (var item in CargaFilter)
                                {
                                    item.precio_hora = 0;
                                    CantCreditos += item.credito;
                                }
                            }

                            else if (DocenteConSuCarga.Data.Docente.TiempoDedicacion == "MT" && filtro.TipoDocente != "A")
                            {
                                CargaFilter = CargaFilter.Where(c => c.HoraContratada == false).ToList();
                                CantCreditosF = CargaFilter.Sum(c => c.credito);

                            }
                            if (DocenteConSuCarga.Data.Docente.TiempoDedicacion == "MT" && filtro.TipoDocente == "A")
                            {
                                CargaFilter = CargaFilter.Where(c => c.HoraContratada == true).ToList();
                                foreach (var item in CargaFilter)
                                {
                                    item.precio_hora = int.Parse(DocenteConSuCarga.Data.Docente.Pago_hora);
                                    Monto += item.precio_hora * item.credito;
                                    MontoPorAsignatura += Monto;
                                    CantCreditos += item.credito;
                                }

                            }


                        }
                        else
                        {
                            CargaFilter = DocenteConSuCarga.Data.Carga;
                            if (DocenteConSuCarga.Data.Docente.TiempoDedicacion == "TC")
                            {
                                Monto = DocenteConSuCarga.Data.MontoVinculacion;
                                foreach (var item in CargaFilter)
                                {
                                    item.precio_hora = 0;
                                    CantCreditos += item.credito;

                                }
                            }
                            else if (DocenteConSuCarga.Data.Docente.TiempoDedicacion == "MT" && filtro.TipoDocente != "A" && filtro.TipoDocente != null)
                            {

                                CargaFilter = CargaFilter.Where(c => c.HoraContratada == false).ToList();
                                CantCreditosF = CargaFilter.Sum(c => c.credito);


                                //foreach (var item in CargaFilter.Where(c => c.HoraContratada == false))
                                //{
                                //    if (CantCreditosF + item.credito > 12)
                                //    {
                                //        // verificacion por si el monto resultante de la suma en mayor a 20 que solo tome despues de; va;pr 20
                                //        CantCreditosF += item.credito;
                                //        if (CantidadPagada == 0)
                                //        {
                                //            CantidadPagada = CantCreditosF - 12;
                                //            item.precio_hora = item.precio_hora;
                                //            item.pago_asignatura = item.precio_hora * CantidadPagada;
                                //            MontoPorAsignatura += item.pago_asignatura;
                                //        }
                                //        else
                                //        {
                                //            item.precio_hora = item.precio_hora;
                                //            item.pago_asignatura = item.precio_hora * item.credito;
                                //            MontoPorAsignatura += item.pago_asignatura;
                                //        }
                                //    }
                                //    else
                                //    {
                                //        CantCreditosF += item.credito;
                                //        item.precio_hora = 0;
                                //        item.pago_asignatura = 0;
                                //    }
                                //}

                            }


                            //else if (DocenteConSuCarga.Data.Carga.Count() == CargaFilter.Count())
                            //{
                            //    Monto = DocenteConSuCarga.Data.MontoSemanal;
                            //    foreach (var item in CargaFilter)
                            //    {

                            //        CantCreditos += item.credito;

                            //    }
                            //}
                            if (filtro.TipoDocente != "MT" && DocenteConSuCarga.Data.Docente.TiempoDedicacion == "MT" || DocenteConSuCarga.Data.Docente.TiempoDedicacion == "A" || DocenteConSuCarga.Data.Docente.TiempoDedicacion == "M" || DocenteConSuCarga.Data.Docente.TiempoDedicacion == "F")
                            {
                                Monto = 0;
                                if(DocenteConSuCarga.Data.Docente.TiempoDedicacion == "MT" && filtro.TipoDocente == "MT" && CargaFilter.Where(c => c.HoraContratada == false).ToList().Sum(c=>c.credito)>=0)
                                {
                                    CargaFilter = CargaFilter.Where(c => c.HoraContratada == false).ToList();
                                }
                                else if(filtro.TipoDocente!=null && filtro.TipoDocente!="MT" )
                                {
                                    if(DocenteConSuCarga.Data.Docente.TiempoDedicacion == "MT"&& CargaFilter.Where(c => c.HoraContratada == false).ToList().Sum(c => c.credito) < 1)
                                    {
                                        CargaFilter = new List<CargaReporteDto>();
                                    }
                                    else
                                    {
                                        CargaFilter = CargaFilter.Where(c => c.HoraContratada == true).ToList();
                                    }

                                }
                                //Esta condicion lo saca de la lista de todos si la carga no tiene por lo menos una fija
                                else if(DocenteConSuCarga.Data.Docente.TiempoDedicacion == "MT" && filtro.TipoDocente==null && CargaFilter.Where(c => c.HoraContratada == false).ToList().Sum(c => c.credito) <1)
                                {
                                    CargaFilter = new List<CargaReporteDto>();
                                }


                                foreach (var item in CargaFilter)
                                {
                                    item.precio_hora = int.Parse(DocenteConSuCarga.Data.Docente.Pago_hora);
                                    item.pago_asignaturaMensual = item.HoraContratada == true ? (item.precio_hora * item.credito) * 4 : 0;
                                    if(item.HoraContratada==true)
                                    {
                                        Monto += item.precio_hora * item.credito;
                                        MontoPorAsignatura += Monto;
                                        CantCreditos += item.credito;
                                    }
                                   
                                   
                                }

                                CantCreditosF = CantCreditos;
                            }
                        }

                        var DocenteCargaListo = new DocenteCargaReporteDto();
                        DocenteCargaListo.Docente = DocenteConSuCarga.Data.Docente;
                        DocenteCargaListo.MontoVinculacion = DocenteConSuCarga.Data.MontoVinculacion;
                        DocenteCargaListo.Carga = CargaFilter;
                        if (docente.tiempoDedicacion == "TC")
                        {
                            DocenteCargaListo.MontoSemanal = 0;
                            DocenteCargaListo.MontoMensual = DocenteConSuCarga.Data.MontoVinculacion;
                            DocenteCargaListo.CantCreditos = CantCreditos;
                        }
                        else if (docente.tiempoDedicacion == "MT")
                        {
                            if (filtro.TipoDocente == null)
                            {
                                DocenteCargaListo.MontoVinculacion = DocenteConSuCarga.Data.MontoVinculacion;
                                DocenteCargaListo.MontoSemanal = Monto;
                                DocenteCargaListo.MontoMensual = DocenteConSuCarga.Data.MontoVinculacion + (Monto * 4);
                                DocenteCargaListo.CantCreditos = CargaFilter.Sum(c=>c.credito);
                            }
                            else
                            {
                                DocenteCargaListo.MontoVinculacion = docente.tiempoDedicacion == "MT" && filtro.TipoDocente == "MT" ? DocenteConSuCarga.Data.MontoVinculacion : 0;
                                DocenteCargaListo.MontoSemanal = Monto;
                                DocenteCargaListo.MontoMensual = docente.tiempoDedicacion == "MT" && filtro.TipoDocente == "MT" ? DocenteConSuCarga.Data.MontoVinculacion : (Monto * 4);
                                DocenteCargaListo.CantCreditos = CargaFilter.Sum(c => c.credito);
                            }

                        }
                        else if (docente.tiempoDedicacion == "A")
                        {
                            DocenteCargaListo.MontoSemanal = Monto;
                            DocenteCargaListo.MontoMensual = Monto * 4;
                            DocenteCargaListo.CantCreditos = CantCreditos;
                        }
                        else
                        {
                            DocenteCargaListo.MontoSemanal = Monto;
                            DocenteCargaListo.MontoMensual = Monto * 4;
                            DocenteCargaListo.CantCreditos = CantCreditos;
                        }

                        if (DocenteCargaListo.Carga.Count > 0)
                        {
                            CargadDocentes.Add(DocenteCargaListo);
                            Total += DocenteCargaListo.MontoMensual;
                            Monto = 0;
                            CantCreditos = 0;
                            CantCreditosF = 0;
                            CantidadPagada = 0;
                            MontoPorAsignatura = 0;
                        }
                    }
                }
            }
            var response = new ServiceResponseReporte<List<DocenteCargaReporteDto>>()
            {
                Status = 200,
                Data = CargadDocentes.OrderBy(c => c.Docente.Nombre).ToList(),
                totalRecinto = Total
            };
            if (filtro.Periodo != null)
            {
                var periodo = await _dataContext.PeriodoAcademicos.Where(c => c.Periodo == filtro.Periodo).FirstAsync();
                response.Anio = periodo.Anio!.Value;
            }
            return response;
        }

        public async Task<ServiceResponseReporte<List<ReportCargaPosgradoDto>>> ReporteByDocentePosgrado(string cedula, string periodo, int idRecinto)
        {
            try
            {
                MontosPosgradosDto tGMontosTotalesPorMes = new MontosPosgradosDto() { Mes1 = 0, Mes2 = 0, Mes3 = 0, Mes4 = 0 };
                var reponseDataList = new List<ReportCargaPosgradoDto>();
                var docenteConsulta = await _docentesService.GetAllFilter(new FiltroDocentesDto() { Filtro = cedula, idPrograma = 2, elementosPorPagina = 100, paginaActual = 1 });
                var DocenteCarga = await _cargaService.GetCargaCallPosgrado(cedula, periodo, 2, docenteConsulta.Data);
                var cargaFilter = DocenteCarga.Data.Value.Item1.Cargas.Where(c => c.Recinto == idRecinto.ToString()).ToList();
                if (!cargaFilter.Any())
                {
                    return new ServiceResponseReporte<List<ReportCargaPosgradoDto>>() { Data = new List<ReportCargaPosgradoDto>(), Status = 204, Message = "Este docente no tiene carga en este recinto." };

                }
                var docente = _mapper.Map<DocenteReporteDto>(docenteConsulta.Data[0]);
                var montoCarga = new MontosPosgradosDto() { Mes1 = 0, Mes2 = 0, Mes3 = 0, Mes4 = 0 };

                if (await _cargaService.ValidateNivelPosgrado(docente.Nivel) == false)
                {
                    return new ServiceResponseReporte<List<ReportCargaPosgradoDto>>() { Data = new List<ReportCargaPosgradoDto>(), Status = 200, Message = "El nivel academico del docente es " + docente.Nivel + ". Este nivel no se encuentra registrado en Akademic Report" };
                }

                if (DocenteCarga.Data.Value.Item1.Cargas.Count < 1)
                {
                    reponseDataList.Add(new ReportCargaPosgradoDto() { Docente = docente, Cargas = new List<CargaPosgradoGet>() });
                    return new ServiceResponseReporte<List<ReportCargaPosgradoDto>>() { Data = reponseDataList, Status = 400 };
                }

                var nivelAcademicoDb = await _dataContext.NivelAcademicos.Where(c => c.IdPrograma == 2).ToListAsync();
                var nivelAcademico = nivelAcademicoDb.Find(c => c.Nivel.Contains(docente.Nivel));
                if (nivelAcademico is null)
                {
                    return new ServiceResponseReporte<List<ReportCargaPosgradoDto>>() { Data = new List<ReportCargaPosgradoDto>(), Status = 200, Message = "El nivel academico del docente es " + docente.Nivel + ". Este nivel no se encuentra registrado en Akademic Report" };
                }
                docente.Pago_hora = nivelAcademico.PagoHora.ToString();
                decimal Monto = 0;
                int cantCredito = 0;

                foreach (var carga in DocenteCarga.Data.Value.Item1.Cargas.Where(c => c.Recinto == idRecinto.ToString()).ToList())
                {
                    Monto += (carga.Credito * nivelAcademico.PagoHora);
                    carga.DistribucionMontos = CalculoTiempoHoras.DistribucionMontosPosgrado(carga.IdMes!.Value, (carga.Credito * nivelAcademico.PagoHora));
                    montoCarga.Mes1 += carga.DistribucionMontos.Mes1;
                    montoCarga.Mes2 += carga.DistribucionMontos.Mes2;
                    montoCarga.Mes3 += carga.DistribucionMontos.Mes3;
                    montoCarga.Mes4 += carga.DistribucionMontos.Mes4;

                    tGMontosTotalesPorMes.Mes1 += carga.DistribucionMontos.Mes1;
                    tGMontosTotalesPorMes.Mes2 += carga.DistribucionMontos.Mes2;
                    tGMontosTotalesPorMes.Mes3 += carga.DistribucionMontos.Mes3;
                    tGMontosTotalesPorMes.Mes4 += carga.DistribucionMontos.Mes4;


                    cantCredito += carga.Credito;
                    carga.pago_asignatura = carga.Credito * nivelAcademico.PagoHora;
                    carga.pago_asignaturaMensual = carga.Credito * nivelAcademico.PagoHora;
                    carga.precio_hora = nivelAcademico.PagoHora;
                }
                ReportCargaPosgradoDto reponse = new ReportCargaPosgradoDto() { Docente = docente, CantCreditos = cantCredito, Cargas = cargaFilter, MontoSemanal = Convert.ToInt32(Monto), MontoMensual = Convert.ToInt32(Monto), MontoPorMes = montoCarga };
                reponseDataList.Add(reponse);
                var p = await _dataContext.PeriodoAcademicos.Where(c => c.Id == int.Parse(DocenteCarga.Data.Value.Item1.Cargas[0].Periodo)).FirstAsync();
                return new ServiceResponseReporte<List<ReportCargaPosgradoDto>>() { Data = reponseDataList, Status = 200, Anio = p.Anio!.Value, MontoTotalesPorMes = tGMontosTotalesPorMes };
            }
            catch (Exception ex)
            {
                string msj = ex.ToString();
                return new ServiceResponseReporte<List<ReportCargaPosgradoDto>>() { Data = null, Status = 500 };
            }
        }



        public async Task<ServiceResponseReporte<List<ReportCargaPosgradoDto>>> ReporteByIdProgramCargaPosgrado(int idConcepto, string periodo, int idRecinto)
        {
            try
            {
                var response = new List<ReportCargaPosgradoDto>();
                MontosPosgradosDto tGMontosTotalesPorMes = new MontosPosgradosDto() { Mes1 = 0, Mes2 = 0, Mes3 = 0, Mes4 = 0 };
                var docentesAmilka = await _docentesService.GetAll();
                var nivelAcademico = await _dataContext.NivelAcademicos.Where(c => c.IdPrograma == 2).ToListAsync();
                int Monto = 0;
                int MontoTotal = 0;
                int CantCreditos = 0;
                foreach (var docente in docentesAmilka.Data.ToList())
                {
                    if (docente.nivel != null && await _cargaService.ValidateNivelPosgrado(docente.nivel) == true)
                    {
                        var DocenteCarga = await _cargaService.GetCargaCallPosgrado(docente.identificacion, periodo, 2, docentesAmilka.Data);
                        if (DocenteCarga.Status == 200 && DocenteCarga.Data.Value.Item1.Cargas != null && DocenteCarga.Data.Value.Item1.Cargas.Count > 0)
                        {
                            var NivelAcademicoDocente = nivelAcademico.FirstOrDefault(c => c.Nivel.Contains(docente.nivel));
                            if (NivelAcademicoDocente != null)
                            {
                                DocenteCarga.Data.Value.Item1.Docente!.Pago_hora = nivelAcademico.FirstOrDefault(c => c.Nivel.Contains(docente.nivel)).PagoHora.ToString();
                                if (idConcepto != 0)
                                {
                                    CantCreditos = 0;
                                    MontosPosgradosDto montoPorMes = new MontosPosgradosDto() { Mes1 = 0, Mes2 = 0, Mes3 = 0, Mes4 = 0 };
                                    var cargaPosgradoList = new List<CargaPosgradoGet>();
                                    foreach (var cargaFiltrada in DocenteCarga.Data.Value.Item1.Cargas.Where(c => c.IdPrograma == 2 && c.Codigo.Id_concepto == idConcepto && c.Recinto == idRecinto.ToString()).ToList())
                                    {
                                        cargaFiltrada.ConceptoAsignatura = _mapper.Map<ConceptoGetDto>(cargaFiltrada.ConceptoAsignatura);
                                        Monto += (cargaFiltrada.Credito * int.Parse(DocenteCarga.Data.Value.Item1.Docente.Pago_hora));
                                        cargaFiltrada.DistribucionMontos = CalculoTiempoHoras.DistribucionMontosPosgrado(cargaFiltrada.IdMes!.Value, (cargaFiltrada.Credito * int.Parse(DocenteCarga.Data.Value.Item1.Docente.Pago_hora)));
                                        montoPorMes.Mes1 += cargaFiltrada.DistribucionMontos.Mes1;
                                        montoPorMes.Mes2 += cargaFiltrada.DistribucionMontos.Mes2;
                                        montoPorMes.Mes3 += cargaFiltrada.DistribucionMontos.Mes3;
                                        montoPorMes.Mes4 += cargaFiltrada.DistribucionMontos.Mes4;
                                        CantCreditos += cargaFiltrada.Credito;
                                        cargaFiltrada.precio_hora = int.Parse(DocenteCarga.Data.Value.Item1.Docente.Pago_hora);
                                        cargaFiltrada.pago_asignatura = cargaFiltrada.Credito * int.Parse(DocenteCarga.Data.Value.Item1.Docente.Pago_hora);
                                        cargaFiltrada.pago_asignaturaMensual = (cargaFiltrada.Credito * int.Parse(DocenteCarga.Data.Value.Item1.Docente.Pago_hora));
                                        cargaPosgradoList.Add(cargaFiltrada);
                                    }


                                    if (cargaPosgradoList.Count > 0)
                                    {
                                        response.Add(new ReportCargaPosgradoDto()
                                        {
                                            Docente = _mapper.Map<DocenteReporteDto>(docente),
                                            Cargas = cargaPosgradoList,
                                            CantCreditos = DocenteCarga.Data.Value.Item1.CantCreditos,
                                            MontoMensual = Monto,
                                            MontoSemanal = Monto,
                                            MontoPorMes = montoPorMes
                                        }); ;
                                        tGMontosTotalesPorMes.Mes1 += montoPorMes.Mes1;
                                        tGMontosTotalesPorMes.Mes2 += montoPorMes.Mes2;
                                        tGMontosTotalesPorMes.Mes3 += montoPorMes.Mes3;
                                        tGMontosTotalesPorMes.Mes4 += montoPorMes.Mes4;
                                        MontoTotal += Monto;
                                        Monto = 0;

                                    }
                                }
                                else
                                {

                                    MontosPosgradosDto montoPorMes = new MontosPosgradosDto() { Mes1 = 0, Mes2 = 0, Mes3 = 0, Mes4 = 0 };
                                    CantCreditos = 0;
                                    var cargaPosgradoList = new List<CargaPosgradoGet>();
                                    foreach (var cargaFiltrada in DocenteCarga.Data.Value.Item1.Cargas.Where(c => c.IdPrograma == 2 && c.Recinto == idRecinto.ToString()))
                                    {
                                        Monto += (cargaFiltrada.Credito * int.Parse(DocenteCarga.Data.Value.Item1.Docente.Pago_hora));
                                        cargaFiltrada.DistribucionMontos = CalculoTiempoHoras.DistribucionMontosPosgrado(cargaFiltrada.IdMes!.Value, (cargaFiltrada.Credito * int.Parse(DocenteCarga.Data.Value.Item1.Docente.Pago_hora)));
                                        montoPorMes.Mes1 += cargaFiltrada.DistribucionMontos.Mes1;
                                        montoPorMes.Mes2 += cargaFiltrada.DistribucionMontos.Mes2;
                                        montoPorMes.Mes3 += cargaFiltrada.DistribucionMontos.Mes3;
                                        montoPorMes.Mes4 += cargaFiltrada.DistribucionMontos.Mes4;
                                        CantCreditos += cargaFiltrada.Credito;
                                        cargaFiltrada.precio_hora = int.Parse(DocenteCarga.Data.Value.Item1.Docente.Pago_hora);
                                        cargaFiltrada.pago_asignatura = cargaFiltrada.Credito * int.Parse(DocenteCarga.Data.Value.Item1.Docente.Pago_hora);
                                        cargaFiltrada.pago_asignaturaMensual = (cargaFiltrada.Credito * int.Parse(DocenteCarga.Data.Value.Item1.Docente.Pago_hora));
                                        cargaPosgradoList.Add(cargaFiltrada);
                                    }

                                    if (cargaPosgradoList.Count > 0)
                                    {
                                        response.Add(new ReportCargaPosgradoDto()
                                        {
                                            Docente = _mapper.Map<DocenteReporteDto>(docente),
                                            Cargas = cargaPosgradoList,
                                            CantCreditos = CantCreditos,
                                            MontoMensual = Monto,
                                            MontoSemanal = Monto,
                                            MontoPorMes = montoPorMes

                                        });
                                        tGMontosTotalesPorMes.Mes1 += montoPorMes.Mes1;
                                        tGMontosTotalesPorMes.Mes2 += montoPorMes.Mes2;
                                        tGMontosTotalesPorMes.Mes3 += montoPorMes.Mes3;
                                        tGMontosTotalesPorMes.Mes4 += montoPorMes.Mes4;
                                        MontoTotal += Monto;
                                        Monto = 0;
                                    }
                                }
                            }

                        }
                    }

                }
                if (response.Count < 1)
                    return new ServiceResponseReporte<List<ReportCargaPosgradoDto>>() { Data = response, Status = 204, Message = Msj.MsjNoData, totalRecinto = MontoTotal, MontoTotalesPorMes = tGMontosTotalesPorMes };
                return new ServiceResponseReporte<List<ReportCargaPosgradoDto>>() { Data = response, Status = 200, Message = Msj.MsjSucces, totalRecinto = MontoTotal, Anio = int.Parse(response[0].Cargas[0].Anio), MontoTotalesPorMes = tGMontosTotalesPorMes };
            }
            catch (Exception ex)
            {
                var error = ex.ToString();
                return new ServiceResponseReporte<List<ReportCargaPosgradoDto>>() { Data = null, Status = 500, Message = $"{Msj.MsjError} {ex.ToString()}" };
            }
        }



        public async Task<ServiceResponseReporte<List<ReportCargaPosgradoDto>>> ReporteByIdRecintoPosgrado(int idRecinto, string periodo)
        {
            //try
            //{
            //    var docentesAmilka = await _docentesService.GetAll();
            //    var nivelAcademico = await _dataContext.NivelAcademicos.Where(c => c.IdPrograma == 2).ToListAsync();
            //    int Monto = 0;
            //    int MontoTotal = 0;
            //    int CantCreditos = 0;
            //    var docenteRecinto = docentesAmilka.Data.Where(c => c.id_recinto == idRecinto.ToString()).ToList();
            //    foreach (var docente in docenteRecinto)
            //    {
            //        var docenteCarga = await ReporteByDocentePosgrado(docente.identificacion, periodo);
            //    }
            //}
            //catch (Exception ex)
            //{ 

            throw new NotImplementedException();
            //}
        }

        public Task<ServiceResponseReporte<List<ReportCargaPosgradoDto>>> ReporteByPeriodoPosgrado(string periodo, int idRecinto)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResponseReporte<List<ReporteConsolidadoResponseDto>>> ReporteConsolidado(FiltroReporteConsolidado filtro)
        {
            var Recintos = await _dataContext.Recintos.ToListAsync();
            var DataList = new List<ReporteConsolidadoResponseDto>();
            int TotalRecintos = 0;
            try
            {
                foreach (var recinto in Recintos)
                {
                    ReportePorRecintoDto filterReporte = new ReportePorRecintoDto();
                    filterReporte.Curricular = filtro.curricular;
                    filterReporte.idRecinto = recinto.Id;
                    filterReporte.Periodo = filtro.periodo;
                    var response = await PorRecinto(filterReporte);
                    var Cargas = new List<CargaGetDto>();
                    var Data = new ReporteConsolidadoResponseDto();
                    Data.idRecinto = recinto.Id;
                    Data.nombreRecinto = recinto.NombreCorto;
                    Data.periodo = filtro.periodo;
                    Data.ano = filtro.periodo!.Split("-")[0].ToString();
                    int Monto = 0;
                    foreach (var item in response.Data)
                    {
                        Monto += item.MontoSemanal;
                    }
                    Data.monto = Monto * 4;

                    DataList.Add(Data);
                    TotalRecintos += Monto * 4;

                }
                return new ServiceResponseReporte<List<ReporteConsolidadoResponseDto>>() { Status = 200, Data = DataList, totalRecinto = TotalRecintos };
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                return new ServiceResponseReporte<List<ReporteConsolidadoResponseDto>>() { Status = 400, Data = DataList, totalRecinto = TotalRecintos };
            }
        }

        public async Task<ServiceResponseReporte<List<DocenteCargaReporteDto>>> ReporteDiplomado(ReportePorRecintoDto filtro)
        {
            //var DataPorRecinto = await PorRecinto(filtro);
            //var result = new List<DocenteCargaReporteDto>();
            //foreach (var item in DataPorRecinto.Data!)
            //{

            //        string filtroIngles1 = "ing";
            //        string filtroIngles12 = "gdo";
            //        item.Carga = item.Carga.Where(c => c.codigo_asignatura.ToUpper().Contains(filtroIngles1.ToUpper())
            //        || c.codigo_asignatura.ToUpper().Contains(filtroIngles12.ToUpper())).ToList();
            //    if(item.Carga.Count>0)
            //    {
            //        var CargaDocente = new DocenteCargaReporteDto();
            //        CargaDocente.Carga =   
            //    }



            //}
            //return DataPorRecinto;
            throw new NotImplementedException();
        }
    }
}
