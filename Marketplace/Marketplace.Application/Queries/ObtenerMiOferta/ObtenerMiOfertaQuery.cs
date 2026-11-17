using Marketplace.Application.Queries.ListarOportunidades;
using MediatR;

namespace Marketplace.Application.Queries.ObtenerMiOferta;

public sealed record ObtenerMiOfertaQuery(Guid Id) : IRequest<OportunidadProyectoDto>;
