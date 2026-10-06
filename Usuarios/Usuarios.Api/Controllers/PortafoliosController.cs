using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Usuarios.Application.Commands.AgregarTrabajoPortafolio;
using Usuarios.Application.Commands.ModificarTrabajoPortafolio;
using Usuarios.Application.Commands.EliminarTrabajoPortafolio;
using Usuarios.Application.Queries.ObtenerPortafolio;

namespace Usuarios.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Estudiante")]
public class PortafoliosController(IMediator mediator) : ControllerBase
{
    [HttpGet("mi-portafolio")]
    public async Task<IActionResult> ObtenerMiPortafolio(CancellationToken cancellationToken)
    {
        if (!TryObtenerUsuarioId(out var usuarioId)) return Unauthorized();
        return Ok(await mediator.Send(new ObtenerPortafolioQuery(usuarioId), cancellationToken));
    }

    [AllowAnonymous]
    [HttpGet("estudiantes/{usuarioId:guid}")]
    public async Task<IActionResult> ObtenerPortafolio(Guid usuarioId, CancellationToken cancellationToken) =>
        Ok(await mediator.Send(new ObtenerPortafolioQuery(usuarioId), cancellationToken));

    [HttpPost("trabajos")]
    public async Task<IActionResult> Agregar(AgregarTrabajoPortafolioCommand command, CancellationToken cancellationToken)
    {
        if (!TryObtenerUsuarioId(out var usuarioId)) return Unauthorized();
        var id = await mediator.Send(command with { UsuarioId = usuarioId }, cancellationToken);
        return CreatedAtAction(nameof(ObtenerMiPortafolio), new { id });
    }

    [HttpPut("trabajos/{id:guid}")]
    public async Task<IActionResult> Modificar(Guid id, ModificarTrabajoPortafolioCommand command, CancellationToken cancellationToken)
    {
        if (!TryObtenerUsuarioId(out var usuarioId)) return Unauthorized();
        await mediator.Send(command with { Id = id, UsuarioId = usuarioId }, cancellationToken);
        return NoContent();
    }

    [HttpDelete("trabajos/{id:guid}")]
    public async Task<IActionResult> Eliminar(Guid id, CancellationToken cancellationToken)
    {
        if (!TryObtenerUsuarioId(out var usuarioId)) return Unauthorized();
        await mediator.Send(new EliminarTrabajoPortafolioCommand(id, usuarioId), cancellationToken);
        return NoContent();
    }

    private bool TryObtenerUsuarioId(out Guid usuarioId) =>
        Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value, out usuarioId)
        && usuarioId != Guid.Empty;
}
