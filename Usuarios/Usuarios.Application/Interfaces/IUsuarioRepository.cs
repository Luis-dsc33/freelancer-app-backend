using Usuarios.Domain.Entities;

namespace Usuarios.Application.Interfaces;

public interface IUsuarioRepository
{
    Task<bool> ExisteEmailAsync(string email);
    Task AgregarAsync(Usuario usuario);
    Task<List<Usuario>> ObtenerTodosAsync();
    Task<Usuario?> ObtenerPorEmailAsync(string email);
}
