namespace Marketplace.Application.Interfaces;

public sealed record EstadoElegibilidadEstudiante(bool PerfilCompleto, bool TienePortafolio);

public interface IUsuariosEstudianteClient
{
    Task<EstadoElegibilidadEstudiante?> ObtenerEstadoEstudianteAsync(CancellationToken cancellationToken);
}
