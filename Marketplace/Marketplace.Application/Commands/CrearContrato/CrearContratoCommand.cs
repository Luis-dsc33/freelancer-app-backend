using MediatR;
using Marketplace.Application.Queries.Contratos;

namespace Marketplace.Application.Commands.CrearContrato;

public sealed record CrearContratoCommand(
    Guid PostulacionId,
    decimal ImporteAcordado,
    DateOnly FechaInicio,
    DateOnly FechaCompromiso) : IRequest<ContratoDto>;
