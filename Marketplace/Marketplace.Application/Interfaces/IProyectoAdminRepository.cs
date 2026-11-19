// ADMIN Marcos: operaciones de datos del rol Administrador sobre ofertas (HU-21, HU-26, HU-28).
// Separado de IProyectoRepository para no tocar el flujo de publicación/listado de estudiantes y clientes.
using Marketplace.Domain.Entities;

namespace Marketplace.Application.Interfaces;

public interface IProyectoAdminRepository
{
    Task<List<Proyecto>> ListarAsync(string? estado, CancellationToken ct);
    Task<Proyecto?> ObtenerParaActualizarAsync(Guid id, CancellationToken ct);
    Task GuardarCambiosAsync(CancellationToken ct);

    Task<int> ContarPorEstadosAsync(IReadOnlyCollection<string> estados, CancellationToken ct);
    Task<Dictionary<string, int>> ContarPorDisciplinaAsync(IReadOnlyCollection<string> estados, CancellationToken ct);
    Task<List<DateTime>> FechasDeCreacionDesdeAsync(DateTime desde, CancellationToken ct);
    Task<Dictionary<string, int>> ContarPostulacionesPorEstadoAsync(CancellationToken ct);
}
