using Chat.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chat.Infrastructure.Persistence;

public class ChatDbContext(DbContextOptions<ChatDbContext> options) : DbContext(options)
{
    public DbSet<Conversacion> Conversaciones => Set<Conversacion>();
    public DbSet<Mensaje> Mensajes => Set<Mensaje>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("chat");

        modelBuilder.Entity<Conversacion>(entity =>
        {
            entity.ToTable("Conversaciones", "chat");
            entity.HasKey(c => c.Id);
            entity.Property(c => c.TituloProyecto).HasMaxLength(150);
            entity.Property(c => c.NombreCliente).HasMaxLength(150);
            entity.HasIndex(c => new { c.ProyectoId, c.ClienteId, c.EstudianteId }).IsUnique();
            entity.HasIndex(c => new { c.ClienteId, c.UltimaActividadEn });
            entity.HasIndex(c => new { c.EstudianteId, c.UltimaActividadEn });
            entity.HasMany(c => c.Mensajes)
                .WithOne(m => m.Conversacion)
                .HasForeignKey(m => m.ConversacionId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Mensaje>(entity =>
        {
            entity.ToTable("Mensajes", "chat");
            entity.HasKey(m => m.Id);
            entity.Property(m => m.Contenido).HasMaxLength(2000).IsRequired();
            entity.HasIndex(m => new { m.ConversacionId, m.EnviadoEn });
        });

        base.OnModelCreating(modelBuilder);
    }
}
