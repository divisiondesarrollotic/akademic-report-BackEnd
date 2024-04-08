using AkademicReport.Dto.RecintoDto;

namespace AkademicReport.Service.RecintoServices
{
    public interface IRecintoService
    {
        Task<ServiceResponseData<List<RecintoGetDto>>> GetAll();
    }
}
