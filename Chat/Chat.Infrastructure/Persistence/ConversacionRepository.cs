using Chat.Application.Interfaces;
using Chat.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chat.Infrastructure.Persistence;

public sealed class ConversacionRepository(ChatDbContext context) : IConversacionRepository
{
    public Task<Conversacion?> ObtenerPorIdAsync(Guid id, CancellationToken cancellationToken) =>
        context.Conversaciones.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

    public Task<Conversacion?> ObtenerPorParticipantesAsync(
        Guid proyectoId,
        Guid clienteId,
        Guid estudianteId,
        CancellationToken cancellationToken) =>
        context.Conversaciones.FirstOrDefaultAsync(
            c => c.ProyectoId == proyectoId && c.ClienteId == clienteId && c.EstudianteId == estudianteId,
            cancellationToken);

    public Task<List<Conversacion>> ListarPorUsuarioAsync(Guid usuarioId, CancellationToken cancellationToken) =>
        context.Conversaciones.AsNoTracking()
            .Where(c => c.ClienteId == usuarioId || c.EstudianteId == usuarioId)
            .OrderByDescending(c => c.UltimaActividadEn)
            .ToListAsync(cancellationToken);

    public Task<bool> EsParticipanteAsync(Guid conversacionId, Guid usuarioId, CancellationToken cancellationToken) =>
        context.Conversaciones.AsNoTracking()
            .AnyAsync(
                c => c.Id == conversacionId && (c.ClienteId == usuarioId || c.EstudianteId == usuarioId),
                cancellationToken);

    public async Task AgregarAsync(Conversacion conversacion, CancellationToken cancellationToken)
    {
        await context.Conversaciones.AddAsync(conversacion, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public Task GuardarCambiosAsync(CancellationToken cancellationToken) =>
        context.SaveChangesAsync(cancellationToken);
}
