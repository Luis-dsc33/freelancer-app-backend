using Marketplace.Application.Exceptions;
using Marketplace.Application.Interfaces;
using Marketplace.Application.Queries.ListarMisPostulaciones;
using Marketplace.Domain.Entities;
using MediatR;

namespace Marketplace.Application.Commands.PostularAOferta;

public sealed class PostularAOfertaHandler(
    IProyectoRepository proyectos,
    IPostulacionRepository postulaciones,
    ICurrentUserService currentUser,
    IUsuariosEstudianteClient? usuariosClient = null)
    : IRequestHandler<PostularAOfertaCommand, PostulacionEstudianteDto>
{
    public async Task<PostulacionEstudianteDto> Handle(
        PostularAOfertaCommand request,
        CancellationToken cancellationToken)
    {
        if (!currentUser.IsAuthenticated || currentUser.UserId == Guid.Empty)
            throw new UnauthorizedAccessException("No se pudo identificar al estudiante autenticado.");

        if (!currentUser.EsEstudiante)
            throw new UnauthorizedAccessException("Solo las cuentas de estudiante pueden postularse a ofertas.");

        var proyecto = await proyectos.ObtenerPorIdAsync(request.OfertaId, cancellationToken)
            ?? throw new KeyNotFoundException("La oferta no existe.");

        var hoy = DateOnly.FromDateTime(DateTime.UtcNow);
        if (proyecto.Estado is not (Proyecto.EstadoAbierto or Proyecto.EstadoPublicada))
            throw new InvalidOperationException("Solo puedes postularte a ofertas abiertas o publicadas.");

        if (proyecto.FechaLimite < hoy)
            throw new InvalidOperationException("La fecha límite de postulación ya venció.");

        if (await postulaciones.ExisteParaEstudianteAsync(proyecto.Id, currentUser.UserId, cancellationToken))
            throw new ConflictException("Ya te postulaste previamente a esta oportunidad.");

        if (usuariosClient is not null)
        {
            var elegibilidad = await usuariosClient.ObtenerEstadoEstudianteAsync(cancellationToken);
            if (elegibilidad is not null)
            {
                if (!elegibilidad.PerfilCompleto)
                    throw new InvalidOperationException("Debes completar tu perfil de estudiante antes de postularte a una oferta.");

                if (proyecto.RequierePortafolio && !elegibilidad.TienePortafolio)
                    throw new InvalidOperationException("Esta oferta requiere que tengas al menos un trabajo publicado en tu portafolio.");
            }
        }

        var postulacion = new Postulacion
        {
            Id = Guid.NewGuid(),
            ProyectoId = proyecto.Id,
            EstudianteId = currentUser.UserId,
            Estado = Postulacion.EstadoPendiente,
            Mensaje = request.Mensaje.Trim(),
            CreadaEn = DateTime.UtcNow,
            Proyecto = proyecto
        };

        await postulaciones.AgregarAsync(postulacion, cancellationToken);

        var presupuesto = proyecto.Presupuesto > 0
            ? proyecto.Presupuesto
            : proyecto.PresupuestoMaximo ?? proyecto.PresupuestoMinimo ?? 0m;

        return new PostulacionEstudianteDto(
            postulacion.Id,
            postulacion.ProyectoId,
            postulacion.EstudianteId,
            postulacion.Mensaje,
            postulacion.Estado,
            DateTime.SpecifyKind(postulacion.CreadaEn, DateTimeKind.Utc),
            null,
            proyecto.Titulo,
            proyecto.Disciplina,
            proyecto.Modalidad.ToString(),
            presupuesto,
            proyecto.NombreCliente,
            proyecto.Estado,
            null);
    }
}
