using Chat.Domain.Entities;

namespace Chat.Application.Interfaces;

public interface IMensajeRepository
{
    Task<List<Mensaje>> ListarPorConversacionAsync(Guid conversacionId, CancellationToken cancellationToken);
    Task<Dictionary<Guid, string>> ObtenerUltimosMensajesPorConversacionAsync(IEnumerable<Guid> conversacionIds, CancellationToken cancellationToken);
    Task AgregarAsync(Mensaje mensaje, CancellationToken cancellationToken);
}
