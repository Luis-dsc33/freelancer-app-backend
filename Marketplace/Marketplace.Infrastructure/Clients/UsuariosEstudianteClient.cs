using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Marketplace.Application.Exceptions;
using Marketplace.Application.Interfaces;

namespace Marketplace.Infrastructure.Clients;

public sealed class UsuariosEstudianteClient(HttpClient http) : IUsuariosEstudianteClient
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public async Task<EstadoElegibilidadEstudiante?> ObtenerEstadoEstudianteAsync(CancellationToken cancellationToken)
    {
        if (http.BaseAddress is null)
            return null;

        HttpResponseMessage perfilResp;
        try
        {
            perfilResp = await http.GetAsync("api/Perfiles/mi-perfil", cancellationToken);
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException && !cancellationToken.IsCancellationRequested)
        {
            throw new ServicioNoDisponibleException("Usuarios", ex);
        }

        if (perfilResp.StatusCode is HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden)
            throw new UnauthorizedAccessException("Tu cuenta no tiene permisos de estudiante para postularte.");

        if (perfilResp.StatusCode == HttpStatusCode.NotFound)
            return new EstadoElegibilidadEstudiante(false, false);

        if ((int)perfilResp.StatusCode >= 500)
            throw new ServicioNoDisponibleException("Usuarios");

        if (!perfilResp.IsSuccessStatusCode)
            throw new InvalidOperationException("No se pudo validar tu perfil de estudiante en este momento.");

        var perfil = await perfilResp.Content.ReadFromJsonAsync<PerfilResumenResponse>(JsonOptions, cancellationToken);
        var perfilCompleto = perfil?.EsPerfilCompleto == true;

        var tienePortafolio = false;
        try
        {
            var portafolioResp = await http.GetAsync("api/Portafolios/mi-portafolio", cancellationToken);
            if ((int)portafolioResp.StatusCode >= 500)
                throw new ServicioNoDisponibleException("Usuarios");
            if (portafolioResp.IsSuccessStatusCode)
            {
                var trabajos = await portafolioResp.Content.ReadFromJsonAsync<List<JsonElement>>(JsonOptions, cancellationToken);
                tienePortafolio = trabajos is { Count: > 0 };
            }
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException && !cancellationToken.IsCancellationRequested)
        {
            throw new ServicioNoDisponibleException("Usuarios", ex);
        }

        return new EstadoElegibilidadEstudiante(perfilCompleto, tienePortafolio);
    }

    private sealed record PerfilResumenResponse(bool EsPerfilCompleto);
}
