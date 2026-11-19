using Chat.Application.Dtos;
using Chat.Application.Interfaces;
using MediatR;

namespace Chat.Application.Queries.ListarMisConversaciones;

public sealed class ListarMisConversacionesHandler(
    IConversacionRepository conversaciones,
    IMensajeRepository mensajes,
    ICurrentUserService currentUser,
    IUsuariosPublicoClient? usuariosClient = null)
    : IRequestHandler<ListarMisConversacionesQuery, IReadOnlyList<ConversacionChatDto>>
{
    public async Task<IReadOnlyList<ConversacionChatDto>> Handle(
        ListarMisConversacionesQuery request,
        CancellationToken cancellationToken)
    {
        if (!currentUser.IsAuthenticated || currentUser.UserId == Guid.Empty)
            throw new UnauthorizedAccessException("No se pudo identificar al usuario autenticado.");

        var userId = currentUser.UserId;
        var lista = await conversaciones.ListarPorUsuarioAsync(userId, cancellationToken);
        if (lista.Count == 0)
            return [];

        var ultimosMensajes = await mensajes.ObtenerUltimosMensajesPorConversacionAsync(
            lista.Select(c => c.Id),
            cancellationToken);

        IReadOnlyDictionary<Guid, string> nombres = new Dictionary<Guid, string>();
        if (usuariosClient is not null)
        {
            var contrapartesIds = lista
                .Select(c => c.ClienteId == userId ? c.EstudianteId : c.ClienteId)
                .Where(id => id != Guid.Empty)
                .Distinct()
                .ToList();

            if (contrapartesIds.Count > 0)
                nombres = await usuariosClient.ObtenerNombresPorIdsAsync(contrapartesIds, cancellationToken);
        }

        return lista.Select(c =>
        {
            var contraparteId = c.ClienteId == userId ? c.EstudianteId : c.ClienteId;
            nombres.TryGetValue(contraparteId, out var nombreResuelto);
            var nombreContraparte = !string.IsNullOrWhiteSpace(nombreResuelto)
                ? nombreResuelto
                : (c.EstudianteId == userId ? c.NombreCliente : null);

            ultimosMensajes.TryGetValue(c.Id, out var ultimoMensaje);

            return new ConversacionChatDto(
                c.Id,
                c.ProyectoId,
                c.TituloProyecto,
                c.PostulacionId,
                c.ContratoId,
                c.ClienteId,
                c.EstudianteId,
                nombreContraparte,
                ultimoMensaje,
                DateTime.SpecifyKind(c.UltimaActividadEn, DateTimeKind.Utc),
                0);
        }).ToList();
    }
}
