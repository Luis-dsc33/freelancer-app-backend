using Marketplace.Application.Interfaces;
using MediatR;

namespace Marketplace.Application.Commands.RetirarPostulacion;

public sealed class RetirarPostulacionHandler(
    IPostulacionRepository postulaciones,
    ICurrentUserService currentUser)
    : IRequestHandler<RetirarPostulacionCommand>
{
    public async Task Handle(RetirarPostulacionCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.IsAuthenticated || currentUser.UserId == Guid.Empty)
            throw new UnauthorizedAccessException("No se pudo identificar al estudiante autenticado.");

        if (!currentUser.EsEstudiante)
            throw new UnauthorizedAccessException("Solo las cuentas de estudiante pueden retirar postulaciones.");

        var postulacion = await postulaciones.ObtenerPorIdAsync(request.PostulacionId, cancellationToken)
            ?? throw new KeyNotFoundException("La postulación no existe.");

        if (postulacion.EstudianteId != currentUser.UserId)
            throw new UnauthorizedAccessException("No tienes permiso para retirar esta postulación.");

        postulacion.Retirar();
        await postulaciones.ActualizarAsync(postulacion, cancellationToken);
    }
}
