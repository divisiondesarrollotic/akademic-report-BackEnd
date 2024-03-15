using AkademicReport.Dto.ReporteDto;
using AkademicReport.Service;
using AkademicReport.Service.ReposteServices;
using Microsoft.AspNetCore.Mvc;

namespace AkademicReport.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReporteController : ControllerBase
    {
        private readonly IReposteService _reposteService;
        public ReporteController(IReposteService reposteService)
        {
            _reposteService = reposteService;
        }
        [HttpPost]
        [Route("docente")]
        public async Task<ActionResult>PorDocente(ReporteDto filtro)
        {
            var response =await _reposteService.PorDocente(filtro);
            if(response.Status==204)
            {
                var ServiceR = new ServicesResponseMessage<string>();
                ServiceR.Status = 2024;
                ServiceR.Message = "Docente no existe";
                return Ok(ServiceR);
            }
            return Ok(response);

        }
        [HttpPost]
        [Route("recinto")]
        public async Task<ActionResult> PorRecinto(ReportePorRecintoDto filtro)
        {
            var response = await _reposteService.PorRecinto (filtro);
            return Ok(response);
           

        }
    }
}
