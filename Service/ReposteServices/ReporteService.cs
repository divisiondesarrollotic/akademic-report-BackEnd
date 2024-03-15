using AkademicReport.Data;
using AkademicReport.Dto.CargaDto;
using AkademicReport.Dto.DocentesDto;
using AkademicReport.Dto.ReporteDto;
using AkademicReport.Models;
using AkademicReport.Service.CargaServices;
using AkademicReport.Service.DocenteServices;
using AutoMapper.Configuration.Conventions;
using Microsoft.EntityFrameworkCore;

namespace AkademicReport.Service.ReposteServices
{
    public class ReporteService : IReposteService
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


       public async Task<ServiceResponseData<DocenteCargaReporteDto>> PorDocente(ReporteDto filtro)
        {
            DocenteCargaReporteDto DataResult = new DocenteCargaReporteDto();
            DataResult.Carga = new List<CargaReporteDto>();
            var Response = new ServiceResponseData<DocenteCargaReporteDto>();
            var CargaMapeada = new List<CargaReporteDto>();
            var CargaLista = new List<CargaGetDto>();
            var Docente = new DocenteReporteDto();
            var cargas = await _cargaService.GetCarga(filtro.Cedula, filtro.Periodo);
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
            var cargarPorRecinto= cargas.Data.Value.Item1.Carga.Where(c=>c.Recinto==filtro.idRecinto).ToList();
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
                    foreach (var item in cargas.Data.Value.Item1.Carga.OrderBy(c => c.dia_id).OrderBy(c => c.hora_inicio))
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
                        c.curricular = item.Curricular;
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
                        c.curricular = item.Curricular;
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
                    DataResult.Monto = Monto;
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
                    foreach (var item in cargas.Data.Value.Item1.Carga.OrderBy(c=>c.dia_id).OrderBy(c=>c.hora_inicio))
                    {
                          var existe = CargaLista.Where(c=>c.cod_asignatura==item.cod_asignatura && c.Seccion==item.Seccion).FirstOrDefault();
                          if (existe==null)
                            {
                               CargaLista.Add(item);
                            }else
                            {
                                item.credito = 0;
                            }

                        var c = new CargaReporteDto();
                        c.curricular = item.Curricular;
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

                    DataResult.Monto = Monto;
                    Response.Data = DataResult;
                    Response.Status = 200;
                    return Response;
                }

            }
            return Response;




        }

        public async Task<ServiceResponseData<List<DocenteCargaReporteDto>>> PorRecinto(ReportePorRecintoDto filtro)
        {  

            var docentes = await _docentesService.GetAll();
            var docentesRecinto = docentes.Data.Where(c=>c.id_recinto==filtro.idRecinto.ToString()).ToList();
            List<DocenteCargaReporteDto> CargadDocentes = new List<DocenteCargaReporteDto>();
            foreach (var docente in docentesRecinto)
            {
                if (docente.identificacion != null)
                {


                    var carga = await _dataContext.CargaDocentes.Where(c => c.Cedula == docente.identificacion && c.Curricular==int.Parse(filtro.Curricular)).ToListAsync();
                    if (carga.Count > 0)
                    {
                        var filter = new ReporteDto();
                        filter.Cedula = docente.identificacion;
                        filter.Periodo = filtro.Periodo;
                        filter.idRecinto = filtro.idRecinto.ToString();
                        var DocenteConSuCarga = await PorDocente(filter);
                        if(DocenteConSuCarga.Data!=null && DocenteConSuCarga.Data.Carga!=null)
                        {
                            var CargaFilter = DocenteConSuCarga.Data.Carga.Where(c => c.curricular == int.Parse(filtro.Curricular)).ToList();
                            var DocenteCargaListo = new DocenteCargaReporteDto()
                            {
                                Docente = DocenteConSuCarga.Data.Docente,
                                Carga = CargaFilter
                            };
                            CargadDocentes.Add(DocenteCargaListo);

                        }
                        
                    }
                }

            }

            return new ServiceResponseData<List<DocenteCargaReporteDto>>() { Status = 200, Data = CargadDocentes };
           
           
        }
    }
}
