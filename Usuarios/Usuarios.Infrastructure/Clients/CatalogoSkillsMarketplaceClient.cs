using System.Net.Http.Json;
using System.Text.Json;
using Usuarios.Application.Exceptions;
using Usuarios.Application.Interfaces;

namespace Usuarios.Infrastructure.Clients;

public sealed class CatalogoSkillsMarketplaceClient(HttpClient http) : ICatalogoSkillsMarketplaceClient
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public async Task<IReadOnlyDictionary<Guid, SkillCatalogoValidadaDto>> ValidarYObtenerSkillsAsync(
        IReadOnlyCollection<Guid> skillIds,
        CancellationToken cancellationToken)
    {
        var idsUnicos = skillIds.Where(id => id != Guid.Empty).Distinct().ToList();
        if (idsUnicos.Count == 0)
            return new Dictionary<Guid, SkillCatalogoValidadaDto>();

        if (http.BaseAddress is null)
            throw new ServicioNoDisponibleException("Marketplace");

        HttpResponseMessage respuesta;
        try
        {
            respuesta = await http.GetAsync("api/catalogo/habilidades", cancellationToken);
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException && !cancellationToken.IsCancellationRequested)
        {
            throw new ServicioNoDisponibleException("Marketplace", ex);
        }

        if ((int)respuesta.StatusCode >= 500)
            throw new ServicioNoDisponibleException("Marketplace");

        if (!respuesta.IsSuccessStatusCode)
            throw new InvalidOperationException("No se pudo validar el catálogo oficial de habilidades.");

        var catalogo = await respuesta.Content.ReadFromJsonAsync<List<SkillMarketplaceResponse>>(JsonOptions, cancellationToken)
            ?? [];

        var mapa = catalogo
            .Where(s => s.Activa)
            .ToDictionary(
                s => s.Id,
                s => new SkillCatalogoValidadaDto(
                    s.Id,
                    s.Nombre,
                    !string.IsNullOrWhiteSpace(s.CategoriaNombre) ? s.CategoriaNombre : s.Categoria));

        foreach (var id in idsUnicos)
        {
            if (!mapa.ContainsKey(id))
                throw new InvalidOperationException($"La habilidad con id '{id}' no existe o no está activa en el catálogo oficial.");
        }

        return idsUnicos.ToDictionary(id => id, id => mapa[id]);
    }

    private sealed record SkillMarketplaceResponse(
        Guid Id,
        string Nombre,
        string? Categoria,
        string? CategoriaNombre,
        bool Activa = true);
}
