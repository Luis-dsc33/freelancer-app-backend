using Marketplace.Application.Commands.ResolverPostulacion;
using Marketplace.Application.Queries.ListarPostulacionesOferta;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Marketplace.Domain.Entities;

namespace Marketplace.Api.Controllers;

[ApiController]
[Route("api")]
[Authorize(Roles = "Cliente")]
public sealed class PostulacionesController(IMediator mediator) : ControllerBase
{
    [HttpGet("ofertas/{ofertaId:guid}/postulaciones")]
    public async Task<IActionResult> Obtener(Guid ofertaId, CancellationToken cancellationToken)
    {
        return Ok(await mediator.Send(
            new ListarPostulacionesOfertaQuery(ofertaId), cancellationToken));
    }

    [HttpPut("postulaciones/{id:guid}/aceptar")]
    public Task<IActionResult> Aceptar(Guid id, CancellationToken cancellationToken) =>
        Resolver(id, Postulacion.EstadoAceptada, cancellationToken);

    [HttpPut("postulaciones/{id:guid}/rechazar")]
    public Task<IActionResult> Rechazar(Guid id, CancellationToken cancellationToken) =>
        Resolver(id, Postulacion.EstadoRechazada, cancellationToken);

    private async Task<IActionResult> Resolver(Guid id, string estado, CancellationToken cancellationToken)
    {
        return Ok(await mediator.Send(
            new ResolverPostulacionCommand(id, estado), cancellationToken));
    }
}
