using Marketplace.Application.Interfaces;
using Marketplace.Application.Queries.ListarOportunidades;
using MediatR;

namespace Marketplace.Application.Queries.ObtenerMiOferta;

public sealed class ObtenerMiOfertaHandler(IProyectoRepository proyectos, ICurrentUserService currentUser)
    : IRequestHandler<ObtenerMiOfertaQuery, OportunidadProyectoDto>
{
    public async Task<OportunidadProyectoDto> Handle(ObtenerMiOfertaQuery request, CancellationToken cancellationToken)
    {
        if (!currentUser.IsAuthenticated || currentUser.UserId == Guid.Empty)
            throw new UnauthorizedAccessException("No se pudo identificar al cliente autenticado.");
        var oferta = await proyectos.ObtenerPorIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException("La oferta no existe.");
        if (oferta.ClienteId != currentUser.UserId)
            throw new UnauthorizedAccessException("La oferta no pertenece al cliente autenticado.");
        return ProyectoMapper.ToDto(oferta);
    }
}
