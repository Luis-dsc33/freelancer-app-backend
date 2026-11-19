using Marketplace.Domain.Entities;

namespace Marketplace.Application.Interfaces;

public sealed record FiltroOportunidadesProyecto(
    string? Busqueda = null,
    string? Disciplina = null,
    string? Modalidad = null,
    decimal? PresupuestoMin = null,
    decimal? PresupuestoMax = null,
    bool? SoloUrgentes = null,
    IReadOnlyCollection<Guid>? SkillIds = null,
    string? Habilidad = null,
    int PageNumber = 1,
    int PageSize = 20);

public interface IProyectoRepository
{
    /// <summary>
    /// Proyectos visibles para estudiantes (estado Abierto o Publicada), ordenados por
    /// urgencia descendente y fecha de creación descendente.
    /// </summary>
    Task<List<Proyecto>> ObtenerAbiertosAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);

    Task<List<Proyecto>> BuscarAbiertosAsync(FiltroOportunidadesProyecto filtro, CancellationToken cancellationToken);

    Task<List<Proyecto>> ObtenerPorClienteAsync(Guid clienteId, int pageNumber, int pageSize, CancellationToken cancellationToken);

    Task<Proyecto?> ObtenerPorIdAsync(Guid id, CancellationToken cancellationToken);

    Task AgregarAsync(Proyecto proyecto, CancellationToken cancellationToken);

    Task ActualizarAsync(Proyecto proyecto, CancellationToken cancellationToken);

    Task EliminarAsync(Proyecto proyecto, CancellationToken cancellationToken);
}
