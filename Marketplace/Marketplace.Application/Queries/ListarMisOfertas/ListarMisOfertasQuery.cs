using MediatR;
using Marketplace.Application.Queries.ListarOportunidades;

namespace Marketplace.Application.Queries.ListarMisOfertas;

public sealed record ListarMisOfertasQuery(int PageNumber = 1, int PageSize = 20) : IRequest<IReadOnlyList<OportunidadProyectoDto>>;
