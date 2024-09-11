using AkademicReport.Dto.PeriodoDto;
using AkademicReport.Models;

namespace AkademicReport.Service.PeriodoServices
{
    public interface IPeriodoService
    {
        Task<ServiceResponseData<List<PeriodoGetDto>>> GetAll();
        Task<ServiceResponseData<List<PeriodoGetDto>>> GetById(int id);
        Task<ServicesResponseMessage<string>> Insert(PeriodoAddDto item);
        Task<ServicesResponseMessage<string>> Update(PeriodoUpdateDto item);
        Task<ServicesResponseMessage<string>> Delete(int id);   
        Task<ServiceResponseData<List<PeriodoGetDto>>> PeriodoActual();
        Task<ServiceResponseData<List<PeriodoGetDto>>> PeriodoActualPosgrado();
        Task<ServicesResponseMessage<string>> ActualizarActual(PeriodoActualUpdateDto periodo);
        Task<ServicesResponseMessage<string>> ActualizarActualPosgrado(PeriodoActualUpdateDto periodo);



    }
}
