using AkademicReport.Dto.AulaDto;
using AkademicReport.Service;
using AkademicReport.Service.AulaServices;
using Microsoft.AspNetCore.Mvc;

namespace AkademicReport.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class aulasRecintoController : ControllerBase
    {
        private readonly IAulaService _service;
        public aulasRecintoController(IAulaService service)
        {
            _service = service;
        }

        /// <summary>
        /// --Este get trae todas las aula de un recinto
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult> GetAllAylasByIdRecinto(int id) 
        {
            return Ok(await _service.GetAllByIdRecinto(id));
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
           var response =  await _service.Delete(id);
            if(response.Status==200)
                return Ok(response);
            return BadRequest(response);
        }
        [HttpPost]
        public async Task<ActionResult> Insert(AulaDto aula)
        {
            var response = await _service.Insert(aula);
            if (response.Status == 200)
                return Ok(response);
            return BadRequest(response);
        }
        [HttpPut]
        public async Task<ActionResult> Update(AulaDto aula)
        {
            var response = await _service.Update(aula);
            if (response.Status == 200)
                return Ok(response);
            return BadRequest(response);
        }
    }
}
