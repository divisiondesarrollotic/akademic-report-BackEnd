using AkademicReport.Dto.RecintoDto;

namespace AkademicReport.Service.RecintoServices
{
    public interface IRecintoService
    {
        Task<List<RecintoGetDto>> GetAll();
    }
}
