using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using AkademicReport.Models;

namespace AkademicReport.Data
{
    public partial class DataContext : DbContext
    {
        public DataContext()
        {
        }

        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Area> Areas { get; set; } = null!;
        public virtual DbSet<AreaDocente> AreaDocentes { get; set; } = null!;
        public virtual DbSet<Asignatura> Asignaturas { get; set; } = null!;
        public virtual DbSet<Aula> Aulas { get; set; } = null!;
        public virtual DbSet<CargaDocente> CargaDocentes { get; set; } = null!;
        public virtual DbSet<Codigo> Codigos { get; set; } = null!;
        public virtual DbSet<Concepto> Conceptos { get; set; } = null!;
        public virtual DbSet<ConceptoPosgrado> ConceptoPosgrados { get; set; } = null!;
        public virtual DbSet<Configuracion> Configuracions { get; set; } = null!;
        public virtual DbSet<Contrato> Contratos { get; set; } = null!;
        public virtual DbSet<Dia> Dias { get; set; } = null!;
        public virtual DbSet<Diplomado> Diplomados { get; set; } = null!;
        public virtual DbSet<Docentereal> Docentereals { get; set; } = null!;
        public virtual DbSet<Mese> Meses { get; set; } = null!;
        public virtual DbSet<NivelAcademico> NivelAcademicos { get; set; } = null!;
        public virtual DbSet<NivelUsuario> NivelUsuarios { get; set; } = null!;
        public virtual DbSet<PagoFijo> PagoFijos { get; set; } = null!;
        public virtual DbSet<Paisesnacionalidade> Paisesnacionalidades { get; set; } = null!;
        public virtual DbSet<PeriodoAcademico> PeriodoAcademicos { get; set; } = null!;
        public virtual DbSet<Planestudio> Planestudios { get; set; } = null!;
        public virtual DbSet<PlanestudioDocente> PlanestudioDocentes { get; set; } = null!;
        public virtual DbSet<ProgramasAcademico> ProgramasAcademicos { get; set; } = null!;
        public virtual DbSet<Recinto> Recintos { get; set; } = null!;
        public virtual DbSet<TipoCarga> TipoCargas { get; set; } = null!;
        public virtual DbSet<TipoCargaCodigo> TipoCargaCodigos { get; set; } = null!;
        public virtual DbSet<TipoModalidad> TipoModalidads { get; set; } = null!;
        public virtual DbSet<TipoModalidadCodigo> TipoModalidadCodigos { get; set; } = null!;
        public virtual DbSet<Usuario> Usuarios { get; set; } = null!;
        public virtual DbSet<Vinculo> Vinculos { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=ConnectionStrings:Sql");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Area>(entity =>
            {
                entity.ToTable("area");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Area1)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("area");
            });

