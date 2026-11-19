using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resenas.Application.Queries.ObtenerEstadisticasResenas;

namespace Resenas.Api.Controllers;

[ApiController]
[Route("api/admin")]
[Authorize(Roles = "Administrador")]
public sealed class AdminController(IMediator mediator) : ControllerBase
{
    [HttpGet("estadisticas")]
    public async Task<IActionResult> Estadisticas(CancellationToken cancellationToken)
        => Ok(await mediator.Send(new ObtenerEstadisticasResenasQuery(), cancellationToken));
}
