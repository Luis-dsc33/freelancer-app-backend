using Marketplace.Application.Queries.ListarOportunidades;
using MediatR;

namespace Marketplace.Application.Queries.ObtenerDetalleOportunidad;

public sealed record ObtenerDetalleOportunidadQuery(Guid ProyectoId) : IRequest<OportunidadProyectoDto>;
