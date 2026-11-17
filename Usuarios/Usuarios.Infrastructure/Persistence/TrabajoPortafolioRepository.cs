using Microsoft.EntityFrameworkCore;
using Usuarios.Application.Interfaces;
using Usuarios.Domain.Entities;

namespace Usuarios.Infrastructure.Persistence;

public class TrabajoPortafolioRepository(UsuariosDbContext context) : ITrabajoPortafolioRepository
{
    public Task<List<TrabajoPortafolio>> ObtenerPorUsuarioAsync(Guid usuarioId, CancellationToken cancellationToken) =>
        context.TrabajosPortafolio.AsNoTracking()
            .Where(t => t.UsuarioId == usuarioId)
            .OrderByDescending(t => t.CreadoEn).ThenBy(t => t.Id)
            .ToListAsync(cancellationToken);

    public Task<TrabajoPortafolio?> ObtenerPropioAsync(Guid id, Guid usuarioId, CancellationToken cancellationToken) =>
        context.TrabajosPortafolio.FirstOrDefaultAsync(t => t.Id == id && t.UsuarioId == usuarioId, cancellationToken);

    public async Task AgregarAsync(TrabajoPortafolio trabajo, CancellationToken cancellationToken)
    {
        context.TrabajosPortafolio.Add(trabajo);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task GuardarCambiosAsync(CancellationToken cancellationToken) =>
        await context.SaveChangesAsync(cancellationToken);

    public async Task EliminarAsync(TrabajoPortafolio trabajo, CancellationToken cancellationToken)
    {
        context.TrabajosPortafolio.Remove(trabajo);
        await context.SaveChangesAsync(cancellationToken);
    }
}
