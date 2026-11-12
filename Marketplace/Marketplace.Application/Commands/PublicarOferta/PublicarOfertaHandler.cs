using Marketplace.Application.Interfaces;
using Marketplace.Application.Queries.ListarOportunidades;
using Marketplace.Domain.Entities;
using MediatR;

namespace Marketplace.Application.Commands.PublicarOferta;

public sealed class PublicarOfertaHandler(IProyectoRepository proyectos)
    : IRequestHandler<PublicarOfertaCommand, OportunidadProyectoDto>
{
    public async Task<OportunidadProyectoDto> Handle(
        PublicarOfertaCommand request,
        CancellationToken cancellationToken)
    {
        var proyecto = new Proyecto
        {
            Id = Guid.NewGuid(),
            Titulo = request.Titulo.Trim(),
            Descripcion = request.Descripcion.Trim(),
            Disciplina = request.Disciplina.Trim(),
            Presupuesto = request.Presupuesto,
            FechaLimite = request.FechaLimite,
            ClienteId = request.ClienteId,
            NombreCliente = request.NombreCliente,
            Estado = Proyecto.EstadoAbierto,
            CreadoEn = DateTime.UtcNow
        };

        await proyectos.AgregarAsync(proyecto, cancellationToken);

        return new OportunidadProyectoDto(
            proyecto.Id,
            proyecto.Titulo,
            proyecto.Descripcion,
            proyecto.Disciplina,
            proyecto.Presupuesto,
            proyecto.FechaLimite,
            proyecto.EsUrgente,
            proyecto.HabilidadesRequeridas,
            proyecto.NombreCliente,
            proyecto.Estado,
            proyecto.CreadoEn);
    }
}
