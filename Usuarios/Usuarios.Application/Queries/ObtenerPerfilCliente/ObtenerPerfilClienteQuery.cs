using MediatR;

namespace Usuarios.Application.Queries.ObtenerPerfilCliente;

public sealed record ObtenerPerfilClienteQuery : IRequest<PerfilClienteDto?>;
