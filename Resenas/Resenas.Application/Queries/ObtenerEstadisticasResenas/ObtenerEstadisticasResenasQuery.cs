using MediatR;

namespace Resenas.Application.Queries.ObtenerEstadisticasResenas;

public sealed record ObtenerEstadisticasResenasQuery : IRequest<EstadisticasResenasDto>;

public sealed record ConteoDto(string Etiqueta, int Total);

public sealed record EstadisticasResenasDto(
    int TotalCalificaciones,
    double? PromedioGeneral,
    IReadOnlyList<ConteoDto> DistribucionEstrellas);
