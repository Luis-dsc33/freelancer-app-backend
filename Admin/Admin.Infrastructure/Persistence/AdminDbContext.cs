using Admin.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Admin.Infrastructure.Persistence;

public class AdminDbContext(DbContextOptions<AdminDbContext> options) : DbContext(options)
{
    // ADMIN Marcos: HU-27 bitácora de acciones administrativas
    public DbSet<AccionAdministrativa> AccionesAdministrativas => Set<AccionAdministrativa>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("admin");

        // ADMIN Marcos: configuración de la tabla de bitácora
        modelBuilder.Entity<AccionAdministrativa>(entity =>
        {
            entity.ToTable("AccionesAdministrativas");
            entity.Property(a => a.AdminNombre).HasMaxLength(150).IsRequired();
            entity.Property(a => a.TipoAccion).HasMaxLength(30).IsRequired();
            entity.Property(a => a.RecursoTipo).HasMaxLength(30).IsRequired();
            entity.Property(a => a.RecursoNombre).HasMaxLength(300).IsRequired();
            entity.Property(a => a.Motivo).HasMaxLength(500).IsRequired();
            entity.HasIndex(a => a.Fecha);
            entity.HasIndex(a => a.RecursoId);
            entity.HasIndex(a => new { a.AdminId, a.TipoAccion });
        });

        base.OnModelCreating(modelBuilder);
    }
}
