using MediatR;

namespace Marketplace.Application.Queries.Contratos;

public sealed record ObtenerContratoPorIdQuery(Guid ContratoId) : IRequest<ContratoDto>;
