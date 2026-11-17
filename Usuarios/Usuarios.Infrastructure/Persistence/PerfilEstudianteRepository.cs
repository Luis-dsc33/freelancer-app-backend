using Microsoft.EntityFrameworkCore;
using Usuarios.Application.Interfaces;
using Usuarios.Domain.Entities;

namespace Usuarios.Infrastructure.Persistence;

public class PerfilEstudianteRepository : IPerfilEstudianteRepository
{
    private readonly UsuariosDbContext _context;

    public PerfilEstudianteRepository(UsuariosDbContext context)
    {
        _context = context;
    }

    public async Task<PerfilEstudiante?> ObtenerPorUsuarioIdAsync(Guid usuarioId)
    {
        return await _context.Set<PerfilEstudiante>()
            .FirstOrDefaultAsync(p => p.UsuarioId == usuarioId);
    }

    public async Task AgregarAsync(PerfilEstudiante perfil)
    {
        await _context.Set<PerfilEstudiante>().AddAsync(perfil);
        await _context.SaveChangesAsync();
    }

    public async Task ActualizarAsync(PerfilEstudiante perfil)
    {
        _context.Set<PerfilEstudiante>().Update(perfil);
        await _context.SaveChangesAsync();
    }

    public Task<List<PerfilEstudiante>> BuscarPublicosAsync(
        string? habilidad,
        string? carrera,
        int pagina,
        int tamanoPagina,
        CancellationToken cancellationToken)
    {
        var query = _context.Set<PerfilEstudiante>().AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(carrera))
        {
            var carreraFiltro = carrera.Trim();
            query = query.Where(p => EF.Functions.ILike(p.Carrera, $"%{carreraFiltro}%"));
        }

        if (!string.IsNullOrWhiteSpace(habilidad))
        {
            var habilidadFiltro = habilidad.Trim();
            query = query.Where(p => p.Habilidades.Any(h =>
                EF.Functions.ILike(h, $"%{habilidadFiltro}%")));
        }

        return query
            .OrderBy(p => p.Carrera)
            .ThenBy(p => p.UsuarioId)
            .Skip((pagina - 1) * tamanoPagina)
            .Take(tamanoPagina)
            .ToListAsync(cancellationToken);
    }
}
