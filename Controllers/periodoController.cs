using AkademicReport.Dto.PeriodoDto;
using AkademicReport.Dto.UsuarioDto;
using AkademicReport.Service.PeriodoServices;
using Microsoft.AspNetCore.Mvc;

namespace AkademicReport.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class periodoController : ControllerBase
    {
        private readonly IPeriodoService _service;
        public periodoController(IPeriodoService service)
        {
            _service = service;
        }
 
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            return Ok(await _service.GetAll());
        }
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            return Ok(await _service.GetById(id));
        }
        [HttpGet]
        [Route("actual")]
        public async Task<ActionResult> GetPeriodoActual()
        {
            return Ok(await _service.PeriodoActual());
        }
        [HttpPost]
        [Route("insert")]
        public async Task<ActionResult> Create(PeriodoAddDto periodo)
        {
            return Ok(await _service.Insert(periodo));
        }
        [HttpPost]
        [Route("update/actual")]
        public async Task<ActionResult> UpdatePeriodoActual(PeriodoActualActualizarDto periodo)
        {
            return Ok(await _service.ActualizarActual(periodo));
        }
        [HttpPost]
        [Route("update")]
        public async Task<ActionResult> Update(PeriodoUpdateDto periodo)
        {
            return Ok(await _service.Update(periodo));
        }
        [HttpGet]
        [Route("delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            return Ok(await _service.Delete(id));
        }
    }
}
