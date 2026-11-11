using MediatR;

namespace Marketplace.Application.Queries.ListarOportunidades;

public sealed record ListarOportunidadesQuery : IRequest<IReadOnlyList<OportunidadProyectoDto>>;

