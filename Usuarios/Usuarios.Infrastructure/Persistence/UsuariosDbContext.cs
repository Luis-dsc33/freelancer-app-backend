using Microsoft.EntityFrameworkCore;
using Usuarios.Domain.Entities;

namespace Usuarios.Infrastructure.Persistence;

public class UsuariosDbContext : DbContext
{
    public UsuariosDbContext(DbContextOptions<UsuariosDbContext> options) : base(options) { }

    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Rol> Roles => Set<Rol>();
    public DbSet<RecuperacionContrasena> RecuperacionesContrasena => Set<RecuperacionContrasena>();

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

        base.OnModelCreating(modelBuilder);   // ← buena práctica agregarla, aunque en este caso no hace nada extra
    }
}
