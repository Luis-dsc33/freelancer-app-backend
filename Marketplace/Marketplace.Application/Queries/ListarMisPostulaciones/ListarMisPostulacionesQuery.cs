using MediatR;

namespace Marketplace.Application.Queries.ListarMisPostulaciones;

public sealed record ListarMisPostulacionesQuery : IRequest<IReadOnlyList<PostulacionEstudianteDto>>;
