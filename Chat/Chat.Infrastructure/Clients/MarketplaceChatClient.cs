using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Chat.Application.Exceptions;
using Chat.Application.Interfaces;

namespace Chat.Infrastructure.Clients;

public sealed class MarketplaceChatClient(HttpClient http) : IMarketplaceChatClient
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public async Task<ParticipacionChatDto> ValidarParticipacionAsync(
        Guid proyectoId,
        Guid? postulacionId,
        Guid? contratoId,
        CancellationToken cancellationToken)
    {
        if (http.BaseAddress is null)
            throw new ServicioNoDisponibleException("Marketplace");

        var query = new List<string>();
        if (postulacionId.HasValue)
            query.Add($"postulacionId={postulacionId.Value}");
        if (contratoId.HasValue)
            query.Add($"contratoId={contratoId.Value}");

        var ruta = $"api/Proyectos/{proyectoId}/participacion-chat"
            + (query.Count > 0 ? "?" + string.Join("&", query) : string.Empty);

        HttpResponseMessage respuesta;
        try
        {
            respuesta = await http.GetAsync(ruta, cancellationToken);
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException && !cancellationToken.IsCancellationRequested)
        {
            throw new ServicioNoDisponibleException("Marketplace", ex);
        }

        if (respuesta.StatusCode == HttpStatusCode.NotFound)
            throw new KeyNotFoundException("La oferta del proyecto no existe.");

        if (respuesta.StatusCode is HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden)
            throw new UnauthorizedAccessException("No tienes permiso para iniciar conversación en este proyecto.");

        if ((int)respuesta.StatusCode >= 500)
            throw new ServicioNoDisponibleException("Marketplace");

        if (!respuesta.IsSuccessStatusCode)
            throw new InvalidOperationException("No se pudo verificar la participación en Marketplace.");

        return await respuesta.Content.ReadFromJsonAsync<ParticipacionChatDto>(JsonOptions, cancellationToken)
            ?? throw new InvalidOperationException("Marketplace devolvió una respuesta vacía.");
    }
}
