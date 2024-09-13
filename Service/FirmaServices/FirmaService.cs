using AkademicReport.Data;
using AkademicReport.Dto.FirmaDto;
using AkademicReport.Dto.FirmasDto;
using AkademicReport.Models;
using AkademicReport.Utilities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AkademicReport.Service.FirmaServices
{
    public class FirmaService : IFirmaService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _datacontext;
        public FirmaService(IMapper mapper, DataContext dataContext)
        {
            _mapper= mapper;
            _datacontext= dataContext;
        }
        public async Task<ServicesResponseMessage<string>> Delete(int id)
        {
            try
            {
                var firmasDb = await _datacontext.Firmas.FirstAsync(c => c.IdFirma == id);
                _datacontext.Firmas.Remove(firmasDb);
                await _datacontext.SaveChangesAsync();  
                return new ServicesResponseMessage<string>() { Status = 200, Message="Se elimino un registro"};
            }
            catch (Exception ex)
            {
                return new ServicesResponseMessage<string>() { Status = 500, Message =Msj.MsjError + ex.ToString() };
            }
        }

        public async Task<ServiceResponseData<List<FirmaGetDto>>> GetAll()
        {
            try
            {
                var firmasDb = await _datacontext.Firmas.Include(c=>c.IdRecintoNavigation).ToListAsync();
                return new ServiceResponseData<List<FirmaGetDto>>() { Status = 200, Data = _mapper.Map<List<FirmaGetDto>>(firmasDb), Message=Msj.MsjSucces };
            }
            catch (Exception ex)
            {
                return new ServiceResponseData<List<FirmaGetDto>>() { Status = 500, Message = Msj.MsjError + ex.ToString() };
            }
        }

      
        public async Task<ServiceResponseData<List<FirmaGetDto>>> GetByIdRecinto(int idRecinto)
        {
            try
            {
                var firmasDb = await _datacontext.Firmas.Where(c => c.IdRecinto == idRecinto).Include(c=>c.IdRecintoNavigation).ToListAsync();
                return new ServiceResponseData<List<FirmaGetDto>>() { Status = 200, Data = _mapper.Map<List<FirmaGetDto>>(firmasDb)};
            }
            catch (Exception ex)
            {
                return new ServiceResponseData<List<FirmaGetDto>>() {Status = 500, Message=Msj.MsjError + ex.ToString()};
            }
        }

        public async Task<ServicesResponseMessage<string>> Insert(FirmaDto item)
        {
            try
            {
                _datacontext.Firmas.Add(_mapper.Map<Firma>(item));
                await _datacontext.SaveChangesAsync();
                return new ServicesResponseMessage<string>() { Status = 200, Message = Msj.MsjInsert };
            }
            catch (Exception ex)
            {
                return new ServicesResponseMessage<string>() { Status = 500, Message = Msj.MsjError + ex.ToString() };
            }
        }

        public async Task<ServicesResponseMessage<string>> Update(FirmaDto item)
        {
            try
            {

                var firmadb = await _datacontext.Firmas.FirstAsync(c => c.IdFirma==item.IdFirma);
                firmadb = _mapper.Map<Firma>(item);
                _datacontext.Entry(firmadb).State = EntityState.Modified;
                await _datacontext.SaveChangesAsync();
                return new ServicesResponseMessage<string>() { Status = 200, Message = Msj.MsjUpdate };
            }
            catch (Exception ex)
            {
                return new ServicesResponseMessage<string>() { Status = 500, Message = Msj.MsjError + ex.ToString() };
            };
        }
    }
}
