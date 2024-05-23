using AkademicReport.Dto.UsuarioDto;
using AkademicReport.Service;
using AkademicReport.Service.UsuarioServices;
using AkademicReport.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace AkademicReport.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class usuarioController : ControllerBase
    {
        private readonly IUsuarioService _service;
        public usuarioController(IUsuarioService service)
        {
            _service = service;
        }
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> Login(UsuarioCredentialsDto credentials)
        {
            var result = await _service.Login(credentials);
            if(result.Message.Value.Item1.Count>0)
            {
                return Ok(new ServicesResponseMessage<List<UsuarioGetDto>>(){Status=200, Message = result.Message.Value.Item1, LoginResetPassword=credentials.contra=="Issu1234"? true : false });
            }
            return Ok(new ServicesResponseMessage<string>() { Status = 204, Message = Msj.MsjCredencialesIncorrectas }); 
        
        }
        [HttpPost]
        [Route("insert")]
        public  async Task<ActionResult>Create(UsuarioAddDto usuario)
        {
            return Ok(await _service.Create(usuario));
        }
        [HttpPost]
        [Route("update")]
        public async Task<ActionResult> Update(UsuarioUpdateDto usuario)
        {
            return Ok(await _service.Update(usuario));
        }

        [HttpPost]
        [Route("updatepassword")]
        public async Task<ActionResult> UpdatePassword(UpdatePasswordDto password)
        {
            return Ok(await _service.UpdatePassword(password));
        }
        [HttpPost]
        [Route("resetpassword")]
        public async Task<ActionResult> ResetPassword([FromBody] int idUsuario)
        {
            return Ok(await _service.ResetPassword(idUsuario));
        }
        [HttpGet]
        [Route("delete")]
        public async Task<ActionResult> Delete(int id)
        {
            return Ok(await _service.Delete(id));
        }
        [HttpGet]
        [Route("recinto/{id}")]
        public async Task<ActionResult> GetByIdRectino(int id)
        {
            return Ok(await _service.GetByIdRecinto(id));
        }
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            return Ok(await _service.GetById(id));
        }
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            return Ok(await _service.GetAll());
        }
    }
}
