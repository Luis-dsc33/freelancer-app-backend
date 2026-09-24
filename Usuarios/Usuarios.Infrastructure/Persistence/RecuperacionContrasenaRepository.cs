using Microsoft.EntityFrameworkCore;
using Usuarios.Application.Interfaces;
using Usuarios.Domain.Entities;

namespace Usuarios.Infrastructure.Persistence;

public class RecuperacionContrasenaRepository : IRecuperacionContrasenaRepository
{
    private readonly UsuariosDbContext _context;

    public RecuperacionContrasenaRepository(UsuariosDbContext context) => _context = context;

    public async Task CrearAsync(
        RecuperacionContrasena recuperacion,
        DateTime ahora,
        CancellationToken cancellationToken)
    {
        var pendientes = await _context.RecuperacionesContrasena
            .Where(r => r.UsuarioId == recuperacion.UsuarioId &&
                        r.ConsumidoEn == null &&
                        r.RevocadoEn == null &&
                        r.ExpiraEn > ahora)
            .ToListAsync(cancellationToken);

        foreach (var pendiente in pendientes)
        {
            pendiente.RevocadoEn = ahora;
            pendiente.Version = Guid.NewGuid();
        }

        _context.RecuperacionesContrasena.Add(recuperacion);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public Task<bool> EsTokenValidoAsync(
        string tokenHash,
        DateTime ahora,
        CancellationToken cancellationToken) =>
        _context.RecuperacionesContrasena.AnyAsync(
            r => r.TokenHash == tokenHash &&
                 r.ConsumidoEn == null &&
                 r.RevocadoEn == null &&
                 r.ExpiraEn > ahora,
            cancellationToken);

    public async Task<bool> RestablecerContrasenaAsync(
        string tokenHash,
        string nuevoPasswordHash,
        DateTime ahora,
        CancellationToken cancellationToken)
    {
        await using var transaccion = await _context.Database.BeginTransactionAsync(cancellationToken);

        var recuperacion = await _context.RecuperacionesContrasena
            .Include(r => r.Usuario)
            .SingleOrDefaultAsync(
                r => r.TokenHash == tokenHash &&
                     r.ConsumidoEn == null &&
                     r.RevocadoEn == null &&
                     r.ExpiraEn > ahora,
                cancellationToken);

        if (recuperacion is null)
        {
            await transaccion.RollbackAsync(cancellationToken);
            return false;
        }

        recuperacion.ConsumidoEn = ahora;
        recuperacion.Version = Guid.NewGuid();
        recuperacion.Usuario.PasswordHash = nuevoPasswordHash;

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            await transaccion.CommitAsync(cancellationToken);
            return true;
        }
        catch (DbUpdateConcurrencyException)
        {
            await transaccion.RollbackAsync(cancellationToken);
            return false;
        }
    }
}
