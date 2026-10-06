using MediatR;
using System.Text.Json.Serialization;

namespace Usuarios.Application.Commands.AgregarTrabajoPortafolio;

public record AgregarTrabajoPortafolioCommand : IRequest<Guid>
{
    [JsonIgnore]
    public Guid UsuarioId { get; init; }
    public string Titulo { get; init; } = string.Empty;
    public string Descripcion { get; init; } = string.Empty;
    public string? Enlace { get; init; }
}
