using System.Security.Claims;
using Usuarios.Application.Interfaces;

namespace Usuarios.Api.Services;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    public Guid UsuarioId
    {
        get
        {
            var userId = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? httpContextAccessor.HttpContext?.User.FindFirst("sub")?.Value;

            if (!Guid.TryParse(userId, out var parsedUserId) || parsedUserId == Guid.Empty)
                throw new UnauthorizedAccessException("No se pudo identificar al usuario autenticado.");

            return parsedUserId;
        }
    }
}
