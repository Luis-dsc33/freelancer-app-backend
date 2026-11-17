using Usuarios.Domain.Entities;

namespace Usuarios.Application.Interfaces;

public interface ITrabajoPortafolioRepository
{
    Task<List<TrabajoPortafolio>> ObtenerPorUsuarioAsync(Guid usuarioId, CancellationToken cancellationToken);
    Task<TrabajoPortafolio?> ObtenerPropioAsync(Guid id, Guid usuarioId, CancellationToken cancellationToken);
    Task AgregarAsync(TrabajoPortafolio trabajo, CancellationToken cancellationToken);
    Task GuardarCambiosAsync(CancellationToken cancellationToken);
    Task EliminarAsync(TrabajoPortafolio trabajo, CancellationToken cancellationToken);
}
