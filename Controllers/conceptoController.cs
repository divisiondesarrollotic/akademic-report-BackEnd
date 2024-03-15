using AkademicReport.Dto.ConceptoDto;
using AkademicReport.Dto.NivelDto;
using AkademicReport.Service.ConceptoServices;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult> Create(ConceptoAddDto concepto)
        {
            return Ok(await _service.Insert(concepto));
        }
       
        [HttpGet]
        [Route("delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            return Ok(await _service.Delete(id));
        }
    }
}
