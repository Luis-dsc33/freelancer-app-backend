using MediatR;
using Marketplace.Application.Interfaces;
using Marketplace.Application.Queries.ListarOportunidades;

namespace Marketplace.Application.Queries.ListarMisOfertas;

public sealed class ListarMisOfertasHandler(
    IProyectoRepository proyectos,
    ICurrentUserService currentUser)
    : IRequestHandler<ListarMisOfertasQuery, IReadOnlyList<OportunidadProyectoDto>>
{
    public async Task<IReadOnlyList<OportunidadProyectoDto>> Handle(
        ListarMisOfertasQuery request,
        CancellationToken cancellationToken)
    {
        if (!currentUser.IsAuthenticated || currentUser.UserId == Guid.Empty)
            throw new UnauthorizedAccessException("No se pudo identificar al cliente autenticado.");

        var pageNumber = Math.Max(1, request.PageNumber);
        var pageSize = Math.Clamp(request.PageSize, 1, 100);
        var ofertas = await proyectos.ObtenerPorClienteAsync(currentUser.UserId, pageNumber, pageSize, cancellationToken);
        return ofertas.Select(ProyectoMapper.ToDto).ToList();
    }
}
