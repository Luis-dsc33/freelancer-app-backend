using Chat.Application.Dtos;
using MediatR;

namespace Chat.Application.Queries.ListarMensajesConversacion;

public sealed record ListarMensajesConversacionQuery(Guid ConversacionId) : IRequest<IReadOnlyList<MensajeChatDto>>;
