using AkademicReport.Dto.AulaDto;
using AkademicReport.Dto.FirmasDto;
using AkademicReport.Service.FirmaServices;
using Microsoft.AspNetCore.Mvc;

namespace AkademicReport.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FirmasController : ControllerBase
    {
        private readonly IFirmaService _service;
        public FirmasController(IFirmaService firmaService)
        {
            _service = firmaService;
        }
        /// <summary>
        /// --Este get trae todas las aula de un recinto
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("get_by_idrecinto/{idrecinto}")]
        public async Task<ActionResult> GetAllAylasByIdRecinto(int idrecinto)
        {
            return Ok(await _service.GetByIdRecinto(idrecinto));
        }
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            return Ok(await _service.GetAll());
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var response = await _service.Delete(id);
            if (response.Status == 200)
                return Ok(response);
            return BadRequest(response);
        }
        [HttpPost]
        public async Task<ActionResult> Insert(FirmaDto firma)
        {
            var response = await _service.Insert(firma);
            if (response.Status == 200)
                return Ok(response);
            return BadRequest(response);
        }
        [HttpPut]
        public async Task<ActionResult> Update(FirmaDto firma)
        {
            var response = await _service.Update(firma);
            if (response.Status == 200)
                return Ok(response);
            return BadRequest(response);
        }
    }
}
