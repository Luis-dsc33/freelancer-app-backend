using Usuarios.Domain.Entities;

namespace Usuarios.Application.Interfaces;

public interface IPerfilClienteRepository
{
    Task<PerfilCliente?> ObtenerPorUsuarioIdAsync(Guid usuarioId);
    Task AgregarAsync(PerfilCliente perfil);
    Task ActualizarAsync(PerfilCliente perfil);
}
