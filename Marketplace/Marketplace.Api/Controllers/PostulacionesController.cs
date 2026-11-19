using Marketplace.Application.Commands.PostularAOferta;
using Marketplace.Application.Commands.ResolverPostulacion;
using Marketplace.Application.Commands.RetirarPostulacion;
using Marketplace.Application.Queries.ListarMisPostulaciones;
using Marketplace.Application.Queries.ListarPostulacionesOferta;
using Marketplace.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Api.Controllers;

[ApiController]
[Route("api")]
[Authorize]
public sealed class PostulacionesController(IMediator mediator) : ControllerBase
{
    [HttpGet("ofertas/{ofertaId:guid}/postulaciones")]
    [Authorize(Roles = "Cliente")]
    public async Task<IActionResult> Obtener(Guid ofertaId, CancellationToken cancellationToken)
    {
        return Ok(await mediator.Send(
            new ListarPostulacionesOfertaQuery(ofertaId), cancellationToken));
    }

    [HttpPost("ofertas/{ofertaId:guid}/postulaciones")]
    [Authorize(Roles = "Estudiante")]
    public async Task<IActionResult> Postular(
        Guid ofertaId,
        [FromBody] CrearPostulacionRequest request,
        CancellationToken cancellationToken)
    {
        var resultado = await mediator.Send(
            new PostularAOfertaCommand(ofertaId, request.Mensaje),
            cancellationToken);
        return StatusCode(StatusCodes.Status201Created, resultado);
    }

    [HttpGet("postulaciones/mis-postulaciones")]
    [Authorize(Roles = "Estudiante")]
    public async Task<IActionResult> MisPostulaciones(CancellationToken cancellationToken)
        => Ok(await mediator.Send(new ListarMisPostulacionesQuery(), cancellationToken));

    [HttpPut("postulaciones/{id:guid}/retirar")]
    [Authorize(Roles = "Estudiante")]
    public async Task<IActionResult> Retirar(Guid id, CancellationToken cancellationToken)
    {
        await mediator.Send(new RetirarPostulacionCommand(id), cancellationToken);
        return NoContent();
    }

    [HttpPut("postulaciones/{id:guid}/aceptar")]
    [Authorize(Roles = "Cliente")]
    public Task<IActionResult> Aceptar(Guid id, CancellationToken cancellationToken) =>
        Resolver(id, Postulacion.EstadoAceptada, cancellationToken);

    [HttpPut("postulaciones/{id:guid}/rechazar")]
    [Authorize(Roles = "Cliente")]
    public Task<IActionResult> Rechazar(Guid id, CancellationToken cancellationToken) =>
        Resolver(id, Postulacion.EstadoRechazada, cancellationToken);

    private async Task<IActionResult> Resolver(Guid id, string estado, CancellationToken cancellationToken)
    {
        return Ok(await mediator.Send(
            new ResolverPostulacionCommand(id, estado), cancellationToken));
    }
}

public sealed record CrearPostulacionRequest(string Mensaje);
