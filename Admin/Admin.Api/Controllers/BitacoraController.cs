// ADMIN Marcos: HU-27 Consultar la bitácora administrativa. Solo lectura: no hay endpoints para editar o borrar.
// Gateway: /api/admin/Bitacora  →  Admin.Api: /api/Bitacora
using Admin.Application.Queries.ObtenerBitacora;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Admin.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Administrador")]
public class BitacoraController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Obtener(
        [FromQuery] Guid? adminId, [FromQuery] string? tipoAccion, [FromQuery] DateOnly? desde, [FromQuery] DateOnly? hasta,
        [FromQuery] int pagina = 1, [FromQuery] int tamanoPagina = 15, CancellationToken ct = default)
        => Ok(await mediator.Send(new ObtenerBitacoraQuery(adminId, tipoAccion, desde, hasta, pagina, tamanoPagina), ct));
}
