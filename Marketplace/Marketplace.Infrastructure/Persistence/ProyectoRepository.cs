using Marketplace.Application.Interfaces;
using Marketplace.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Infrastructure.Persistence;

public class ProyectoRepository(MarketplaceDbContext context) : IProyectoRepository
{
    public Task<List<Proyecto>> ObtenerAbiertosAsync(CancellationToken cancellationToken) =>
        context.Proyectos.AsNoTracking()
            .Where(p => p.Estado == Proyecto.EstadoAbierto)
            .OrderByDescending(p => p.EsUrgente)
            .ThenByDescending(p => p.CreadoEn)
            .ToListAsync(cancellationToken);

    public async Task AgregarAsync(Proyecto proyecto, CancellationToken cancellationToken)
    {
        await context.Proyectos.AddAsync(proyecto, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }
}
