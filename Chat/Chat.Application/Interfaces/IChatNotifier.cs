using Chat.Application.Dtos;

namespace Chat.Application.Interfaces;

public interface IChatNotifier
{
    Task NotificarMensajeAsync(Guid conversacionId, MensajeChatDto mensaje, CancellationToken cancellationToken);
}
