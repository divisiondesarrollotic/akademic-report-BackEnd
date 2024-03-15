using AkademicReport.Data;
using AkademicReport.Dto.CargaDto;
using AkademicReport.Dto.DocentesDto;
using AkademicReport.Dto.UsuarioDto;
using AkademicReport.Models;
using AkademicReport.Service.DocenteServices;
using AkademicReport.Utilities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AkademicReport.Service.CargaServices
{
    public class CargaService : ICargaDocenteService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _dataContext;
        private readonly IDocenteService _docenteService;

        public CargaService(IMapper mapper, DataContext dataContext, IDocenteService docenteService )
        {
            _mapper = mapper;
            _dataContext = dataContext;
            _docenteService = docenteService;
        }

        public async Task<ServicesResponseMessage<string>> Delete(int id)
        {
            try
            {
                var carga = await _dataContext.CargaDocentes.FirstOrDefaultAsync(c => c.Id == id);
                if (carga == null)
                    return new ServicesResponseMessage<string>() { Status = 204, Message = Msj.MsjNoRegistros };
                _dataContext.CargaDocentes.Remove(carga);
                await _dataContext.SaveChangesAsync();
                return new ServicesResponseMessage<string>() { Status = 200, Message = Msj.MsjDelete };
            }
            catch (Exception ex)
            {
                return new ServicesResponseMessage<string>() { Status = 500, Message = Msj.MsjError + ex.ToString };
            }
        }

        public async Task<ServiceResponseCarga<DocenteCargaDto, string>> GetCarga(string Cedula, string Periodo)
        {
            var ResulData = new DocenteCargaDto();
            var carga = await _dataContext.CargaDocentes.Where(c => c.Cedula.Contains(Cedula) && c.Periodo==Periodo).Include(c=>c.DiasNavigation).ToListAsync();
            var docente = await _docenteService.GetAll();
            
            var consultaDocente = docente.Data.Where(c => c.identificacion==Cedula).FirstOrDefault();
            if (consultaDocente== null)
            {
                ResulData.Carga = null;
                return new ServiceResponseCarga<DocenteCargaDto, string>() { Status = 204, Data = (ResulData, "Docente no existe") };
            }
            if (carga==null)
            {
                ResulData.Carga = new List<CargaGetDto>();
                ResulData.Docente = consultaDocente;
                return new ServiceResponseCarga<DocenteCargaDto, string>() { Status = 200, Data = (ResulData, "No hay carga") };
            }
            var CargaMap = _mapper.Map<List<CargaGetDto>>(carga);
            foreach (var i in CargaMap)
            {
                var codigo = await _dataContext.Codigos.Where(c => c.Codigo1.Contains(i.cod_asignatura)).FirstOrDefaultAsync();
               if(codigo!=null)
                {
                    i.id_asignatura = codigo.Id.ToString();
                    i.id_concepto = codigo.IdConcepto.ToString();
                }   
            }
            ResulData.Carga = CargaMap.OrderBy(c => c.dia_id).ToList();
            ResulData.Docente = consultaDocente;
            return new ServiceResponseCarga<DocenteCargaDto, string> {Data=(ResulData, ""),Status = 200};
        }

        public async Task<ServicesResponseMessage<string>> Insert(CargaAddDto item)
        {
            try
            {
                _dataContext.CargaDocentes.Add(_mapper.Map<CargaDocente>(item));
                await _dataContext.SaveChangesAsync();
                return new ServicesResponseMessage<string>() { Status = 200, Message = Msj.MsjInsert };
            }
            catch (Exception ex)
            {
                return new ServicesResponseMessage<string>() { Status = 500, Message = Msj.MsjError + ex.ToString() };
            }
        }

        public async Task<ServicesResponseMessage<string>> Update(CargaUpdateDto item)
        {
            try
            {
                var carga = await _dataContext.CargaDocentes.AsNoTracking().FirstOrDefaultAsync(c => c.Id == Convert.ToInt32(item.Id));
                carga = _mapper.Map<CargaDocente>(item);
                _dataContext.Entry(carga).State = EntityState.Modified;
                await _dataContext.SaveChangesAsync();
                return new ServicesResponseMessage<string>() { Status = 200, Message = Msj.MsjUpdate };
            }
            catch (Exception ex)
            {
                return new ServicesResponseMessage<string>() { Status = 500, Message = Msj.MsjError + ex.ToString() };
            }
        }

    }
}
