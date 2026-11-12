using Admin.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Admin.Application.Queries.ObtenerMetricas;

public class ObtenerMetricasHandler : IRequestHandler<ObtenerMetricasQuery, MetricasPanelDto>
{
    private readonly IUsuariosClient _usuarios;
    private readonly IMarketplaceClient _marketplace;
    private readonly IResenasClient _resenas;
    private readonly ICasosClient _casos;
    private readonly IMetricasSettings _settings;
    private readonly ILogger<ObtenerMetricasHandler> _logger;

    public ObtenerMetricasHandler(
        IUsuariosClient usuarios,
        IMarketplaceClient marketplace,
        IResenasClient resenas,
        ICasosClient casos,
        IMetricasSettings settings,
        ILogger<ObtenerMetricasHandler> logger)
    {
        _usuarios = usuarios;
        _marketplace = marketplace;
        _resenas = resenas;
        _casos = casos;
        _settings = settings;
        _logger = logger;
    }

    public async Task<MetricasPanelDto> Handle(ObtenerMetricasQuery request, CancellationToken ct)
    {
        // Los 4 servicios se consultan en paralelo; si uno falla, solo su sección queda en null
        // y las demás se muestran normalmente (HU-28-02).
        var usuarios = IntentarAsync("Usuarios", () => _usuarios.ObtenerEstadisticasAsync(ct), ct);
        var marketplace = IntentarAsync("Marketplace", () => _marketplace.ObtenerEstadisticasAsync(ct), ct);
        var resenas = IntentarAsync("Reseñas", () => _resenas.ObtenerEstadisticasAsync(ct), ct);
        var casos = IntentarAsync("Casos", () => _casos.ObtenerEstadisticasAsync(ct), ct);

        await Task.WhenAll(usuarios, marketplace, resenas, casos);

        return new MetricasPanelDto(
            FechaCorte: DateTime.UtcNow,
            EsDemostracion: _settings.UsarDatosDemostracion,
            Usuarios: usuarios.Result,
            Marketplace: marketplace.Result,
            Resenas: resenas.Result,
            Casos: casos.Result);
    }

    private async Task<T?> IntentarAsync<T>(string seccion, Func<Task<T>> consulta, CancellationToken ct) where T : class
    {
        try
        {
            return await consulta();
        }
        // Un timeout del HttpClient también es una cancelación: solo se propaga si quien canceló fue el cliente
        catch (Exception ex) when (!ct.IsCancellationRequested)
        {
            _logger.LogWarning(ex, "La sección {Seccion} del panel no está disponible", seccion);
            return null;
        }
    }
}
