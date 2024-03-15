using AkademicReport.Data;
using AkademicReport.Dto.AulaDto;
using AkademicReport.Dto.NivelDto;
using AkademicReport.Models;
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
        
        public  async Task<ServiceResponseData<List<AulaGettDto>>> GetAllByIdRecinto(int id)
        {
            try
            {
                var aulas = await _dataContext.Aulas.Where(c=>c.Idrecinto==id).ToListAsync();
                if (aulas.Count < 1)
                    return new ServiceResponseData<List<AulaGettDto>>() { Status = 204 };
                return new ServiceResponseData<List<AulaGettDto>>() { Status = 200, Data =_mapper.Map<List<AulaGettDto>>(aulas) };
            }
            catch (Exception ex)
            {
                return new ServiceResponseData<List<AulaGettDto>>() { Status = 500 };
            }
        }
    }
}
