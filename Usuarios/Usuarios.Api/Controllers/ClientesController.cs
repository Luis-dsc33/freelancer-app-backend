using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Usuarios.Application.Commands.CrearOActualizarPerfilCliente;
using Usuarios.Application.Queries.ObtenerPerfilCliente;

namespace Usuarios.Api.Controllers;

[ApiController]
[Route("api/clientes")]
[Authorize(Roles = "Cliente")]
public sealed class ClientesController(IMediator mediator) : ControllerBase
{
    [HttpGet("mi-perfil")]
    public async Task<IActionResult> ObtenerMiPerfil(CancellationToken cancellationToken)
    {
        var perfil = await mediator.Send(new ObtenerPerfilClienteQuery(), cancellationToken);
        return perfil is null ? NotFound() : Ok(perfil);
    }

    [HttpPost("mi-perfil")]
    public async Task<IActionResult> CrearPerfil(
        CrearOActualizarPerfilClienteCommand command,
        CancellationToken cancellationToken)
    {
        var perfilId = await mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(ObtenerMiPerfil), new { }, new { PerfilId = perfilId });
    }

    [HttpPut("mi-perfil")]
    public async Task<IActionResult> ActualizarPerfil(
        CrearOActualizarPerfilClienteCommand command,
        CancellationToken cancellationToken)
    {
        var perfilId = await mediator.Send(command, cancellationToken);
        return Ok(new { PerfilId = perfilId });
    }
}
