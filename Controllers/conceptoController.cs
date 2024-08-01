using AkademicReport.Dto.CargaDto;
using AkademicReport.Dto.ConceptoDto;
using AkademicReport.Dto.ConceptoPosgradoDto;
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
        [HttpGet]
        [Route("getall/{idprograma}")]
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



        [HttpGet]
        [Route("gestall_posgrado")]
        public async Task<ActionResult> GetAllPosgrado()
        {
            return Ok(await _service.GetAllPos());
        }
        /// <summary>
        /// --Este get un concepto por id
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("getbyid_posgrado/{id}")]
        public async Task<ActionResult> GetByIdPosgrado(int id)
        {
            return Ok(await _service.GetByIdPos(id));
        }

        /// <summary>
        /// --Este post agrega un nuevo concepto
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        [Route("insertposgrado")]
        public async Task<ActionResult> CreatePosgrado(ConceptoPosDto concepto)
        {
            return Ok(await _service.InsertPos(concepto));
        }
        /// <summary>
        /// --Este get elimina un concepto
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete_posgrado/{id}")]
        public async Task<ActionResult> DeletePos(int id)
        {
            return Ok(await _service.DeletePos(id));
        }
        /// <summary>
        /// --Este post actualiza un concepto
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("posgrado_update")]
        public async Task<ActionResult> UpdatePosgrado(ConceptoPosDto concepto)
        {
            return Ok(await _service.UpdatePos(concepto));
        }
    }
}
