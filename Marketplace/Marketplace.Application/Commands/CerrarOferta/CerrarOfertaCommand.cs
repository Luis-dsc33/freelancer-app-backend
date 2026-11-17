using MediatR;

namespace Marketplace.Application.Commands.CerrarOferta;

public sealed record CerrarOfertaCommand(Guid Id) : IRequest;
