using Marketplace.Api.Requests.Proyectos;
using Marketplace.Application.Commands.PublicarOferta;
using Marketplace.Application.Commands.CrearOferta;
using Marketplace.Application.Commands.PublicarOfertaExistente;
using Marketplace.Application.Commands.CancelarOferta;
using Marketplace.Application.Commands.ActualizarOferta;
using Marketplace.Application.Commands.CerrarOferta;
using Marketplace.Application.Commands.EliminarOferta;
using Marketplace.Application.Queries.ListarMisOfertas;
using Marketplace.Application.Queries.ListarOportunidades;
using Marketplace.Application.Queries.ObtenerMiOferta;
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

    [HttpGet("mis-ofertas/{id:guid}")]
    [Authorize(Roles = "Cliente")]
    public async Task<ActionResult<OportunidadProyectoDto>> ObtenerMiOferta(Guid id, CancellationToken ct) =>
        Ok(await _mediator.Send(new ObtenerMiOfertaQuery(id), ct));

    [HttpPost]
    [Authorize(Roles = "Cliente")]
    public async Task<ActionResult<OportunidadProyectoDto>> Publicar(
        [FromBody] CrearOfertaRequest request,
        CancellationToken ct)
    {
        var resultado = await _mediator.Send(
            new CrearOfertaCommand(
                request.Titulo,
                request.Descripcion,
                request.Disciplina,
                request.Presupuesto,
                request.PresupuestoMinimo,
                request.PresupuestoMaximo,
                request.TipoPresupuesto,
                request.Moneda,
                request.FechaLimite,
                request.FechaEntregaEstimada,
                request.Modalidad,
                request.Ubicacion,
                request.Alcance,
                request.HabilidadesRequeridas ?? [],
                request.HabilidadesDeseables ?? [],
                request.Entregables ?? [],
                request.NivelAvance,
                request.NivelAcademicoSugerido,
                request.RequierePortafolio,
                request.DedicacionSemanalEstimada,
                request.FlexibilidadHorario,
                request.EsquemaPago,
                request.HitosPago ?? [],
                request.GastosAdicionales,
                request.CantidadVacantes,
                request.PermitePortafolioPublico,
                request.RequiereConfidencialidad,
                request.PropiedadIntelectual),
            ct);

        return StatusCode(StatusCodes.Status201Created, resultado);
    }

    [HttpPut("{id:guid}/publicar")]
    [Authorize(Roles = "Cliente")]
    public async Task<ActionResult<OportunidadProyectoDto>> PublicarExistente(Guid id, CancellationToken ct) =>
        Ok(await _mediator.Send(new PublicarOfertaExistenteCommand(id), ct));

    [HttpPut("{id:guid}/cancelar")]
    [Authorize(Roles = "Cliente")]
    public async Task<IActionResult> Cancelar(Guid id, CancellationToken ct)
    {
        await _mediator.Send(new CancelarOfertaCommand(id), ct);
        return NoContent();
    }

    [HttpGet("mis-ofertas")]
    [Authorize(Roles = "Cliente")]
    public async Task<ActionResult<IReadOnlyList<OportunidadProyectoDto>>> MisOfertas([FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default)
    {
        return Ok(await _mediator.Send(new ListarMisOfertasQuery(page, pageSize), ct));
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Cliente")]
    public async Task<ActionResult<OportunidadProyectoDto>> Actualizar(
        Guid id,
        [FromBody] CrearOfertaRequest request,
        CancellationToken ct)
    {
        var resultado = await _mediator.Send(new ActualizarOfertaCommand(
            id, request.Titulo, request.Descripcion, request.Disciplina,
            request.Presupuesto, request.PresupuestoMinimo, request.PresupuestoMaximo,
            request.TipoPresupuesto, request.Moneda, request.FechaLimite,
            request.FechaEntregaEstimada, request.Modalidad, request.Ubicacion, request.Alcance,
            request.HabilidadesRequeridas ?? [], request.HabilidadesDeseables ?? [], request.Entregables ?? [],
            request.NivelAvance, request.NivelAcademicoSugerido, request.RequierePortafolio,
            request.DedicacionSemanalEstimada, request.FlexibilidadHorario, request.EsquemaPago,
            request.HitosPago ?? [], request.GastosAdicionales, request.CantidadVacantes,
            request.PermitePortafolioPublico, request.RequiereConfidencialidad, request.PropiedadIntelectual), ct);
        return Ok(resultado);
    }

    [HttpPut("{id:guid}/cerrar")]
    [Authorize(Roles = "Cliente")]
    public async Task<IActionResult> Cerrar(Guid id, CancellationToken ct)
    {
        await _mediator.Send(new CerrarOfertaCommand(id), ct);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Cliente")]
    public async Task<IActionResult> Eliminar(Guid id, CancellationToken ct)
    {
        await _mediator.Send(new EliminarOfertaCommand(id), ct);
        return NoContent();
    }

    [HttpGet]
    [Authorize(Roles = "Estudiante")]
    public async Task<ActionResult<IReadOnlyList<OportunidadProyectoDto>>> Listar([FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default)
    {
        var resultado = await _mediator.Send(new ListarOportunidadesQuery(page, pageSize), ct);
        return Ok(resultado);
    }
}
