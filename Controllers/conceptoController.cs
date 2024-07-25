using AkademicReport.Dto.ConceptoDto;
using AkademicReport.Dto.NivelDto;
using AkademicReport.Service.ConceptoServices;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AkademicReport.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class conceptoController : ControllerBase
    {
        public readonly IConceptoService _service;
        public conceptoController(IConceptoService service)
        {
            _service = service;
        }
        /// <summary>
        /// --Este get trae todos los conceptos
        /// </summary>
        /// <returns></returns>
        [HttpGet("idprograma")]
        public async Task<ActionResult> GetAll([Required]int idprograma)
        {
            return Ok(await _service.GetAll(idprograma));
        }
        /// <summary>
        /// --Este get un concepto por id
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            return Ok(await _service.GetById(id));
        }

        /// <summary>
        /// --Este post agrega un nuevo concepto
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        [Route("insert")]
        public async Task<ActionResult> Create(ConceptoAddDto concepto)
        {
            return Ok(await _service.Insert(concepto));
        }
        /// <summary>
        /// --Este get elimina un concepto
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            return Ok(await _service.Delete(id));
        }
        /// <summary>
        /// --Este post actualiza un concepto
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("update")]
        public async Task<ActionResult> Update(ConceptoGetDto concepto)
        {
            return Ok(await _service.Update(concepto));
        }
    }
}
