using Marketplace.Application.Interfaces;
using Marketplace.Application.Queries.ListarOportunidades;
using Marketplace.Domain.Entities;
using MediatR;

namespace Marketplace.Application.Commands.PublicarOferta;

public sealed class PublicarOfertaHandler(
    IProyectoRepository proyectos,
    ICurrentUserService currentUser)
    : IRequestHandler<PublicarOfertaCommand, OportunidadProyectoDto>
{
    public async Task<OportunidadProyectoDto> Handle(
        PublicarOfertaCommand request,
        CancellationToken cancellationToken)
    {
        if (!currentUser.IsAuthenticated || currentUser.UserId == Guid.Empty)
            throw new UnauthorizedAccessException("No se pudo identificar al cliente autenticado.");

        var proyecto = new Proyecto
        {
            Id = Guid.NewGuid(),
            Titulo = request.Titulo.Trim(),
            Descripcion = request.Descripcion.Trim(),
            Disciplina = request.Disciplina.Trim(),
            Presupuesto = request.Presupuesto,
            FechaLimite = request.FechaLimite,
            ClienteId = currentUser.UserId,
            NombreCliente = string.IsNullOrWhiteSpace(currentUser.Nombre) ? "Cliente" : currentUser.Nombre,
            EsUrgente = request.EsUrgente,
            HabilidadesRequeridas = request.HabilidadesRequeridas
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList(),
            Estado = Proyecto.EstadoAbierto,
            CreadoEn = DateTime.UtcNow
        };

        await proyectos.AgregarAsync(proyecto, cancellationToken);

        return ProyectoMapper.ToDto(proyecto);
    }
}
