using MediatR;
using Usuarios.Application.Exceptions;
using Usuarios.Application.Interfaces;

namespace Usuarios.Application.Commands.EliminarTrabajoPortafolio;

public class EliminarTrabajoPortafolioHandler(ITrabajoPortafolioRepository trabajos, IUsuarioRepository usuarios)
    : IRequestHandler<EliminarTrabajoPortafolioCommand>
{
    public async Task Handle(EliminarTrabajoPortafolioCommand request, CancellationToken cancellationToken)
    {
        var usuario = await usuarios.ObtenerPorIdAsync(request.UsuarioId, cancellationToken);
        if (usuario is null || usuario.Rol.Nombre != "Estudiante" || usuario.Estado != "Activo")
            throw new InvalidOperationException("Solo un estudiante activo puede administrar su portafolio.");

        var trabajo = await trabajos.ObtenerPropioAsync(request.Id, request.UsuarioId, cancellationToken)
            ?? throw new RecursoNoEncontradoException("No se encontró el trabajo en tu portafolio.");
        await trabajos.EliminarAsync(trabajo, cancellationToken);
    }
}
