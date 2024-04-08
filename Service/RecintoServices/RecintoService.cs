using AkademicReport.Data;
using AkademicReport.Dto.RecintoDto;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace AkademicReport.Service.RecintoServices
{
    public class RecintoService : IRecintoService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _dataContext;
        public RecintoService(IMapper mapper, DataContext dataContext) { _mapper = mapper; _dataContext = dataContext; }
        public async Task<ServiceResponseData<List<RecintoGetDto>>> GetAll()
        {
            
            var recintos= await _dataContext.Recintos.ProjectTo<RecintoGetDto>(_mapper.ConfigurationProvider).ToListAsync();
            return new ServiceResponseData<List<RecintoGetDto>>() { Data = recintos, Status=200};
        
        }
    }
}
