using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Usuarios.Application.Commands.CrearOActualizarPerfil;
using Usuarios.Application.Queries.ObtenerPerfil;

namespace Usuarios.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Estudiante")]
public class PerfilesController : ControllerBase
{
    private readonly IMediator _mediator;

    public PerfilesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("mi-perfil")]
    public async Task<IActionResult> ObtenerMiPerfil()
    {
        var usuarioIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(usuarioIdStr, out var usuarioId))
            return Unauthorized();

        var query = new ObtenerPerfilQuery(usuarioId);
        var perfil = await _mediator.Send(query);

        return Ok(perfil);
    }

    [HttpPost("mi-perfil")]
    [HttpPut("mi-perfil")]
    public async Task<IActionResult> GuardarPerfil(CrearOActualizarPerfilCommand command)
    {
        var usuarioIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(usuarioIdStr, out var usuarioId))
            return Unauthorized();

        var commandConUsuario = command with { UsuarioId = usuarioId };

        var resultId = await _mediator.Send(commandConUsuario);
        return Ok(new { PerfilId = resultId });
    }
}
