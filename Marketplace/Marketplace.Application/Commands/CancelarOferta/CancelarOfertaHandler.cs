using Marketplace.Application.Interfaces;
using Marketplace.Domain.Entities;
using MediatR;

namespace Marketplace.Application.Commands.CancelarOferta;

public sealed class CancelarOfertaHandler(
    IProyectoRepository proyectos,
    ICurrentUserService currentUser) : IRequestHandler<CancelarOfertaCommand>
{
    public async Task Handle(CancelarOfertaCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.IsAuthenticated || currentUser.UserId == Guid.Empty)
            throw new UnauthorizedAccessException("No se pudo identificar al cliente autenticado.");

        var proyecto = await proyectos.ObtenerPorIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException("La oferta no existe.");
        if (proyecto.ClienteId != currentUser.UserId)
            throw new UnauthorizedAccessException("La oferta no pertenece al cliente autenticado.");
        if (proyecto.Estado is Proyecto.EstadoCerrado or Proyecto.EstadoCancelado)
            throw new InvalidOperationException("La oferta ya no puede cancelarse.");

        proyecto.Estado = Proyecto.EstadoCancelado;
        proyecto.ActualizadoEn = DateTime.UtcNow;
        await proyectos.ActualizarAsync(proyecto, cancellationToken);
    }
}
