using Chat.Application.Dtos;
using Chat.Application.Interfaces;
using MediatR;

namespace Chat.Application.Queries.ListarMensajesConversacion;

public sealed class ListarMensajesConversacionHandler(
    IConversacionRepository conversaciones,
    IMensajeRepository mensajes,
    ICurrentUserService currentUser)
    : IRequestHandler<ListarMensajesConversacionQuery, IReadOnlyList<MensajeChatDto>>
{
    public async Task<IReadOnlyList<MensajeChatDto>> Handle(
        ListarMensajesConversacionQuery request,
        CancellationToken cancellationToken)
    {
        if (!currentUser.IsAuthenticated || currentUser.UserId == Guid.Empty)
            throw new UnauthorizedAccessException("No se pudo identificar al usuario autenticado.");

        var conversacion = await conversaciones.ObtenerPorIdAsync(request.ConversacionId, cancellationToken)
            ?? throw new KeyNotFoundException("La conversación no existe.");

        if (!conversacion.Participa(currentUser.UserId))
            throw new UnauthorizedAccessException("No tienes permiso para acceder a esta conversación.");

        var lista = await mensajes.ListarPorConversacionAsync(conversacion.Id, cancellationToken);

        return lista.Select(m => new MensajeChatDto(
            m.Id,
            m.ConversacionId,
            m.RemitenteId,
            m.Contenido,
            DateTime.SpecifyKind(m.EnviadoEn, DateTimeKind.Utc))).ToList();
    }
}
