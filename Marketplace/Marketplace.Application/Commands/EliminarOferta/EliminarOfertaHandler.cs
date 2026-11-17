using MediatR;
using Marketplace.Application.Interfaces;
using Marketplace.Domain.Entities;

namespace Marketplace.Application.Commands.EliminarOferta;

public sealed class EliminarOfertaHandler(
    IProyectoRepository proyectos,
    ICurrentUserService currentUser) : IRequestHandler<EliminarOfertaCommand>
{
    public async Task Handle(EliminarOfertaCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.IsAuthenticated || currentUser.UserId == Guid.Empty)
            throw new UnauthorizedAccessException("No se pudo identificar al cliente autenticado.");

        var proyecto = await proyectos.ObtenerPorIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException("La oferta no existe.");
        if (proyecto.ClienteId != currentUser.UserId)
            throw new UnauthorizedAccessException("La oferta no pertenece al cliente autenticado.");
        if (proyecto.Estado == Proyecto.EstadoCancelado)
            throw new InvalidOperationException("La oferta ya está cancelada.");
        if (proyecto.Estado == Proyecto.EstadoEnProceso)
            throw new InvalidOperationException("Una oferta en proceso no puede eliminarse.");

        proyecto.Estado = Proyecto.EstadoCancelado;
        proyecto.ActualizadoEn = DateTime.UtcNow;
        await proyectos.ActualizarAsync(proyecto, cancellationToken);
    }
}
