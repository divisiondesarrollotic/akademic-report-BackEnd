using AkademicReport.Dto.CargaDto;
using AkademicReport.Dto.DocentesDto;
using AkademicReport.Dto.ReporteDto;
using AkademicReport.Dto.TiposReporteDto;
using AkademicReport.Dto.UsuarioDto;
using AkademicReport.Models;
using AkademicReport.Service;
using AkademicReport.Service.AsignaturaServices;
using AkademicReport.Service.CargaServices;
using AkademicReport.Service.DocenteServices;
using AkademicReport.Utilities;
using AutoMapper.Configuration.Annotations;
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

            var result = await _service.GetCargaCall(filtro.Cedula, filtro.Periodo, filtro.idPrograma, filtro.IdTipoReporte.Value, filtro.IdTipoReporteI.Value);
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
                return Ok(new ServicesResponseMessage<string>() { Status = 204, Message = result.Data.Value.Item2 });
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
        /// --Este post inserta un comentario en caso de que la carga sea irregular
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("nota_carga_irregular")]
        public async Task<ActionResult> InsertNotaCargaIrregular(NotaCargaIrregularDto nota)
        {
            var result = await _service.InsertNotaCargaIrregular(nota);
            if(result.Status==200)
                return Ok(result);
            return BadRequest(result);
        }
        /// <summary>
        /// --Este post inserta un comentario en caso de que la carga sea irregular
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("nota_carga_irregular")]
        public async Task<ActionResult> UpdateNotaCargaIrregular(NotaCargaIrregularDto nota)
        {
            var result = await _service.UpdateNotaCargaIrregular(nota);
            if (result.Status == 200)
                return Ok(result);
            return BadRequest(result);
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

        /// <summary>
        /// --Este  get trae todos los meses del ano
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("getmesesbycuatrimestre{cuatrimestre}")]
        public async Task<ActionResult<List<MesGetDto>>> GetMesesByCuatrimestre(int cuatrimestre)
        {
            return Ok(await _service.GetMesesByCuatrimestre(cuatrimestre));
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
        [Route("sincronizar_carga/{periodo}/{recinto}")]
        public async Task<ActionResult<List<MesGetDto>>> SincroniZarCarga(string periodo, int recinto)
        {
            var result = await _service.SincronizarCarga(periodo, recinto);
            if (result.Status == 200)
                return Ok(result);
            return BadRequest(result);
        }
        /// <summary>
        /// --Este  metodo uestra la carga de univeritas con la de akademic para fines de aprobacion
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("getsincronizar_carga/{periodo}/{recinto}")]
        public async Task<ActionResult<List<MesGetDto>>> GetCargaUniversitasBeforeSincronizacion(string periodo, int recinto)
        {
            var result = await _service.GetCargaAkadeicWithUniversitas(periodo, recinto);
            if (result.Status == 200)
                return Ok(result);
            return BadRequest(result);
        }
        /// <summary>
        /// --Este reubica la carga de un docente a otro
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("change_carga/{idCarga}/{cedula}/{profesor}")]
        public async Task<ActionResult<List<MesGetDto>>> ChangeCarga([Required] int idCarga, [Required] string cedula, [Required] string profesor)
        {
            var result = await _service.ChangeCarga(cedula, profesor, idCarga);
            if (result.Status == 200)
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
        public async Task<ActionResult> GetTipoCarga(int idprograma)
        {
            return Ok(await _service.GetTipoCarga(idprograma));
        }
        [HttpPut]
        [Route("update_horas_contratadas")]
        public async Task<ActionResult> UpdateHorasContratadas(int idCarga)
        {
            return Ok(await _service.UpdateHorasContratadas(idCarga));
        }
        [HttpGet]
        [Route("tipos_reportes")]
        public async Task<ActionResult<ServiceResponseData<List<TipoReporteGetDto>>>> getTipoReporte()
        {
            var response =  await _service.GetTipoReporte();
            if (response.Status == 200)
                return Ok(response);
            return BadRequest(response);
        }
        [HttpGet]
        [Route("tipos_reportes_irregulares")]
        public async Task<ActionResult<ServiceResponseData<List<TipoReporteIrregularGetDto>>>> getTipoReporteIrregulares()
        {
            var response = await _service.GetTipoReporteIrregular();
            if (response.Status == 200)
                return Ok(response);
            return BadRequest(response);
        }

        // <summary>
        /// --Autoriza la carga del id que se le mande
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("auth_carga")]
        public async Task<ActionResult> AutCarga(List<AuthCargaDto> Cargas)
        {
            var response = await _service.AutorizarCarga(Cargas);
            if (response.Status == 200)
                 return Ok( response);
            return BadRequest(response);
        }












    }
}

