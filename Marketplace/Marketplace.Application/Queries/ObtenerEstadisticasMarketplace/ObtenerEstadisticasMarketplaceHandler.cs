// ADMIN Marcos: HU-28
using Marketplace.Application.Interfaces;
using Marketplace.Domain.Entities;
using MediatR;

namespace Marketplace.Application.Queries.ObtenerEstadisticasMarketplace;

public class ObtenerEstadisticasMarketplaceHandler(IProyectoAdminRepository repositorio)
    : IRequestHandler<ObtenerEstadisticasMarketplaceQuery, EstadisticasMarketplaceDto>
{
    private const int Semanas = 8;
    // Las ofertas visibles para estudiantes usan tanto Abierto (flujo inicial) como Publicada (flujo Cliente).
    private static readonly string[] EstadosDisponibles = [Proyecto.EstadoAbierto, Proyecto.EstadoPublicada];

    public async Task<EstadisticasMarketplaceDto> Handle(ObtenerEstadisticasMarketplaceQuery request, CancellationToken ct)
    {
        var disponibles = await repositorio.ContarPorEstadosAsync(EstadosDisponibles, ct);
        var porDisciplina = await repositorio.ContarPorDisciplinaAsync(EstadosDisponibles, ct);

        var hoy = DateOnly.FromDateTime(DateTime.UtcNow);
        var lunesActual = hoy.AddDays(-(((int)hoy.DayOfWeek + 6) % 7));
        var inicio = lunesActual.AddDays(-7 * (Semanas - 1));
        var fechas = await repositorio.FechasDeCreacionDesdeAsync(inicio.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc), ct);

        var porSemana = Enumerable.Range(0, Semanas)
            .Select(i => inicio.AddDays(7 * i))
            .Select(semana => new ConteoSemanalDto(semana, fechas.Count(f =>
            {
                var dia = DateOnly.FromDateTime(f);
                return dia >= semana && dia < semana.AddDays(7);
            })))
            .ToList();

        var postulaciones = await repositorio.ContarPostulacionesPorEstadoAsync(ct) ?? [];
        var pendientes = postulaciones.GetValueOrDefault(Postulacion.EstadoPendiente);
        var aceptadas = postulaciones.GetValueOrDefault(Postulacion.EstadoAceptada);
        var rechazadas = postulaciones.GetValueOrDefault(Postulacion.EstadoRechazada);
        var retiradas = postulaciones.GetValueOrDefault(Postulacion.EstadoRetirada);

        return new EstadisticasMarketplaceDto(
            disponibles,
            porDisciplina.Select(d => new ConteoDto(d.Key, d.Value)).OrderByDescending(d => d.Total).ToList(),
            porSemana,
            new EmbudoPostulacionesDto(
                Enviadas: pendientes + aceptadas + rechazadas + retiradas,
                EnRevision: pendientes,
                Aceptadas: aceptadas,
                NoSeleccionadas: rechazadas + retiradas));
    }
}
