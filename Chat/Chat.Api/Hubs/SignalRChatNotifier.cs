using Chat.Application.Dtos;
using Chat.Application.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace Chat.Api.Hubs;

public sealed class SignalRChatNotifier(IHubContext<ChatHub> hubContext) : IChatNotifier
{
    public Task NotificarMensajeAsync(
        Guid conversacionId,
        MensajeChatDto mensaje,
        CancellationToken cancellationToken) =>
        hubContext.Clients
            .Group(ChatHub.GrupoConversacion(conversacionId))
            .SendAsync("MensajeRecibido", mensaje, cancellationToken);
}
