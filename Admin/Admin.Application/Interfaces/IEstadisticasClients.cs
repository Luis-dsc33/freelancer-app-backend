using Admin.Application.Dtos;

namespace Admin.Application.Interfaces;

// "Promesas" de lectura hacia otros microservicios. Application solo conoce estas interfaces;
// la implementación real (HttpClient) o la de demostración (Fake) vive en Infrastructure.

public interface IUsuariosClient
{
    Task<EstadisticasUsuariosDto> ObtenerEstadisticasAsync(CancellationToken ct);
}

public interface IMarketplaceClient
{
    Task<EstadisticasMarketplaceDto> ObtenerEstadisticasAsync(CancellationToken ct);
}

public interface IResenasClient
{
    Task<EstadisticasResenasDto> ObtenerEstadisticasAsync(CancellationToken ct);
}

public interface ICasosClient
{
    Task<EstadisticasCasosDto> ObtenerEstadisticasAsync(CancellationToken ct);
}

public interface IMetricasSettings
{
    // true = se usan datos de demostración (clientes Fake). El panel lo muestra con una etiqueta visible.
    bool UsarDatosDemostracion { get; }
}
