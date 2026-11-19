using Microsoft.EntityFrameworkCore;
using Usuarios.Application.Interfaces;
using Usuarios.Domain.Entities;

namespace Usuarios.Infrastructure.Persistence;

public sealed class PerfilClienteRepository(UsuariosDbContext context) : IPerfilClienteRepository
{
    public Task<PerfilCliente?> ObtenerPorUsuarioIdAsync(Guid usuarioId) =>
        context.PerfilesCliente
            .FirstOrDefaultAsync(p => p.UsuarioId == usuarioId);

    public async Task AgregarAsync(PerfilCliente perfil)
    {
        await context.PerfilesCliente.AddAsync(perfil);
        await context.SaveChangesAsync();
    }

    public async Task ActualizarAsync(PerfilCliente perfil)
    {
        context.PerfilesCliente.Update(perfil);
        await context.SaveChangesAsync();
    }
}
