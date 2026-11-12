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
}
