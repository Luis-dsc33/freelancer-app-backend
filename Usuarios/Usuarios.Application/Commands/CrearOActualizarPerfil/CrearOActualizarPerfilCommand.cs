using MediatR;

namespace Usuarios.Application.Commands.CrearOActualizarPerfil;

public record CrearOActualizarPerfilCommand(
    string Carrera,
    List<string> Habilidades,
    string Descripcion) : IRequest<Guid>;