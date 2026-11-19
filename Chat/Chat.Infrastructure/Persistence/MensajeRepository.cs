using Chat.Application.Interfaces;
using Chat.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chat.Infrastructure.Persistence;

public sealed class MensajeRepository(ChatDbContext context) : IMensajeRepository
{
    public Task<List<Mensaje>> ListarPorConversacionAsync(Guid conversacionId, CancellationToken cancellationToken) =>
        context.Mensajes.AsNoTracking()
            .Where(m => m.ConversacionId == conversacionId)
            .OrderBy(m => m.EnviadoEn)
            .ToListAsync(cancellationToken);

    public async Task<Dictionary<Guid, string>> ObtenerUltimosMensajesPorConversacionAsync(
        IEnumerable<Guid> conversacionIds,
        CancellationToken cancellationToken)
    {
        var ids = conversacionIds.Distinct().ToList();
        if (ids.Count == 0)
            return [];

        var ultimos = await context.Mensajes.AsNoTracking()
            .Where(m => ids.Contains(m.ConversacionId))
            .GroupBy(m => m.ConversacionId)
            .Select(g => g.OrderByDescending(m => m.EnviadoEn).Select(m => new { m.ConversacionId, m.Contenido }).First())
            .ToListAsync(cancellationToken);

        return ultimos.ToDictionary(x => x.ConversacionId, x => x.Contenido);
    }

    public async Task AgregarAsync(Mensaje mensaje, CancellationToken cancellationToken)
    {
        await context.Mensajes.AddAsync(mensaje, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }
}
