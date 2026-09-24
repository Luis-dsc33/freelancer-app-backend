using MediatR;

namespace Usuarios.Application.Commands.IniciarSesion;

public record IniciarSesionCommand(string Email, string Password) : IRequest<IniciarSesionResultado>;

public record IniciarSesionResultado(string Token, DateTime ExpiraEn, string Nombre, string Rol);