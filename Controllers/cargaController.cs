using AkademicReport.Dto.CargaDto;
using AkademicReport.Dto.DocentesDto;
using AkademicReport.Dto.ReporteDto;
using AkademicReport.Dto.UsuarioDto;
using AkademicReport.Service;
using AkademicReport.Service.AsignaturaServices;
using AkademicReport.Service.CargaServices;
using AkademicReport.Service.DocenteServices;
using AkademicReport.Utilities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace AkademicReport.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class cargaController : ControllerBase
    {
        private readonly ICargaDocenteService _service;
        private readonly IDocenteService _docenteService;
        public cargaController(ICargaDocenteService service, IDocenteService docenteService)
        {
            _service = service;
            _docenteService = docenteService;
        }

        /// <summary>
        /// --Este post trea la carga del docente expecificado en los parametros
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("docente")]
        public async Task<ActionResult> GetCarga(DtoCarga filtro)
        {

            var result = await _service.GetCargaCall(filtro.Cedula, filtro.Periodo, filtro.idPrograma);
            if (result.Data.Value.Item1.Docente == null && result.Data.Value.Item1.Carga == null)
            {
                return Ok(new ServicesResponseMessage<string>() { Status = 204, Message = "Docente no existe" });
            }
            if (result.Data.Value.Item1.Carga == null)
            {
                return Ok(new ServiceResponseData<DocenteCargaDto>() { Status = 200, Data = result.Data.Value.Item1 });
            }

            return Ok(new ServiceResponseData<DocenteCargaDto>() { Status = 200, Data = result.Data.Value.Item1 });

        }
        [HttpPost]
        [Route("docente_posgrado")]
        public async Task<ActionResult> GetCargaPosgrado(DtoCarga filtro)
        {
            var docenteConsulta = await _docenteService.GetAllFilter(new FiltroDocentesDto() { Filtro = filtro.Cedula, idPrograma = 2, elementosPorPagina = 100, paginaActual = 1 });
            var result = await _service.GetCargaCallPosgrado(filtro.Cedula, filtro.Periodo, filtro.idPrograma, docenteConsulta.Data, int.Parse(docenteConsulta.Data[0].id_recinto));
            if (result.Data.Value.Item1 == null)
            {
                return Ok(new ServicesResponseMessage<string>() { Status = 204, Message = result.Data.Value.Item2});
            }
            if (result.Data.Value.Item1.Docente == null && result.Data.Value.Item1.Cargas == null)
            {
                return Ok(new ServicesResponseMessage<string>() { Status = 204, Message = "Docente no existe" });
            }
            if (result.Data.Value.Item1.Cargas == null)
            {
                return Ok(new ServiceResponseData<ReportCargaPosgradoDto>() { Status = 200, Data = result.Data.Value.Item1 });
            }

            return Ok(new ServiceResponseData<ReportCargaPosgradoDto>() { Status = 200, Data = result.Data.Value.Item1 });

        }
        /// <summary>
        /// --Este post inserta una carga a un docente x
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("detalle")]
        public async Task<ActionResult> Insert(CargaAddDto Carga)
        {
            return Ok(await _service.Insert(Carga));
        }
        /// <summary>
        /// --Este post inserta una carga a un docente  de posgradox
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("insert_carga_posgrado")]
        public async Task<ActionResult> InsertCargaPosgrado(CargaPosgradroDto Carga)
        {
            return Ok(await _service.InsertCargaPosgrado(Carga));
        }
        /// <summary>
        /// --Este put actualiza la carga de posgrado una carga a un docente  de posgradox
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("update_carga_posgrado")]
        public async Task<ActionResult> UpdateCargaPosgrado(CargaPosgradroDto Carga)
        {
            return Ok(await _service.UpdateCargaPosgrado(Carga));
        }

        /// <summary>
        /// --Este  get trae todos los meses del ano
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("getmeses")]
        public async Task<ActionResult<List<MesGetDto>>> GetMeses()
        {
            return Ok(await _service.GetMeses());
        }
        [HttpGet]
        [Route("getdias")]
        public async Task<ActionResult<List<MesGetDto>>> GetDias()
        {
            return Ok(await _service.GetDias());
        }

        /// <summary>
        /// --Este  metodo sincroniza la carga de univeritas con la de akademic si existe la actualiza y si no la agrega
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("sincronizar_carga/{periodo}")]
        public async Task<ActionResult<List<MesGetDto>>> SincroniZarCarga([Required] string periodo)
        {
            var result = await _service.SincronizarCarga(periodo);
            if(result.Status==200)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost]
        [Route("update")]
        public async Task<ActionResult> Update(CargaUpdateDto Carga)
        {
            return Ok(await _service.Update(Carga));
        }
        /// <summary>
        /// --Este post elimina una carga a un docente x
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("delete/{id}/{idUsuario}")]
        public async Task<ActionResult> Delete(int id, int idUsuario)
        {
            return Ok(await _service.Delete(id, idUsuario));
        }

        /// <summary>
        /// --Este get trae todos los tipo de carga que puede tener un codigo
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("get-tipo-carga/{idprograma}")]
        public async Task<ActionResult>GetTipoCarga(int idprograma)
        {
            return Ok(await _service.GetTipoCarga(idprograma));
        }
        [HttpPut]
        [Route("update_horas_contratadas")]
        public async Task<ActionResult> UpdateHorasContratadas(int idCarga)
        {
            return Ok(await _service.UpdateHorasContratadas(idCarga));
        }
    }
}

