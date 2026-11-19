using Chat.Application.Dtos;
using Chat.Application.Interfaces;
using Chat.Domain.Entities;
using MediatR;

namespace Chat.Application.Commands.AbrirConversacion;

public sealed class AbrirConversacionHandler(
    IConversacionRepository conversaciones,
    IMensajeRepository mensajes,
    IMarketplaceChatClient marketplaceClient,
    ICurrentUserService currentUser)
    : IRequestHandler<AbrirConversacionCommand, AbrirConversacionResult>
{
    public async Task<AbrirConversacionResult> Handle(
        AbrirConversacionCommand request,
        CancellationToken cancellationToken)
    {
        if (!currentUser.IsAuthenticated || currentUser.UserId == Guid.Empty)
            throw new UnauthorizedAccessException("No se pudo identificar al usuario autenticado.");

        var participacion = await marketplaceClient.ValidarParticipacionAsync(
            request.ProyectoId,
            request.PostulacionId,
            request.ContratoId,
            cancellationToken);

        if (!participacion.Autorizado || participacion.ClienteId == Guid.Empty || participacion.EstudianteId == Guid.Empty)
            throw new UnauthorizedAccessException("Necesitas una postulación vigente o un contrato activo para abrir esta conversación.");

        var existente = await conversaciones.ObtenerPorParticipantesAsync(
            request.ProyectoId,
            participacion.ClienteId,
            participacion.EstudianteId,
            cancellationToken);

        if (existente is not null)
        {
            existente.ActualizarContexto(
                participacion.PostulacionId ?? request.PostulacionId,
                participacion.ContratoId ?? request.ContratoId,
                participacion.TituloProyecto,
                participacion.NombreCliente);
            await conversaciones.GuardarCambiosAsync(cancellationToken);

            var ultimos = await mensajes.ObtenerUltimosMensajesPorConversacionAsync([existente.Id], cancellationToken);
            ultimos.TryGetValue(existente.Id, out var ultimoMensaje);

            var nombreContraparteExistente = existente.EstudianteId == currentUser.UserId
                ? (string.IsNullOrWhiteSpace(participacion.NombreCliente) ? existente.NombreCliente : participacion.NombreCliente)
                : null;

            return new AbrirConversacionResult(
                new ConversacionChatDto(
                    existente.Id,
                    existente.ProyectoId,
                    existente.TituloProyecto,
                    existente.PostulacionId,
                    existente.ContratoId,
                    existente.ClienteId,
                    existente.EstudianteId,
                    nombreContraparteExistente,
                    ultimoMensaje,
                    DateTime.SpecifyKind(existente.UltimaActividadEn, DateTimeKind.Utc),
                    0),
                EsNueva: false);
        }

        var nueva = new Conversacion(
            request.ProyectoId,
            participacion.PostulacionId ?? request.PostulacionId,
            participacion.ContratoId ?? request.ContratoId,
            participacion.ClienteId,
            participacion.EstudianteId,
            participacion.TituloProyecto,
            participacion.NombreCliente);

        await conversaciones.AgregarAsync(nueva, cancellationToken);

        var nombreContraparte = nueva.EstudianteId == currentUser.UserId ? nueva.NombreCliente : null;

        return new AbrirConversacionResult(
            new ConversacionChatDto(
                nueva.Id,
                nueva.ProyectoId,
                nueva.TituloProyecto,
                nueva.PostulacionId,
                nueva.ContratoId,
                nueva.ClienteId,
                nueva.EstudianteId,
                nombreContraparte,
                null,
                DateTime.SpecifyKind(nueva.UltimaActividadEn, DateTimeKind.Utc),
                0),
            EsNueva: true);
    }
}
