using Usuarios.Domain.Entities;

namespace Usuarios.Application.Interfaces;

public interface IRecuperacionContrasenaRepository
{
    Task CrearAsync(RecuperacionContrasena recuperacion, DateTime ahora, CancellationToken cancellationToken);

    Task<bool> EsTokenValidoAsync(string tokenHash, DateTime ahora, CancellationToken cancellationToken);

    Task<bool> RestablecerContrasenaAsync(
        string tokenHash,
        string nuevoPasswordHash,
        DateTime ahora,
        CancellationToken cancellationToken);
}
