using System.Security.Claims;
using Marketplace.Api.Requests.Proyectos;
using Marketplace.Application.Commands.PublicarOferta;
using Marketplace.Application.Queries.ListarOportunidades;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProyectosController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProyectosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize(Roles = "Cliente")]
    public async Task<ActionResult<OportunidadProyectoDto>> Publicar(
        [FromBody] PublicarOfertaRequest request,
        CancellationToken ct)
    {
        var idClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var nombreCliente = User.FindFirstValue(ClaimTypes.Name);
        if (!Guid.TryParse(idClaim, out var clienteId) || string.IsNullOrWhiteSpace(nombreCliente))
            return Unauthorized();

        var resultado = await _mediator.Send(
            new PublicarOfertaCommand(
                request.Titulo,
                request.Descripcion,
                request.Disciplina,
                request.Presupuesto,
                request.FechaLimite,
                clienteId,
                nombreCliente),
            ct);

        return StatusCode(StatusCodes.Status201Created, resultado);
    }

    [HttpGet]
    [Authorize(Roles = "Estudiante")]
    public async Task<ActionResult<IReadOnlyList<OportunidadProyectoDto>>> Listar(CancellationToken ct)
    {
        var resultado = await _mediator.Send(new ListarOportunidadesQuery(), ct);
        return Ok(resultado);
    }
}
