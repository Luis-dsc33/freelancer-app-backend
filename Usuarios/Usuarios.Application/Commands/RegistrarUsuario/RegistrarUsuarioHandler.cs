using MediatR;
using Usuarios.Application.Interfaces;
using Usuarios.Domain.Entities;

namespace Usuarios.Application.Commands.RegistrarUsuario;

public class RegistrarUsuarioHandler : IRequestHandler<RegistrarUsuarioCommand, Guid>
{
    private readonly IUsuarioRepository _usuarioRepositorio;
    private readonly IRolRepository _rolRepositorio;
    private readonly IPasswordHasher _passwordHasher;

    public RegistrarUsuarioHandler(
        IUsuarioRepository usuarioRepositorio,
        IRolRepository rolRepositorio,
        IPasswordHasher passwordHasher)
    {
        _usuarioRepositorio = usuarioRepositorio;
        _rolRepositorio = rolRepositorio;
        _passwordHasher = passwordHasher;
    }

    public async Task<Guid> Handle(RegistrarUsuarioCommand request, CancellationToken ct)
    {
        var existe = await _usuarioRepositorio.ExisteEmailAsync(request.Email);
        if (existe)
            throw new InvalidOperationException("El correo ya está registrado.");

        var rol = await _rolRepositorio.ObtenerPorNombreAsync(request.Rol);
        if (rol is null)
            throw new InvalidOperationException("El rol indicado no es válido.");

        // Asignacion de la identidad segun el domain
        var usuario = new Usuario
        {
            Id = Guid.NewGuid(),
            Nombre = request.Nombre,
            Email = request.Email,
            PasswordHash = _passwordHasher.Hash(request.Password),
            RolId = rol.Id,
            Estado = "Activo",
            FechaRegistro = DateTime.UtcNow
        };

        await _usuarioRepositorio.AgregarAsync(usuario);
        return usuario.Id;
    }
}