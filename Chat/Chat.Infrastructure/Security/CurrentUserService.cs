using System.Security.Claims;
using Chat.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Chat.Infrastructure.Security;

public sealed class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    public bool IsAuthenticated =>
        httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated == true;

    public Guid UserId
    {
        get
        {
            if (!IsAuthenticated)
                return Guid.Empty;

            var value = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? httpContextAccessor.HttpContext?.User.FindFirstValue("sub");

            return Guid.TryParse(value, out var userId) ? userId : Guid.Empty;
        }
    }

    public string? Role =>
        httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Role)
        ?? httpContextAccessor.HttpContext?.User.FindFirstValue("role");

    public string? Nombre =>
        httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Name)
        ?? httpContextAccessor.HttpContext?.User.FindFirstValue("name");

    public bool EsEstudiante => string.Equals(Role, "Estudiante", StringComparison.OrdinalIgnoreCase);

    public bool EsCliente => string.Equals(Role, "Cliente", StringComparison.OrdinalIgnoreCase);
}
