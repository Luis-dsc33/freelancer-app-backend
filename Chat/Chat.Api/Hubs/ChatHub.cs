using System.Security.Claims;
using Chat.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Chat.Api.Hubs;

[Authorize]
public sealed class ChatHub(IConversacionRepository conversaciones) : Hub
{
    public static string GrupoConversacion(Guid conversacionId) => $"conv-{conversacionId}";

    public async Task JoinConversation(Guid conversacionId)
    {
        var userIdValue = Context.UserIdentifier
            ?? Context.User?.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? Context.User?.FindFirstValue("sub");

        if (!Guid.TryParse(userIdValue, out var userId) || userId == Guid.Empty)
            throw new HubException("Usuario no identificado.");

        var esParticipante = await conversaciones.EsParticipanteAsync(
            conversacionId,
            userId,
            Context.ConnectionAborted);

        if (!esParticipante)
            throw new HubException("No tienes permiso para unirte a esta conversación.");

        await Groups.AddToGroupAsync(Context.ConnectionId, GrupoConversacion(conversacionId), Context.ConnectionAborted);
    }

    public Task LeaveConversation(Guid conversacionId) =>
        Groups.RemoveFromGroupAsync(Context.ConnectionId, GrupoConversacion(conversacionId), Context.ConnectionAborted);
}
