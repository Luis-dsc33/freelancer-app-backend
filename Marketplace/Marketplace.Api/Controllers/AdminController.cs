// ADMIN Marcos: endpoints que consume el servicio Admin (no el front directamente).
// Gateway: /api/marketplace/admin/...  →  Marketplace.Api: /api/admin/...
using Marketplace.Application.Commands.CambiarVisibilidadOferta;
using Marketplace.Application.Queries.ListarOfertasAdmin;
using Marketplace.Application.Queries.ObtenerEstadisticasMarketplace;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Api.Controllers;

[ApiController]
[Route("api/admin")]
[Authorize(Roles = "Administrador")]
public class AdminController(IMediator mediator) : ControllerBase
{
    [HttpGet("ofertas")]
    public async Task<IActionResult> Listar([FromQuery] string? estado, CancellationToken ct)
        => Ok(await mediator.Send(new ListarOfertasAdminQuery(estado), ct));

    // HU-21
    [HttpPost("ofertas/{id:guid}/ocultar")]
    public async Task<IActionResult> Ocultar(Guid id, CancellationToken ct)
        => Ok(await mediator.Send(new CambiarVisibilidadOfertaCommand(id, Ocultar: true), ct));

    // HU-26
    [HttpPost("ofertas/{id:guid}/restaurar")]
    public async Task<IActionResult> Restaurar(Guid id, CancellationToken ct)
        => Ok(await mediator.Send(new CambiarVisibilidadOfertaCommand(id, Ocultar: false), ct));

    // HU-28
    [HttpGet("estadisticas")]
    public async Task<IActionResult> Estadisticas(CancellationToken ct)
        => Ok(await mediator.Send(new ObtenerEstadisticasMarketplaceQuery(), ct));
}
