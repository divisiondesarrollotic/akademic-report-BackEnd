using AkademicReport.Data;
using AkademicReport.Dto.DocentesDto;
using AkademicReport.Utilities;
using AutoMapper;
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
        public DocenteService(IMapper mapper, DataContext dataContext)
        {
            _mapper = mapper;
            _dataContext = dataContext;

        }

        public async Task<ServiceResponseData<List<DocenteGetDto>>> GetAll()
        {

            FiltroDocentesDto filtro = new FiltroDocentesDto();
            string b = "Maestría";
            b = b.Replace('í', 'i');
            string BaseUrl = "https://isfodosudocentes.herokuapp.com/get_docente_api";
            var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(filtro), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync(BaseUrl, content);
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var docentesApi = JsonConvert.DeserializeObject<List<DocenteAmilcaDto>>(jsonResponse);

                List<DocenteGetDto> Docentes = new List<DocenteGetDto>();
                foreach (var d in docentesApi)
                {
                    var docente = new DocenteGetDto();
                    docente.id = d._id;
                    docente.tiempoDedicacion = d.tiempo_dedicacion;
                    docente.identificacion = d.cedula_pasaporte;
                    if (d.cedula_pasaporte != null)
                    {
                        if (d.cedula_pasaporte[0].ToString().Trim() == "0" && d.cedula_pasaporte[1].ToString().Trim() == "0" && d.cedula_pasaporte[2].ToString().Trim() == "-" || d.cedula_pasaporte.Length == 13)
                        {
                            docente.identificacion = docente.identificacion.Replace("00-", "");
                            docente.tipoIdentificacion = "Cedula";
                        }
                        else
                        {
                            docente.tipoIdentificacion = "Pasaporte";
                        }
                    }
                   
                    //docente.tipoIdentificacion = d.ti
                    
                    docente.nombre = d.nombre;
                    docente.nacionalidad = d.nacionalidad;
                    docente.sexo = d.sexo;
                    if (d.tiempo_dedicacion != null)
                    {
                        var vinculo = await _dataContext.Vinculos
                            .Where(n => n.Corto.Replace("á", "a")
                                                .Replace("é", "e")
                                                .Replace("í", "i")
                                                .Replace("ó", "o")
                                                .Replace("ú", "u")
                                                .Contains(d.tiempo_dedicacion.Replace("á", "a")
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
                    if (d.recinto != null)
                    {
                        var recinto = await _dataContext.Recintos.FirstOrDefaultAsync(c => c.NombreCorto.ToUpper().Trim() == d.recinto.ToUpper().Trim());
                        if (recinto != null)
                        {
                            docente.id_recinto = recinto.Id.ToString();
                            docente.recinto = recinto.Recinto1;
                            docente.nombre_corto = recinto.NombreCorto;
                        }
                    }

                    if (d.maximo_nivel_academico != null)
                    {
                        var nivelA = d.maximo_nivel_academico.Split(" ");
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


                    Docentes.Add(docente);

                }

                return new ServiceResponseData<List<DocenteGetDto>>() { Data = Docentes, Status = 200 };

            }
            return new ServiceResponseData<List<DocenteGetDto>>() { Status = 500 };


        }

        public async Task<ServiceResponseDataPaginacion<List<DocenteGetDto>>> GetAllPaginacion(FiltroDocentesDto filtro)
        {

            var Docentes = await GetAll();
            int valor = Docentes.Data.Count;
            var DataFilter = Docentes.Data = Docentes.Data.OrderBy(c => c.nombre).Skip(filtro.paginaActual.Value).Take(filtro.elementosPorPagina.Value).ToList();
            if (filtro.Filtro != "" && filtro.Filtro!=null)
            {
                DataFilter = DataFilter.Where(c => c.identificacion.Contains(filtro.Filtro)).ToList();
                valor = DataFilter.Count;

            }
            return new ServiceResponseDataPaginacion<List<DocenteGetDto>>(){Data = DataFilter, Status = 200, TotalPaginas = valor/filtro.elementosPorPagina};
        }

        public async Task<ServiceResponseData<List<DocenteGetDto>>> GetAllRecinto(FiltroDocentesDto filtro, int id)
        {
            List<DocenteGetDto> ResulDat = new List<DocenteGetDto>();
            var Docentes = await GetAll();
            var Recintos = await _dataContext.Recintos.ToListAsync();
            string Nombre_Corto = Recintos.Where(c => c.Id == id).FirstOrDefault().NombreCorto;
            ResulDat = Docentes.Data.Where(c => c.nombre_corto == Nombre_Corto).ToList();
            if(filtro.Filtro!="" || filtro.Filtro!=string.Empty)
            {
                ResulDat = ResulDat.Where(c => c.nombre.ToUpper().Contains(filtro.Filtro) || c.identificacion.ToUpper().Contains(filtro.Filtro)).ToList();
            }
            return new ServiceResponseData<List<DocenteGetDto>>() { Data = ResulDat, Status = 200 };

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
