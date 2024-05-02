using AkademicReport.Data;
using AkademicReport.Dto.CargaDto;
using AkademicReport.Dto.DocentesDto;
using AkademicReport.Dto.ReporteDto;
using AkademicReport.Models;
using AkademicReport.Service.CargaServices;
using AkademicReport.Service.DocenteServices;
using AutoMapper.Configuration.Conventions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Collections.Generic;

namespace AkademicReport.Service.ReposteServices
{
    public class ReporteService : IReporteService
    {
        private readonly ICargaDocenteService _cargaService;
        private readonly IDocenteService _docentesService;
        private readonly DataContext _dataContext;


        public ReporteService(ICargaDocenteService cargaService, DataContext dataContext, IDocenteService docenteService)
        {
            _cargaService = cargaService;
            _dataContext = dataContext;
            _docentesService = docenteService;
        }


       public async Task<ServiceResponseData<DocenteCargaReporteDto>> PorDocente(ReporteDto filtro, List<DocenteGetDto>DocentesAmilca)
        {
            DocenteCargaReporteDto DataResult = new DocenteCargaReporteDto();
            DataResult.Carga = new List<CargaReporteDto>();
            var Response = new ServiceResponseData<DocenteCargaReporteDto>();
            var CargaMapeada = new List<CargaReporteDto>();
            var CargaLista = new List<CargaGetDto>();
            var Docente = new DocenteReporteDto();
            var cargas = await _cargaService.GetCarga(filtro.Cedula, filtro.Periodo, DocentesAmilca);
            if (cargas.Data.Value.Item1.Docente != null )
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
                Docente.Recinto = recinto.Recinto1;
                Docente.Nombre_corto = cargas.Data.Value.Item1.Docente.nombre_corto;
                Docente.Id_nivel_academico = cargas.Data.Value.Item1.Docente.id_nivel_academico;
                Docente.Nivel = cargas.Data.Value.Item1.Docente.nivel;
                DataResult.Docente = Docente;
                
            }
            if (cargas.Data.Value.Item1.Carga==null)
            {
                DataResult.Docente = Docente;
                Response.Data = DataResult;
                Response.Status = 204;
                return Response;
            }
            //var cargarPorRecinto= cargas.Data.Value.Item1.Carga.Where(c=>c.Recinto==filtro.idRecinto).ToList();
            if(cargas.Data.Value.Item1.Carga != null)
            {
                
                int Monto = 0;
                if(DataResult.Docente.TiempoDedicacion== "TC")
                {
                  var vinculacion = await _dataContext.Vinculos.FirstOrDefaultAsync(c => c.Corto == "TC");
                  Monto = vinculacion.Monto;
                  DataResult.Monto = Monto;

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
                    DataResult.Docente.Pago_hora = nivelAcademico.PagoHora.ToString();
                    foreach (var item in cargas.Data.Value.Item1.Carga)
                    {
                        var existe = CargaLista.Where(c => c.cod_asignatura == item.cod_asignatura && c.Seccion == item.Seccion).FirstOrDefault();
                        if (existe == null)
                        {
                            CargaLista.Add(item);
                        }
                        else
                        {
                            item.credito = 0;
                        }

                        var c = new CargaReporteDto();
                        c.TiposCarga = item.TiposCarga;
                        c.Periodo = item.Periodo;
                        c.codigo_asignatura = item.cod_asignatura;
                        c.nombre_asignatura = item.nombre_asignatura;
                        c.id = item.Id;
                        c.seccion = item.Seccion;
                        c.Horario_dia = item.dia_nombre;
                        c.Horario_inicio = $"{item.hora_inicio} : {item.minuto_inicio}";
                        c.Horario_final = $"{item.hora_fin} : {item.minuto_fin}";
                        c.credito = item.credito;
                        var recinto = await _dataContext.Recintos.FirstOrDefaultAsync(c => c.Id == int.Parse(item.Recinto));
                        c.recinto = recinto.NombreCorto;
                        c.precio_hora = nivelAcademico.PagoHora;
                        c.pago_asignatura = c.precio_hora * c.credito;
                        DataResult.Carga.Add(c);
                        
                    }
                    int CantCreditos = 0;
                    foreach (var item in DataResult.Carga)
                    {
                        CantCreditos += item.credito;
                    }
                    DataResult.CantCreditos = CantCreditos;

                    Response.Data = DataResult;
                  Response.Status= 200;
                    
                  return Response;
                }
                else if(DataResult.Docente.TiempoDedicacion == "MT")
                {
                    var vinculacion = await _dataContext.Vinculos.FirstOrDefaultAsync(c => c.Corto == "MT");
                    var nivelAcademico = await _dataContext.NivelAcademicos
                         .Where(n => n.Nivel.ToUpper().Replace("á", "a")
                                             .Replace("é", "e")
                                             .Replace("í", "i")
                                             .Replace("ó", "o")
                                             .Replace("ú", "u")
                                             .Contains(DataResult.Docente.Nivel.ToUpper().Replace("á", "a")
                                                                 .Replace("é", "e")
                                                                 .Replace("í", "i")
                                                                 .Replace("ó", "o")
                                                                 .Replace("ú", "u")))
                         .FirstOrDefaultAsync();
                    DataResult.Docente.Pago_hora = nivelAcademico.PagoHora.ToString();

                    Monto = vinculacion.Monto;

                    foreach (var item in cargas.Data.Value.Item1.Carga)
                    {
                        
                        var CargaExistente = cargas.Data.Value.Item1.Carga.Where(c => c.cod_asignatura == item.cod_asignatura && c.Seccion == item.Seccion).ToList();
                        if(CargaExistente.Count>1)
                        {
                            Monto += (CargaExistente[0].credito * nivelAcademico.PagoHora);
                        }   
                        else
                        {
                            Monto += (item.credito * nivelAcademico.PagoHora);
                        }
                        var c = new CargaReporteDto();
                        c.TiposCarga = item.TiposCarga;
                        c.Periodo = item.Periodo;
                        c.codigo_asignatura = item.cod_asignatura;
                        c.nombre_asignatura = item.nombre_asignatura;
                        c.id = item.Id;
                        c.seccion = item.Seccion;
                        c.Horario_dia = item.dia_nombre;
                        c.Horario_inicio = $"{item.hora_inicio} : {item.minuto_inicio}";
                        c.Horario_final = $"{item.hora_fin} : {item.minuto_fin}";
                        c.credito = item.credito;
                        var recinto = await _dataContext.Recintos.FirstOrDefaultAsync(c => c.Id == int.Parse(item.Recinto));
                        c.recinto = recinto.NombreCorto;
                        c.precio_hora = nivelAcademico.PagoHora;
                        c.pago_asignatura = c.precio_hora * c.credito;
                        DataResult.Carga.Add(c);

                    }
                    int CantCreditos = 0;
                    foreach (var item in DataResult.Carga)
                    {
                        CantCreditos += item.credito;
                    }
                    DataResult.CantCreditos = CantCreditos;
                    DataResult.Monto = Monto;
                    Response.Data = DataResult;
                   
                    Response.Status = 200;
                    return Response;
                }
                else if (DataResult.Docente.TiempoDedicacion == "A" || DataResult.Docente.TiempoDedicacion=="M" || DataResult.Docente.TiempoDedicacion=="F")
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
                          var existe = CargaLista.Where(c=>c.cod_asignatura==item.cod_asignatura && c.Seccion==item.Seccion).FirstOrDefault();
                          if (existe==null)
                            {
                               CargaLista.Add(item);
                            }else
                            {
                                item.credito = 0;
                                CargaLista.Add(item);
                            }
 
                        var c = new CargaReporteDto();
                        c.Periodo = item.Periodo;
                        c.TiposCarga = item.TiposCarga;
                        c.codigo_asignatura = item.cod_asignatura;
                        c.nombre_asignatura = item.nombre_asignatura;
                        c.id = item.Id;
                        c.seccion = item.Seccion;
                        c.Horario_dia = item.dia_nombre;
                        c.Horario_inicio = $"{item.hora_inicio} : {item.minuto_inicio}";
                        c.Horario_final = $"{item.hora_fin} : {item.minuto_fin}";
                        c.credito = item.credito;
                        var recinto = await _dataContext.Recintos.FirstOrDefaultAsync(c => c.Id == int.Parse(item.Recinto));
                        c.recinto = recinto.NombreCorto;
                        c.precio_hora = nivelAcademico.PagoHora;
                        c.pago_asignatura = c.precio_hora * c.credito;
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
                    DataResult.Monto = Monto;
             
                    Response.Data = DataResult;
                    Response.Status = 200;
                    return Response;
                }

            }
            return Response;
        }

        public async Task<ServiceResponseData<DocenteCargaReporteDto>> PorDocenteCall(ReporteDto filtro)
        {
            var Docentes = await _docentesService.GetAll();
            var Result = await PorDocente(filtro, Docentes.Data);
            return Result;
        }

        public async Task<ServiceResponseReporte<List<DocenteCargaReporteDto>>> PorRecinto(ReportePorRecintoDto filtro)
        {  

            var docentes = await _docentesService.GetAll();
            var docentesRecinto = docentes.Data.Where(c=>c.id_recinto==filtro.idRecinto.ToString()).ToList();
            List<DocenteCargaReporteDto> CargadDocentes = new List<DocenteCargaReporteDto>();
            int Monto = 0;
            int Total = 0;

            foreach (var docente in docentesRecinto)
            {
                if (docente.identificacion != null)
                {
                   
                    List<Models.CargaDocente> carga = new List<Models.CargaDocente>();
                    if(filtro.Curricular!="0" && filtro.Curricular!=null)
                    {
   
                        carga = await _dataContext.CargaDocentes.Where(c => c.Cedula == docente.identificacion && c.Curricular == int.Parse(filtro.Curricular)).ToListAsync();

                    }
                    else
                    {
                        carga = await _dataContext.CargaDocentes.Where(c => c.Cedula == docente.identificacion).ToListAsync();

                    }
                    if (carga.Count > 0 && docente.nivel!=null)
                    {
                        var filter = new ReporteDto();
                        filter.Cedula = docente.identificacion;
                        filter.Periodo = filtro.Periodo;
                        filter.idRecinto = filtro.idRecinto.ToString();
                        var nivelAcademico = await _dataContext.NivelAcademicos
                        .Where(n => n.Nivel.Replace("á", "a")
                                            .Replace("é", "e")
                                            .Replace("í", "i")
                                            .Replace("ó", "o")
                                            .Replace("ú", "u")
                                            .Contains(docente.nivel.Replace("á", "a")
                                                                .Replace("é", "e")
                                                                .Replace("í", "i")
                                                                .Replace("ó", "o")
                                                                .Replace("ú", "u")))
                        .FirstOrDefaultAsync();
                        ServiceResponseData<DocenteCargaReporteDto> DocenteConSuCarga = await PorDocente(filter, docentes.Data);
                        if(DocenteConSuCarga.Data!=null && DocenteConSuCarga.Data.Carga!=null)
                        {
                            List<CargaReporteDto> CargaFilter = new List<CargaReporteDto>();
                            if(filtro.Curricular!="0" && filtro.Curricular!=null)
                            {
                                CargaFilter = DocenteConSuCarga.Data.Carga.Where(c => c.TiposCarga.Id == int.Parse(filtro.Curricular)).ToList();
                            }
                            else
                            {
                                CargaFilter = DocenteConSuCarga.Data.Carga;
                            }

                            if(docente.tiempoDedicacion!="TC" && docente.tiempoDedicacion!="MD")
                            {
                                CargaFilter.ForEach(c =>
                                {
                                    Monto += c.precio_hora * c.credito;
                                });
                            }
                            else
                            {
                                Monto = DocenteConSuCarga.Data.Monto;
                            }
                           
                            var DocenteCargaListo = new DocenteCargaReporteDto()
                            {
                                Docente = DocenteConSuCarga.Data.Docente,
                                Carga = CargaFilter,
                                Monto = Monto,
                                CantCreditos = DocenteConSuCarga.Data.CantCreditos

                            };
                            Total += Monto;
                            Monto = 0;
                            if (DocenteCargaListo.Carga.Count>0)
                            {

                                CargadDocentes.Add(DocenteCargaListo);
                                
                            }
                        } 
                    }
                }
            }

            return new ServiceResponseReporte<List<DocenteCargaReporteDto>>() { Status = 200, Data = CargadDocentes.OrderBy(c=>c.Docente.Nombre).ToList(), totalRecinto=Total};
           
           
        }

        public async Task<ServiceResponseReporte<List<ReporteConsolidadoResponseDto>>> ReporteConsolidado(FiltroReporteConsolidado filtro)
        {
            var Recintos = await _dataContext.Recintos.ToListAsync();
            var DataList = new List<ReporteConsolidadoResponseDto>();
            int TotalRecintos = 0;
            
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
                     
                     Monto+= item.Monto;
                }
                Data.monto = Monto;
                
                DataList.Add(Data);
                TotalRecintos += Monto;

            }
            return new ServiceResponseReporte<List<ReporteConsolidadoResponseDto>>() { Status = 200, Data = DataList, totalRecinto=TotalRecintos };




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
