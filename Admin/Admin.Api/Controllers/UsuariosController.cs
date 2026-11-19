// ADMIN Marcos: HU-25 Consultar usuarios, HU-21 suspender y HU-26 reactivar cuentas.
// Gateway: /api/admin/Usuarios  →  Admin.Api: /api/Usuarios
using Admin.Application.Commands.AplicarMedidaModeracion;
using Admin.Application.Queries.BuscarUsuarios;
using Admin.Application.Queries.ObtenerHistorialRecurso;
using Admin.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Admin.Api.Controllers;

public record MotivoRequest(string Motivo);

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Administrador")]
public class UsuariosController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Buscar(
        [FromQuery] string? busqueda, [FromQuery] string? rol, [FromQuery] string? estado,
        [FromQuery] int pagina = 1, [FromQuery] int tamanoPagina = 10, CancellationToken ct = default)
        => Ok(await mediator.Send(new BuscarUsuariosQuery(busqueda, rol, estado, pagina, tamanoPagina), ct));

    [HttpGet("{id:guid}/historial")]
    public async Task<IActionResult> Historial(Guid id, CancellationToken ct)
        => Ok(await mediator.Send(new ObtenerHistorialRecursoQuery(id), ct));

    [HttpPost("{id:guid}/suspender")]
    public async Task<IActionResult> Suspender(Guid id, MotivoRequest request, CancellationToken ct)
        => Ok(await mediator.Send(new AplicarMedidaModeracionCommand(AccionAdministrativa.Suspender, id, request.Motivo ?? ""), ct));

    [HttpPost("{id:guid}/reactivar")]
    public async Task<IActionResult> Reactivar(Guid id, MotivoRequest request, CancellationToken ct)
        => Ok(await mediator.Send(new AplicarMedidaModeracionCommand(AccionAdministrativa.Reactivar, id, request.Motivo ?? ""), ct));
}
