using AkademicReport.Data;
using AkademicReport.Dto.PeriodoDto;
using AkademicReport.Models;
using AkademicReport.Utilities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AkademicReport.Service.PeriodoServices
{
    public class PeriodoService : IPeriodoService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _dataContext;
        public PeriodoService(IMapper mapper, DataContext dataContext)
        {
            _mapper = mapper;
            _dataContext = dataContext;
        }

        public async Task<ServicesResponseMessage<string>> ActualizarActual(PeriodoActualActualizarDto periodo)
        {
            try
            {
                var periododb = await _dataContext.Configuracions.AsNoTracking().FirstOrDefaultAsync(c => c.Nombre.ToUpper() == "PERIODO");
                periododb.Valor = periodo.periodo;
                _dataContext.Entry(periododb).State = EntityState.Modified;
                await _dataContext.SaveChangesAsync();
                return new ServicesResponseMessage<string>() { Status = 200, Message = Msj.MsjUpdate };
            }
            catch (Exception ex)
            {
                return new ServicesResponseMessage<string>() { Status = 500 };
            }
        }

        public async Task<ServicesResponseMessage<string>> Delete(int id)
        {
            try
            {
                var periodo = await _dataContext.PeriodoAcademicos.FirstOrDefaultAsync(c => c.Id == id);
                if (periodo == null)
                    return new ServicesResponseMessage<string>() { Status = 204, Message = Msj.MsjNoRegistros };
                _dataContext.Remove(periodo);
                await _dataContext.SaveChangesAsync();
                return new ServicesResponseMessage<string>() { Status = 200, Message = Msj.MsjDelete };
            }
            catch (Exception ex)
            {
                return new ServicesResponseMessage<string>() { Status = 500, Message = Msj.MsjError + ex.ToString };
            }
        }

        public async Task<ServiceResponseData<List<PeriodoGetDto>>> GetAll()
        {
             try
            {
                var periodo = await _dataContext.PeriodoAcademicos.ToListAsync();
                return new ServiceResponseData<List<PeriodoGetDto>>() { Status = 200, Data = _mapper.Map<List<PeriodoGetDto>>(periodo) };
            }
            catch (Exception ex)
            {
                return new ServiceResponseData<List<PeriodoGetDto>>() { Status = 500 };
            }
        }

        public async Task<ServiceResponseData<List<PeriodoGetDto>>> GetById(int id)
        {
            try
            {
                var periodo = await _dataContext.PeriodoAcademicos.Where(c => c.Id == id).ToListAsync(); ;
                return new ServiceResponseData<List<PeriodoGetDto>>() { Status = 200, Data = _mapper.Map<List<PeriodoGetDto>>(periodo)};
            }
            catch (Exception ex)
            {
                return new ServiceResponseData<List<PeriodoGetDto>>() { Status = 500};
            }
        }

        public async Task<ServicesResponseMessage<string>> Insert(PeriodoAddDto item)
        {
            try
            {
         
                _dataContext.PeriodoAcademicos.Add(_mapper.Map<PeriodoAcademico>(item));
                await _dataContext.SaveChangesAsync();
                return new ServicesResponseMessage<string>() { Status = 200, Message = Msj.MsjInsert };
            }
            catch (Exception ex)
            {
                return new ServicesResponseMessage<string>() { Status = 500, Message = Msj.MsjError + ex.ToString() };
            }
        }

        public async Task<ServiceResponseData<List<ConfiguracionGetDto>>> PeriodoActual()
        {
            try
            {
                var periodo = await _dataContext.Configuracions.Where(c=>c.Nombre== "periodo").ToListAsync();
               
                return new ServiceResponseData<List<ConfiguracionGetDto>>() { Status = 200, Data = _mapper.Map<List<ConfiguracionGetDto>>(periodo) };
            }
            catch (Exception ex)
            {
                return new ServiceResponseData<List<ConfiguracionGetDto>>() { Status = 500};
            }
      
        }

        public async Task<ServicesResponseMessage<string>> Update(PeriodoUpdateDto item)
        {
            try
            {
                var periodo = await _dataContext.PeriodoAcademicos.AsNoTracking().FirstOrDefaultAsync(c => c.Id == Convert.ToInt32(item.Id));
                if (periodo == null)
                    return new ServicesResponseMessage<string>() { Status = 204, Message = Msj.MsjNoRegistros };
                periodo = _mapper.Map<PeriodoAcademico>(item);
                _dataContext.Entry(periodo).State = EntityState.Modified;
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
