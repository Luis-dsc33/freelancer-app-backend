using MediatR;

namespace Usuarios.Application.Queries.ObtenerPortafolio;

public record ObtenerPortafolioQuery(Guid UsuarioId) : IRequest<List<TrabajoPortafolioDto>>;
