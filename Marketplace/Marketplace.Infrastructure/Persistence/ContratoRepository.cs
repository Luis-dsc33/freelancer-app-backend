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
        context.Contratos.AsNoTracking().Where(c => c.ClienteId == clienteId)
            .OrderByDescending(c => c.CreadoEn).ToListAsync(cancellationToken);

    public Task<Contrato?> ObtenerPorIdAsync(Guid id, CancellationToken cancellationToken) =>
        context.Contratos.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

    public async Task ActualizarAsync(Contrato contrato, CancellationToken cancellationToken)
    {
        context.Contratos.Update(contrato);
        await context.SaveChangesAsync(cancellationToken);
    }
}
