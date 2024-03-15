using AkademicReport.Dto.DocentesDto;
using AkademicReport.Service.DocenteServices;
using Microsoft.AspNetCore.Mvc;

namespace AkademicReport.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class docenteController : ControllerBase
    {
        private readonly IDocenteService _service;
        public docenteController(IDocenteService service)
        {
            _service = service;
        }
        [HttpPost]
        public async Task<ActionResult> GetAll()
        {
            return Ok(await _service.GetAll());
        }
        [HttpPost]
        [Route("pag")]
        public async Task<ActionResult> GetAllPaginacion(FiltroDocentesDto filtro)
        {
            var response = await _service.GetAllPaginacion(filtro);
            return Ok(response);
        }
        [HttpGet]
        [Route("cantidadxrecinto")]
        public async Task<ActionResult> GetAllCntDocentesxRecinto()
        {
            var response = await _service.GetDocentexRecinto();
            return Ok(response);
        }
        [HttpPost]
        [Route("porRecinto/{id}")]
        public async Task<ActionResult> GetAllxRecintoFilter(FiltroDocentesDto filtro, int id)
        {
            var response = await _service.GetAllRecinto(filtro, id);
            return Ok(response);
        }
        [HttpPost]
        [Route("nacionalidad")]
        public async Task<ActionResult> GetAllNacionalidadesFilter(FiltroDto filtro)
        {
            var response = await _service.GetNacionalidades(filtro);
            return Ok(response);
        }
    }
}
