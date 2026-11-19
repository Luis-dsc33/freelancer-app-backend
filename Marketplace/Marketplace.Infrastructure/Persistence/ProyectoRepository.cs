using Marketplace.Application.Interfaces;
using Marketplace.Domain.Entities;
using Marketplace.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Infrastructure.Persistence;

public class ProyectoRepository(MarketplaceDbContext context) : IProyectoRepository
{
    public Task<List<Proyecto>> ObtenerAbiertosAsync(int pageNumber, int pageSize, CancellationToken cancellationToken) =>
        BuscarAbiertosAsync(new FiltroOportunidadesProyecto(PageNumber: pageNumber, PageSize: pageSize), cancellationToken);

    public Task<List<Proyecto>> BuscarAbiertosAsync(FiltroOportunidadesProyecto filtro, CancellationToken cancellationToken)
    {
        var hoy = DateOnly.FromDateTime(DateTime.UtcNow);
        var query = context.Proyectos.AsNoTracking()
            .Include(p => p.Skills).ThenInclude(ps => ps.Skill).ThenInclude(s => s.Categoria)
            .Include(p => p.Entregables)
            .Include(p => p.HitosPago)
            .Where(p => (p.Estado == Proyecto.EstadoAbierto || p.Estado == Proyecto.EstadoPublicada)
                && p.FechaLimite >= hoy);

        if (!string.IsNullOrWhiteSpace(filtro.Busqueda))
        {
            var termino = filtro.Busqueda.Trim();
            query = query.Where(p =>
                EF.Functions.ILike(p.Titulo, $"%{termino}%") ||
                EF.Functions.ILike(p.Descripcion, $"%{termino}%"));
        }

        if (!string.IsNullOrWhiteSpace(filtro.Disciplina))
        {
            var disciplina = filtro.Disciplina.Trim();
            query = query.Where(p => EF.Functions.ILike(p.Disciplina, $"%{disciplina}%"));
        }

        if (!string.IsNullOrWhiteSpace(filtro.Modalidad))
        {
            var normalizada = filtro.Modalidad.Trim()
                .Replace("í", "i", StringComparison.OrdinalIgnoreCase)
                .Replace("Í", "I", StringComparison.OrdinalIgnoreCase);

            if (Enum.TryParse<ModalidadProyecto>(normalizada, ignoreCase: true, out var modalidadEnum))
            {
                query = query.Where(p => p.Modalidad == modalidadEnum);
            }
            else
            {
                query = query.Where(_ => false);
            }
        }

        if (filtro.PresupuestoMin.HasValue)
        {
            var min = filtro.PresupuestoMin.Value;
            query = query.Where(p => (p.PresupuestoMaximo ?? p.Presupuesto) >= min);
        }

        if (filtro.PresupuestoMax.HasValue)
        {
            var max = filtro.PresupuestoMax.Value;
            query = query.Where(p => (p.PresupuestoMinimo ?? p.Presupuesto) <= max);
        }

        if (filtro.SoloUrgentes == true)
        {
            query = query.Where(p => p.EsUrgente);
        }

        if (filtro.SkillIds is { Count: > 0 })
        {
            var ids = filtro.SkillIds.Where(id => id != Guid.Empty).Distinct().ToList();
            if (ids.Count > 0)
            {
                query = query.Where(p => p.Skills.Any(s => ids.Contains(s.SkillId)));
            }
        }

        if (!string.IsNullOrWhiteSpace(filtro.Habilidad))
        {
            var habilidad = filtro.Habilidad.Trim();
            query = query.Where(p =>
                p.Skills.Any(s => EF.Functions.ILike(s.Skill.Nombre, $"%{habilidad}%")) ||
                p.HabilidadesRequeridas.Any(h => EF.Functions.ILike(h, $"%{habilidad}%")));
        }

        var pageNumber = Math.Max(1, filtro.PageNumber);
        var pageSize = Math.Clamp(filtro.PageSize, 1, 100);

        return query
            .OrderByDescending(p => p.EsUrgente)
            .ThenByDescending(p => p.CreadoEn)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public Task<List<Proyecto>> ObtenerPorClienteAsync(Guid clienteId, int pageNumber, int pageSize, CancellationToken cancellationToken) =>
        context.Proyectos.AsNoTracking()
            .Include(p => p.Skills).ThenInclude(ps => ps.Skill).ThenInclude(s => s.Categoria)
            .Include(p => p.Entregables)
            .Include(p => p.HitosPago)
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
