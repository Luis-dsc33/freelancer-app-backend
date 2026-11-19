using MediatR;
using Marketplace.Application.Interfaces;
using Marketplace.Application.Queries.ListarPostulacionesOferta;
using Marketplace.Domain.Entities;

namespace Marketplace.Application.Commands.ResolverPostulacion;

public sealed class ResolverPostulacionHandler(
    IPostulacionRepository postulaciones,
    ICurrentUserService currentUser)
    : IRequestHandler<ResolverPostulacionCommand, PostulacionDto>
{
    public async Task<PostulacionDto> Handle(ResolverPostulacionCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.IsAuthenticated || currentUser.UserId == Guid.Empty)
            throw new UnauthorizedAccessException("No se pudo identificar al cliente autenticado.");

        var postulacion = await postulaciones.ObtenerPorIdAsync(request.PostulacionId, cancellationToken)
            ?? throw new KeyNotFoundException("La postulación no existe.");
        if (postulacion.Proyecto.ClienteId != currentUser.UserId)
            throw new UnauthorizedAccessException("La postulación no pertenece a una oferta del cliente autenticado.");
        if (postulacion.Estado != Postulacion.EstadoPendiente)
            throw new InvalidOperationException("La postulación ya fue resuelta.");
        if (postulacion.Proyecto.Estado != Proyecto.EstadoAbierto)
            throw new InvalidOperationException("La oferta no está disponible.");

        if (request.Estado == Postulacion.EstadoAceptada)
        {
            var existentes = await postulaciones.ObtenerPorProyectoAsync(postulacion.ProyectoId, cancellationToken);
            if (existentes.Any(x => x.Estado == Postulacion.EstadoAceptada))
                throw new InvalidOperationException("La oferta ya tiene una postulación aceptada.");
        }

        postulacion.Estado = request.Estado;
        postulacion.ResueltaEn = DateTime.UtcNow;
        await postulaciones.ActualizarAsync(postulacion, cancellationToken);

        return new PostulacionDto(
            postulacion.Id,
            postulacion.ProyectoId,
            postulacion.EstudianteId,
            postulacion.Estado,
            postulacion.Mensaje,
            postulacion.CreadaEn,
            postulacion.ResueltaEn);
    }
}
