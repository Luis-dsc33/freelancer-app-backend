// ADMIN Marcos: implementación EF Core de IUsuarioAdminRepository. Todos los filtros y conteos viajan en el SQL.
using Microsoft.EntityFrameworkCore;
using Usuarios.Application.Interfaces;
using Usuarios.Domain.Entities;

namespace Usuarios.Infrastructure.Persistence;

public class UsuarioAdminRepository(UsuariosDbContext context) : IUsuarioAdminRepository
{
    public async Task<(List<Usuario> Usuarios, int Total)> BuscarAsync(
        string? busqueda, string? rol, string? estado, int pagina, int tamanoPagina, CancellationToken ct)
    {
        var query = context.Usuarios.AsNoTracking().Include(u => u.Rol).AsQueryable();

        if (!string.IsNullOrWhiteSpace(busqueda))
        {
            var patron = $"%{busqueda.Trim()}%";
            query = query.Where(u => EF.Functions.ILike(u.Nombre, patron) || EF.Functions.ILike(u.Email, patron));
        }
        if (!string.IsNullOrWhiteSpace(rol))
            query = query.Where(u => u.Rol.Nombre == rol);
        if (!string.IsNullOrWhiteSpace(estado))
            query = query.Where(u => u.Estado == estado);

        var total = await query.CountAsync(ct);
        var usuarios = await query
            .OrderByDescending(u => u.FechaRegistro)
            .Skip((pagina - 1) * tamanoPagina)
            .Take(tamanoPagina)
            .ToListAsync(ct);

        return (usuarios, total);
    }

    public Task<Usuario?> ObtenerParaActualizarAsync(Guid id, CancellationToken ct) =>
        context.Usuarios.Include(u => u.Rol).FirstOrDefaultAsync(u => u.Id == id, ct);

    public Task GuardarCambiosAsync(CancellationToken ct) => context.SaveChangesAsync(ct);

    public Task<Dictionary<string, int>> ContarPorRolAsync(CancellationToken ct) =>
        context.Usuarios
            .GroupBy(u => u.Rol.Nombre)
            .Select(g => new { Rol = g.Key, Total = g.Count() })
            .ToDictionaryAsync(x => x.Rol, x => x.Total, ct);

    public Task<int> ContarPorEstadoAsync(string estado, CancellationToken ct) =>
        context.Usuarios.CountAsync(u => u.Estado == estado, ct);

    public Task<List<DateTime>> FechasDeRegistroDesdeAsync(DateTime desde, CancellationToken ct) =>
        context.Usuarios.Where(u => u.FechaRegistro >= desde).Select(u => u.FechaRegistro).ToListAsync(ct);
}
