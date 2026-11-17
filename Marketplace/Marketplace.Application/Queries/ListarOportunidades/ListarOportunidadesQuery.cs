using MediatR;

namespace Marketplace.Application.Queries.ListarOportunidades;

public sealed record ListarOportunidadesQuery(int PageNumber = 1, int PageSize = 20) : IRequest<IReadOnlyList<OportunidadProyectoDto>>;
