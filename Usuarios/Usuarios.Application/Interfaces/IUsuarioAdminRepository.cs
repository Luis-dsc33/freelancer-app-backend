// ADMIN Marcos: operaciones de datos que necesita el rol Administrador (HU-25, HU-21, HU-26, HU-28).
// Se separa de IUsuarioRepository para no mezclar el flujo de autenticación con el de administración.
using Usuarios.Domain.Entities;

namespace Usuarios.Application.Interfaces;

public interface IUsuarioAdminRepository
{
    Task<(List<Usuario> Usuarios, int Total)> BuscarAsync(
        string? busqueda, string? rol, string? estado, int pagina, int tamanoPagina, CancellationToken ct);

    Task<Usuario?> ObtenerParaActualizarAsync(Guid id, CancellationToken ct);
    Task GuardarCambiosAsync(CancellationToken ct);

    Task<Dictionary<string, int>> ContarPorRolAsync(CancellationToken ct);
    Task<int> ContarPorEstadoAsync(string estado, CancellationToken ct);
    Task<List<DateTime>> FechasDeRegistroDesdeAsync(DateTime desde, CancellationToken ct);
}
