// ADMIN Marcos: endpoints que consume el servicio Admin (no el front directamente).
// Gateway: /api/usuarios/admin/...  →  Usuarios.Api: /api/admin/...
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Usuarios.Application.Commands.CambiarEstadoUsuario;
using Usuarios.Application.Queries.BuscarUsuariosAdmin;
using Usuarios.Application.Queries.ObtenerEstadisticasUsuarios;

namespace Usuarios.Api.Controllers;

[ApiController]
[Route("api/admin")]
[Authorize(Roles = "Administrador")]
public class AdminController(IMediator mediator) : ControllerBase
{
    // HU-25
    [HttpGet("usuarios")]
    public async Task<IActionResult> Buscar(
        [FromQuery] string? busqueda, [FromQuery] string? rol, [FromQuery] string? estado,
        [FromQuery] int pagina = 1, [FromQuery] int tamanoPagina = 10, CancellationToken ct = default)
        => Ok(await mediator.Send(new BuscarUsuariosAdminQuery(busqueda, rol, estado, pagina, tamanoPagina), ct));

    // HU-21
    [HttpPost("usuarios/{id:guid}/suspender")]
    public async Task<IActionResult> Suspender(Guid id, CancellationToken ct)
        => Ok(await mediator.Send(new CambiarEstadoUsuarioCommand(id, Suspender: true), ct));

    // HU-26
    [HttpPost("usuarios/{id:guid}/reactivar")]
    public async Task<IActionResult> Reactivar(Guid id, CancellationToken ct)
        => Ok(await mediator.Send(new CambiarEstadoUsuarioCommand(id, Suspender: false), ct));

    // HU-28
    [HttpGet("estadisticas")]
    public async Task<IActionResult> Estadisticas(CancellationToken ct)
        => Ok(await mediator.Send(new ObtenerEstadisticasUsuariosQuery(), ct));
}
