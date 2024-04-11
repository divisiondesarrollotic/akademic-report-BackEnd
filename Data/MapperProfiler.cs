using AkademicReport.Dto.AsignaturaDto;
using AkademicReport.Dto.AulaDto;
using AkademicReport.Dto.CargaDto;
using AkademicReport.Dto.ConceptoDto;
using AkademicReport.Dto.DocentesDto;
using AkademicReport.Dto.NivelDto;
using AkademicReport.Dto.PeriodoDto;
using AkademicReport.Dto.RecintoDto;
using AkademicReport.Dto.UsuarioDto;
using AkademicReport.Models;
using AutoMapper;

namespace AkademicReport.Data
{
    public class MapperProfiler :Profile
    {
        public MapperProfiler()
        {
            CreateMap<Recinto, RecintoGetDto>().ForMember(c => c.recinto, opt => opt.MapFrom(c => c.Recinto1)).ForMember(c=>c.nombre_corto, opt=>opt.MapFrom(c=>c.NombreCorto));
            CreateMap<UsuarioAddDto, Usuario>();
            CreateMap<UsuarioUpdateDto, Usuario>();
            CreateMap<Usuario, UsuarioGetDto>().ForMember(c => c.nivel, opt => opt.MapFrom(c => c.Nivel))
               .ForMember(c => c.nombre_corto, opt => opt.MapFrom(c => c.IdRecintoNavigation.NombreCorto))
               .ForMember(c => c.recinto, opt => opt.MapFrom(c => c.IdRecintoNavigation.Recinto1))
               .ForMember(c => c.id_recinto, opt => opt.MapFrom(c => c.IdRecinto))
            .ForMember(c => c.nivelNombre, opt => opt.MapFrom(c => c.NivelNavigation.Nivel));

            CreateMap<PeriodoAcademico, PeriodoGetDto>();
            CreateMap<PeriodoAddDto, PeriodoAcademico>();
            CreateMap<PeriodoUpdateDto, PeriodoAcademico>();
            CreateMap<Configuracion, ConfiguracionGetDto>();

            CreateMap<NivelAddDto, NivelAcademico>();
            CreateMap<NivelUpdateDto, NivelAcademico>();
            CreateMap<NivelAcademico, NivelGetDto>();

            CreateMap<ConceptoAddDto, Concepto>();
            CreateMap<Concepto, ConceptoGetDto>();

            CreateMap<Aula, AulaGettDto>();
            CreateMap<Paisesnacionalidade, NacionalidadDto>();

            CreateMap<TipoCarga, TipoDeCargaDto>();


            CreateMap<AsignaturaAddDto, Codigo>();
            CreateMap<AsignaturaUpdateDto, Codigo>();
            CreateMap<Codigo, AsignaturaGetDto>()
                .ForMember(c=>c.NombreConcepto, opt=>opt
                .MapFrom(c=>c.IdConceptoNavigation.Nombre))
                .ForMember(c=>c.Id_concepto, opt=>opt
                .MapFrom(c=>c.IdConcepto)) 
                .ForMember(c=>c.Codigo, opt=>opt.MapFrom(c=>c.Codigo1))
                .ForMember(c=>c.Modalida, opt=>opt.Ignore());

            
            CreateMap<CargaAddDto, CargaDocente>().ForMember(c=>c.CodAsignatura, o=>o.MapFrom(c=>c.cod_asignatura))
                .ForMember(c=>c.NombreAsignatura, o=>o.MapFrom(c=>c.nombre_asignatura))
                .ForMember(c => c.CodUniversitas, o => o.MapFrom(c => c.cod_universitas))
                .ForMember(c => c.Dias, o => o.MapFrom(c => c.dia_id))
                .ForMember(c => c.HoraInicio, o => o.MapFrom(c => c.hora_inicio))
                .ForMember(c => c.HoraFin, o => o.MapFrom(c => c.hora_fin))
                .ForMember(c => c.MinutoInicio, o => o.MapFrom(c => c.minuto_inicio))
                .ForMember(c => c.MinutoFin, o => o.MapFrom(c => c.minuto_fin))
                .ForMember(c => c.NumeroHora, o => o.MapFrom(c => c.numero_hora))
                .ForMember(c => c.NombreProfesor, o => o.MapFrom(c => c.nombre_profesor));
           
            CreateMap<CargaUpdateDto, CargaDocente>().ForMember(c => c.CodAsignatura, o => o.MapFrom(c => c.cod_asignatura))
               .ForMember(c => c.NombreAsignatura, o => o.MapFrom(c => c.nombre_asignatura))
               .ForMember(c => c.CodUniversitas, o => o.MapFrom(c => c.cod_universitas))
               .ForMember(c => c.Dias, o => o.MapFrom(c => c.dia_id))
               .ForMember(c => c.HoraInicio, o => o.MapFrom(c => c.hora_inicio))
               .ForMember(c => c.HoraFin, o => o.MapFrom(c => c.hora_fin))
               .ForMember(c => c.MinutoFin, o => o.MapFrom(c => c.minuto_fin))
               .ForMember(c => c.NumeroHora, o => o.MapFrom(c => c.numero_hora))
               .ForMember(c => c.NombreProfesor, o => o.MapFrom(c => c.nombre_profesor));
            
            CreateMap<CargaDocente, CargaGetDto>().ForMember(c => c.cod_asignatura, o => o.MapFrom(c => c.CodAsignatura))
             .ForMember(c => c.nombre_asignatura, o => o.MapFrom(c => c.NombreAsignatura))
             .ForMember(c => c.cod_universitas, o => o.MapFrom(c => c.CodUniversitas))
             .ForMember(c => c.dia_id, o => o.MapFrom(c => c.Dias))
             .ForMember(c => c.hora_inicio, o => o.MapFrom(c => c.HoraInicio))
             .ForMember(c => c.minuto_inicio, o => o.MapFrom(c => c.MinutoInicio))
             .ForMember(c => c.hora_fin, o=> o.MapFrom(c => c.HoraFin))
             .ForMember(c => c.minuto_fin, o => o.MapFrom(c => c.MinutoFin))
             .ForMember(c => c.numero_hora, o => o.MapFrom(c => c.NumeroHora))
             .ForMember(c => c.nombre_profesor, o => o.MapFrom(c => c.NombreProfesor))
             .ForMember(c => c.dia_nombre, o => o.MapFrom(c => c.DiasNavigation.Nombre))
             .ForMember(c => c.curricularName, o => o.MapFrom(c => c.CurricularNavigation.Nombre));




        }
    }
}
