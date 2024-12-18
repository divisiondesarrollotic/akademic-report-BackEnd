using AkademicReport.Data;
using AkademicReport.Dto.CargaDto;
using AkademicReport.Dto.DocentesDto;
using AkademicReport.Utilities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft;
using Newtonsoft.Json;
using System.Drawing;
using System.Linq;
using System.Text;

namespace AkademicReport.Service.DocenteServices
{
    public class DocenteService : IDocenteService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _dataContext;
        private readonly HttpClient _httpClient = new HttpClient();
        public List<DocenteGetDto> DocentesLimpios = new List<DocenteGetDto>();
        public DocenteService(IMapper mapper, DataContext dataContext)
        {
            _mapper = mapper;
            _dataContext = dataContext;

        }


        public async Task<ServiceResponseData<List<DocenteGetDto>>> CleanData(List<DocenteAmilcaDto> DocentesAmilca, int accion)
        {

            List<DocenteGetDto> Docentes = new List<DocenteGetDto>();
            foreach (var d in DocentesAmilca)
            {
                if(d.CedulaPasaporte == "402-2529221-4")
                {

                }
                var docente = new DocenteGetDto();
                docente.id = d.Id;
                docente.tiempoDedicacion = d.TiempoDedicacion;
                docente.identificacion = d.CedulaPasaporte;
                if (d.CedulaPasaporte != null && d.MaximoNivelAcademico != null)
                {
                    if (d.CedulaPasaporte[0].ToString().Trim() == "0" && d.CedulaPasaporte[1].ToString().Trim() == "0" && d.CedulaPasaporte[2].ToString().Trim() == "-" || d.CedulaPasaporte.Length == 13)
                    {
                        //docente.identificacion = docente.identificacion.Replace("00-", "");
                        docente.tipoIdentificacion = "Cedula";
                    }
                    else
                    {
                        docente.tipoIdentificacion = "Pasaporte";
                    }
                }

                docente.nombre = d.Nombre;
                docente.TipoDocente = d.TipoDocente;
                docente.nacionalidad = d.Nacionalidad;
                docente.sexo = d.Sexo;
                if (d.TiempoDedicacion != null && d.TiempoDedicacion != "N/A"  &&  d.TiempoDedicacion != "N/D")
                {
                    var vinculo = await _dataContext.Vinculos
                        .Where(n => n.Corto.Replace("á", "a")
                                            .Replace("é", "e")
                                            .Replace("í", "i")
                                            .Replace("ó", "o")
                                            .Replace("ú", "u")
                                             ==(d.TiempoDedicacion.Replace("á", "a")
                                                                .Replace("é", "e")
                                                                .Replace("í", "i")
                                                                .Replace("ó", "o")
                                                                .Replace("ú", "u")))
                        .FirstOrDefaultAsync();
                    if (vinculo != null)
                    {
                        docente.id_vinculo = vinculo.Id.ToString();
                        docente.nombreVinculo = vinculo.Nombre;
                    }
                }
                if (d.Recinto != null)
                {
                    var recinto = await _dataContext.Recintos.FirstOrDefaultAsync(c => c.NombreCorto.ToUpper().Trim() == d.Recinto.ToUpper().Trim());
                    if (recinto != null)
                    {
                        docente.id_recinto = recinto.Id.ToString();
                        docente.recinto = recinto.Recinto1;
                        docente.nombre_corto = recinto.NombreCorto;
                    }
                }
                if (d.MaximoNivelAcademico != null)
                {
                    var nivelA = d.MaximoNivelAcademico.Split(" ");
                    foreach (var item in nivelA)
                    {
                        var nivelAcademico = await _dataContext.NivelAcademicos
                      .Where(n => n.Nivel.Replace("á", "a")
                                          .Replace("é", "e")
                                          .Replace("í", "i")
                                          .Replace("ó", "o")
                                          .Replace("ú", "u")
                                          .Contains(item.Replace("á", "a")
                                                              .Replace("é", "e")
                                                              .Replace("í", "i")
                                                              .Replace("ó", "o")
                                                              .Replace("ú", "u")))
                      .FirstOrDefaultAsync();
                        if (nivelAcademico != null)
                        {
                            docente.id_nivel_academico = nivelAcademico.Id.ToString();
                            docente.nivel = nivelAcademico.Nivel;
                            break;
                        }
                    }
                }
                if(accion==1)
                {
                    Docentes.Add(docente);
                }
                else
                {
                    if (docente.tiempoDedicacion == "TC" || docente.tiempoDedicacion == "MT" || docente.tiempoDedicacion == "M" || docente.tiempoDedicacion == "F" || docente.tiempoDedicacion == "A")
                    {
                        Docentes.Add(docente);
                    }
                }
                
               

            }
            return new ServiceResponseData<List<DocenteGetDto>>() { Data = Docentes, Status = 200 };

        }
 

