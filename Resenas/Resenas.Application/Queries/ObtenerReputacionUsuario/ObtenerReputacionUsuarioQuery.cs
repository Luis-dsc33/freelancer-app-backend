using MediatR;
using Resenas.Application.Dtos;

namespace Resenas.Application.Queries.ObtenerReputacionUsuario;

public sealed record ObtenerReputacionUsuarioQuery(Guid UsuarioId) : IRequest<ReputacionUsuarioDto>;
