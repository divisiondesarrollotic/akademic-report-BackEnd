using AkademicReport.Data;
using AkademicReport.Dto.AsignaturaDto;
using AkademicReport.Dto.DocentesDto;
using AkademicReport.Dto.NivelDto;
using AkademicReport.Models;
using AkademicReport.Utilities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Drawing;
using System.Linq;

namespace AkademicReport.Service.AsignaturaServices
{
    public class AsignaturaService : IAsignaturaService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _dataContext;
        public AsignaturaService(IMapper mapper, DataContext dataContext)
        {
            _mapper = mapper;
            _dataContext = dataContext;
        }


        public async Task<ServicesResponseMessage<string>> Delete(int id)
        {

            try
            {
                var codigos = await _dataContext.Codigos.FirstOrDefaultAsync(c => c.Id == id);
                var tipoCaragra = await _dataContext.TipoCargaCodigos.Where(c => c.IdCodigo == id).ToListAsync();
                var modalidades = await _dataContext.TipoModalidadCodigos.Where(c => c.Idcodigo == id).ToListAsync();
                if (tipoCaragra.Count > 0)
                {
                    _dataContext.TipoCargaCodigos.RemoveRange(tipoCaragra);
                }
                if (modalidades.Count > 0)
                {
                    _dataContext.TipoModalidadCodigos.RemoveRange(modalidades);
                }

                if (codigos == null)
                    return new ServicesResponseMessage<string>() { Status = 204, Message = Msj.MsjNoRegistros };
                _dataContext.Codigos.Remove(codigos);
                await _dataContext.SaveChangesAsync();
                return new ServicesResponseMessage<string>() { Status = 200, Message = Msj.MsjDelete };
            }
            catch (Exception ex)
            {
                return new ServicesResponseMessage<string>() { Status = 500, Message = Msj.MsjError + ex.ToString };
            }
        }

        public async Task<ServiceResponseData<List<AsignaturaGetDto>>> GetAll()
        {
            try
            {
                var codigos = await _dataContext.Codigos.Include(c => c.TipoCargaCodigos).ThenInclude(c => c.IdTipoCargaNavigation).Include(c => c.IdConceptoNavigation).Include(c=>c.TipoModalidadCodigos).ToListAsync();

                var CodigoMap = _mapper.Map<List<AsignaturaGetDto>>(codigos);
                foreach (var item in CodigoMap)
                {
                        var a = codigos.FirstOrDefault(c => c.Id == item.Id).TipoCargaCodigos.Where(c => c.IdCodigo == item.Id).ToList();
                        
                        if (item.TiposCargas != null && item.TiposCargas.Any())
                        {
                            item.TiposCargas = new List<TipoCargaDto>();
                            foreach (var element in a)
                            {
                                var tipoCarg = new TipoCargaDto();
                                tipoCarg.Id = element.IdTipoCargaNavigation.Id;
                                tipoCarg.Nombre = element.IdTipoCargaNavigation.Nombre;
                                item.TiposCargas.Add(tipoCarg);
                            }
                            
                        }
                        else
                        {
                            var tipos = new List<TipoCargaDto>();
                            var tipo = new TipoCargaDto() { Id = 0, Nombre = "NO ASIGNADO" };
                            tipos.Add(tipo);
                            item.TiposCargas = tipos;
                        }
                    var tiposModalidad = await _dataContext.TipoModalidadCodigos.Where(c => c.Idcodigo == item.Id).Include(c => c.IdTipoModalidadNavigation).ToListAsync();
                    foreach (var i in tiposModalidad)
                    {
                        var TipoModalida = new TipoModalidadDto();
                        TipoModalida =_mapper.Map<TipoModalidadDto>(i.IdTipoModalidadNavigation);
                        item.Modalidades.Add(TipoModalida);
                    }
                }

                if (codigos.Count < 1)
                    return new ServiceResponseData<List<AsignaturaGetDto>>() { Status = 204 };
                return new ServiceResponseData<List<AsignaturaGetDto>>() { Status = 200, Data = CodigoMap };
            }
            catch (Exception ex)
            {
                return new ServiceResponseData<List<AsignaturaGetDto>>() { Status = 500 };
            }
        }

        public async Task<ServiceResponseData<List<AsignaturaGetDto>>> GetAllByIdConcepto(int idConcepto)
        {
            var Data = await GetAll();
            Data.Data = Data.Data.Where(c => c.Id_concepto == idConcepto).ToList();
            return Data;
        }

