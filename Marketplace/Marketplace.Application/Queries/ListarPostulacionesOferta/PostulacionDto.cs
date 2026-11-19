namespace Marketplace.Application.Queries.ListarPostulacionesOferta;

public sealed record PostulacionDto(
    Guid Id,
    Guid ProyectoId,
    Guid EstudianteId,
    string Estado,
    string Mensaje,
    DateTime CreadaEn,
    DateTime? ResueltaEn);
