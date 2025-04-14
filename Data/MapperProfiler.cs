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
using AkademicReport.Dto.NivelesAcademicoDto;
using AkademicReport.Dto.PeriodoDto;
using AkademicReport.Dto.ProgramaDto;
using AkademicReport.Dto.RecintoDto;
using AkademicReport.Dto.ReporteDto;
using AkademicReport.Dto.TiposReporteDto;
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
               .ForMember(c=>c.RecintoObj, o=>o.MapFrom(c=>c.IdRecintoNavigation))
            .ForMember(c => c.nivelNombre, opt => opt.MapFrom(c => c.NivelNavigation.Nivel));

            CreateMap<PeriodoAcademico, PeriodoGetDto>();
            CreateMap<PeriodoAddDto, PeriodoAcademico>();
            CreateMap<PeriodoUpdateDto, PeriodoAcademico>();
            CreateMap<Configuracion, ConfiguracionGetDto>();
           
            CreateMap<TipoCargaCodigo, TipoCargaDto>();
            CreateMap<TipoCargaDto, TipoCargaCodigo>();

            CreateMap<DocentesOtroPrecio, DocenteOtroPrecioDto>();
            CreateMap<DocenteOtroPrecioDto, DocentesOtroPrecio>();

            CreateMap<NivelAcademico, NivelAcademicoGetDto>();

            CreateMap<ProgramasAcademico, ProgramaGetDto>();

            CreateMap<TipoReporte, TipoReporteGetDto>();
            CreateMap<TipoReporteIrregular, TipoReporteIrregularGetDto>();

            CreateMap<DayCantSemanasDto, CantSemanasMe>().ForMember(c=>c.IdCarga, o=>o.Ignore()).ForMember(c=>c.Id, o=>o.Ignore());
            CreateMap<CantSemanasMe, DayCantSemanasDto>();


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

            CreateMap<NotasCargaIrregular, NotaCargaIrregularDto>();
            CreateMap<NotaCargaIrregularDto, NotasCargaIrregular>();


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

            CreateMap<DocenteReporteDto, DocenteGetDto>().ForMember(c => c.id, o => o.MapFrom(c => c.Id))
               .ForMember(c => c.id, o => o.MapFrom(c => c.Id))
               .ForMember(c => c.tiempoDedicacion, o => o.MapFrom(c => c.TiempoDedicacion))
               .ForMember(c => c.identificacion, o => o.MapFrom(c => c.Identificacion))
               .ForMember(c => c.nombre, o => o.MapFrom(c => c.Nombre))
               .ForMember(c => c.nacionalidad, o => o.MapFrom(c => c.Nacionalidad))
               .ForMember(c => c.sexo, o => o.MapFrom(c => c.Sexo))
               .ForMember(c => c.id_vinculo, o => o.MapFrom(c => c.Id_vinculo))
               .ForMember(c => c.nombreVinculo, o => o.MapFrom(c => c.NombreVinculo))
               .ForMember(c => c.id_recinto, o => o.MapFrom(c => c. Id_recinto))
               .ForMember(c => c.recinto, o => o.MapFrom(c => c.Recinto))
               .ForMember(c => c.nombre_corto, o => o.MapFrom(c => c.Nombre_corto))
               .ForMember(c => c.id_nivel_academico, o => o.MapFrom(c => c.Id_nivel_academico))
               .ForMember(c => c.nivel, o => o.MapFrom(c => c.Nivel));



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

            CreateMap<Dia, DiaGetDto>();
            CreateMap<CantSemanasMe, CantSemanaMesDto>()
                .ForMember(c=>c.MesObj, o=>o.MapFrom(c=>c.MesNavigation));
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
             .ForMember(c=>c.DiaObj, o=>o.MapFrom(c=>c.DiasNavigation))
             .ForMember(c => c.dia_nombre, o => o.MapFrom(c => c.DiasNavigation.Nombre))
             .ForMember(c => c.ConceptoPosgrado, o => o.MapFrom(c => c.IdConceptoPosgradoNavigation))
             .ForMember(c => c.TipoModalidad, o => o.MapFrom(c => c.ModalidadNavigation))
             .ForMember(c => c.PeriodoObj, o => o.MapFrom(c => c.IdPeriodoNavigation))
             .ForMember(c => c.credito, o => o.MapFrom(c => c.Credito))
             //.ForMember(c => c.TipoReporteObj, o => o.MapFrom(c => c.IdTipoReporteNavigation))
             //.ForMember(c => c.TipoReporteIrregularObj, o => o.MapFrom(c => c.IdTipoReporteIrregularNavigation))
             .ForMember(c=>c.AsignatureObj, o=>o.MapFrom(c=>c.IdCodigoNavigation));


            CreateMap<CargaGetDto, CargaDocente>().ForMember(c => c.CodAsignatura, o => o.MapFrom(c => c.cod_asignatura))
             .ForMember(c => c.NombreAsignatura, o => o.MapFrom(c => c.nombre_asignatura))
             .ForMember(c => c.CodUniversitas, o => o.MapFrom(c => c.CodUniversitas))
             .ForMember(c => c.HoraInicio, o => o.MapFrom(c => c.hora_inicio))
             .ForMember(c => c.MinutoInicio, o => o.MapFrom(c => c.minuto_inicio))
             .ForMember(c => c.HoraFin, o => o.MapFrom(c => c.hora_fin))
             .ForMember(c => c.MinutoFin, o => o.MapFrom(c => c.minuto_fin))
             .ForMember(c => c.NumeroHora, o => o.MapFrom(c => c.numero_hora))
             .ForMember(c => c.NombreProfesor, o => o.MapFrom(c => c.nombre_profesor))
             .ForMember(c => c.Modalidad, o => o.MapFrom(c => c.Modalidad))
             .ForMember(c => c.Curricular, o => o.MapFrom(c => c.TiposCarga.Id))
             .ForMember(c => c.Deleted, o => o.MapFrom(c => false))
             .ForMember(c => c.HoraContratada, o => o.MapFrom(c => false))
             .ForMember(c => c.Dias, o => o.MapFrom(c => c.dia_id))
             .ForMember(c => c.Aula, o => o.MapFrom(c => c.Aula))
             .ForMember(c => c.Credito, o => o.MapFrom(c => c.credito));


            CreateMap<CargaReporteDto, GetCargaIrregularDto>()
             .ForMember(c => c.TipoCarga, o => o.MapFrom(c => c.TiposCarga));

             



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

            CreateMap<ResponseApiUniversitas.Items, CargaGetDto>()
                .ForMember(c => c.Periodo, o => o.MapFrom(c => c.any_anyaca))
                .ForMember(c => c.RecintoNombre, o => o.MapFrom(c => c.recinto))
                .ForMember(c => c.cod_asignatura, o => o.MapFrom(c => c.nomid1.Substring(0, 7).ToString()))
                .ForMember(c => c.nombre_asignatura, o => o.MapFrom(c => c.nomid1.Substring(7)))
                .ForMember(c => c.cod_universitas, o => o.MapFrom(c => c.id_assignatura))
                .ForMember(c => c.CodUniversitas, o => o.MapFrom(c => c.id_assignatura))
                //.ForMember(c => c.Seccion, o => o.MapFrom(c => c.id_grp_activ[1]))
                .ForMember(c => c.Recinto, o => o.MapFrom(c => c.id_grp_activ[0]))
                .ForMember(c => c.Aula, o => o.MapFrom(c => c.aul_desc))
                .ForMember(c => c.hora_inicio, o => o.MapFrom(c => c.horini))
                .ForMember(c => c.minuto_inicio, o => o.MapFrom(c => c.minini))
                .ForMember(c => c.hora_fin, o => o.MapFrom(c => c.horfin))
                .ForMember(c => c.minuto_fin, o => o.MapFrom(c => c.minfin))
                .ForMember(c => c.Cedula, o => o.MapFrom(c => c.identificador))
                .ForMember(c => c.nombre_profesor, o => o.MapFrom(c => c.descripcion))
                .ForMember(c => c.IdPrograma, o => o.MapFrom(c => 1))
                .ForMember(c => c.HoraContratada, o => o.MapFrom(c => false))
                .ForMember(c => c.TiposCarga, o => o.MapFrom(c => new TipoCargaDto() { Id = 1 }))
                .ForMember(c => c.TipoModalidad, o => o.MapFrom(c => new TipoModalidadDto() { Id = 1 }))
                .ForMember(c => c.Modalidad, o => o.MapFrom(c => 1))
                .ForMember(c => c.Curricular, o => o.MapFrom(c => 1))
                .ForMember(c => c.credito, o => o.MapFrom(c => c.numcre))
                .ForMember(c=>c.numero_hora, o=>o.MapFrom(c=>c.numcre))
                .ForMember(c => c.dia_id, o => o.MapFrom(c => c.dsm_codnum));

        }
    }
}
