using AkademicReport.Data;
using AkademicReport.Dto.AulaDto;
using AkademicReport.Dto.NivelDto;
using AkademicReport.Dto.PeriodoDto;
using AkademicReport.Models;
using AkademicReport.Utilities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace AkademicReport.Service.AulaServices
{
    public class AulaService : IAulaService
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;
        public AulaService(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper=mapper;
        }

        public async Task<ServicesResponseMessage<string>> Delete(int id)
        {
            try
            {
                // Verificacion si existe 
                var carga = await _dataContext.CargaDocentes.Where(c => c.Id == id).FirstOrDefaultAsync();
                if (carga != null)
                {
                    return new ServicesResponseMessage<string>() { Status = 204, Message=Msj.MsjNoDelete };
                }
                var aula = await _dataContext.Aulas.FirstOrDefaultAsync(c => c.Id == id);
                if (aula==null)
                {
                    return new ServicesResponseMessage<string>() { Status = 204, Message = Msj.MsjNoData };
                }
                _dataContext.Aulas.Remove(aula);
                await _dataContext.SaveChangesAsync();
                return new ServicesResponseMessage<string>() { Status = 200, Message = Msj.MsjDelete };
            }
            catch (Exception ex)
            {
                return new ServicesResponseMessage<string>() { Status = 500, Message = Msj.MsjError + ex.ToString() };
            }
        }

      

        public  async Task<ServiceResponseData<List<AulaGettDto>>> GetAllByIdRecinto(int id)
        {
            try
            {
                var aulas = await _dataContext.Aulas.Where(c=>c.Idrecinto==id).Include(c=>c.IdrecintoNavigation).ToListAsync();
                if (aulas.Count < 1)
                    return new ServiceResponseData<List<AulaGettDto>>() {Data = _mapper.Map<List<AulaGettDto>>(aulas),  Status = 204 };
                return new ServiceResponseData<List<AulaGettDto>>() { Status = 200, Data =_mapper.Map<List<AulaGettDto>>(aulas) };
            }
            catch (Exception ex)
            {
                return new ServiceResponseData<List<AulaGettDto>>() { Status = 500 };
            }
        }

        public async Task<ServiceResponseData<AulaGettDto>> GetById(int id)
        {
            try
            {
                var aulas = await _dataContext.Aulas.FirstOrDefaultAsync(c => c.Id == id);
                if (aulas == null)
                    return new ServiceResponseData<AulaGettDto>() { Status = 204, Message=Msj.MsjNoData };
                return new ServiceResponseData<AulaGettDto>() { Status = 200, Data = _mapper.Map<AulaGettDto>(aulas) };
            }
            catch (Exception ex)
            {
                return new ServiceResponseData<AulaGettDto>() { Status = 500 };
            }
        }

        public async Task<ServicesResponseMessage<string>> Insert(AulaDto item)
        {
            try
            {
                var AulaMap = _mapper.Map<Aula>(item);
                _dataContext.Aulas.Add(AulaMap);
                await _dataContext.SaveChangesAsync();
                return new ServicesResponseMessage<string>() { Status = 200, Message =Msj.MsjInsert };
            }
            catch (Exception ex)
            {
                return new ServicesResponseMessage<string>() { Status = 500, Message =Msj.MsjError + ex.ToString() };
            }
        }

        public async Task<ServicesResponseMessage<string>> Update(AulaDto item)
        {
            try
            {
                var AulaDb = await _dataContext.Aulas.AsNoTracking().FirstOrDefaultAsync(c => c.Id == item.Id);
                var AulaMap = _mapper.Map<Aula>(item);
                _dataContext.Entry(AulaMap).State = EntityState.Modified; 
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
