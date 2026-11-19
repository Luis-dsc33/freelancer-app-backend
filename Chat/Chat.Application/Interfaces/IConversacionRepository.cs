using Chat.Domain.Entities;

namespace Chat.Application.Interfaces;

public interface IConversacionRepository
{
    Task<Conversacion?> ObtenerPorIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Conversacion?> ObtenerPorParticipantesAsync(Guid proyectoId, Guid clienteId, Guid estudianteId, CancellationToken cancellationToken);
    Task<List<Conversacion>> ListarPorUsuarioAsync(Guid usuarioId, CancellationToken cancellationToken);
    Task<bool> EsParticipanteAsync(Guid conversacionId, Guid usuarioId, CancellationToken cancellationToken);
    Task AgregarAsync(Conversacion conversacion, CancellationToken cancellationToken);
    Task GuardarCambiosAsync(CancellationToken cancellationToken);
}
