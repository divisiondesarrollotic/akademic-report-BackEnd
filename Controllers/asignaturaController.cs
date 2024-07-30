using AkademicReport.Dto.AsignaturaDto;
using AkademicReport.Dto.DocentesDto;
using AkademicReport.Dto.NivelDto;
using AkademicReport.Service.AsignaturaServices;
using Microsoft.AspNetCore.Mvc;

namespace AkademicReport.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class asignaturaController : ControllerBase
    {
        private readonly IAsignaturaService _service;
        public asignaturaController(IAsignaturaService service)
        {
            _service = service;
        }
        /// <summary>
        /// --Este get trae todos los codigos con todas sus caracteristicas
        /// </summary>
        /// <returns></returns>
        [HttpGet("{idprograma}")]
        public async Task<ActionResult> GetAll(int idprograma)
        {
            return Ok(await _service.GetAll(idprograma));
        }
        /// <summary>
        /// --Este get trae las modalidades existentes en el sistema
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("gettipomodalidad")]
        public async Task<ActionResult> GetAllTipoModalidad()
        {
            return Ok(await _service.GetAllTipoModalid());
        }
        /// <summary>
        /// --Este get es un filtro el cual trae los codigos por nombre y por codigo
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("codigo-filter/{filtro}/{idprograma}")]
        public async Task<ActionResult> GetAllFilter( string filtro, int idprograma)
        {
            return Ok(await _service.GetAllFilter(filtro, idprograma));
        }

        /// <summary>
        /// --Este post trael todas los codigos/asignaturas con una paginacion
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("pag")]
        public async Task<ActionResult> GetAllPaginacion(FiltroDocentesDto paginacion)
        {
            return Ok(await _service.GetAllPaginacion(paginacion));
        }



        /// <summary>
        /// --Este get  busca un concepto por id
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("concepto/{id}/{idprograma}")]
        public async Task<ActionResult> GetAllByIdConcepto(int id, int idprograma)
        {
            return Ok(await _service.GetAllByIdConcepto(id, idprograma));
        }

        /// <summary>
        /// --Este get  busca un codigo/asignatura por id
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}/{idprograma}")]
        public async Task<ActionResult> GetById(int id, int idprograma)
        {
            return Ok(await _service.GetById(id, idprograma));
        }

        /// <summary>
        /// --Este post crea un codigo/asignatura
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("insert")]
        public async Task<ActionResult> Create(AsignaturaAddDto asignatura)
        {
            return Ok(await _service.Insert(asignatura));
        }


        /// <summary>
        /// --Este post actualiza un codigo/asignatura
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("update")]
        public async Task<ActionResult> Update(AsignaturaUpdateDto asignatura)
        {
            return Ok(await _service.Update(asignatura));
        }
        /// <summary>
        /// --Este post elimina un codigo/asignatura
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
