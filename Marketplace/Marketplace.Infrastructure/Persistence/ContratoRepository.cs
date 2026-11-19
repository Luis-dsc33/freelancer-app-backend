using Marketplace.Application.Interfaces;
using Marketplace.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Infrastructure.Persistence;

public sealed class ContratoRepository(MarketplaceDbContext context) : IContratoRepository
{
    public Task<bool> ExisteParaPostulacionAsync(Guid postulacionId, CancellationToken cancellationToken) =>
        context.Contratos.AnyAsync(c => c.PostulacionId == postulacionId, cancellationToken);

    public async Task AgregarAsync(Contrato contrato, CancellationToken cancellationToken)
    {
        await context.Contratos.AddAsync(contrato, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public Task<List<Contrato>> ObtenerPorClienteAsync(Guid clienteId, CancellationToken cancellationToken) =>
        context.Contratos.AsNoTracking()
            .Include(c => c.Postulacion)
            .ThenInclude(p => p.Proyecto)
            .Where(c => c.ClienteId == clienteId)
            .OrderByDescending(c => c.CreadoEn)
            .ToListAsync(cancellationToken);

    public Task<List<Contrato>> ObtenerPorEstudianteAsync(Guid estudianteId, CancellationToken cancellationToken) =>
        context.Contratos.AsNoTracking()
            .Include(c => c.Postulacion)
            .ThenInclude(p => p.Proyecto)
            .Where(c => c.EstudianteId == estudianteId)
            .OrderByDescending(c => c.CreadoEn)
            .ToListAsync(cancellationToken);

    public Task<Contrato?> ObtenerPorIdAsync(Guid id, CancellationToken cancellationToken) =>
        context.Contratos
            .Include(c => c.Postulacion)
            .ThenInclude(p => p.Proyecto)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

    public Task<Contrato?> ObtenerParaEstudianteEnProyectoAsync(Guid proyectoId, Guid estudianteId, CancellationToken cancellationToken) =>
        context.Contratos.AsNoTracking()
            .Where(c => c.ProyectoId == proyectoId && c.EstudianteId == estudianteId)
            .OrderByDescending(c => c.CreadoEn)
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<Dictionary<Guid, Guid>> ObtenerContratosPorPostulacionesAsync(
        IEnumerable<Guid> postulacionIds,
        CancellationToken cancellationToken)
    {
        var ids = postulacionIds.Distinct().ToList();
        if (ids.Count == 0)
            return [];

        return await context.Contratos.AsNoTracking()
            .Where(c => ids.Contains(c.PostulacionId))
            .ToDictionaryAsync(c => c.PostulacionId, c => c.Id, cancellationToken);
    }

    public async Task ActualizarAsync(Contrato contrato, CancellationToken cancellationToken)
    {
        context.Contratos.Update(contrato);
        await context.SaveChangesAsync(cancellationToken);
    }
}
