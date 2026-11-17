using MediatR;

namespace Marketplace.Application.Queries.Contratos;

public sealed record ListarContratosClienteQuery : IRequest<IReadOnlyList<ContratoDto>>;
