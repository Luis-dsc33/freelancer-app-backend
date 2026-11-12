using Admin.Application.Dtos;
using MediatR;

namespace Admin.Application.Queries.ObtenerMetricas;

public record ObtenerMetricasQuery : IRequest<MetricasPanelDto>;

// Cada sección es null cuando su servicio no respondió (HU-28-02: "No disponible").
public record MetricasPanelDto(
    DateTime FechaCorte,
    bool EsDemostracion,
    EstadisticasUsuariosDto? Usuarios,
    EstadisticasMarketplaceDto? Marketplace,
    EstadisticasResenasDto? Resenas,
    EstadisticasCasosDto? Casos);
