using Chat.Application.Dtos;
using Chat.Application.Interfaces;
using Chat.Domain.Entities;
using MediatR;

namespace Chat.Application.Commands.EnviarMensajeChat;

public sealed class EnviarMensajeChatHandler(
    IConversacionRepository conversaciones,
    IMensajeRepository mensajes,
    IChatNotifier chatNotifier,
    ICurrentUserService currentUser)
    : IRequestHandler<EnviarMensajeChatCommand, MensajeChatDto>
{
    public async Task<MensajeChatDto> Handle(
        EnviarMensajeChatCommand request,
        CancellationToken cancellationToken)
    {
        if (!currentUser.IsAuthenticated || currentUser.UserId == Guid.Empty)
            throw new UnauthorizedAccessException("No se pudo identificar al usuario autenticado.");

        var conversacion = await conversaciones.ObtenerPorIdAsync(request.ConversacionId, cancellationToken)
            ?? throw new KeyNotFoundException("La conversación no existe.");

        if (!conversacion.Participa(currentUser.UserId))
            throw new UnauthorizedAccessException("No tienes permiso para enviar mensajes en esta conversación.");

        var mensaje = new Mensaje(conversacion.Id, currentUser.UserId, request.Contenido.Trim());
        conversacion.RegistrarActividad(mensaje.EnviadoEn);

        await mensajes.AgregarAsync(mensaje, cancellationToken);
        await conversaciones.GuardarCambiosAsync(cancellationToken);

        var dto = new MensajeChatDto(
            mensaje.Id,
            mensaje.ConversacionId,
            mensaje.RemitenteId,
            mensaje.Contenido,
            DateTime.SpecifyKind(mensaje.EnviadoEn, DateTimeKind.Utc));

        await chatNotifier.NotificarMensajeAsync(conversacion.Id, dto, cancellationToken);

        return dto;
    }
}
