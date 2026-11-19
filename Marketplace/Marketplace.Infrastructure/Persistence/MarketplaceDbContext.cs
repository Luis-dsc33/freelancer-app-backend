using Marketplace.Domain.Entities;
using Marketplace.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Infrastructure.Persistence;

public class MarketplaceDbContext : DbContext
{
    public MarketplaceDbContext(DbContextOptions<MarketplaceDbContext> options) : base(options) { }

    public DbSet<Proyecto> Proyectos => Set<Proyecto>();
    public DbSet<SkillCategory> SkillCategories => Set<SkillCategory>();
    public DbSet<Skill> Skills => Set<Skill>();
    public DbSet<ProyectoSkill> ProyectoSkills => Set<ProyectoSkill>();
    public DbSet<Entregable> Entregables => Set<Entregable>();
    public DbSet<HitoPago> HitosPago => Set<HitoPago>();
    public DbSet<Postulacion> Postulaciones => Set<Postulacion>();
    public DbSet<Contrato> Contratos => Set<Contrato>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("marketplace");

        modelBuilder.Entity<Proyecto>(entity =>
        {
            entity.Property(p => p.Titulo).HasMaxLength(150).IsRequired();
            entity.Property(p => p.Descripcion).HasMaxLength(4000).IsRequired();
            entity.Property(p => p.Disciplina).HasMaxLength(100).IsRequired();
            entity.Property(p => p.Presupuesto).HasPrecision(12, 2);
            entity.Property(p => p.TipoPresupuesto).HasConversion<string>().HasMaxLength(20).IsRequired();
            entity.Property(p => p.PresupuestoMinimo).HasPrecision(12, 2);
            entity.Property(p => p.PresupuestoMaximo).HasPrecision(12, 2);
            entity.Property(p => p.Moneda).HasMaxLength(3).IsRequired();
            entity.Property(p => p.EsquemaPago).HasConversion<string>().HasMaxLength(20).IsRequired();
            entity.Property(p => p.FechaLimite).IsRequired();
            entity.Property(p => p.Modalidad).HasConversion<string>().HasMaxLength(20).IsRequired();
            entity.Property(p => p.NivelAvance).HasConversion<string>().HasMaxLength(20);
            entity.Property(p => p.PropiedadIntelectual).HasConversion<string>().HasMaxLength(20);
            entity.Property(p => p.NombreCliente).HasMaxLength(150).IsRequired();
            entity.Property(p => p.Estado).HasMaxLength(30).IsRequired();
            entity.Property(p => p.HabilidadesRequeridas).IsRequired();
            entity.Property(p => p.Ubicacion).HasMaxLength(250);
            entity.Property(p => p.FlexibilidadHorario).HasMaxLength(1000);
            entity.Property(p => p.DedicacionSemanalEstimada).HasMaxLength(100);
            entity.Property(p => p.NivelAcademicoSugerido).HasMaxLength(150);
            entity.Property(p => p.GastosAdicionales).HasMaxLength(2000);
            entity.Property(p => p.Alcance).HasMaxLength(4000);
            entity.HasCheckConstraint("CK_Proyecto_CantidadVacantes", "\"CantidadVacantes\" > 0");
            entity.HasCheckConstraint("CK_Proyecto_Presupuesto", "\"Presupuesto\" >= 0 AND (\"PresupuestoMinimo\" IS NULL OR \"PresupuestoMinimo\" > 0) AND (\"PresupuestoMaximo\" IS NULL OR \"PresupuestoMaximo\" > 0)");

            // Soporta el filtro por estado + orden del listado (esUrgente DESC, creadoEn DESC)
            entity.HasIndex(p => new { p.Estado, p.EsUrgente, p.CreadoEn });

            // Datos semilla de desarrollo (ejemplos de la guia del contrato)
            entity.HasData(
                new Proyecto
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Titulo = "App móvil universitaria",
                    Descripcion = "MVP para estudiantes",
                    Disciplina = "Desarrollo de software",
                    Presupuesto = 10000m,
                    FechaLimite = new DateOnly(2026, 11, 30),
                    ClienteId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                    EsUrgente = false,
                    HabilidadesRequeridas = new List<string> { "Angular", "Ionic" },
                    NombreCliente = "CodeBridge",
                    Estado = Proyecto.EstadoAbierto,
                    CreadoEn = new DateTime(2026, 11, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new Proyecto
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    Titulo = "Landing de recaudación",
                    Descripcion = "Sitio marketing con formularios",
                    Disciplina = "Marketing digital",
                    Presupuesto = 4500m,
                    FechaLimite = new DateOnly(2026, 11, 30),
                    ClienteId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                    EsUrgente = true,
                    HabilidadesRequeridas = new List<string> { "HTML", "CSS", "SEO" },
                    NombreCliente = "Club Alumni",
                    Estado = Proyecto.EstadoAbierto,
                    CreadoEn = new DateTime(2026, 11, 5, 15, 30, 0, DateTimeKind.Utc)
                });

        modelBuilder.Entity<Contrato>(entity =>
        {
            entity.Property(c => c.ImporteAcordado).HasPrecision(12, 2).IsRequired();
            entity.Property(c => c.Estado).HasMaxLength(30).IsRequired();
            entity.HasIndex(c => c.PostulacionId).IsUnique();
            entity.HasIndex(c => new { c.ClienteId, c.Estado });
            entity.HasIndex(c => new { c.EstudianteId, c.Estado });
            entity.HasOne(c => c.Postulacion)
                .WithMany()
                .HasForeignKey(c => c.PostulacionId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Postulacion>(entity =>
        {
            entity.Property(p => p.Estado).HasMaxLength(30).IsRequired();
            entity.Property(p => p.Mensaje).HasMaxLength(2000).IsRequired();
            entity.HasIndex(p => new { p.ProyectoId, p.EstudianteId }).IsUnique();
            entity.HasIndex(p => new { p.ProyectoId, p.Estado });
            entity.HasOne(p => p.Proyecto)
                .WithMany()
                .HasForeignKey(p => p.ProyectoId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<SkillCategory>(entity =>
        {
            entity.Property(c => c.Nombre).HasMaxLength(120).IsRequired();
            entity.Property(c => c.Descripcion).HasMaxLength(1000).IsRequired();
            entity.HasIndex(c => c.Nombre).IsUnique();
            entity.HasData(CatalogoSkillsInicial.Categories);
        });

        modelBuilder.Entity<Skill>(entity =>
        {
            entity.Property(s => s.Nombre).HasMaxLength(120).IsRequired();
            entity.Property(s => s.NombreNormalizado).HasMaxLength(120).IsRequired();
            entity.Property(s => s.Descripcion).HasMaxLength(1000).IsRequired();
            entity.HasIndex(s => s.NombreNormalizado).IsUnique();
            entity.HasOne(s => s.Categoria)
                .WithMany(c => c.Skills)
                .HasForeignKey(s => s.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasData(CatalogoSkillsInicial.Skills);
        });

        modelBuilder.Entity<ProyectoSkill>(entity =>
        {
            entity.HasKey(s => new { s.ProyectoId, s.SkillId });
            entity.Property(s => s.TipoRequisito).HasConversion<string>().HasMaxLength(20).IsRequired();
            entity.HasIndex(s => new { s.SkillId, s.TipoRequisito });
            entity.HasOne(s => s.Proyecto).WithMany(p => p.Skills).HasForeignKey(s => s.ProyectoId).OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(s => s.Skill).WithMany(s => s.ProyectoSkills).HasForeignKey(s => s.SkillId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Entregable>(entity =>
        {
            entity.Property(e => e.Descripcion).HasMaxLength(1000).IsRequired();
            entity.HasIndex(e => new { e.ProyectoId, e.Orden }).IsUnique();
            entity.HasOne(e => e.Proyecto).WithMany(p => p.Entregables).HasForeignKey(e => e.ProyectoId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<HitoPago>(entity =>
        {
            entity.Property(h => h.Descripcion).HasMaxLength(1000).IsRequired();
            entity.Property(h => h.Porcentaje).HasPrecision(5, 2);
            entity.Property(h => h.Importe).HasPrecision(12, 2);
            entity.HasIndex(h => new { h.ProyectoId, h.Orden }).IsUnique();
            entity.HasOne(h => h.Proyecto).WithMany(p => p.HitosPago).HasForeignKey(h => h.ProyectoId).OnDelete(DeleteBehavior.Cascade);
        });
        });

        base.OnModelCreating(modelBuilder);
    }
}
