using MediatR;

namespace Marketplace.Application.Queries.ParticipacionChat;

public sealed record ObtenerParticipacionChatQuery(
    Guid ProyectoId,
    Guid? PostulacionId = null,
    Guid? ContratoId = null) : IRequest<ParticipacionChatDto>;
