using MediatR;

namespace Marketplace.Application.Queries.ListarPostulacionesOferta;

public sealed record ListarPostulacionesOfertaQuery(
    Guid ProyectoId) : IRequest<IReadOnlyList<PostulacionDto>>;
