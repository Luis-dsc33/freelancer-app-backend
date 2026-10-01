using MediatR;

namespace Usuarios.Application.Commands.EliminarTrabajoPortafolio;

public record EliminarTrabajoPortafolioCommand(Guid Id, Guid UsuarioId) : IRequest;
