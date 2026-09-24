using MediatR;
using Usuarios.Application.Interfaces;

namespace Usuarios.Application.Commands.IniciarSesion;

public class IniciarSesionHandler : IRequestHandler<IniciarSesionCommand, IniciarSesionResultado>
{
    private readonly IUsuarioRepository _usuarioRepositorio;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtService _jwtService;

    public IniciarSesionHandler(IUsuarioRepository usuarioRepositorio, IPasswordHasher passwordHasher, IJwtService jwtService)
    {
        _usuarioRepositorio = usuarioRepositorio;
        _passwordHasher = passwordHasher;
        _jwtService = jwtService;
    }

    public async Task<IniciarSesionResultado> Handle(IniciarSesionCommand request, CancellationToken ct)
    {
        var usuario = await _usuarioRepositorio.ObtenerPorEmailAsync(request.Email);

        // Mensaje genérico intencional — HU-02 Escenario 02-02: no distinguir
        // si falló porque el correo no existe o porque la contraseña es incorrecta.
        if (usuario is null || !_passwordHasher.Verify(request.Password, usuario.PasswordHash))
            throw new InvalidOperationException("Correo o contraseña incorrectos.");

        // HU-02 Escenario 02-03: cuenta suspendida — aquí sí es correcto ser específico,
        // porque las credenciales ya se validaron correctamente.
        if (usuario.Estado != "Activo")
            throw new InvalidOperationException("Tu cuenta está suspendida. Contacta a un administrador.");

        var (token, expiraEn) = _jwtService.GenerarToken(usuario);
        return new IniciarSesionResultado(token, expiraEn, usuario.Nombre, usuario.Rol.Nombre);
    }
}