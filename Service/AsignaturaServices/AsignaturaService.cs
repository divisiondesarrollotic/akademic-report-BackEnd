using AkademicReport.Data;
using AkademicReport.Dto.AsignaturaDto;
using AkademicReport.Dto.NivelDto;
using AkademicReport.Models;
using AkademicReport.Utilities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
                var codigos = await _dataContext.Codigos.Include(c=>c.IdConceptoNavigation).ToListAsync();
                var CodigoMap = _mapper.Map<List<AsignaturaGetDto>>(codigos);
                foreach (var item in CodigoMap)
                {
                    item.Modalida = codigos.Where(c => c.Id == int.Parse(item.Id)).First().Modalida.Split(',').ToList(); 
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
            Data.Data = Data.Data.Where(c => c.Id_concepto == idConcepto.ToString()).ToList();
            return Data;
        }

        public async Task<ServiceResponseDataPaginacion<List<AsignaturaGetDto>>> GetAllPaginacion(AsignaturaPaginacionDto paginacion)
        {
            try
            {
                var codigos = await _dataContext.Codigos.Include(c => c.IdConceptoNavigation).ToListAsync();
                var CodigoMap = _mapper.Map<List<AsignaturaGetDto>>(codigos).OrderBy(c=>c.Id).Skip(paginacion.paginaActual).Take(paginacion.elementosPorPagina).ToList();
                foreach (var item in CodigoMap)
                {
                    item.Modalida = codigos.Where(c => c.Id == int.Parse(item.Id)).First().Modalida.Split(',').ToList();
                }
            
                if (codigos.Count < 1)
                    return new ServiceResponseDataPaginacion<List<AsignaturaGetDto>>() { Status = 204 };
                return new ServiceResponseDataPaginacion<List<AsignaturaGetDto>>() { Status = 200, Data = CodigoMap, TotalPaginas=codigos.Count / paginacion.elementosPorPagina };
            }
            catch (Exception ex)
            {
                return new ServiceResponseDataPaginacion<List<AsignaturaGetDto>>() { Status = 500 };
            }
        }

        public async Task<ServiceResponseData<List<AsignaturaGetDto>>> GetById(int id)
        {
            try
            {
                var codigos = await _dataContext.Codigos.Include(c=>c.IdConceptoNavigation).ToListAsync();
                var CodigoMap = _mapper.Map<List<AsignaturaGetDto>>(codigos);
                foreach (var item in CodigoMap)
                {
                    item.Modalida = codigos.Where(c => c.Id == int.Parse(item.Id)).First().Modalida.Split(',').ToList();
                }

                if (CodigoMap.Count < 1)
                    return new ServiceResponseData<List<AsignaturaGetDto>>() { Status = 204 };
                return new ServiceResponseData<List<AsignaturaGetDto>>() { Status = 200, Data = _mapper.Map<List<AsignaturaGetDto>>(CodigoMap.Where(c => c.Id ==  id.ToString())) };
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
                //var codigos = await _dataContext.Codigos.FirstOrDefaultAsync(c => c.Nivel.ToUpper() == item.Nivel.ToUpper());
                //if (codigos != null)
                //    return new ServicesResponseMessage<string>() { Status = 204, Message = Msj.MsjUsuarioExiste };
                _dataContext.Codigos.Add(_mapper.Map<Codigo>(item));
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
                var nivel = await _dataContext.Codigos.FirstOrDefaultAsync(c => c.Id == Convert.ToInt32(item.Id));
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
