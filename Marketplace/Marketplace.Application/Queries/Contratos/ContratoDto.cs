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
    DateTime? CerradoEn)
{
    public string? TituloProyecto { get; init; }
    public string? NombreCliente { get; init; }
}
