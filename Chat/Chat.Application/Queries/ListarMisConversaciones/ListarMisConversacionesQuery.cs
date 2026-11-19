using Chat.Application.Dtos;
using MediatR;

namespace Chat.Application.Queries.ListarMisConversaciones;

public sealed record ListarMisConversacionesQuery : IRequest<IReadOnlyList<ConversacionChatDto>>;
