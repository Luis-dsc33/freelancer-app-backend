using Marketplace.Application.Commands.CambiarEstadoContrato;
using Marketplace.Application.Commands.CrearContrato;
using Marketplace.Application.Queries.Contratos;
using Marketplace.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Api.Controllers;

[ApiController]
[Route("api/contratos")]
[Authorize(Roles = "Cliente,Estudiante")]
public sealed class ContratosController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "Cliente")]
    public async Task<IActionResult> Crear(CrearContratoRequest request, CancellationToken cancellationToken)
    {
        var resultado = await mediator.Send(new CrearContratoCommand(
            request.PostulacionId, request.ImporteAcordado,
            request.FechaInicio, request.FechaCompromiso), cancellationToken);
        return CreatedAtAction(nameof(ObtenerPorId), new { id = resultado.Id }, resultado);
    }

    [HttpGet("mis-contratos")]
    public async Task<IActionResult> MisContratos(CancellationToken cancellationToken)
    {
        return Ok(await mediator.Send(new ListarMisContratosQuery(), cancellationToken));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> ObtenerPorId(Guid id, CancellationToken cancellationToken)
    {
        return Ok(await mediator.Send(new ObtenerContratoPorIdQuery(id), cancellationToken));
    }

    [HttpPut("{id:guid}/finalizar")]
    public Task<IActionResult> Finalizar(Guid id, CancellationToken cancellationToken) =>
        CambiarEstado(id, Contrato.EstadoFinalizado, cancellationToken);

    [HttpPut("{id:guid}/cancelar")]
    public Task<IActionResult> Cancelar(Guid id, CancellationToken cancellationToken) =>
        CambiarEstado(id, Contrato.EstadoCancelado, cancellationToken);

    private async Task<IActionResult> CambiarEstado(Guid id, string estado, CancellationToken cancellationToken)
    {
        return Ok(await mediator.Send(new CambiarEstadoContratoCommand(id, estado), cancellationToken));
    }
}

public sealed record CrearContratoRequest(
    Guid PostulacionId,
    decimal ImporteAcordado,
    DateOnly FechaInicio,
    DateOnly FechaCompromiso);
