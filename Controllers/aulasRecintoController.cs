using AkademicReport.Service;
using AkademicReport.Service.AulaServices;
using Microsoft.AspNetCore.Mvc;

namespace AkademicReport.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class aulasRecintoController : ControllerBase
    {
        private readonly IAulaService _service;
        public aulasRecintoController(IAulaService service)
        {
            _service = service;
        }

        /// <summary>
        /// --Este get trae todas las aula de un recinto
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult> GetAllAylasByIdRecinto(int id) 
        {
            return Ok(await _service.GetAllByIdRecinto(id));
        }
    }
}
