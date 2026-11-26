using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resenas.Application.Commands.CalificarCliente;
using Resenas.Application.Commands.CalificarEstudiante;
using Resenas.Application.Queries.ListarMisResenasEmitidas;
using Resenas.Application.Queries.ObtenerReputacionUsuario;

namespace Resenas.Api.Controllers;

[ApiController]
[Route("api")]
[Authorize]
public sealed class ResenasController(IMediator mediator) : ControllerBase
{
    [HttpPost("contratos/{contratoId:guid}/cliente")]
    public async Task<IActionResult> CalificarCliente(
        Guid contratoId,
        [FromBody] CalificarClienteRequest request,
        CancellationToken cancellationToken)
    {
        var resultado = await mediator.Send(
            new CalificarClienteCommand(contratoId, request.Puntuacion, request.Comentario),
            cancellationToken);
        return StatusCode(StatusCodes.Status201Created, resultado);
    }

    [HttpPost("contratos/{contratoId:guid}/estudiante")]
    public async Task<IActionResult> CalificarEstudiante(
        Guid contratoId,
        [FromBody] CalificarEstudianteRequest request,
        CancellationToken cancellationToken)
    {
        var resultado = await mediator.Send(
            new CalificarEstudianteCommand(contratoId, request.Puntuacion, request.Comentario),
            cancellationToken);
        return StatusCode(StatusCodes.Status201Created, resultado);
    }

    [HttpGet("usuarios/{usuarioId:guid}/reputacion")]
    public async Task<IActionResult> ObtenerReputacion(Guid usuarioId, CancellationToken cancellationToken)
        => Ok(await mediator.Send(new ObtenerReputacionUsuarioQuery(usuarioId), cancellationToken));

    [HttpGet("mis-resenas-emitidas")]
    public async Task<IActionResult> MisResenasEmitidas(CancellationToken cancellationToken)
        => Ok(await mediator.Send(new ListarMisResenasEmitidasQuery(), cancellationToken));
}

public sealed record CalificarClienteRequest(int Puntuacion, string Comentario);

public sealed record CalificarEstudianteRequest(int Puntuacion, string Comentario);
