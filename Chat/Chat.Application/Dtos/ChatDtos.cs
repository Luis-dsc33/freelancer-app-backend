namespace Chat.Application.Dtos;

public sealed record ConversacionChatDto(
    Guid Id,
    Guid ProyectoId,
    string? TituloProyecto,
    Guid? PostulacionId,
    Guid? ContratoId,
    Guid ClienteId,
    Guid EstudianteId,
    string? NombreContraparte,
    string? UltimoMensaje,
    DateTime? UltimaActividadEn,
    int NoLeidos = 0);

public sealed record MensajeChatDto(
    Guid Id,
    Guid ConversacionId,
    Guid RemitenteId,
    string Contenido,
    DateTime EnviadoEn);

public sealed record AbrirConversacionResult(
    ConversacionChatDto Conversacion,
    bool EsNueva);
