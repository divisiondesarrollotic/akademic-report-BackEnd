using AkademicReport.Dto.RecintoDto;
using AkademicReport.Service.RecintoServices;
using Microsoft.AspNetCore.Mvc;

namespace AkademicReport.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class recintoController : ControllerBase
    {
        public readonly IRecintoService _service;
        public recintoController(IRecintoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<RecintoGetDto>>GetAll()
        {
            var result = await _service.GetAll();   
            return Ok(result); 
        }
    }
}
