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

    public async Task<HashSet<Guid>> ObtenerProyectosPostuladosPorEstudianteAsync(
        Guid estudianteId,
        IEnumerable<Guid> proyectoIds,
        CancellationToken cancellationToken)
    {
        var ids = proyectoIds.Distinct().ToList();
        if (ids.Count == 0)
            return [];

        var postulados = await context.Postulaciones.AsNoTracking()
            .Where(p => p.EstudianteId == estudianteId && ids.Contains(p.ProyectoId))
            .Select(p => p.ProyectoId)
            .Distinct()
            .ToListAsync(cancellationToken);

        return postulados.ToHashSet();
    }

    public Task<List<Postulacion>> ListarPorEstudianteAsync(Guid estudianteId, CancellationToken cancellationToken) =>
        context.Postulaciones.AsNoTracking()
            .Include(p => p.Proyecto)
            .Where(p => p.EstudianteId == estudianteId)
            .OrderByDescending(p => p.CreadaEn)
            .ToListAsync(cancellationToken);

    public Task<Postulacion?> ObtenerParaEstudianteEnProyectoAsync(Guid proyectoId, Guid estudianteId, CancellationToken cancellationToken) =>
        context.Postulaciones.AsNoTracking()
            .FirstOrDefaultAsync(p => p.ProyectoId == proyectoId && p.EstudianteId == estudianteId, cancellationToken);

    public async Task AgregarAsync(Postulacion postulacion, CancellationToken cancellationToken)
    {
        await context.Postulaciones.AddAsync(postulacion, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task ActualizarAsync(Postulacion postulacion, CancellationToken cancellationToken)
    {
        context.Postulaciones.Update(postulacion);
        await context.SaveChangesAsync(cancellationToken);
    }
}
