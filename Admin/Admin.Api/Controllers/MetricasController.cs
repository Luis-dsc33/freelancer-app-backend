using Admin.Application.Queries.ObtenerMetricas;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Admin.Api.Controllers;

// Gateway: GET /api/admin/Metricas  →  Admin.Api: GET /api/Metricas
[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Administrador")]
public class MetricasController : ControllerBase
{
    private readonly IMediator _mediator;
    public MetricasController(IMediator mediator) => _mediator = mediator;

    // HU-28 Panel de métricas
    [HttpGet]
    public async Task<IActionResult> Obtener(CancellationToken ct)
    {
        var metricas = await _mediator.Send(new ObtenerMetricasQuery(), ct);
        return Ok(metricas);
    }
}