        public async Task<ServiceResponseData<List<AsignaturaGetDto>>> GetAllFilter(string filtro)
        {
            try
            {
                var codigos = await _dataContext.Codigos.Where(c => c.Codigo1.ToUpper().Contains(filtro.ToUpper().Trim()) || c.Nombre.ToUpper().Contains(filtro.ToUpper())).Include(c => c.TipoCargaCodigos).ThenInclude(c=>c.IdTipoCargaNavigation).Include(c => c.IdConceptoNavigation).Include(c=>c.TipoModalidadCodigos).ToListAsync();

                var CodigoMap = _mapper.Map<List<AsignaturaGetDto>>(codigos);
                foreach (var item in CodigoMap)
                {

                    var a = codigos.FirstOrDefault(c => c.Id == item.Id).TipoCargaCodigos.Where(c => c.IdCodigo == item.Id).ToList(); 
                    if (item.TiposCargas != null && item.TiposCargas.Any())
                    {
                        item.TiposCargas = new List<TipoCargaDto>();
                        foreach (var element in a)
                        {
                            var tipoCarg = new TipoCargaDto();
                            tipoCarg.Id = element.IdTipoCargaNavigation.Id;
                            tipoCarg.Nombre = element.IdTipoCargaNavigation.Nombre;
                            item.TiposCargas.Add(tipoCarg);
                        }

                    }
                    else
                    {
                        var tipos = new List<TipoCargaDto>();
                        var tipo = new TipoCargaDto() { Id = 0, Nombre = "NO ASIGNADO" };
                        tipos.Add(tipo);
                        item.TiposCargas = tipos;

                    }

                    var tiposModalidad = await _dataContext.TipoModalidadCodigos.Where(c => c.Idcodigo == item.Id).Include(c => c.IdTipoModalidadNavigation).ToListAsync();
                    foreach (var i in tiposModalidad)
                    {
                        var TipoModalida = new TipoModalidadDto();
                        TipoModalida = _mapper.Map<TipoModalidadDto>(i.IdTipoModalidadNavigation);
                        item.Modalidades.Add(TipoModalida);
                    }


                }

                if (codigos.Count < 1)
                    return new ServiceResponseData<List<AsignaturaGetDto>>() { Data=new List<AsignaturaGetDto>(), Status = 204 };
                return new ServiceResponseData<List<AsignaturaGetDto>>() { Status = 200, Data = CodigoMap };
            }
            catch (Exception ex)
            {
                return new ServiceResponseData<List<AsignaturaGetDto>>() { Status = 500 };
            }
        }

        public async Task<ServiceResponseDataPaginacion<List<AsignaturaGetDto>>> GetAllPaginacion(FiltroDocentesDto filtro)
        {
            try
            {
                var codigos = await GetAll();
              //  var CodigoMap = _mapper.Map<List<AsignaturaGetDto>>(codigos).OrderBy(c=>c.Id).Skip(filtro.paginaActual.Value-1 * filtro.elementosPorPagina.Value).Take(filtro.elementosPorPagina.Value).ToList();
                int CantReg = codigos.Data.Count;
                int CantRegistrosSolicitado = filtro.elementosPorPagina.Value;
                decimal TotalPage = Convert.ToDecimal(CantReg) / Convert.ToDecimal(CantRegistrosSolicitado);
                var result = new List<AsignaturaGetDto>();
                if(filtro.Filtro!=string.Empty)
                {
                    result = _mapper.Map<List<AsignaturaGetDto>>(codigos.Data).Where(c=>c.Codigo!.ToUpper().Contains(filtro.Filtro!.ToUpper().Trim())
                    || c.Nombre!.ToUpper().Contains(filtro.Filtro.ToUpper().ToUpper().Trim()) || c.NombreConcepto!.ToUpper().Contains(filtro.Filtro.Trim())).OrderBy(c => c.Nombre).Skip((filtro.paginaActual!.Value - 1) * CantRegistrosSolicitado).Take(CantRegistrosSolicitado).ToList();

                }
                else
                {
                    result = _mapper.Map<List<AsignaturaGetDto>>(codigos.Data).OrderBy(c => c.Nombre).Skip((filtro.paginaActual!.Value - 1) * CantRegistrosSolicitado).Take(CantRegistrosSolicitado).ToList();

                }
                decimal ParteEntera = Math.Truncate(TotalPage);
                if (TotalPage > ParteEntera && (CantRegistrosSolicitado / CantReg) > 1)
                { TotalPage = TotalPage + 1; }
                //foreach (var item in result)
                //{
                //    item.Modalida = codigos.Data.Where(c => c.Id == item.Id).First().Modalida.ToString().Split(',').ToList();
                //}
            
                if (codigos.Data.Count < 1)
                    return new ServiceResponseDataPaginacion<List<AsignaturaGetDto>>() { Status = 204 };
                return new ServiceResponseDataPaginacion<List<AsignaturaGetDto>>() { Status = 200, Data = result, TotalPaginas=codigos.Data.Count / filtro.elementosPorPagina, TotalRegistros = codigos.Data.Count };
            }
            catch (Exception ex)
            {
                return new ServiceResponseDataPaginacion<List<AsignaturaGetDto>>() { Status = 500 };
            }
        }

