using Microsoft.AspNetCore.Http;

namespace Admin.Infrastructure.Clients;

// Reenvía el JWT del administrador que hizo la petición a los demás servicios,
// para que cada uno valide [Authorize(Roles = "Administrador")] por su cuenta (HU-22).
public class ReenviarTokenHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _accessor;

    public ReenviarTokenHandler(IHttpContextAccessor accessor) => _accessor = accessor;

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct)
    {
        var authorization = _accessor.HttpContext?.Request.Headers.Authorization.ToString();
        if (!string.IsNullOrWhiteSpace(authorization))
            request.Headers.TryAddWithoutValidation("Authorization", authorization);

        return base.SendAsync(request, ct);
    }
}
