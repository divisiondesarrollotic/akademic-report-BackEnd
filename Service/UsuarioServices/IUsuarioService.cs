using AkademicReport.Dto.UsuarioDto;
using AkademicReport.Models;
using Microsoft.OpenApi.Any;

namespace AkademicReport.Service.UsuarioServices
{
    public interface IUsuarioService
    {
        Task<ServiceResponseData<List<UsuarioGetDto>>> GetAll();
        Task<ServiceResponseData<UsuarioGetDto>> GetById(int id);
        Task<ServiceResponseData<List<NivelUsuario>>> GetNiveles();
        Task<ServiceResponseData<List<UsuarioGetDto>>> GetByIdRecinto(int id);
        Task<ServicesResponseMessage<string>> Create(UsuarioAddDto usuario);
        Task<ServicesResponseMessage<string>> Update(UsuarioUpdateDto usuario);
        Task<ServicesResponseMessage<string>> UpdatePassword(UpdatePasswordDto password);
        Task<ServicesResponseMessage<string>> ResetPassword(int idUsuario);
        Task<ServicesResponseMessage<string>> Delete(int id);
        Task<ServisResponseLogin<List<UsuarioGetDto>, string>> Login(UsuarioCredentialsDto credentials);



    }
}
