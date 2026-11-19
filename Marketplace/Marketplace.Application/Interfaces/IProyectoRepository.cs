using Marketplace.Domain.Entities;

namespace Marketplace.Application.Interfaces;

public interface IProyectoRepository
{
    /// <summary>
    /// Proyectos visibles para estudiantes (estado Abierto), ordenados por
    /// urgencia descendente y fecha de creación descendente.
    /// </summary>
    Task<List<Proyecto>> ObtenerAbiertosAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);

    Task<List<Proyecto>> ObtenerPorClienteAsync(Guid clienteId, int pageNumber, int pageSize, CancellationToken cancellationToken);

    Task<Proyecto?> ObtenerPorIdAsync(Guid id, CancellationToken cancellationToken);

    Task AgregarAsync(Proyecto proyecto, CancellationToken cancellationToken);

    Task ActualizarAsync(Proyecto proyecto, CancellationToken cancellationToken);

    Task EliminarAsync(Proyecto proyecto, CancellationToken cancellationToken);
}
