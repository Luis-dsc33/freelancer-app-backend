using Marketplace.Domain.Entities;

namespace Marketplace.Application.Interfaces;

public interface IPostulacionRepository
{
    Task<List<Postulacion>> ObtenerPorProyectoAsync(Guid proyectoId, CancellationToken cancellationToken);
    Task<Postulacion?> ObtenerPorIdAsync(Guid id, CancellationToken cancellationToken);
    Task<bool> ExisteParaEstudianteAsync(Guid proyectoId, Guid estudianteId, CancellationToken cancellationToken);
    Task<HashSet<Guid>> ObtenerProyectosPostuladosPorEstudianteAsync(Guid estudianteId, IEnumerable<Guid> proyectoIds, CancellationToken cancellationToken);
    Task<List<Postulacion>> ListarPorEstudianteAsync(Guid estudianteId, CancellationToken cancellationToken);
    Task<Postulacion?> ObtenerParaEstudianteEnProyectoAsync(Guid proyectoId, Guid estudianteId, CancellationToken cancellationToken);
    Task AgregarAsync(Postulacion postulacion, CancellationToken cancellationToken);
    Task ActualizarAsync(Postulacion postulacion, CancellationToken cancellationToken);
}
