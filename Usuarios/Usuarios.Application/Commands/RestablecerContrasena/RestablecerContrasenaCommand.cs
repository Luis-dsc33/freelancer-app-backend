using MediatR;

namespace Usuarios.Application.Commands.RestablecerContrasena;

public record RestablecerContrasenaCommand(
    string Token,
    string Password,
    string ConfirmarPassword) : IRequest<Unit>;