        public async Task<ServiceResponseData<List<DocenteGetDto>>> GetAll()
        {


            try
            {
                FiltroDocentesDto filtro = new FiltroDocentesDto();
                //string b = "Maestría";
                //b = b.Replace('í', 'i');
                string BaseUrl = "https://akademic.isfodosu.edu.do/apiDocente/Docente";
                var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(filtro), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.GetAsync(BaseUrl);
                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    var docentesApi = JsonConvert.DeserializeObject<List<DocenteAmilcaDto>>(jsonResponse);
                    var DocentesLimpio = await CleanData(docentesApi, 1);
                    return new ServiceResponseData<List<DocenteGetDto>>() { Data = DocentesLimpio.Data, Status = 200 };
                }
                return new ServiceResponseData<List<DocenteGetDto>>() { Status = 500 };
            }
            catch (Exception ex)
            {
                return new ServiceResponseData<List<DocenteGetDto>>() { Status = 500, Message = ex.ToString() };
            }
        }

        public async Task<ServiceResponseData<List<DocenteGetDto>>> GetAlla()
        {

            try
            {
                var DocentesList = await _dataContext.Docentereals.Include(c => c.IdRecintoNavigation).Include(c => c.IdVinculoNavigation).Include(c=>c.IdNivelAcademicoNavigation) .ToListAsync();
                List<DocenteGetDto> Docentes = new List<DocenteGetDto>();
                foreach (var d in DocentesList)
                {
                    var docente = new DocenteGetDto();
                    docente.id = d.Id.ToString();
                    if(d.Identificacion.Length > 9)
                    {
                        docente.tipoIdentificacion = "Cédula";
                    }
                    else
                    {
                        docente.tipoIdentificacion = "Pasaporte";
                    }
                   
                    docente.tiempoDedicacion = d.TiempoDedicacion;
                  
                    docente.identificacion = d.Identificacion;
                    docente.nombre = d.Nombre;
                    docente.nacionalidad = d.Nacionalidad;
                    docente.sexo= d.Sexo;
                    docente.id_vinculo = d.IdVinculo.ToString();
                    docente.id_recinto = d.IdRecinto.ToString();
                    docente.id_nivel_academico = d.IdNivelAcademico.ToString();
                    docente.id_recinto = d.IdRecinto.ToString();
                    docente.recinto = d.IdRecintoNavigation.Recinto1;
                    docente.nombre_corto = d.IdRecintoNavigation.NombreCorto;
                    docente.id_vinculo = d.IdVinculoNavigation.Id.ToString();
                    docente.nombreVinculo = d.IdVinculoNavigation.Nombre;
                    docente.nivel = d.IdNivelAcademicoNavigation.Nivel;
                   
                    Docentes.Add(docente);

                }

                    return new ServiceResponseData<List<DocenteGetDto>>() { Data = Docentes, Status = 200 };
             }
                
            catch (Exception)
            {

                return new ServiceResponseData<List<DocenteGetDto>>() { Status = 500 };
            }

            

             }
      
        public async Task<ServiceResponseData<List<DocenteGetDto>>> GetAllFilter(FiltroDocentesDto filtro)
        {
             string b = "Maestría";
             b = b.Replace('í', 'i');
             string BaseUrl = "https://akademic.isfodosu.edu.do/apiDocente/Docente/filter";
             var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(filtro), Encoding.UTF8, "application/json");
             HttpResponseMessage response = await _httpClient.PostAsync(BaseUrl, content);
             if (response.IsSuccessStatusCode)
             {
                 string jsonResponse = await response.Content.ReadAsStringAsync();
                 var docentesApi = JsonConvert.DeserializeObject<List<DocenteAmilcaDto>>(jsonResponse);
                 var DocentesLimpio = await CleanData(docentesApi, 2);
                 return new ServiceResponseData<List<DocenteGetDto>>() { Data = DocentesLimpio.Data, Status = 200 };
             }
             else
             {
                return new ServiceResponseData<List<DocenteGetDto>>(){Status = 400 };
             }

        }

        public async Task<ServiceResponseDataPaginacion<List<DocenteGetDto>>> GetAllPaginacion(FiltroDocentesDto filtro)
        {
            var Docentes = await GetAll();
            int valor = Docentes.Data.Count;
            int CantReg = Docentes.Data.Count;
            int CantRegistrosSolicitado = filtro.elementosPorPagina.Value;
            decimal TotalPage = Convert.ToDecimal(CantReg) / Convert.ToDecimal(CantRegistrosSolicitado);
            foreach (var dpcente in Docentes.Data)
            {
                dpcente.identificacion = dpcente.identificacion == null ? string.Empty : dpcente.identificacion;
            }
            if (filtro.Filtro != null && filtro.Filtro.Trim() != string.Empty)
            {
                var result = Docentes.Data.Where(c => c.identificacion.Contains(filtro.Filtro) || c.nombre.ToUpper().Contains(filtro.Filtro.ToUpper())).OrderBy(c => c.nombre).Skip((filtro.paginaActual.Value - 1) * CantRegistrosSolicitado).Take(CantRegistrosSolicitado).ToList();
                decimal ParteEntera = Math.Truncate(TotalPage);
                if (TotalPage > ParteEntera && (CantRegistrosSolicitado / CantReg) > 1)
                { TotalPage = TotalPage + 1; }
                valor = result.Count;
                return new ServiceResponseDataPaginacion<List<DocenteGetDto>>() { Data = result, Status = 200, TotalPaginas = Convert.ToInt32(TotalPage), TotalRegistros = CantReg };
            }

            var resultNoFilter = Docentes.Data.OrderBy(c => c.nombre).Skip((filtro.paginaActual.Value - 1) * CantRegistrosSolicitado).Take(CantRegistrosSolicitado).ToList();
            decimal ParteEnteraNoFilter = Math.Truncate(TotalPage);
            if (TotalPage > ParteEnteraNoFilter && (CantRegistrosSolicitado / CantReg) > 1)
            { TotalPage = TotalPage + 1; }
            valor = resultNoFilter.Count;
            return new ServiceResponseDataPaginacion<List<DocenteGetDto>>() { Data = resultNoFilter, Status = 200, TotalPaginas = Convert.ToInt32(TotalPage), TotalRegistros = CantReg };

        }

        public async Task<ServiceResponseData<List<DocenteGetDto>>> GetAllRecinto(FiltroDocentesDto filtro, int id)
        {
            try
            {
                var recintoActual = await _dataContext.Recintos.Where(c => c.Id == id).FirstOrDefaultAsync();
                string b = "Maestría";
                b = b.Replace('í', 'i');
                string BaseUrl = $"https://akademic.isfodosu.edu.do/apiDocente/Docente/docentesxrecinto/{recintoActual.NombreCorto}";
                var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(filtro), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.GetAsync(BaseUrl);
                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    var docentesApi = JsonConvert.DeserializeObject<List<DocenteAmilcaDto>>(jsonResponse);
                    var DocentesLimpio = await CleanData(docentesApi,2);
                    return new ServiceResponseData<List<DocenteGetDto>>() { Data = DocentesLimpio.Data, Status = 200 };
                }
                return new ServiceResponseData<List<DocenteGetDto>>() { Status = 500 };
            }
            catch (Exception ex)
            {
                return new ServiceResponseData<List<DocenteGetDto>>() { Status = 500, Message = ex.ToString() };
            }

        }

        public async Task<ServiceResponseDataDocentes<List<DocenteCantidadDto>>> GetDocentexRecinto()
        {
            List<DocenteCantidadDto> ResulDat = new List<DocenteCantidadDto>();
            var Docentes = await GetAll();
            int Cant = 0;
            var Recinto = await _dataContext.Recintos.ToListAsync();
            if (Recinto.Count > 0 && Docentes.Data != null)
            {
                foreach (var recinto in Recinto)
                {
                     DocenteCantidadDto cantidadDto = new DocenteCantidadDto();
                     cantidadDto.nombre_corto = recinto.NombreCorto;
                     cantidadDto.cantidad = Convert.ToString(Docentes.Data.Where(c => c.nombre_corto==recinto.NombreCorto.Trim().ToUpper()).ToList().Count);
                     ResulDat.Add(cantidadDto);
                    Cant += int.Parse(cantidadDto.cantidad);
                }
            }
            return new ServiceResponseDataDocentes<List<DocenteCantidadDto>>() { Data = ResulDat, Status = 200, Total= Cant };

        }

        public async Task<ServiceResponseData<List<NacionalidadDto>>> GetNacionalidades(FiltroDto filtro)
        {
            var Nacionalidades = await _dataContext.Paisesnacionalidades.ToListAsync();
            var DataFilter = _mapper.Map<List<NacionalidadDto>>(Nacionalidades);
            if(filtro.filtro!="" || filtro.filtro!=string.Empty)
            {
                DataFilter = DataFilter.Where(c => c.Nacionalidad.ToUpper().Contains(filtro.filtro.ToUpper()) || c.Pais.ToUpper().Contains(filtro.filtro.ToUpper())).ToList();
            }

            return new ServiceResponseData<List<NacionalidadDto>>() { Data = DataFilter, Status = 200};

        }

      
    }
}
