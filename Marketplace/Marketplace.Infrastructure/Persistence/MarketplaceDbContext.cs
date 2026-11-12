using Marketplace.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Infrastructure.Persistence;

public class MarketplaceDbContext : DbContext
{
    public MarketplaceDbContext(DbContextOptions<MarketplaceDbContext> options) : base(options) { }

    public DbSet<Proyecto> Proyectos => Set<Proyecto>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("marketplace");

        modelBuilder.Entity<Proyecto>(entity =>
        {
            entity.Property(p => p.Titulo).HasMaxLength(150).IsRequired();
            entity.Property(p => p.Descripcion).HasMaxLength(4000).IsRequired();
            entity.Property(p => p.Presupuesto).HasPrecision(12, 2);
            entity.Property(p => p.NombreCliente).HasMaxLength(150).IsRequired();
            entity.Property(p => p.Estado).HasMaxLength(30).IsRequired();
            entity.Property(p => p.HabilidadesRequeridas).IsRequired();

            // Soporta el filtro por estado + orden del listado (esUrgente DESC, creadoEn DESC)
            entity.HasIndex(p => new { p.Estado, p.EsUrgente, p.CreadoEn });

            // Datos semilla de desarrollo (ejemplos de la guia del contrato)
            entity.HasData(
                new Proyecto
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Titulo = "App móvil universitaria",
                    Descripcion = "MVP para estudiantes",
                    Presupuesto = 10000m,
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
                    Presupuesto = 4500m,
                    EsUrgente = true,
                    HabilidadesRequeridas = new List<string> { "HTML", "CSS", "SEO" },
                    NombreCliente = "Club Alumni",
                    Estado = Proyecto.EstadoAbierto,
                    CreadoEn = new DateTime(2026, 11, 5, 15, 30, 0, DateTimeKind.Utc)
                });
        });

        base.OnModelCreating(modelBuilder);
    }
}
