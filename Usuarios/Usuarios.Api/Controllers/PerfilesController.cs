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
        if (!TryObtenerUsuarioId(out var usuarioId))
            return Unauthorized();

        var perfil = await _mediator.Send(new ObtenerPerfilQuery(usuarioId));
        return Ok(perfil);
    }

    [HttpPost("mi-perfil")]
    [HttpPut("mi-perfil")]
    public async Task<IActionResult> GuardarPerfil(CrearOActualizarPerfilCommand command)
    {
        if (!TryObtenerUsuarioId(out var usuarioId))
            return Unauthorized();

        var resultId = await _mediator.Send(command with { UsuarioId = usuarioId });
        return Ok(new { PerfilId = resultId });
    }
=======
[HttpPut("mi-perfil")]
public async Task<IActionResult> GuardarPerfil(GuardarPerfilRequest request)
{
    if (!TryObtenerUsuarioId(out var usuarioId))
        return Unauthorized();

    var command = new CrearOActualizarPerfilCommand(
        usuarioId, request.Carrera, request.Habilidades, request.Descripcion);

    var resultId = await _mediator.Send(command);
    return Ok(new { PerfilId = resultId });
}

    private bool TryObtenerUsuarioId(out Guid usuarioId)
    {
        var valor = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                    ?? User.FindFirst("sub")?.Value;
        return Guid.TryParse(valor, out usuarioId);
    }
}
}
public record GuardarPerfilRequest(string Carrera, List<string> Habilidades, string Descripcion);

