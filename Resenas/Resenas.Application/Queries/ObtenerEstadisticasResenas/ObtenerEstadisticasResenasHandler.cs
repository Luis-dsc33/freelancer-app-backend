using MediatR;
using Resenas.Application.Interfaces;

namespace Resenas.Application.Queries.ObtenerEstadisticasResenas;

public sealed class ObtenerEstadisticasResenasHandler(IResenaRepository resenas)
    : IRequestHandler<ObtenerEstadisticasResenasQuery, EstadisticasResenasDto>
{
    public async Task<EstadisticasResenasDto> Handle(
        ObtenerEstadisticasResenasQuery request,
        CancellationToken cancellationToken)
    {
        var conteos = await resenas.ContarPorPuntuacionAsync(cancellationToken);

        var total = conteos.Values.Sum();
        double? promedio = total == 0
            ? null
            : Math.Round(conteos.Sum(kv => kv.Key * (double)kv.Value) / total, 2);

        var distribucion = Enumerable.Range(1, 5)
            .Select(estrella => new ConteoDto(estrella.ToString(), conteos.GetValueOrDefault(estrella)))
            .ToList();

        return new EstadisticasResenasDto(total, promedio, distribucion);
    }
}
