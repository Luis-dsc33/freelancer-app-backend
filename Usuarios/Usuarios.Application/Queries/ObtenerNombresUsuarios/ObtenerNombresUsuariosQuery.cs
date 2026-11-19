using MediatR;

namespace Usuarios.Application.Queries.ObtenerNombresUsuarios;

public sealed record ObtenerNombresUsuariosQuery(IReadOnlyCollection<Guid> UsuarioIds)
    : IRequest<IReadOnlyDictionary<Guid, string>>;
