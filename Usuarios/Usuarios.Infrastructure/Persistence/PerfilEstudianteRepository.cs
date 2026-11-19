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
            .Include(p => p.SkillsCatalogo)
            .FirstOrDefaultAsync(p => p.UsuarioId == usuarioId);
    }

    public async Task AgregarAsync(PerfilEstudiante perfil)
    {
        await _context.Set<PerfilEstudiante>().AddAsync(perfil);
        await _context.SaveChangesAsync();
    }

    public async Task ActualizarAsync(PerfilEstudiante perfil)
    {
        if (_context.Entry(perfil).State == EntityState.Detached)
        {
            _context.Set<PerfilEstudiante>().Update(perfil);
        }
        else
        {
            var existentes = await _context.EstudianteSkills
                .Where(s => s.PerfilEstudianteId == perfil.Id)
                .ToListAsync();

            var actualesIds = perfil.SkillsCatalogo.Select(s => s.Id).ToHashSet();
            var aEliminar = existentes.Where(e => !actualesIds.Contains(e.Id)).ToList();
            if (aEliminar.Count > 0)
            {
                _context.EstudianteSkills.RemoveRange(aEliminar);
            }

            foreach (var skill in perfil.SkillsCatalogo)
            {
                var entry = _context.Entry(skill);
                if (entry.State == EntityState.Detached || !existentes.Any(e => e.Id == skill.Id))
                {
                    entry.State = EntityState.Added;
                }
            }
        }

        await _context.SaveChangesAsync();
    }

    public Task<List<PerfilEstudiante>> BuscarPublicosAsync(
        string? habilidad,
        string? carrera,
        int pagina,
        int tamanoPagina,
        CancellationToken cancellationToken)
    {
        var query = _context.Set<PerfilEstudiante>()
            .AsNoTracking()
            .Include(p => p.SkillsCatalogo)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(carrera))
        {
            var carreraFiltro = carrera.Trim();
            query = query.Where(p => EF.Functions.ILike(p.Carrera, $"%{carreraFiltro}%"));
        }

        if (!string.IsNullOrWhiteSpace(habilidad))
        {
            var habilidadFiltro = habilidad.Trim();
            query = query.Where(p =>
                p.Habilidades.Any(h => EF.Functions.ILike(h, $"%{habilidadFiltro}%")) ||
                p.SkillsCatalogo.Any(s => EF.Functions.ILike(s.NombreSkill, $"%{habilidadFiltro}%")));
        }

        return query
            .OrderBy(p => p.Carrera)
            .ThenBy(p => p.UsuarioId)
            .Skip((pagina - 1) * tamanoPagina)
            .Take(tamanoPagina)
            .ToListAsync(cancellationToken);
    }
}
