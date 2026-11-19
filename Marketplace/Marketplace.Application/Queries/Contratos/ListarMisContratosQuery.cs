using MediatR;

namespace Marketplace.Application.Queries.Contratos;

public sealed record ListarMisContratosQuery : IRequest<IReadOnlyList<ContratoDto>>;
