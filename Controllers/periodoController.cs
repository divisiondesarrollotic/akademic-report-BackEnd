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
        /// <summary>
        /// --Este get trae todos los periodos {"id": "1","periodo": "2023-03"}
        /// /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            return Ok(await _service.GetAll());
        }
        /// <summary>
        /// --Este get trae un periodos por id  {"id": "1","periodo": "2023-03"}
        /// /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            return Ok(await _service.GetById(id));
        }
        /// <summary>
        /// --Este get trae el periodo actual establecido
        /// /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("actual")]
        public async Task<ActionResult> GetPeriodoActual()
        {
            return Ok(await _service.PeriodoActual());
        }

        /// <summary>
        /// --Este post crea un nuevo periodo
        /// /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("insert")]
        public async Task<ActionResult> Create(PeriodoAddDto periodo)
        {
            return Ok(await _service.Insert(periodo));
        }

        /// <summary>
        /// --Este post actualiza el periodo actual
        /// /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("update/actual")]
        public async Task<ActionResult> UpdatePeriodoActual(PeriodoActualUpdateDto periodo)
        {
            return Ok(await _service.ActualizarActual(periodo));
        }
        /// <summary>
        /// --Este post actualiza un periodo
        /// /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("update")]
        public async Task<ActionResult> Update(PeriodoUpdateDto periodo)
        {
            return Ok(await _service.Update(periodo));
        }
        /// <summary>
        /// --Este post elimina un periodo
        /// /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            return Ok(await _service.Delete(id));
        }
    }
}
