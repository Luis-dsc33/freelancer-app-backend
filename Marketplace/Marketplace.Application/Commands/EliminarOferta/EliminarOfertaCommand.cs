using MediatR;

namespace Marketplace.Application.Commands.EliminarOferta;

public sealed record EliminarOfertaCommand(Guid Id) : IRequest;
