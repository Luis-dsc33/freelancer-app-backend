// ADMIN Marcos: HU-27 acceso a la bitácora (schema admin). Solo agrega y consulta: nunca edita ni borra.
using Admin.Application.Dtos;
using Admin.Application.Interfaces;
using Admin.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Admin.Infrastructure.Persistence;

public class AccionAdministrativaRepository(AdminDbContext context) : IAccionAdministrativaRepository
{
    public async Task AgregarAsync(AccionAdministrativa accion, CancellationToken ct)
    {
        context.AccionesAdministrativas.Add(accion);
        await context.SaveChangesAsync(ct);
    }

    public async Task<(List<AccionAdministrativa> Acciones, int Total)> BuscarAsync(
        Guid? adminId, string? tipoAccion, DateTime? desde, DateTime? hastaExclusivo, int pagina, int tamanoPagina, CancellationToken ct)
    {
        var query = context.AccionesAdministrativas.AsNoTracking();
        if (adminId.HasValue) query = query.Where(a => a.AdminId == adminId);
        if (!string.IsNullOrWhiteSpace(tipoAccion)) query = query.Where(a => a.TipoAccion == tipoAccion);
        if (desde.HasValue) query = query.Where(a => a.Fecha >= desde);
        if (hastaExclusivo.HasValue) query = query.Where(a => a.Fecha < hastaExclusivo);

        var total = await query.CountAsync(ct);
        var acciones = await query.OrderByDescending(a => a.Fecha)
            .Skip((pagina - 1) * tamanoPagina).Take(tamanoPagina).ToListAsync(ct);
        return (acciones, total);
    }

    public Task<List<AccionAdministrativa>> ObtenerPorRecursoAsync(Guid recursoId, CancellationToken ct) =>
        context.AccionesAdministrativas.AsNoTracking()
            .Where(a => a.RecursoId == recursoId)
            .OrderByDescending(a => a.Fecha)
            .ToListAsync(ct);

    public async Task<Dictionary<Guid, AccionAdministrativa>> ObtenerUltimasAsync(
        string tipoAccion, IReadOnlyCollection<Guid> recursoIds, CancellationToken ct)
    {
        if (recursoIds.Count == 0) return [];
        var acciones = await context.AccionesAdministrativas.AsNoTracking()
            .Where(a => a.TipoAccion == tipoAccion && recursoIds.Contains(a.RecursoId))
            .OrderByDescending(a => a.Fecha)
            .ToListAsync(ct);
        return acciones.GroupBy(a => a.RecursoId).ToDictionary(g => g.Key, g => g.First());
    }

    public Task<List<AdministradorDto>> ObtenerAdministradoresAsync(CancellationToken ct) =>
        context.AccionesAdministrativas.AsNoTracking()
            .GroupBy(a => a.AdminId)
            .Select(g => new AdministradorDto(g.Key, g.OrderByDescending(a => a.Fecha).Select(a => a.AdminNombre).First()))
            .ToListAsync(ct);
}
