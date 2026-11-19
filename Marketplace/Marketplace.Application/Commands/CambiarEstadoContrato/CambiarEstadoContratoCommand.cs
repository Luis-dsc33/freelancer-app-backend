using MediatR;
using Marketplace.Application.Queries.Contratos;

namespace Marketplace.Application.Commands.CambiarEstadoContrato;

public sealed record CambiarEstadoContratoCommand(
    Guid ContratoId,
    string Estado) : IRequest<ContratoDto>;
