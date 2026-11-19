namespace Chat.Application.Interfaces;

public sealed record ParticipacionChatDto(
    bool Autorizado,
    Guid ClienteId,
    Guid EstudianteId,
    string TituloProyecto,
    string NombreCliente,
    Guid? PostulacionId,
    Guid? ContratoId);

public interface IMarketplaceChatClient
{
    Task<ParticipacionChatDto> ValidarParticipacionAsync(
        Guid proyectoId,
        Guid? postulacionId,
        Guid? contratoId,
        CancellationToken cancellationToken);
}
