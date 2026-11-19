using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Usuarios.Application.Queries.ObtenerNombresUsuarios;

namespace Usuarios.Api.Controllers;

[ApiController]
[Route("api/usuarios")]
[Authorize]
public sealed class UsuariosController(IMediator mediator) : ControllerBase
{
    [HttpPost("nombres")]
    public async Task<IActionResult> ObtenerNombres(
        [FromBody] ObtenerNombresRequest request,
        CancellationToken cancellationToken)
        => Ok(await mediator.Send(new ObtenerNombresUsuariosQuery(request.UsuarioIds ?? []), cancellationToken));
}

public sealed record ObtenerNombresRequest(IReadOnlyCollection<Guid>? UsuarioIds);
