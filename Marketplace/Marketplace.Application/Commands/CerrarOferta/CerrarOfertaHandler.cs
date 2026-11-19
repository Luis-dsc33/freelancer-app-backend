using MediatR;
using Marketplace.Application.Interfaces;
using Marketplace.Domain.Entities;

namespace Marketplace.Application.Commands.CerrarOferta;

public sealed class CerrarOfertaHandler(
    IProyectoRepository proyectos,
    ICurrentUserService currentUser) : IRequestHandler<CerrarOfertaCommand>
{
    public async Task Handle(CerrarOfertaCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.IsAuthenticated || currentUser.UserId == Guid.Empty)
            throw new UnauthorizedAccessException("No se pudo identificar al cliente autenticado.");

        var proyecto = await proyectos.ObtenerPorIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException("La oferta no existe.");
        if (proyecto.ClienteId != currentUser.UserId)
            throw new UnauthorizedAccessException("La oferta no pertenece al cliente autenticado.");
        if (proyecto.Estado == Proyecto.EstadoCerrado)
            throw new InvalidOperationException("La oferta ya está cerrada.");

        proyecto.Estado = Proyecto.EstadoCerrado;
        await proyectos.ActualizarAsync(proyecto, cancellationToken);
    }
}
