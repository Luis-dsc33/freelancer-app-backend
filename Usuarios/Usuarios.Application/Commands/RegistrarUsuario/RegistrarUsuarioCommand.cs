using MediatR;

namespace Usuarios.Application.Commands.RegistrarUsuario;

public record RegistrarUsuarioCommand(string Nombre, string Email, string Password, string Rol) : IRequest<Guid>;