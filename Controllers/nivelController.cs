using AkademicReport.Dto.NivelDto;
using AkademicReport.Dto.PeriodoDto;
using AkademicReport.Service.NivelServices;
using Microsoft.AspNetCore.Mvc;

namespace AkademicReport.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class nivelController : ControllerBase
    {
        private readonly INivelAcademicoService _service;
        public nivelController(INivelAcademicoService service)
        {
            _service = service;
        }

        /// <summary>
        /// --Este get trae los niveles academico  y sus precios { "id": "1", "nivel": "Maestria", "pagoHora": "780"}
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            return Ok(await _service.GetAll());
        }
        /// <summary>
        /// --Este get trae los niveles academico  y sus precios  por id.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            return Ok(await _service.GetById(id));
        }
        /// <summary>
        /// --Este post crea un nuevo nivel
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("insert")]
        public async Task<ActionResult> Create(NivelAddDto nivel)
        {
            return Ok(await _service.Insert(nivel));
        }
        /// <summary>
        /// --Este post actualiza un  nivel
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("update")]
        public async Task<ActionResult> Update(NivelUpdateDto nivel)
        {
            return Ok(await _service.Update(nivel));
        }

        /// <summary>
        /// --Este elimina un nivel
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            return Ok(await _service.Delete(id));
        }
    }
}
