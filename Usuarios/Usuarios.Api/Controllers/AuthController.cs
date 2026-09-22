using MediatR;
using Microsoft.AspNetCore.Mvc;
using Usuarios.Application.Commands.RegistrarUsuario;
using Usuarios.Application.Commands.IniciarSesion;

namespace Usuarios.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    public AuthController(IMediator mediator) => _mediator = mediator;

    [HttpPost("registro")]
    public async Task<IActionResult> Registrar(RegistrarUsuarioCommand command)
    {
        var id = await _mediator.Send(command);
        return Ok(new { id });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(IniciarSesionCommand command)
    {
        var resultado = await _mediator.Send(command);
        return Ok(resultado);
    }
}