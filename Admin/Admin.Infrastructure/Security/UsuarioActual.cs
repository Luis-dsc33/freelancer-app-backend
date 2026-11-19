// ADMIN Marcos: identifica al administrador desde los claims del JWT emitido por Usuarios (nunca desde el body).
using System.Security.Claims;
using Admin.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Admin.Infrastructure.Security;

public class UsuarioActual(IHttpContextAccessor accessor) : IUsuarioActual
{
    private ClaimsPrincipal Usuario =>
        accessor.HttpContext?.User ?? throw new UnauthorizedAccessException("No hay un usuario autenticado.");

    public Guid Id =>
        Guid.TryParse(Usuario.FindFirstValue(ClaimTypes.NameIdentifier) ?? Usuario.FindFirstValue("sub"), out var id)
            ? id
            : throw new UnauthorizedAccessException("El token no identifica al administrador.");

    public string Nombre => Usuario.FindFirstValue(ClaimTypes.Name) ?? "Administrador";
}
