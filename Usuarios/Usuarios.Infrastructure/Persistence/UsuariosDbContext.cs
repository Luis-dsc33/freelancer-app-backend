using Microsoft.EntityFrameworkCore;
using Usuarios.Domain.Entities;

namespace Usuarios.Infrastructure.Persistence;

public class UsuariosDbContext : DbContext
{
    public UsuariosDbContext(DbContextOptions<UsuariosDbContext> options) : base(options) { }

    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Rol> Roles => Set<Rol>();
    public DbSet<RecuperacionContrasena> RecuperacionesContrasena => Set<RecuperacionContrasena>();
    public DbSet<PerfilEstudiante> PerfilesEstudiante => Set<PerfilEstudiante>();

    public DbSet<TrabajoPortafolio> TrabajosPortafolio => Set<TrabajoPortafolio>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("usuarios");   // EMMA: aqui es donde se indica que todo lo de este DbContext va a estar en el schema "usuarios"

        modelBuilder.Entity<Usuario>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<Rol>().HasData(
            new Rol { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Nombre = "Estudiante" },
            new Rol { Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), Nombre = "Cliente" },
            new Rol { Id = Guid.Parse("33333333-3333-3333-3333-333333333333"), Nombre = "Administrador" }
        );

        modelBuilder.Entity<RecuperacionContrasena>(entity =>
        {
            entity.HasIndex(r => r.TokenHash).IsUnique();
            entity.HasIndex(r => new { r.UsuarioId, r.ExpiraEn });
            entity.Property(r => r.TokenHash).HasMaxLength(64).IsRequired();
            entity.Property(r => r.Version).IsConcurrencyToken();
            entity.HasOne(r => r.Usuario)
                .WithMany()
                .HasForeignKey(r => r.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<TrabajoPortafolio>(entity =>
        {
            entity.HasIndex(t => t.UsuarioId);
            entity.Property(t => t.Titulo).HasMaxLength(150).IsRequired();
            entity.Property(t => t.Descripcion).HasMaxLength(2000).IsRequired();
            entity.Property(t => t.Enlace).HasMaxLength(2048);
            entity.HasOne(t => t.Usuario).WithMany().HasForeignKey(t => t.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // HU-04 escenario 02: un unico perfil por cuenta de usuario
        modelBuilder.Entity<PerfilEstudiante>()
            .HasIndex(p => p.UsuarioId)
            .IsUnique();

        base.OnModelCreating(modelBuilder);   // ← buena práctica agregarla, aunque en este caso no hace nada extra
    }
}