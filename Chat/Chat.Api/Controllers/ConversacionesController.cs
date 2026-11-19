using Chat.Application.Commands.AbrirConversacion;
using Chat.Application.Commands.EnviarMensajeChat;
using Chat.Application.Queries.ListarMensajesConversacion;
using Chat.Application.Queries.ListarMisConversaciones;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Api.Controllers;

[ApiController]
[Route("api/conversaciones")]
[Authorize]
public sealed class ConversacionesController(IMediator mediator) : ControllerBase
{
    [HttpGet("mias")]
    public async Task<IActionResult> Mias(CancellationToken cancellationToken)
        => Ok(await mediator.Send(new ListarMisConversacionesQuery(), cancellationToken));

    [HttpPost]
    public async Task<IActionResult> Abrir(
        [FromBody] AbrirConversacionRequest request,
        CancellationToken cancellationToken)
    {
        var resultado = await mediator.Send(
            new AbrirConversacionCommand(request.ProyectoId, request.PostulacionId, request.ContratoId),
            cancellationToken);

        return resultado.EsNueva
            ? StatusCode(StatusCodes.Status201Created, resultado.Conversacion)
            : Ok(resultado.Conversacion);
    }

    [HttpGet("{id:guid}/mensajes")]
    public async Task<IActionResult> Mensajes(Guid id, CancellationToken cancellationToken)
        => Ok(await mediator.Send(new ListarMensajesConversacionQuery(id), cancellationToken));

    [HttpPost("{id:guid}/mensajes")]
    public async Task<IActionResult> EnviarMensaje(
        Guid id,
        [FromBody] EnviarMensajeRequest request,
        CancellationToken cancellationToken)
    {
        var creado = await mediator.Send(
            new EnviarMensajeChatCommand(id, request.Contenido),
            cancellationToken);
        return StatusCode(StatusCodes.Status201Created, creado);
    }
}

public sealed record AbrirConversacionRequest(
    Guid ProyectoId,
    Guid? PostulacionId = null,
    Guid? ContratoId = null);

public sealed record EnviarMensajeRequest(string Contenido);
