using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;

namespace Chat.Api.Hubs;

public sealed class JwtUserIdProvider : IUserIdProvider
{
    public string? GetUserId(HubConnectionContext connection) =>
        connection.User?.FindFirstValue(ClaimTypes.NameIdentifier)
        ?? connection.User?.FindFirstValue("sub");
}
