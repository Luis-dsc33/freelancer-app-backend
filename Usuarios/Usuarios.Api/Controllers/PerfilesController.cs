using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Usuarios.Application.Commands.GuardarPerfilEstudiante;
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
        [FromBody] GuardarPerfilEstudianteRequest request,
        CancellationToken cancellationToken)
    {
        var resultado = await mediator.Send(
            new GuardarPerfilEstudianteCommand(
                request.Carrera,
                request.Descripcion,
                request.Habilidades ?? [],
                request.SkillsCatalogo),
            cancellationToken);
        return Ok(resultado);
    }
}

public sealed record GuardarPerfilEstudianteRequest(
    string Carrera,
    string Descripcion,
    List<string>? Habilidades,
    List<EstudianteSkillInputDto>? SkillsCatalogo);
