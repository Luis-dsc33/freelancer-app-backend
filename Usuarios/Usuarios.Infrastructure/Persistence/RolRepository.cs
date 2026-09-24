using Microsoft.EntityFrameworkCore;
using Usuarios.Application.Interfaces;
using Usuarios.Domain.Entities;

namespace Usuarios.Infrastructure.Persistence;

public class RolRepository : IRolRepository
{
    private readonly UsuariosDbContext _context;
    public RolRepository(UsuariosDbContext context) => _context = context;

    public async Task<Rol?> ObtenerPorNombreAsync(string nombre) =>
        await _context.Roles.FirstOrDefaultAsync(r => r.Nombre == nombre);
}