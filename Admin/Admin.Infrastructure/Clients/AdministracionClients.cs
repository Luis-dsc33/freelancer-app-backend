// ADMIN Marcos: clientes HTTP de las operaciones de administración (HU-25, HU-21, HU-26).
// Reenvían el JWT del administrador (ReenviarTokenHandler) y traducen las respuestas del servicio dueño:
//  - 2xx              → resultado
//  - 4xx con {error}  → InvalidOperationException con el mismo mensaje (ej. "La cuenta ya está suspendida.")
//  - sin respuesta    → ServicioNoDisponibleException (HU-21-04)
using System.Net.Http.Json;
using System.Text.Json;
using Admin.Application.Dtos;
using Admin.Application.Exceptions;
using Admin.Application.Interfaces;

namespace Admin.Infrastructure.Clients;

public abstract class ClienteAdministracion(HttpClient http, string servicio)
{
    protected async Task<T> EnviarAsync<T>(HttpMethod metodo, string ruta, CancellationToken ct)
    {
        HttpResponseMessage respuesta;
        try
        {
            if (http.BaseAddress is null) throw new InvalidOperationException("URL no configurada");
            respuesta = await http.SendAsync(new HttpRequestMessage(metodo, ruta), ct);
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException or InvalidOperationException && !ct.IsCancellationRequested)
        {
            throw new ServicioNoDisponibleException(servicio, ex);
        }

        if (respuesta.IsSuccessStatusCode)
            return (await respuesta.Content.ReadFromJsonAsync<T>(ct))!;

        if ((int)respuesta.StatusCode >= 500)
            throw new ServicioNoDisponibleException(servicio);

        throw new InvalidOperationException(await LeerErrorAsync(respuesta, ct));
    }

    private static async Task<string> LeerErrorAsync(HttpResponseMessage respuesta, CancellationToken ct)
    {
        try
        {
            using var json = JsonDocument.Parse(await respuesta.Content.ReadAsStringAsync(ct));
            if (json.RootElement.TryGetProperty("error", out var error)) return error.GetString() ?? "Solicitud rechazada.";
        }
        catch (JsonException) { }
        return "El servicio rechazó la operación.";
    }
}

public class UsuariosAdminClient(HttpClient http) : ClienteAdministracion(http, "Usuarios"), IUsuariosAdminClient
{
    public Task<PaginaUsuariosDto> BuscarAsync(string? busqueda, string? rol, string? estado, int pagina, int tamanoPagina, CancellationToken ct)
    {
        var ruta = $"api/admin/usuarios?pagina={pagina}&tamanoPagina={tamanoPagina}"
                   + (string.IsNullOrWhiteSpace(busqueda) ? "" : $"&busqueda={Uri.EscapeDataString(busqueda)}")
                   + (string.IsNullOrWhiteSpace(rol) ? "" : $"&rol={Uri.EscapeDataString(rol)}")
                   + (string.IsNullOrWhiteSpace(estado) ? "" : $"&estado={Uri.EscapeDataString(estado)}");
        return EnviarAsync<PaginaUsuariosDto>(HttpMethod.Get, ruta, ct);
    }

    public Task<UsuarioDto> SuspenderAsync(Guid usuarioId, CancellationToken ct) =>
        EnviarAsync<UsuarioDto>(HttpMethod.Post, $"api/admin/usuarios/{usuarioId}/suspender", ct);

    public Task<UsuarioDto> ReactivarAsync(Guid usuarioId, CancellationToken ct) =>
        EnviarAsync<UsuarioDto>(HttpMethod.Post, $"api/admin/usuarios/{usuarioId}/reactivar", ct);
}

public class MarketplaceAdminClient(HttpClient http) : ClienteAdministracion(http, "Marketplace"), IMarketplaceAdminClient
{
    public async Task<IReadOnlyList<OfertaDto>> ListarOfertasAsync(string? estado, CancellationToken ct) =>
        await EnviarAsync<List<OfertaDto>>(HttpMethod.Get,
            "api/admin/ofertas" + (string.IsNullOrWhiteSpace(estado) ? "" : $"?estado={Uri.EscapeDataString(estado)}"), ct);

    public Task<OfertaDto> OcultarAsync(Guid ofertaId, CancellationToken ct) =>
        EnviarAsync<OfertaDto>(HttpMethod.Post, $"api/admin/ofertas/{ofertaId}/ocultar", ct);

    public Task<OfertaDto> RestaurarAsync(Guid ofertaId, CancellationToken ct) =>
        EnviarAsync<OfertaDto>(HttpMethod.Post, $"api/admin/ofertas/{ofertaId}/restaurar", ct);
}
