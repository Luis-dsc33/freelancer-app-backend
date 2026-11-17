using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Usuarios.Application.Queries.BuscarEstudiantes;

namespace Usuarios.Api.Controllers;

[ApiController]
[Route("api/estudiantes")]
[Authorize(Roles = "Cliente")]
public sealed class EstudiantesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Buscar(
        [FromQuery] string? habilidad,
        [FromQuery] string? carrera,
        [FromQuery] int pagina = 1,
        [FromQuery] int tamanoPagina = 20,
        CancellationToken cancellationToken = default)
    {
        var resultado = await mediator.Send(
            new BuscarEstudiantesQuery(habilidad, carrera, pagina, tamanoPagina),
            cancellationToken);
        return Ok(resultado);
    }
}


// ERJ: LUIS ESTE ES PARA CUANDO EL CLIENTE HACE BUSQUEDA DE ESTUDIANTES