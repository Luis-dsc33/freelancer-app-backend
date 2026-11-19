using Marketplace.Domain.Entities;

namespace Marketplace.Application.Interfaces;

public interface IPostulacionRepository
{
    Task<List<Postulacion>> ObtenerPorProyectoAsync(Guid proyectoId, CancellationToken cancellationToken);
    Task<Postulacion?> ObtenerPorIdAsync(Guid id, CancellationToken cancellationToken);
    Task<bool> ExisteParaEstudianteAsync(Guid proyectoId, Guid estudianteId, CancellationToken cancellationToken);
    Task ActualizarAsync(Postulacion postulacion, CancellationToken cancellationToken);
}
