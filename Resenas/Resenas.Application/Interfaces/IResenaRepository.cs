using Resenas.Domain.Entities;

namespace Resenas.Application.Interfaces;

public interface IResenaRepository
{
    Task<bool> ExisteAsync(Guid contratoId, Guid autorId, Guid destinatarioId, CancellationToken cancellationToken);
    Task AgregarAsync(Resena resena, CancellationToken cancellationToken);
    Task<List<Resena>> ListarPorDestinatarioAsync(Guid destinatarioId, CancellationToken cancellationToken);
    Task<List<Resena>> ListarPorAutorAsync(Guid autorId, CancellationToken cancellationToken);
    Task<Dictionary<int, int>> ContarPorPuntuacionAsync(CancellationToken cancellationToken);
}
