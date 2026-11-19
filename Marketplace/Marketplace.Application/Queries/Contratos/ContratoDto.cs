namespace Marketplace.Application.Queries.Contratos;

public sealed record ContratoDto(
    Guid Id,
    Guid PostulacionId,
    Guid ProyectoId,
    Guid ClienteId,
    Guid EstudianteId,
    decimal ImporteAcordado,
    DateOnly FechaInicio,
    DateOnly FechaCompromiso,
    string Estado,
    DateTime CreadoEn,
    DateTime? CerradoEn);
