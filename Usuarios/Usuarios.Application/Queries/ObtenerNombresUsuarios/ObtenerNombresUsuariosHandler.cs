using MediatR;
using Usuarios.Application.Interfaces;

namespace Usuarios.Application.Queries.ObtenerNombresUsuarios;

public sealed class ObtenerNombresUsuariosHandler(
    IUsuarioRepository usuarios,
    ICurrentUserService currentUser)
    : IRequestHandler<ObtenerNombresUsuariosQuery, IReadOnlyDictionary<Guid, string>>
{
    public Task<IReadOnlyDictionary<Guid, string>> Handle(
        ObtenerNombresUsuariosQuery request,
        CancellationToken cancellationToken)
    {
        if (!currentUser.IsAuthenticated || currentUser.UserId == Guid.Empty)
            throw new UnauthorizedAccessException("No se pudo identificar al usuario autenticado.");

        return usuarios.ObtenerNombresPorIdsAsync(request.UsuarioIds ?? [], cancellationToken);
    }
}
