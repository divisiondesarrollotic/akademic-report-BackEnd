using AkademicReport.Dto.CargaDto;
using AkademicReport.Dto.UsuarioDto;
using AkademicReport.Service;
using AkademicReport.Service.AsignaturaServices;
using AkademicReport.Service.CargaServices;
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
        public cargaController(ICargaDocenteService service)
        {
            _service = service;
        }
        [HttpPost]
        [Route("docente")]
        public async Task<ActionResult> GetCarga(DtoCarga filtro)
        {
            var result = await _service.GetCargaCall(filtro.Cedula, filtro.Periodo);
            if (result.Data.Value.Item1.Docente == null && result.Data.Value.Item1.Carga == null)
            {
                return Ok(new ServicesResponseMessage<string>() { Status = 204, Message = "Docente no existe" });
            }
            if (result.Data.Value.Item1.Carga==null)
            {
                return Ok(new ServiceResponseData<DocenteCargaDto>() { Status = 200, Data = result.Data.Value.Item1 });
            }
          
            return Ok(new ServiceResponseData<DocenteCargaDto>() { Status = 200, Data= result.Data.Value.Item1});

        }
        [HttpPost]
        [Route("detalle")]
        public async Task<ActionResult> Insert(CargaAddDto Carga)
        {
            return Ok(await _service.Insert(Carga));
        }
        [HttpPost]
        [Route("update")]
        public async Task<ActionResult> Update(CargaUpdateDto Carga)
        {
            return Ok(await _service.Update(Carga));
        }
        [HttpGet]
        [Route("delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            return Ok(await _service.Delete(id));
        }

        [HttpGet]
        [Route("get-tipo-carga")]
        public async Task<ActionResult>GetTipoCarga()
        {
            return Ok(await _service.GetTipoCarga());
        }
    }
}

