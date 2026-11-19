using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Resenas.Application.Exceptions;
using Resenas.Application.Interfaces;

namespace Resenas.Infrastructure.Clients;

public sealed class MarketplaceContratosClient(HttpClient http) : IMarketplaceContratosClient
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public async Task<ContratoResumenDto?> ObtenerContratoAsync(Guid contratoId, CancellationToken cancellationToken)
    {
        if (http.BaseAddress is null)
            throw new ServicioNoDisponibleException("Marketplace");

        HttpResponseMessage respuesta;
        try
        {
            respuesta = await http.GetAsync($"api/contratos/{contratoId}", cancellationToken);
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException && !cancellationToken.IsCancellationRequested)
        {
            throw new ServicioNoDisponibleException("Marketplace", ex);
        }

        if (respuesta.StatusCode == HttpStatusCode.NotFound)
            return null;

        if (respuesta.StatusCode is HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden)
            throw new UnauthorizedAccessException("No tienes permiso para acceder a este contrato.");

        if ((int)respuesta.StatusCode >= 500)
            throw new ServicioNoDisponibleException("Marketplace");

        if (!respuesta.IsSuccessStatusCode)
            throw new InvalidOperationException("No se pudo validar el contrato en Marketplace.");

        return await respuesta.Content.ReadFromJsonAsync<ContratoResumenDto>(JsonOptions, cancellationToken);
    }
}
