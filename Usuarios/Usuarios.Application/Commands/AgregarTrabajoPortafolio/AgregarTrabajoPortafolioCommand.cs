using MediatR;

namespace Usuarios.Application.Commands.AgregarTrabajoPortafolio;

public record AgregarTrabajoPortafolioCommand : IRequest<Guid>
{
    public string Titulo { get; init; } = string.Empty;
    public string Descripcion { get; init; } = string.Empty;
    public string? Enlace { get; init; }
}
