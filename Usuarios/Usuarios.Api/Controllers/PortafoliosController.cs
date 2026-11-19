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
        => Ok(await mediator.Send(new ObtenerMiPortafolioQuery(), cancellationToken));

    [AllowAnonymous]
    [HttpGet("estudiantes/{usuarioId:guid}")]
    public async Task<IActionResult> ObtenerPortafolio(Guid usuarioId, CancellationToken cancellationToken) =>
        Ok(await mediator.Send(new ObtenerPortafolioQuery(usuarioId), cancellationToken));

    [HttpPost("trabajos")]
    public async Task<IActionResult> Agregar(
        AgregarTrabajoPortafolioCommand command,
        CancellationToken cancellationToken)
    {
        var id = await mediator.Send(command, cancellationToken);
        return StatusCode(StatusCodes.Status201Created, new { TrabajoId = id });
    }

    [HttpPut("trabajos/{id:guid}")]
    public async Task<IActionResult> Modificar(
        Guid id,
        ModificarTrabajoPortafolioCommand command,
        CancellationToken cancellationToken)
    {
        await mediator.Send(command with { Id = id }, cancellationToken);
        return NoContent();
    }

    [HttpDelete("trabajos/{id:guid}")]
    public async Task<IActionResult> Eliminar(Guid id, CancellationToken cancellationToken)
    {
        await mediator.Send(new EliminarTrabajoPortafolioCommand(id), cancellationToken);
        return NoContent();
    }
}
