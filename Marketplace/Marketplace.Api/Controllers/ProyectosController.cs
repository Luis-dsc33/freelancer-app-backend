using Marketplace.Application.Queries.ListarOportunidades;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Estudiante")]   // 401 sin token / token inválido, 403 con otro rol
public class ProyectosController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProyectosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<OportunidadProyectoDto>>> Listar(CancellationToken ct)
    {
        var resultado = await _mediator.Send(new ListarOportunidadesQuery(), ct);
        return Ok(resultado);
    }
}

