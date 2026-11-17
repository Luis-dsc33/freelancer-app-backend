using MediatR;
using Usuarios.Application.Exceptions;
using Usuarios.Application.Interfaces;
using Usuarios.Domain.Entities;

namespace Usuarios.Application.Commands.AgregarTrabajoPortafolio;

public class AgregarTrabajoPortafolioHandler(
    ITrabajoPortafolioRepository trabajos,
    IUsuarioRepository usuarios,
    ICurrentUserService currentUser)
    : IRequestHandler<AgregarTrabajoPortafolioCommand, Guid>
{
    public async Task<Guid> Handle(AgregarTrabajoPortafolioCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.IsAuthenticated || currentUser.UserId == Guid.Empty)
            throw new UnauthorizedAccessException("No se pudo identificar al usuario autenticado.");

        var usuarioId = currentUser.UserId;
        var usuario = await usuarios.ObtenerPorIdAsync(usuarioId, cancellationToken);
        if (usuario is null || usuario.Rol.Nombre != "Estudiante" || usuario.Estado != "Activo")
            throw new InvalidOperationException("Solo un estudiante activo puede administrar su portafolio.");

        var trabajo = new TrabajoPortafolio { Id = Guid.NewGuid(), UsuarioId = usuarioId };
        trabajo.Titulo = request.Titulo.Trim();
        trabajo.Descripcion = request.Descripcion.Trim();
        trabajo.Enlace = string.IsNullOrWhiteSpace(request.Enlace) ? null : request.Enlace.Trim();
        await trabajos.AgregarAsync(trabajo, cancellationToken);
        return trabajo.Id;
    }
}
