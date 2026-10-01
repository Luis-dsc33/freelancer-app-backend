using MediatR;
using Usuarios.Application.Exceptions;
using Usuarios.Application.Interfaces;
using Usuarios.Domain.Entities;

namespace Usuarios.Application.Commands.ModificarTrabajoPortafolio;

public class ModificarTrabajoPortafolioHandler(ITrabajoPortafolioRepository trabajos, IUsuarioRepository usuarios)
    : IRequestHandler<ModificarTrabajoPortafolioCommand, Guid>
{
    public async Task<Guid> Handle(ModificarTrabajoPortafolioCommand request, CancellationToken cancellationToken)
    {
        var usuario = await usuarios.ObtenerPorIdAsync(request.UsuarioId, cancellationToken);
        if (usuario is null || usuario.Rol.Nombre != "Estudiante" || usuario.Estado != "Activo")
            throw new InvalidOperationException("Solo un estudiante activo puede administrar su portafolio.");

        var trabajo = await trabajos.ObtenerPropioAsync(request.Id, request.UsuarioId, cancellationToken)
            ?? throw new RecursoNoEncontradoException("No se encontró el trabajo en tu portafolio.");
        trabajo.Titulo = request.Titulo.Trim();
        trabajo.Descripcion = request.Descripcion.Trim();
        trabajo.Enlace = string.IsNullOrWhiteSpace(request.Enlace) ? null : request.Enlace.Trim();
        trabajo.ActualizadoEn = DateTime.UtcNow;
        await trabajos.GuardarCambiosAsync(cancellationToken);
        return trabajo.Id;
    }
}
