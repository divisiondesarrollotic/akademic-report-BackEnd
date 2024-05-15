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

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            return Ok(await _service.GetAll());
        }

        [HttpPost]
        [Route("codigo-filter")]
        public async Task<ActionResult> GetAllFilter(FiltroDocentesDto filtro)
        {
            return Ok(await _service.GetAllFilter(filtro));
        }
        [HttpPost]
        [Route("pag")]
        public async Task<ActionResult> GetAllPaginacion(FiltroDocentesDto paginacion)
        {
            return Ok(await _service.GetAllPaginacion(paginacion));
        }
        [HttpGet]
        [Route("concepto/{id}")]
        public async Task<ActionResult> GetAllByIdConcepto(int id)
        {
            return Ok(await _service.GetAllByIdConcepto(id));
        }
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            return Ok(await _service.GetById(id));
        }

        [HttpPost]
        [Route("insert")]
        public async Task<ActionResult> Create(AsignaturaAddDto asignatura)
        {
            return Ok(await _service.Insert(asignatura));
        }

        [HttpPost]
        [Route("update")]
        public async Task<ActionResult> Update(AsignaturaUpdateDto asignatura)
        {
            return Ok(await _service.Update(asignatura));
        }
        [HttpGet]
        [Route("delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            return Ok(await _service.Delete(id));
        }
    }
}
