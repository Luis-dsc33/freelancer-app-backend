using MediatR;

namespace Marketplace.Application.Commands.RetirarPostulacion;

public sealed record RetirarPostulacionCommand(Guid PostulacionId) : IRequest;
