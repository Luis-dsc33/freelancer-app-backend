using Chat.Application.Dtos;
using Chat.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Chat.Api.Hubs;

[Authorize]
public sealed class ChatHub(IConversacionRepository conversaciones) : Hub
{
    public static string GrupoConversacion(Guid conversacionId) => $"conv-{conversacionId}";

    public override async Task OnConnectedAsync()
    {
        if (!Guid.TryParse(Context.UserIdentifier, out var userId) || userId == Guid.Empty)
            throw new HubException("Usuario no identificado.");

        await Clients.Caller.SendAsync(
            "Conectado",
            new ChatConectadoDto(userId, Context.ConnectionId, DateTime.UtcNow),
            Context.ConnectionAborted);

        await base.OnConnectedAsync();
    }

    public async Task JoinConversation(Guid conversacionId)
    {
        if (!Guid.TryParse(Context.UserIdentifier, out var userId) || userId == Guid.Empty)
            throw new HubException("Usuario no identificado.");

        var esParticipante = await conversaciones.EsParticipanteAsync(
            conversacionId,
            userId,
            Context.ConnectionAborted);

        if (!esParticipante)
            throw new HubException("No tienes permiso para unirte a esta conversación.");

        await Groups.AddToGroupAsync(Context.ConnectionId, GrupoConversacion(conversacionId), Context.ConnectionAborted);
        await Clients.Group(GrupoConversacion(conversacionId)).SendAsync(
            "ParticipanteConectado",
            new ChatConexionDto(userId, conversacionId, Context.ConnectionId, DateTime.UtcNow),
            Context.ConnectionAborted);
    }

    public Task LeaveConversation(Guid conversacionId) =>
        Groups.RemoveFromGroupAsync(Context.ConnectionId, GrupoConversacion(conversacionId), Context.ConnectionAborted);
}
