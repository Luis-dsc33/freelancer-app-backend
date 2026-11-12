using Usuarios.Domain.Entities;

namespace Usuarios.Application.Interfaces;

public interface IPerfilEstudianteRepository
{
    Task<PerfilEstudiante?> ObtenerPorUsuarioIdAsync(Guid usuarioId);
    Task AgregarAsync(PerfilEstudiante perfil);
    Task ActualizarAsync(PerfilEstudiante perfil);
}
