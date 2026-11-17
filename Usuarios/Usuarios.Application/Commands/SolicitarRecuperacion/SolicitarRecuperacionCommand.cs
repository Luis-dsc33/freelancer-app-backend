using MediatR;

namespace Usuarios.Application.Commands.SolicitarRecuperacion;

public record SolicitarRecuperacionCommand(string Email) : IRequest<string>;
