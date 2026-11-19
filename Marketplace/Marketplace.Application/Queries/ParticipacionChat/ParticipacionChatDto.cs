namespace Marketplace.Application.Queries.ParticipacionChat;

public sealed record ParticipacionChatDto(
    bool Autorizado,
    Guid ClienteId,
    Guid EstudianteId,
    string TituloProyecto,
    string NombreCliente,
    Guid? PostulacionId,
    Guid? ContratoId);
