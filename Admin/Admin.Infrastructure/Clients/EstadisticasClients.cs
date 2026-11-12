using System.Net.Http.Json;
using Admin.Application.Dtos;
using Admin.Application.Interfaces;

namespace Admin.Infrastructure.Clients;

// Implementación REAL: pide las estadísticas por HTTP al servicio dueño del dato.
// Nunca se consulta el schema de otro servicio (regla del equipo).
// Ruta interna esperada en cada servicio: GET api/admin/estadisticas
// (a través del gateway queda como /api/<servicio>/admin/estadisticas).
public abstract class EstadisticasHttpClient<T> where T : class
{
    public const string RutaEstadisticas = "api/admin/estadisticas";

    private readonly HttpClient _http;

    protected EstadisticasHttpClient(HttpClient http) => _http = http;

    public async Task<T> ObtenerEstadisticasAsync(CancellationToken ct) =>
        await _http.GetFromJsonAsync<T>(RutaEstadisticas, ct)
        ?? throw new InvalidOperationException("El servicio devolvió estadísticas vacías.");
}

public class UsuariosClient(HttpClient http)
    : EstadisticasHttpClient<EstadisticasUsuariosDto>(http), IUsuariosClient;

public class MarketplaceClient(HttpClient http)
    : EstadisticasHttpClient<EstadisticasMarketplaceDto>(http), IMarketplaceClient;

public class ResenasClient(HttpClient http)
    : EstadisticasHttpClient<EstadisticasResenasDto>(http), IResenasClient;

// Pendiente acuerdo A-04: si los casos viven en Admin, esto se reemplaza por un repositorio propio.
public class CasosClient(HttpClient http)
    : EstadisticasHttpClient<EstadisticasCasosDto>(http), ICasosClient;
