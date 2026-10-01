using MediatR;
using System.Text.Json.Serialization;

namespace Usuarios.Application.Commands.ModificarTrabajoPortafolio;

public record ModificarTrabajoPortafolioCommand : IRequest<Guid>
{
    [JsonIgnore]
    public Guid UsuarioId { get; init; }
    [JsonIgnore]
    public Guid Id { get; init; }
    public string Titulo { get; init; } = string.Empty;
    public string Descripcion { get; init; } = string.Empty;
    public string? Enlace { get; init; }
}
