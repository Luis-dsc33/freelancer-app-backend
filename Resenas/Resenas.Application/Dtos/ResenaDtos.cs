namespace Resenas.Application.Dtos;

public sealed record ResenaDto(
    Guid Id,
    Guid ContratoId,
    Guid AutorId,
    Guid DestinatarioId,
    int Puntuacion,
    string Comentario,
    DateTime CreadaEn);

public sealed record ResenaRecibidaDto(
    Guid Id,
    Guid ContratoId,
    Guid AutorId,
    string? NombreAutor,
    int Puntuacion,
    string Comentario,
    DateTime CreadaEn);

public sealed record ReputacionUsuarioDto(
    Guid UsuarioId,
    double Promedio,
    int TotalResenas,
    List<ResenaRecibidaDto> Resenas);
