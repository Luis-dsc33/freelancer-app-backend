using MediatR;
using Marketplace.Application.Queries.ListarPostulacionesOferta;

namespace Marketplace.Application.Commands.ResolverPostulacion;

public sealed record ResolverPostulacionCommand(
    Guid PostulacionId,
    string Estado) : IRequest<PostulacionDto>;
