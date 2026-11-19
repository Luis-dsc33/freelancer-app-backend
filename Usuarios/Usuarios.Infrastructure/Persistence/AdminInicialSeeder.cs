using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Usuarios.Application.Interfaces;
using Usuarios.Domain.Entities;

namespace Usuarios.Infrastructure.Persistence;

// Crea la cuenta Administrador inicial al arrancar el servicio, SOLO si no existe.
// Nadie puede registrarse como Administrador desde la app (HU-01, HU-22-05), por eso se siembra aquí.
// Las credenciales salen de la configuración (AdminInicial:*), nunca del código ni del repo:
//   - local: appsettings.Development.json (ignorado por git)
//   - servidor: variables de entorno AdminInicial__Email, AdminInicial__Password, AdminInicial__Nombre
// Si no están configuradas, no se crea nada.
public class AdminInicialSeeder
{
    private readonly IUsuarioRepository _usuarios;
    private readonly IRolRepository _roles;
    private readonly IPasswordHasher _hasher;
    private readonly IConfiguration _config;
    private readonly ILogger<AdminInicialSeeder> _logger;

    public AdminInicialSeeder(
        IUsuarioRepository usuarios,
        IRolRepository roles,
        IPasswordHasher hasher,
        IConfiguration config,
        ILogger<AdminInicialSeeder> logger)
    {
        _usuarios = usuarios;
        _roles = roles;
        _hasher = hasher;
        _config = config;
        _logger = logger;
    }

    public async Task SembrarAsync()
    {
        // TEMPORAL: credenciales por defecto hardcodeadas para probar funciones de Admin en producción.
        // Eliminar estos valores por defecto y usar solo AdminInicial:* (variables de entorno) al terminar pruebas.
        var email = (string.IsNullOrWhiteSpace(_config["AdminInicial:Email"])
            ? "admin@codebridge.com"
            : _config["AdminInicial:Email"]!).Trim().ToLowerInvariant();
        var password = string.IsNullOrWhiteSpace(_config["AdminInicial:Password"])
            ? "AdminCodeBridge2026!"
            : _config["AdminInicial:Password"]!;
        var nombre = string.IsNullOrWhiteSpace(_config["AdminInicial:Nombre"])
            ? "Administrador CodeBridge"
            : _config["AdminInicial:Nombre"]!;

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            _logger.LogInformation("AdminInicial no configurado: no se creó cuenta Administrador.");
            return;
        }

        if (await _usuarios.ExisteEmailAsync(email))
            return; // ya existe: no se toca (ni contraseña ni rol)

        var rol = await _roles.ObtenerPorNombreAsync("Administrador")
                  ?? throw new InvalidOperationException("No existe el rol Administrador en la base.");

        await _usuarios.AgregarAsync(new Usuario
        {
            Id = Guid.NewGuid(),
            Nombre = nombre,
            Email = email,
            PasswordHash = _hasher.Hash(password),
            RolId = rol.Id,
            Estado = "Activo",
            FechaRegistro = DateTime.UtcNow
        });

        _logger.LogInformation("Cuenta Administrador inicial creada.");
    }
}
