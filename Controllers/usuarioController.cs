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
        /// <summary>
        /// --Este es un post de inicio de sesión
        ///  </summary>
        /// <returns></returns>
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
        /// <summary>
        /// --Este es un post crea un nuevo usuario
        ///  </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("insert")]
        public  async Task<ActionResult>Create(UsuarioAddDto usuario)
        {
            return Ok(await _service.Create(usuario));
        }
        /// <summary>
        /// --Este es un post actualiza un usuario
        ///  </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("update")]
        public async Task<ActionResult> Update(UsuarioUpdateDto usuario)
        {
            return Ok(await _service.Update(usuario));
        }
        // <summary>
        /// --Este es un post actualiza la contraseña
        ///  </summary>
        /// <returns></returns>

        [HttpPost]
        [Route("updatepassword")]
        public async Task<ActionResult> UpdatePassword(UpdatePasswordDto password)
        {
            return Ok(await _service.UpdatePassword(password));
        }

        // <summary>
        /// --Este es un post para resetear la contraseña a una por default (Issu1234)
        ///  </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("resetpassword")]
        public async Task<ActionResult> ResetPassword([FromBody] int idUsuario)
        {
            return Ok(await _service.ResetPassword(idUsuario));
        }

        // <summary>
        /// --Este es un post elimina un usuario
        ///  </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            return Ok(await _service.Delete(id));
        }


        // <summary>
        /// --Este es un get que trae todos los niveles de acceso del sistema
        ///  </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("getniveles")]
        public async Task<ActionResult> GetNiveles()
        {
            return Ok(await _service.GetNiveles());
        }

        // <summary>
        /// --Este get trae los usuarios por recinto
        ///  </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("recinto/{id}")]
        public async Task<ActionResult> GetByIdRectino(int id)
        {
            return Ok(await _service.GetByIdRecinto(id));
        }
        // <summary>
        /// --Este get trae los usuarios por id
        ///  </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            return Ok(await _service.GetById(id));
        }

        // <summary>
        /// --Este get trae todos los usuarios
        ///  </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            return Ok(await _service.GetAll());
        }
    }
}
