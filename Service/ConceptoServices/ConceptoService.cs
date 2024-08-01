using AkademicReport.Data;
using AkademicReport.Dto.ConceptoDto;
using AkademicReport.Dto.ConceptoPosgradoDto;
using AkademicReport.Dto.NivelDto;
using AkademicReport.Models;
using AkademicReport.Utilities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AkademicReport.Service.ConceptoServices
{
    public class ConceptoService : IConceptoService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _dataContext;
        public ConceptoService(IMapper mapper, DataContext dataContext)
        {
            _mapper = mapper;
            _dataContext = dataContext;
        }

        public async Task<ServicesResponseMessage<string>> Delete(int id)
        {
            try
            {
                var nivel = await _dataContext.Conceptos.FirstOrDefaultAsync(c => c.Id == id);
                if (nivel == null)
                    return new ServicesResponseMessage<string>() { Status = 204, Message = Msj.MsjNoRegistros };

                var codigodb = await _dataContext.Codigos.Where(c => c.IdConcepto == id).ToListAsync();
                if (codigodb.Count>0)
                    return new ServicesResponseMessage<string>() { Status = 204, Message = Msj.MsjNoEliminarCodigo };
                _dataContext.Conceptos.Remove(nivel);
                await _dataContext.SaveChangesAsync();
                return new ServicesResponseMessage<string>() { Status = 200, Message = Msj.MsjDelete };
            }
            catch (Exception ex)
            {
                return new ServicesResponseMessage<string>() { Status = 500, Message = Msj.MsjError + ex.ToString };
            }
        }

        public async Task<ServicesResponseMessage<string>> DeletePos(int id)
        {
            try
            {
                var concepto = await _dataContext.ConceptoPosgrados.FirstOrDefaultAsync(c => c.IdConceptoPosgrado == id);
                if (concepto == null)
                    return new ServicesResponseMessage<string>() { Status = 204, Message = Msj.MsjNoRegistros };

                var cargaDb = await _dataContext.CargaDocentes.Where(c => c.IdConceptoPosgrado == id).ToListAsync();
                if (cargaDb.Count > 0)
                    return new ServicesResponseMessage<string>() { Status = 204, Message = Msj.MsjNoEliminarCodigo };
                _dataContext.ConceptoPosgrados.Remove(concepto);
                await _dataContext.SaveChangesAsync();
                return new ServicesResponseMessage<string>() { Status = 200, Message = Msj.MsjDelete };
            }
            catch (Exception ex)
            {
                return new ServicesResponseMessage<string>() { Status = 500, Message = Msj.MsjError + ex.ToString };
            }
        }

        public async Task<ServiceResponseData<List<ConceptoGetDto>>> GetAll(int idPrograma)
        {
            try
            {
                var concepto = await _dataContext.Conceptos.Where(c => c.IdPrograma == idPrograma).Include(c=>c.IdProgramaNavigation).ToListAsync();
                if (concepto.Count < 1)
                    return new ServiceResponseData<List<ConceptoGetDto>>() { Data =_mapper.Map<List<ConceptoGetDto>>(concepto), Status = 200, Message=Msj.MsjNoData };
                return new ServiceResponseData<List<ConceptoGetDto>>() { Status = 200, Data = _mapper.Map<List<ConceptoGetDto>>(concepto) };
            }
            catch (Exception ex)
            {
                return new ServiceResponseData<List<ConceptoGetDto>>() { Status = 500 };
            }
        }

        public async Task<ServiceResponseData<List<ConceptoPosDto>>> GetAllPos()
        {
            try
            {
                var concepto = await _dataContext.ConceptoPosgrados.ToListAsync();
                if (concepto.Count < 1)
                    return new ServiceResponseData<List<ConceptoPosDto>>() { Data = _mapper.Map<List<ConceptoPosDto>>(concepto), Status = 200, Message = Msj.MsjNoData };
                return new ServiceResponseData<List<ConceptoPosDto>>() { Status = 200, Data = _mapper.Map<List<ConceptoPosDto>>(concepto) };
            }
            catch (Exception ex)
            {
                return new ServiceResponseData<List<ConceptoPosDto>>() { Status = 500 };
            }
        }

        public async Task<ServiceResponseData<List<ConceptoGetDto>>> GetById(int id)
        {
            try
            {
                var concepto = await _dataContext.Conceptos.ToListAsync();
                if (concepto.Count < 1)
                    return new ServiceResponseData<List<ConceptoGetDto>>() { Status = 204 };
                return new ServiceResponseData<List<ConceptoGetDto>>() { Status = 200, Data = _mapper.Map<List<ConceptoGetDto>>(concepto.Where(c => c.Id == id))};
            }
            catch (Exception ex)
            {
                return new ServiceResponseData<List<ConceptoGetDto>>() { Status = 500 };
            }
        }

        public async Task<ServiceResponseData<List<ConceptoPosDto>>> GetByIdPos(int id)
        {
            try
            {
                var concepto = await _dataContext.ConceptoPosgrados.ToListAsync();
                if (concepto.Count < 1)
                    return new ServiceResponseData<List<ConceptoPosDto>>() { Status = 204 };
                return new ServiceResponseData<List<ConceptoPosDto>>() { Status = 200, Data = _mapper.Map<List<ConceptoPosDto>>(concepto.Where(c => c.IdConceptoPosgrado == id)) };
            }
            catch (Exception ex)
            {
                return new ServiceResponseData<List<ConceptoPosDto>>() { Status = 500 };
            }
        }

        public async Task<ServicesResponseMessage<string>> Insert(ConceptoAddDto item)
        {
            try
            {
                _dataContext.Conceptos.Add(_mapper.Map<Concepto>(item));
                await _dataContext.SaveChangesAsync();
                return new ServicesResponseMessage<string>() { Status = 200, Message = Msj.MsjInsert };
            }
            catch (Exception ex)
            {
                return new ServicesResponseMessage<string>() { Status = 500, Message = Msj.MsjError + ex.ToString() };
            }
        }

        public async Task<ServicesResponseMessage<string>> InsertPos(ConceptoPosDto item)
        {
            try
            {
                _dataContext.ConceptoPosgrados.Add(_mapper.Map<ConceptoPosgrado>(item));
                await _dataContext.SaveChangesAsync();
                return new ServicesResponseMessage<string>() { Status = 200, Message = Msj.MsjInsert };
            }
            catch (Exception ex)
            {
                return new ServicesResponseMessage<string>() { Status = 500, Message = Msj.MsjError + ex.ToString() };
            }
        }

        public async Task<ServicesResponseMessage<string>> Update(ConceptoGetDto item)
        {
            try
            {
                var concepto = await _dataContext.Conceptos.AsNoTracking().FirstOrDefaultAsync(c=>c.Id==item.Id);
                concepto = _mapper.Map<Concepto>(item);
                
                _dataContext.Entry(concepto).State = EntityState.Modified;
                _dataContext.SaveChangesAsync();
                if (concepto==null)
                    return new ServicesResponseMessage<string>() { Status = 204 };
                return new ServicesResponseMessage<string>() { Status = 200, Message = Msj.MsjUpdate};
            }
            catch (Exception ex)
            {
                return new ServicesResponseMessage< string>() { Status = 500 , Message=Msj.MsjError + ex.ToString() };
            }
        }

        public async Task<ServicesResponseMessage<string>> UpdatePos(ConceptoPosDto item)
        {
            try
            {
                var concepto = await _dataContext.ConceptoPosgrados.AsNoTracking().FirstOrDefaultAsync(c => c.IdConceptoPosgrado == item.IdConceptoPosgrado);
                concepto = _mapper.Map<ConceptoPosgrado>(item);
                _dataContext.Entry(concepto).State = EntityState.Modified;
                _dataContext.SaveChangesAsync();
                if (concepto == null)
                    return new ServicesResponseMessage<string>() { Status = 204 };
                return new ServicesResponseMessage<string>() { Status = 200, Message = Msj.MsjUpdate };
            }
            catch (Exception ex)
            {
                return new ServicesResponseMessage<string>() { Status = 500, Message = Msj.MsjError + ex.ToString() };
            }
        }
    }
}
