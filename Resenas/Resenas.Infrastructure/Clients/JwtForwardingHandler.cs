using Microsoft.AspNetCore.Http;

namespace Resenas.Infrastructure.Clients;

public sealed class JwtForwardingHandler(IHttpContextAccessor accessor) : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct)
    {
        var auth = accessor.HttpContext?.Request.Headers.Authorization.ToString();
        if (!string.IsNullOrWhiteSpace(auth))
            request.Headers.TryAddWithoutValidation("Authorization", auth);

        return base.SendAsync(request, ct);
    }
}
