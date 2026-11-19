using Chat.Application.Dtos;
using MediatR;

namespace Chat.Application.Commands.EnviarMensajeChat;

public sealed record EnviarMensajeChatCommand(
    Guid ConversacionId,
    string Contenido) : IRequest<MensajeChatDto>;
