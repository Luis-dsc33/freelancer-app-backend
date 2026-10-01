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
        // Normalizar ANTES de comparar y de guardar — evita que "Ana@X.com" y "ana@x.com"
        // se traten como correos distintos (HU-01: un correo = una sola cuenta).
        var emailNormalizado = request.Email.Trim().ToLowerInvariant();

        var existe = await _usuarioRepositorio.ExisteEmailAsync(emailNormalizado);
        if (existe)
            throw new InvalidOperationException("El correo ya está registrado.");

        var rol = await _rolRepositorio.ObtenerPorNombreAsync(request.Rol);
        if (rol is null)
            throw new InvalidOperationException("El rol indicado no es válido.");

        var usuario = new Usuario
        {
            Id = Guid.NewGuid(),
            Nombre = request.Nombre,
            Email = emailNormalizado,
            PasswordHash = _passwordHasher.Hash(request.Password),
            RolId = rol.Id,
            Estado = "Activo",
            FechaRegistro = DateTime.UtcNow
        };

        await _usuarioRepositorio.AgregarAsync(usuario);
        return usuario.Id;
    }
}