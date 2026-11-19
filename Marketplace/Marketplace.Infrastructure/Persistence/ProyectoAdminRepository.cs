// ADMIN Marcos: implementación EF Core de IProyectoAdminRepository. Filtros y conteos viajan en el SQL.
using Marketplace.Application.Interfaces;
using Marketplace.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Infrastructure.Persistence;

public class ProyectoAdminRepository(MarketplaceDbContext context) : IProyectoAdminRepository
{
    public Task<List<Proyecto>> ListarAsync(string? estado, CancellationToken ct)
    {
        var query = context.Proyectos.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(estado))
            query = query.Where(p => p.Estado == estado);
        return query.OrderByDescending(p => p.CreadoEn).ToListAsync(ct);
    }

    public Task<Proyecto?> ObtenerParaActualizarAsync(Guid id, CancellationToken ct) =>
        context.Proyectos.FirstOrDefaultAsync(p => p.Id == id, ct);

    public Task GuardarCambiosAsync(CancellationToken ct) => context.SaveChangesAsync(ct);

    public Task<int> ContarPorEstadosAsync(IReadOnlyCollection<string> estados, CancellationToken ct) =>
        context.Proyectos.CountAsync(p => estados.Contains(p.Estado), ct);

    public Task<Dictionary<string, int>> ContarPorDisciplinaAsync(IReadOnlyCollection<string> estados, CancellationToken ct) =>
        context.Proyectos
            .Where(p => estados.Contains(p.Estado))
            .GroupBy(p => p.Disciplina)
            .Select(g => new { Disciplina = g.Key, Total = g.Count() })
            .ToDictionaryAsync(x => x.Disciplina, x => x.Total, ct);

    public Task<List<DateTime>> FechasDeCreacionDesdeAsync(DateTime desde, CancellationToken ct) =>
        context.Proyectos.Where(p => p.CreadoEn >= desde).Select(p => p.CreadoEn).ToListAsync(ct);
}
