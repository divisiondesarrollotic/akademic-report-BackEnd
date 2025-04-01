using AkademicReport.Dto.DocentesDto;
using AkademicReport.Dto.NivelesAcademicoDto;
using AkademicReport.Service.DocenteServices;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

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
        /// <summary>
        /// --Este post trae filtrados por nombre y identificacion.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("filter_docente")]
        public async Task<ActionResult> GetAll(FiltroDocentesDto filter)
        {
            return Ok(await _service.GetAllFilter(filter));
        }

        /// <summary>
        /// --Este get trae los docentes con una pagincacion.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("pag")]
        public async Task<ActionResult> GetAllPaginacion(FiltroDocentesDto filtro)
        {
            var response = await _service.GetAllPaginacion(filtro);
            return Ok(response);
        }

        /// <summary>
        /// --Este get trea le cantidad de docentes por recinto
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// --Este post filtra nacionalidades+
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("nacionalidad")]
        public async Task<ActionResult> GetAllNacionalidadesFilter(FiltroDto filtro)
        {
            var response = await _service.GetNacionalidades(filtro);
            return Ok(response);
        }


        /// <summary>
        /// --Trae todas los niveles academios del programa
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("{idPrograma}")]
        public async Task<ActionResult> GetAllNivelesAcademico(int idPrograma)
        {
            var response = await _service.GetNivelAcademico(idPrograma);
            return Ok(response);
        }


        /// <summary>
        /// Actualiza los precios de los niveles academicos segun sean enviados
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("niveles_academicos")]
        public async Task<ActionResult> UpdateNivelesAcademicos([Required] List<NivelAcademicoUpdatePrice> Niveles)
        {
            var response = await _service.UpdatePriceNivelAcademico(Niveles);
            return Ok(response);
        }
    }
}
