using Microsoft.EntityFrameworkCore;
using Resenas.Domain.Entities;

namespace Resenas.Infrastructure.Persistence;

public class ResenasDbContext(DbContextOptions<ResenasDbContext> options) : DbContext(options)
{
    public DbSet<Resena> Resenas => Set<Resena>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("resenas");
        modelBuilder.Entity<Resena>(entity =>
        {
            entity.Property(r => r.Puntuacion).IsRequired();
            entity.Property(r => r.Comentario).HasMaxLength(2000).IsRequired();
            entity.HasIndex(r => new { r.ContratoId, r.AutorId, r.DestinatarioId }).IsUnique();
            entity.HasIndex(r => r.DestinatarioId);
            entity.HasIndex(r => r.AutorId);
        });
        base.OnModelCreating(modelBuilder);
    }
}
