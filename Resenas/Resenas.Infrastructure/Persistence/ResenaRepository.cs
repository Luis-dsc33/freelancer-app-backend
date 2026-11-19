using Microsoft.EntityFrameworkCore;
using Resenas.Application.Interfaces;
using Resenas.Domain.Entities;

namespace Resenas.Infrastructure.Persistence;

public sealed class ResenaRepository(ResenasDbContext context) : IResenaRepository
{
    public Task<bool> ExisteAsync(
        Guid contratoId,
        Guid autorId,
        Guid destinatarioId,
        CancellationToken cancellationToken) =>
        context.Resenas.AnyAsync(
            r => r.ContratoId == contratoId && r.AutorId == autorId && r.DestinatarioId == destinatarioId,
            cancellationToken);

    public async Task AgregarAsync(Resena resena, CancellationToken cancellationToken)
    {
        await context.Resenas.AddAsync(resena, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public Task<List<Resena>> ListarPorDestinatarioAsync(Guid destinatarioId, CancellationToken cancellationToken) =>
        context.Resenas.AsNoTracking()
            .Where(r => r.DestinatarioId == destinatarioId)
            .OrderByDescending(r => r.CreadaEn)
            .ToListAsync(cancellationToken);

    public Task<List<Resena>> ListarPorAutorAsync(Guid autorId, CancellationToken cancellationToken) =>
        context.Resenas.AsNoTracking()
            .Where(r => r.AutorId == autorId)
            .OrderByDescending(r => r.CreadaEn)
            .ToListAsync(cancellationToken);

    public Task<Dictionary<int, int>> ContarPorPuntuacionAsync(CancellationToken cancellationToken) =>
        context.Resenas.AsNoTracking()
            .GroupBy(r => r.Puntuacion)
            .Select(g => new { Puntuacion = g.Key, Total = g.Count() })
            .ToDictionaryAsync(x => x.Puntuacion, x => x.Total, cancellationToken);
}
