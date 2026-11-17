using MediatR;
using Marketplace.Application.Interfaces;

namespace Marketplace.Application.Queries.ListarPostulacionesOferta;

public sealed class ListarPostulacionesOfertaHandler(
    IProyectoRepository proyectos,
    IPostulacionRepository postulaciones,
    ICurrentUserService currentUser)
    : IRequestHandler<ListarPostulacionesOfertaQuery, IReadOnlyList<PostulacionDto>>
{
    public async Task<IReadOnlyList<PostulacionDto>> Handle(
        ListarPostulacionesOfertaQuery request,
        CancellationToken cancellationToken)
    {
        if (!currentUser.IsAuthenticated || currentUser.UserId == Guid.Empty)
            throw new UnauthorizedAccessException("No se pudo identificar al cliente autenticado.");

        var proyecto = await proyectos.ObtenerPorIdAsync(request.ProyectoId, cancellationToken)
            ?? throw new KeyNotFoundException("La oferta no existe.");
        if (proyecto.ClienteId != currentUser.UserId)
            throw new UnauthorizedAccessException("La oferta no pertenece al cliente autenticado.");

        var resultado = await postulaciones.ObtenerPorProyectoAsync(request.ProyectoId, cancellationToken);
        return resultado.Select(p => new PostulacionDto(
            p.Id, p.ProyectoId, p.EstudianteId, p.Estado, p.Mensaje, p.CreadaEn, p.ResueltaEn)).ToList();
    }
}
