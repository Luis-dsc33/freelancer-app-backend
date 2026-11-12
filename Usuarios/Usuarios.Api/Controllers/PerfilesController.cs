using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Usuarios.Application.Commands.CrearOActualizarPerfil;
using Usuarios.Application.Queries.ObtenerPerfil;

namespace Usuarios.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Estudiante")]
public class PerfilesController(IMediator mediator) : ControllerBase
{
    [HttpGet("mi-perfil")]
    public async Task<IActionResult> ObtenerMiPerfil(CancellationToken cancellationToken)
        => Ok(await mediator.Send(new ObtenerPerfilQuery(), cancellationToken));

    [HttpPost("mi-perfil")]
    [HttpPut("mi-perfil")]
    public async Task<IActionResult> GuardarPerfil(
        CrearOActualizarPerfilCommand command,
        CancellationToken cancellationToken)
    {
        var resultId = await mediator.Send(command, cancellationToken);
        return Ok(new { PerfilId = resultId });
    }
}
