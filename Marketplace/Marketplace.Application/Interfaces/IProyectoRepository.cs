using Marketplace.Domain.Entities;

namespace Marketplace.Application.Interfaces;

public interface IProyectoRepository
{
    /// <summary>
    /// Proyectos visibles para estudiantes (estado Abierto), ordenados por
    /// urgencia descendente y fecha de creación descendente.
    /// </summary>
    Task<List<Proyecto>> ObtenerAbiertosAsync(CancellationToken cancellationToken);
}
