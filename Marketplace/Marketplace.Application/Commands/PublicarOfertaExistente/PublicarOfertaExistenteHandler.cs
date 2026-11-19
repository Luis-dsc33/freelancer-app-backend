using Marketplace.Application.Interfaces;
using Marketplace.Application.Queries.ListarOportunidades;
using Marketplace.Domain.Entities;
using MediatR;

namespace Marketplace.Application.Commands.PublicarOfertaExistente;

public sealed class PublicarOfertaExistenteHandler(
    IProyectoRepository proyectos,
    ICurrentUserService currentUser) : IRequestHandler<PublicarOfertaExistenteCommand, OportunidadProyectoDto>
{
    public async Task<OportunidadProyectoDto> Handle(PublicarOfertaExistenteCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.IsAuthenticated || currentUser.UserId == Guid.Empty)
            throw new UnauthorizedAccessException("No se pudo identificar al cliente autenticado.");

        var proyecto = await proyectos.ObtenerPorIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException("La oferta no existe.");
        if (proyecto.ClienteId != currentUser.UserId)
            throw new UnauthorizedAccessException("La oferta no pertenece al cliente autenticado.");
        if (proyecto.Estado is Proyecto.EstadoCerrado or Proyecto.EstadoCancelado)
            throw new InvalidOperationException("La oferta no puede publicarse desde su estado actual.");
        // Moderación: una oferta ocultada por un administrador no puede volver a publicarse desde el cliente
        // (solo el admin la restaura). Evita que el cliente deshaga la moderación.
        if (proyecto.Estado == Proyecto.EstadoOculta)
            throw new InvalidOperationException("La oferta fue ocultada por moderación y no puede publicarse.");
        if (proyecto.FechaLimite < DateOnly.FromDateTime(DateTime.UtcNow))
            throw new InvalidOperationException("La fecha límite de postulación ya venció.");
        if (proyecto.Skills.Count(x => x.TipoRequisito == Domain.Enums.TipoRequisitoSkill.Requerida) == 0)
            throw new InvalidOperationException("La oferta requiere al menos una habilidad obligatoria.");

        proyecto.Estado = Proyecto.EstadoPublicada;
        proyecto.PublicadoEn = DateTime.UtcNow;
        proyecto.ActualizadoEn = DateTime.UtcNow;
        await proyectos.ActualizarAsync(proyecto, cancellationToken);
        return ProyectoMapper.ToDto(proyecto);
    }
}
