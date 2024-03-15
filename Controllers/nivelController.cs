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
        
        [HttpPost]
        [Route("insert")]
        public async Task<ActionResult> Create(NivelAddDto nivel)
        {
            return Ok(await _service.Insert(nivel));
        }
        
        [HttpPost]
        [Route("update")]
        public async Task<ActionResult> Update(NivelUpdateDto nivel)
        {
            return Ok(await _service.Update(nivel));
        }
        [HttpGet]
        [Route("delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            return Ok(await _service.Delete(id));
        }
    }
}
