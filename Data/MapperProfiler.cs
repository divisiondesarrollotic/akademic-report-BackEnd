using AkademicReport.Dto;
using AkademicReport.Dto.AsignaturaDto;
using AkademicReport.Dto.AulaDto;
using AkademicReport.Dto.CargaDto;
using AkademicReport.Dto.ConceptoDto;
using AkademicReport.Dto.ConceptoPosgradoDto;
using AkademicReport.Dto.DocentesDto;
using AkademicReport.Dto.FirmaDto;
using AkademicReport.Dto.FirmasDto;
using AkademicReport.Dto.NivelDto;
using AkademicReport.Dto.PeriodoDto;
using AkademicReport.Dto.ProgramaDto;
using AkademicReport.Dto.RecintoDto;
using AkademicReport.Dto.UsuarioDto;
using AkademicReport.Models;
using AkademicReport.Utilities;
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
               .ForMember(c=>c.Programa, o=>o.MapFrom(c=>c.IdProgramaNavigation))
            .ForMember(c => c.nivelNombre, opt => opt.MapFrom(c => c.NivelNavigation.Nivel));

            CreateMap<PeriodoAcademico, PeriodoGetDto>();
            CreateMap<PeriodoAddDto, PeriodoAcademico>();
            CreateMap<PeriodoUpdateDto, PeriodoAcademico>();
            CreateMap<Configuracion, ConfiguracionGetDto>();
           
            CreateMap<TipoCargaCodigo, TipoCargaDto>();
            CreateMap<TipoCargaDto, TipoCargaCodigo>();

            CreateMap<ProgramasAcademico, ProgramaGetDto>();




            CreateMap<NivelAddDto, NivelAcademico>();
            CreateMap<NivelUpdateDto, NivelAcademico>();
            CreateMap<NivelAcademico, NivelGetDto>();

            CreateMap<ConceptoAddDto, Concepto>();
            CreateMap<Concepto, ConceptoGetDto>();
            CreateMap<ConceptoGetDto, Concepto>();

            CreateMap<Aula, AulaGettDto>().ForMember(c=>c.Recinto, o=>o.MapFrom(c=>c.IdrecintoNavigation));
            CreateMap<AulaDto, Aula>();
          
            CreateMap<Paisesnacionalidade, NacionalidadDto>();

            CreateMap<TipoCarga, TipoDeCargaDto>().ForMember(c=>c.Programa, o=>o.MapFrom(c=>c.IdProgramaNavigation));
            CreateMap<TipoCarga, TipoCargaDto>();
            CreateMap<TipoCargaPivot, TipoCargaCodigo>();

            CreateMap<DocenteGetDto, DocenteReporteDto>().ForMember(c => c.Id, o => o.MapFrom(c => c.id))
                .ForMember(c => c.Id, o => o.MapFrom(c => c.id))
                .ForMember(c => c.TiempoDedicacion, o => o.MapFrom(c => c.tiempoDedicacion))
                .ForMember(c => c.Identificacion, o => o.MapFrom(c => c.identificacion))
                .ForMember(c => c.Nombre, o => o.MapFrom(c => c.nombre))
                .ForMember(c => c.Nacionalidad, o => o.MapFrom(c => c.nacionalidad))
                .ForMember(c => c.Sexo, o => o.MapFrom(c => c.sexo))
                .ForMember(c => c.Id_vinculo, o => o.MapFrom(c => c.id_vinculo))
                .ForMember(c => c.NombreVinculo, o => o.MapFrom(c => c.nombreVinculo))
                .ForMember(c => c.Id_recinto, o => o.MapFrom(c => c.id_recinto))
                .ForMember(c => c.Recinto, o => o.MapFrom(c => c.recinto))
                .ForMember(c => c.Nombre_corto, o => o.MapFrom(c => c.nombre_corto))
                .ForMember(c => c.Id_nivel_academico, o => o.MapFrom(c => c.id_nivel_academico))
                .ForMember(c => c.Nivel, o => o.MapFrom(c => c.nivel));

            CreateMap<TipoModalidad, TipoModalidadDto>();


            CreateMap<AsignaturaAddDto, Codigo>().ForMember(c=>c.Codigo1, opt=>opt.MapFrom(c=>c.Codigo)).ForMember(c=>c.IdConcepto, opt=>opt.MapFrom(c=>c.id_concepto));
            CreateMap<AsignaturaUpdateDto, Codigo>().ForMember(c => c.Codigo1, opt => opt.MapFrom(c => c.Codigo)).ForMember(c => c.IdConcepto, opt => opt.MapFrom(c => c.id_concepto));
            CreateMap<Codigo, AsignaturaGetDto>()
                .ForMember(c => c.NombreConcepto, opt => opt
                .MapFrom(c => c.IdConceptoNavigation.Nombre))
                .ForMember(c => c.Id_concepto, opt => opt
                .MapFrom(c => c.IdConcepto))
                .ForMember(c => c.Codigo, opt => opt.MapFrom(c => c.Codigo1))
                .ForMember(a => a.Modalidades, opt => opt.MapFrom(c=>c.TipoModalidadCodigos.FirstOrDefault(c=>c.Idcodigo==c.Idcodigo).IdTipoModalidadNavigation))
                .ForMember(c => c.TiposCargas, opt => opt.MapFrom(c=>c.TipoCargaCodigos))
                .ForMember(c=>c.Programa, o=>o.MapFrom(c=>c.IdProgramaNavigation));



            CreateMap<CargaAddDto, CargaDocente>().ForMember(c=>c.CodAsignatura, o=>o.MapFrom(c=>c.cod_asignatura))
                .ForMember(c=>c.NombreAsignatura, o=>o.MapFrom(c=>c.nombre_asignatura))
                .ForMember(c => c.CodUniversitas, o => o.MapFrom(c => c.cod_universitas))
                .ForMember(c => c.Dias, o => o.MapFrom(c => c.dia_id))
                .ForMember(c => c.HoraInicio, o => o.MapFrom(c => c.hora_inicio))
                .ForMember(c => c.HoraFin, o => o.MapFrom(c => c.hora_fin))
                .ForMember(c => c.MinutoInicio, o => o.MapFrom(c => c.minuto_inicio))
                .ForMember(c => c.MinutoFin, o => o.MapFrom(c => c.minuto_fin))
                .ForMember(c => c.NumeroHora, o => o.MapFrom(c => c.numero_hora))
                .ForMember(c => c.Modalidad, o => o.MapFrom(c => c.idModalidad))
                .ForMember(c => c.NombreProfesor, o => o.MapFrom(c => c.nombre_profesor))
                .ForMember(c=>c.Curricular, o=>o.MapFrom(c=>c.idTipoCarga));
           
            CreateMap<CargaUpdateDto, CargaDocente>().ForMember(c => c.CodAsignatura, o => o.MapFrom(c => c.cod_asignatura))
               .ForMember(c => c.NombreAsignatura, o => o.MapFrom(c => c.nombre_asignatura))
               .ForMember(c => c.CodUniversitas, o => o.MapFrom(c => c.cod_universitas))
               .ForMember(c => c.Dias, o => o.MapFrom(c => c.dia_id))
               .ForMember(c => c.HoraInicio, o => o.MapFrom(c => c.hora_inicio))
               .ForMember(c => c.HoraFin, o => o.MapFrom(c => c.hora_fin))
               .ForMember(c => c.MinutoInicio, o => o.MapFrom(c => c.minuto_inicio))
               .ForMember(c => c.MinutoFin, o => o.MapFrom(c => c.minuto_fin))
               .ForMember(c => c.NumeroHora, o => o.MapFrom(c => c.numero_hora))
                    .ForMember(c => c.Modalidad, o => o.MapFrom(c => c.idModalidad))
               .ForMember(c => c.NombreProfesor, o => o.MapFrom(c => c.nombre_profesor));

            CreateMap<LogTransDto, LogTransacional>();

            CreateMap<CargaDocente, CargaGetDto>().ForMember(c => c.cod_asignatura, o => o.MapFrom(c => c.CodAsignatura))
             .ForMember(c => c.nombre_asignatura, o => o.MapFrom(c => c.NombreAsignatura))
             .ForMember(c => c.cod_universitas, o => o.MapFrom(c => c.CodUniversitas))
             .ForMember(c => c.dia_id, o => o.MapFrom(c => c.Dias))
             .ForMember(c => c.hora_inicio, o => o.MapFrom(c => c.HoraInicio))
             .ForMember(c => c.minuto_inicio, o => o.MapFrom(c => c.MinutoInicio))
             .ForMember(c => c.hora_fin, o => o.MapFrom(c => c.HoraFin))
             .ForMember(c => c.minuto_fin, o => o.MapFrom(c => c.MinutoFin))
             .ForMember(c => c.numero_hora, o => o.MapFrom(c => c.NumeroHora))
             .ForMember(c => c.TipoModalidad, o => o.Ignore())
             
             .ForMember(c => c.nombre_profesor, o => o.MapFrom(c => c.NombreProfesor))
             .ForMember(c => c.dia_nombre, o => o.MapFrom(c => c.DiasNavigation.Nombre))
             .ForMember(c=>c.ConceptoPosgrado, o=>o.MapFrom(c=>c.IdConceptoPosgradoNavigation))
             .ForMember(c=>c.PeriodoObj, o=>o.MapFrom(c=>c.IdPeriodoNavigation));

            //.ForMember(c => c.TiposCarga, o => o.MapFrom(c => c.CurricularNavigation));

            CreateMap<Mese, MesGetDto>();
            CreateMap<ConceptoPosgrado, ConceptoPosDto>();
            CreateMap<ConceptoPosgrado, ConceptoPosDto>();
            CreateMap<ConceptoPosDto, ConceptoPosgrado>();

            CreateMap<CargaPosgradroDto, CargaDocente>().ForMember(c=>c.Curricular, o=>o.MapFrom(c=>c.idTipoCarga));
            CreateMap<CargaDocente, CargaPosgradoGet>().ForMember(c => c.TipoModalidad, o => o.MapFrom(c => c.ModalidadNavigation))
                .ForMember(c => c.Mes, o => o.MapFrom(c => c.IdMesNavigation))
            .ForMember(c => c.ConceptoPosgrado, o => o.MapFrom(c => c.IdConceptoPosgradoNavigation))
            .ForMember(c => c.TipoCarga, o => o.MapFrom(c => c.CurricularNavigation))
            .ForMember(c=>c.DiaNombre, o=>o.MapFrom(c=>c.DiasNavigation.Nombre))
            .ForMember(c=>c.Codigo, o=>o.MapFrom(c=>c.IdCodigoNavigation))
            .ForMember(c=>c.Periodo, o=>o.MapFrom(c=>c.IdPeriodo))
            .ForMember(c=>c.RecintoObj, o=>o.MapFrom(c=>c.RecintoNavigation));

            CreateMap<Firma, FirmaGetDto>().ForMember(c=>c.RecintoObj, o=>o.MapFrom(c=>c.IdRecintoNavigation));
            CreateMap<FirmaDto, Firma>();


        }
    }
}
