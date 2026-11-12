using MediatR;
using Usuarios.Application.Exceptions;
using Usuarios.Application.Interfaces;

namespace Usuarios.Application.Commands.EliminarTrabajoPortafolio;

public class EliminarTrabajoPortafolioHandler(
    ITrabajoPortafolioRepository trabajos,
    IUsuarioRepository usuarios,
    ICurrentUserService currentUser)
    : IRequestHandler<EliminarTrabajoPortafolioCommand>
{
    public async Task Handle(EliminarTrabajoPortafolioCommand request, CancellationToken cancellationToken)
    {
        var usuarioId = currentUser.UsuarioId;
        var usuario = await usuarios.ObtenerPorIdAsync(usuarioId, cancellationToken);
        if (usuario is null || usuario.Rol.Nombre != "Estudiante" || usuario.Estado != "Activo")
            throw new InvalidOperationException("Solo un estudiante activo puede administrar su portafolio.");

        var trabajo = await trabajos.ObtenerPropioAsync(request.Id, usuarioId, cancellationToken)
            ?? throw new RecursoNoEncontradoException("No se encontró el trabajo en tu portafolio.");
        await trabajos.EliminarAsync(trabajo, cancellationToken);
    }
}
