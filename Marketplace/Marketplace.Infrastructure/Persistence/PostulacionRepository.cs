using Marketplace.Application.Interfaces;
using Marketplace.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Infrastructure.Persistence;

public sealed class PostulacionRepository(MarketplaceDbContext context) : IPostulacionRepository
{
    public Task<List<Postulacion>> ObtenerPorProyectoAsync(Guid proyectoId, CancellationToken cancellationToken) =>
        context.Postulaciones.AsNoTracking()
            .Where(p => p.ProyectoId == proyectoId)
            .OrderByDescending(p => p.CreadaEn)
            .ToListAsync(cancellationToken);

    public Task<Postulacion?> ObtenerPorIdAsync(Guid id, CancellationToken cancellationToken) =>
        context.Postulaciones.Include(p => p.Proyecto)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

    public Task<bool> ExisteParaEstudianteAsync(Guid proyectoId, Guid estudianteId, CancellationToken cancellationToken) =>
        context.Postulaciones.AnyAsync(p => p.ProyectoId == proyectoId && p.EstudianteId == estudianteId, cancellationToken);

    public async Task ActualizarAsync(Postulacion postulacion, CancellationToken cancellationToken)
    {
        context.Postulaciones.Update(postulacion);
        await context.SaveChangesAsync(cancellationToken);
    }
}
