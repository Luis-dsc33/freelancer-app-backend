using System.Net.Http.Json;
using System.Text.Json;
using Resenas.Application.Interfaces;

namespace Resenas.Infrastructure.Clients;

public sealed class UsuariosPublicoClient(HttpClient http) : IUsuariosPublicoClient
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public async Task<IReadOnlyDictionary<Guid, string>> ObtenerNombresPorIdsAsync(
        IEnumerable<Guid> usuarioIds,
        CancellationToken cancellationToken)
    {
        var ids = usuarioIds.Where(id => id != Guid.Empty).Distinct().ToList();
        if (ids.Count == 0 || http.BaseAddress is null)
            return new Dictionary<Guid, string>();

        try
        {
            var respuesta = await http.PostAsJsonAsync(
                "api/usuarios/nombres",
                new { usuarioIds = ids },
                cancellationToken);

            if (!respuesta.IsSuccessStatusCode)
                return new Dictionary<Guid, string>();

            return await respuesta.Content.ReadFromJsonAsync<Dictionary<Guid, string>>(JsonOptions, cancellationToken)
                ?? new Dictionary<Guid, string>();
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException or JsonException)
        {
            return new Dictionary<Guid, string>();
        }
    }
}
