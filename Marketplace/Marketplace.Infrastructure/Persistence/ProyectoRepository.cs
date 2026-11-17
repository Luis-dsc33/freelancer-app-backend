using Marketplace.Application.Interfaces;
using Marketplace.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Infrastructure.Persistence;

public class ProyectoRepository(MarketplaceDbContext context) : IProyectoRepository
{
    public Task<List<Proyecto>> ObtenerAbiertosAsync(int pageNumber, int pageSize, CancellationToken cancellationToken) =>
        context.Proyectos.AsNoTracking()
            .Include(p => p.Skills).ThenInclude(ps => ps.Skill).ThenInclude(s => s.Categoria)
            .Include(p => p.Entregables)
            .Where(p => (p.Estado == Proyecto.EstadoAbierto || p.Estado == Proyecto.EstadoPublicada)
                && p.FechaLimite >= DateOnly.FromDateTime(DateTime.UtcNow))
            .OrderByDescending(p => p.EsUrgente)
            .ThenByDescending(p => p.CreadoEn)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

    public Task<List<Proyecto>> ObtenerPorClienteAsync(Guid clienteId, int pageNumber, int pageSize, CancellationToken cancellationToken) =>
        context.Proyectos.AsNoTracking()
            .Include(p => p.Skills).ThenInclude(ps => ps.Skill).ThenInclude(s => s.Categoria)
            .Include(p => p.Entregables)
            .Where(p => p.ClienteId == clienteId)
            .OrderByDescending(p => p.CreadoEn)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

    public Task<Proyecto?> ObtenerPorIdAsync(Guid id, CancellationToken cancellationToken) =>
        context.Proyectos
            .Include(p => p.Skills).ThenInclude(ps => ps.Skill).ThenInclude(s => s.Categoria)
            .Include(p => p.Entregables)
            .Include(p => p.HitosPago)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

    public async Task AgregarAsync(Proyecto proyecto, CancellationToken cancellationToken)
    {
        await context.Proyectos.AddAsync(proyecto, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task ActualizarAsync(Proyecto proyecto, CancellationToken cancellationToken)
    {
        context.Proyectos.Update(proyecto);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task EliminarAsync(Proyecto proyecto, CancellationToken cancellationToken)
    {
        context.Proyectos.Remove(proyecto);
        await context.SaveChangesAsync(cancellationToken);
    }
}
