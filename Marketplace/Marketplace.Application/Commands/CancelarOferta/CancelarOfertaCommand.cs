using MediatR;

namespace Marketplace.Application.Commands.CancelarOferta;

public sealed record CancelarOfertaCommand(Guid Id) : IRequest;