            modelBuilder.Entity<AreaDocente>(entity =>
            {
                entity.ToTable("area_docente");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IdArea).HasColumnName("id_area");

                entity.Property(e => e.IdDocente).HasColumnName("id_docente");

                entity.HasOne(d => d.IdAreaNavigation)
                    .WithMany(p => p.AreaDocentes)
                    .HasForeignKey(d => d.IdArea)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_area_areaDocente");

                entity.HasOne(d => d.IdDocenteNavigation)
                    .WithMany(p => p.AreaDocentes)
                    .HasForeignKey(d => d.IdDocente)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_docente_areaDocente");
            });

            modelBuilder.Entity<Asignatura>(entity =>
            {
                entity.ToTable("asignatura");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CodPem)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("cod_pem");

                entity.Property(e => e.CodUni).HasColumnName("cod_uni");

                entity.Property(e => e.Creditos).HasColumnName("creditos");

                entity.Property(e => e.Horario)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("horario");

                entity.Property(e => e.Seccion)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("seccion");

                entity.Property(e => e.Titulo)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("titulo");
            });

            modelBuilder.Entity<Aula>(entity =>
            {
                entity.ToTable("aula");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Edificio)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("edificio");

                entity.Property(e => e.Idrecinto).HasColumnName("idrecinto");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("nombre");

                entity.HasOne(d => d.IdrecintoNavigation)
                    .WithMany(p => p.Aulas)
                    .HasForeignKey(d => d.Idrecinto)
                    .HasConstraintName("FK__aula__idrecinto__6ABAD62E");
            });

            modelBuilder.Entity<CargaDocente>(entity =>
            {
                entity.ToTable("carga_docente");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Anio)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("anio");

                entity.Property(e => e.Aula)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("aula");

                entity.Property(e => e.Cedula)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("cedula");

                entity.Property(e => e.CodAsignatura)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("cod_asignatura");

                entity.Property(e => e.CodUniversitas)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("cod_universitas");

                entity.Property(e => e.Credito).HasColumnName("credito");

                entity.Property(e => e.Curricular).HasColumnName("curricular");

                entity.Property(e => e.DiaMes).HasColumnName("diaMes");

                entity.Property(e => e.Dias).HasColumnName("dias");

                entity.Property(e => e.HoraFin)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("hora_fin");

                entity.Property(e => e.HoraInicio)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("hora_inicio");

                entity.Property(e => e.IdConceptoPosgrado).HasColumnName("idConceptoPosgrado");

                entity.Property(e => e.IdMes).HasColumnName("idMes");

                entity.Property(e => e.IdPrograma).HasColumnName("idPrograma");

                entity.Property(e => e.MinutoFin)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("minuto_fin");

                entity.Property(e => e.MinutoInicio)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("minuto_inicio");

                entity.Property(e => e.Modalidad).HasColumnName("modalidad");

                entity.Property(e => e.NombreAsignatura)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("nombre_asignatura");

                entity.Property(e => e.NombreProfesor)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("nombre_profesor");

                entity.Property(e => e.NumeroHora).HasColumnName("numero_hora");

                entity.Property(e => e.Periodo)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("periodo");

                entity.Property(e => e.Recinto)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("recinto");

                entity.Property(e => e.Seccion)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("seccion");

                entity.HasOne(d => d.CurricularNavigation)
                    .WithMany(p => p.CargaDocentes)
                    .HasForeignKey(d => d.Curricular)
                    .HasConstraintName("TipoCarga_CargaDocente");

                entity.HasOne(d => d.DiasNavigation)
                    .WithMany(p => p.CargaDocentes)
                    .HasForeignKey(d => d.Dias)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Dias_cargaDocente");

                entity.HasOne(d => d.IdConceptoPosgradoNavigation)
                    .WithMany(p => p.CargaDocentes)
                    .HasForeignKey(d => d.IdConceptoPosgrado)
                    .HasConstraintName("FK__carga_doc__idCon__0EF836A4");

                entity.HasOne(d => d.IdMesNavigation)
                    .WithMany(p => p.CargaDocentes)
                    .HasForeignKey(d => d.IdMes)
                    .HasConstraintName("FK__carga_doc__idMes__02925FBF");

                entity.HasOne(d => d.IdProgramaNavigation)
                    .WithMany(p => p.CargaDocentes)
                    .HasForeignKey(d => d.IdPrograma)
                    .HasConstraintName("FK__carga_doc__idPro__038683F8");

                entity.HasOne(d => d.ModalidadNavigation)
                    .WithMany(p => p.CargaDocentes)
                    .HasForeignKey(d => d.Modalidad)
                    .HasConstraintName("FK_modalidad_cargaDocente");
            });

            modelBuilder.Entity<Codigo>(entity =>
            {
                entity.ToTable("codigo");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Codigo1)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("codigo");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(2000)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");

                entity.Property(e => e.Horas)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("horas");

                entity.Property(e => e.IdConcepto).HasColumnName("id_concepto");

                entity.Property(e => e.IdPrograma).HasColumnName("idPrograma");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("nombre");

                entity.HasOne(d => d.IdConceptoNavigation)
                    .WithMany(p => p.Codigos)
                    .HasForeignKey(d => d.IdConcepto)
                    .HasConstraintName("codigo_concepto_FK");

                entity.HasOne(d => d.IdProgramaNavigation)
                    .WithMany(p => p.Codigos)
                    .HasForeignKey(d => d.IdPrograma)
                    .HasConstraintName("FK__codigo__idProgra__056ECC6A");
            });

            modelBuilder.Entity<Concepto>(entity =>
            {
                entity.ToTable("concepto");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IdPrograma).HasColumnName("idPrograma");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("nombre");

                entity.HasOne(d => d.IdProgramaNavigation)
                    .WithMany(p => p.Conceptos)
                    .HasForeignKey(d => d.IdPrograma)
                    .HasConstraintName("FK__concepto__idProg__7DCDAAA2");
            });

            modelBuilder.Entity<ConceptoPosgrado>(entity =>
            {
                entity.HasKey(e => e.IdConceptoPosgrado)
                    .HasName("PK__Concepto__49BA2621CA18F044");

                entity.ToTable("ConceptoPosgrado");

                entity.Property(e => e.IdConceptoPosgrado).HasColumnName("idConceptoPosgrado");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("nombre");
            });

            modelBuilder.Entity<Configuracion>(entity =>
            {
                entity.ToTable("configuracion");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("nombre");

                entity.Property(e => e.Valor)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("valor");
            });

            modelBuilder.Entity<Contrato>(entity =>
            {
                entity.ToTable("contrato");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Contrato1)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("contrato");
            });

            modelBuilder.Entity<Dia>(entity =>
            {
                entity.ToTable("dias");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("nombre");
            });

            modelBuilder.Entity<Diplomado>(entity =>
            {
                entity.HasKey(e => e.CodUnicersita)
                    .HasName("PK__diplomad__09A0DB548ACAFDFD");

                entity.ToTable("diplomado");

                entity.Property(e => e.CodUnicersita).HasColumnName("cod_unicersita");

                entity.Property(e => e.CodPlanEstudio)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("cod_planEstudio");
            });

            modelBuilder.Entity<Docentereal>(entity =>
            {
                entity.ToTable("docentereal");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IdNivelAcademico).HasColumnName("id_nivel_academico");

                entity.Property(e => e.IdRecinto).HasColumnName("id_recinto");

                entity.Property(e => e.IdVinculo).HasColumnName("id_vinculo");

                entity.Property(e => e.Identificacion)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("identificacion");

                entity.Property(e => e.Nacionalidad)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nacionalidad");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("nombre");

                entity.Property(e => e.Sexo)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("sexo")
                    .IsFixedLength();

                entity.Property(e => e.TiempoDedicacion)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("tiempoDedicacion");

                entity.Property(e => e.TipoIdentificacion)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("tipoIdentificacion");

                entity.HasOne(d => d.IdNivelAcademicoNavigation)
                    .WithMany(p => p.Docentereals)
                    .HasForeignKey(d => d.IdNivelAcademico)
                    .HasConstraintName("FK_nivelAcademico_docentereal");

                entity.HasOne(d => d.IdRecintoNavigation)
                    .WithMany(p => p.Docentereals)
                    .HasForeignKey(d => d.IdRecinto)
                    .HasConstraintName("FK_recinto_docentereal");

                entity.HasOne(d => d.IdVinculoNavigation)
                    .WithMany(p => p.Docentereals)
                    .HasForeignKey(d => d.IdVinculo)
                    .HasConstraintName("FK_vinculo_docente");
            });

            modelBuilder.Entity<Mese>(entity =>
            {
                entity.HasKey(e => e.IdMes)
                    .HasName("PK__Meses__0D1357C0DA646C00");

                entity.Property(e => e.IdMes).ValueGeneratedNever();

                entity.Property(e => e.Nombre)
                    .HasMaxLength(15)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<NivelAcademico>(entity =>
            {
                entity.ToTable("nivel_academico");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IdPrograma).HasColumnName("idPrograma");

                entity.Property(e => e.Nivel)
                    .HasMaxLength(260)
                    .IsUnicode(false)
                    .HasColumnName("nivel");

                entity.Property(e => e.PagoHora).HasColumnName("pago_hora");

                entity.HasOne(d => d.IdProgramaNavigation)
                    .WithMany(p => p.NivelAcademicos)
                    .HasForeignKey(d => d.IdPrograma)
                    .HasConstraintName("FK__nivel_aca__idPro__047AA831");
            });

            modelBuilder.Entity<NivelUsuario>(entity =>
            {
                entity.ToTable("nivel_usuario");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Abreviado)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("abreviado");

                entity.Property(e => e.Nivel)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("nivel");
            });

            modelBuilder.Entity<PagoFijo>(entity =>
            {
                entity.ToTable("pago_fijo");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nombre");

                entity.Property(e => e.PagoHora).HasColumnName("pago_hora");
            });

            modelBuilder.Entity<Paisesnacionalidade>(entity =>
            {
                entity.ToTable("paisesnacionalidades");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Nacionalidad)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("nacionalidad");

                entity.Property(e => e.Pais)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("pais");
            });

            modelBuilder.Entity<PeriodoAcademico>(entity =>
            {
                entity.ToTable("periodo_academico");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Estado)
                    .HasColumnName("estado")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Periodo)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("periodo");
            });

            modelBuilder.Entity<Planestudio>(entity =>
            {
                entity.ToTable("planestudio");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("nombre");
            });

            modelBuilder.Entity<PlanestudioDocente>(entity =>
            {
                entity.ToTable("planestudio_docente");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IdDocente).HasColumnName("id_docente");

                entity.Property(e => e.IdPlanestudio).HasColumnName("id_planestudio");
            });

            modelBuilder.Entity<ProgramasAcademico>(entity =>
            {
                entity.HasKey(e => e.IdPrograma)
                    .HasName("PK__Programa__467DDFD621484A76");

                entity.Property(e => e.IdPrograma).HasColumnName("idPrograma");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("nombre");
            });

            modelBuilder.Entity<Recinto>(entity =>
            {
                entity.ToTable("recinto");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.NombreCorto)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("nombre_corto");

                entity.Property(e => e.Recinto1)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("recinto");
            });

            modelBuilder.Entity<TipoCarga>(entity =>
            {
                entity.ToTable("tipo_carga");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IdPrograma).HasColumnName("idPrograma");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("nombre");

                entity.HasOne(d => d.IdProgramaNavigation)
                    .WithMany(p => p.TipoCargas)
                    .HasForeignKey(d => d.IdPrograma)
                    .HasConstraintName("FK__tipo_carg__idPro__0662F0A3");
            });

            modelBuilder.Entity<TipoCargaCodigo>(entity =>
            {
                entity.ToTable("tipo_carga_codigo");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IdCodigo).HasColumnName("idCodigo");

                entity.Property(e => e.IdTipoCarga).HasColumnName("idTipoCarga");

                entity.HasOne(d => d.IdCodigoNavigation)
                    .WithMany(p => p.TipoCargaCodigos)
                    .HasForeignKey(d => d.IdCodigo)
                    .HasConstraintName("FK__tipo_carg__idCod__7E02B4CC");

                entity.HasOne(d => d.IdTipoCargaNavigation)
                    .WithMany(p => p.TipoCargaCodigos)
                    .HasForeignKey(d => d.IdTipoCarga)
                    .HasConstraintName("FK__tipo_carg__idTip__7D0E9093");
            });

            modelBuilder.Entity<TipoModalidad>(entity =>
            {
                entity.ToTable("tipo_modalidad");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasColumnName("nombre");
            });

            modelBuilder.Entity<TipoModalidadCodigo>(entity =>
            {
                entity.ToTable("tipo_modalidad_codigo");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IdTipoModalidad).HasColumnName("idTipoModalidad");

                entity.Property(e => e.Idcodigo).HasColumnName("idcodigo");

                entity.HasOne(d => d.IdTipoModalidadNavigation)
                    .WithMany(p => p.TipoModalidadCodigos)
                    .HasForeignKey(d => d.IdTipoModalidad)
                    .HasConstraintName("FK_ModalidadTipoModalidad");

                entity.HasOne(d => d.IdcodigoNavigation)
                    .WithMany(p => p.TipoModalidadCodigos)
                    .HasForeignKey(d => d.Idcodigo)
                    .HasConstraintName("FK_codigoTipoModalidad");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("usuario");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Contra)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("contra");

                entity.Property(e => e.Correo)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("correo");

                entity.Property(e => e.IdPrograma).HasColumnName("idPrograma");

                entity.Property(e => e.IdRecinto)
                    .HasColumnName("id_recinto")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Nivel).HasColumnName("nivel");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("nombre");

                entity.Property(e => e.SoftDelete).HasColumnName("soft_delete");

                entity.HasOne(d => d.IdProgramaNavigation)
                    .WithMany(p => p.Usuarios)
                    .HasForeignKey(d => d.IdPrograma)
                    .HasConstraintName("FK__usuario__idProgr__7CD98669");

                entity.HasOne(d => d.IdRecintoNavigation)
                    .WithMany(p => p.Usuarios)
                    .HasForeignKey(d => d.IdRecinto)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_recinto_usuario");

                entity.HasOne(d => d.NivelNavigation)
                    .WithMany(p => p.Usuarios)
                    .HasForeignKey(d => d.Nivel)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_nivel_usuario");
            });

            modelBuilder.Entity<Vinculo>(entity =>
            {
                entity.ToTable("vinculo");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Corto)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("corto");

                entity.Property(e => e.Monto).HasColumnName("monto");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nombre");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