        public async Task<ServiceResponseData<List<TipoModalidadDto>>> GetAllTipoModalid()
        {
            try
            {
                var Modalidad = await _dataContext.TipoModalidads.ToListAsync();
                var ModalidadMap = _mapper.Map<List<TipoModalidadDto>>(Modalidad);
                if(ModalidadMap.Count<1)
                {
                    return new ServiceResponseData<List<TipoModalidadDto>>() { Status = 204 };

                }
          
                return new ServiceResponseData<List<TipoModalidadDto>>() { Status = 200, Data = ModalidadMap };
            }
            catch (Exception ex)
            {
                return new ServiceResponseData<List<TipoModalidadDto>>() { Status = 500 };
            }
        }
    

        public async Task<ServiceResponseData<List<AsignaturaGetDto>>> GetById(int id)
        {
            try
            {
                var codigos = await GetAll();
                 var data  = codigos.Data.Where(c => c.Id==id).ToList();
                var CodigoMap = _mapper.Map<List<AsignaturaGetDto>>(data);
                //foreach (var item in CodigoMap)
                //{
                //    item.Modalida = data.Where(c => c.Id == item.Id).First().Modalida.ToString().Split(',').ToList();
                //}

                if (CodigoMap.Count < 1)
                    return new ServiceResponseData<List<AsignaturaGetDto>>() { Status = 204 };
                return new ServiceResponseData<List<AsignaturaGetDto>>() { Status = 200, Data = _mapper.Map<List<AsignaturaGetDto>>(CodigoMap.Where(c => c.Id ==  id)) };
            }
            catch (Exception ex)
            {
                return new ServiceResponseData<List<AsignaturaGetDto>>() { Status = 500 };
            }
        }

        public async Task<ServicesResponseMessage<string>> Insert(AsignaturaAddDto item)
        {
            try
            {
                EntityEntry<Codigo> result =  _dataContext.Codigos.Add(_mapper.Map<Codigo>(item));
                await _dataContext.SaveChangesAsync();
                foreach (var i in item.TiposCargas)
                {
                    TipoCargaPivot Privot = new TipoCargaPivot();
                    Privot.IdTipoCarga = i.Id;
                    Privot.IdCodigo = result.Entity.Id;
                    _dataContext.TipoCargaCodigos.Add(_mapper.Map<TipoCargaCodigo>(Privot));

                }
                foreach (var modalidad in item.Modalidades)
                {
                    TipoModalidadCodigo Privot = new TipoModalidadCodigo();
                    Privot.IdTipoModalidad = modalidad.Id;
                    Privot.Idcodigo = result.Entity.Id;
                    _dataContext.TipoModalidadCodigos.Add(Privot);
                }
                await _dataContext.SaveChangesAsync();
                return new ServicesResponseMessage<string>() { Status = 200, Message = Msj.MsjInsert };
            }
            catch (Exception ex)
            {
                return new ServicesResponseMessage<string>() { Status = 500, Message = Msj.MsjError + ex.ToString() };
            }
        }

        public async Task<ServicesResponseMessage<string>> Update(AsignaturaUpdateDto item)
        {
            try
            {
                var nivel = await _dataContext.Codigos.AsNoTracking().FirstOrDefaultAsync(c => c.Id == Convert.ToInt32(item.Id));
                var tipoCargas = await _dataContext.TipoCargaCodigos.Where(c => c.IdCodigo == item.Id).ToListAsync();
                var modalidades = await _dataContext.TipoModalidadCodigos.Where(c=>c.Idcodigo==item.Id).ToListAsync();  
               if(tipoCargas.Count>0)
                {
                    _dataContext.TipoCargaCodigos.RemoveRange(tipoCargas);

                }
                if (modalidades.Count > 0)
                {
                    _dataContext.TipoModalidadCodigos.RemoveRange(modalidades);

                }


                foreach (var i in item.TiposCargas)
                {
                    TipoCargaCodigo Privot = new TipoCargaCodigo();
                    Privot.IdTipoCarga = i.Id;
                    Privot.IdCodigo = item.Id;
                    _dataContext.TipoCargaCodigos.Add(Privot);
                }
                foreach (var modalidad in item.Modalidades)
                {
                    TipoModalidadCodigo Privot = new TipoModalidadCodigo();
                    Privot.IdTipoModalidad = modalidad.Id;
                    Privot.Idcodigo = item.Id; ;
                    _dataContext.TipoModalidadCodigos.Add(Privot);
                }
                nivel = _mapper.Map<Codigo>(item);
                _dataContext.Entry(nivel).State = EntityState.Modified;
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
