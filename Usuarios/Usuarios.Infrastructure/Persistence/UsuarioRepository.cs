using Microsoft.EntityFrameworkCore;
using Usuarios.Application.Interfaces;
using Usuarios.Domain.Entities;

namespace Usuarios.Infrastructure.Persistence;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly UsuariosDbContext _context;
    public UsuarioRepository(UsuariosDbContext context) => _context = context;

    public async Task<bool> ExisteEmailAsync(string email) =>
        await _context.Usuarios.AnyAsync(u => u.Email == email);

    public async Task AgregarAsync(Usuario usuario)
    {
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Usuario>> ObtenerTodosAsync() =>
        await _context.Usuarios.ToListAsync();

    public async Task<Usuario?> ObtenerPorEmailAsync(string email) =>
    await _context.Usuarios
        .Include(u => u.Rol) 
        .FirstOrDefaultAsync(u => u.Email == email);
}