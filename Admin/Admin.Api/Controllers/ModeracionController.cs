// ADMIN Marcos: HU-21 ocultar y HU-26 restaurar contenido; listado de medidas vigentes.
// Gateway: /api/admin/Moderacion  →  Admin.Api: /api/Moderacion
using Admin.Application.Commands.AplicarMedidaModeracion;
using Admin.Application.Queries.ObtenerModeracion;
using Admin.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Admin.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Administrador")]
public class ModeracionController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Obtener(CancellationToken ct)
        => Ok(await mediator.Send(new ObtenerModeracionQuery(), ct));

    [HttpPost("ofertas/{id:guid}/ocultar")]
    public async Task<IActionResult> Ocultar(Guid id, MotivoRequest request, CancellationToken ct)
        => Ok(await mediator.Send(new AplicarMedidaModeracionCommand(AccionAdministrativa.Ocultar, id, request.Motivo ?? ""), ct));

    [HttpPost("ofertas/{id:guid}/restaurar")]
    public async Task<IActionResult> Restaurar(Guid id, MotivoRequest request, CancellationToken ct)
        => Ok(await mediator.Send(new AplicarMedidaModeracionCommand(AccionAdministrativa.Restaurar, id, request.Motivo ?? ""), ct));
}
