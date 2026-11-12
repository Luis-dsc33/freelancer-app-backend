namespace Marketplace.Application.Queries.ListarOportunidades;

/// <summary>
/// Contrato que consume el frontend (modelo OportunidadProyecto). Se serializa en camelCase.
/// </summary>
public sealed record OportunidadProyectoDto(
    Guid Id,
    string Titulo,
    string Descripcion,
    string Disciplina,
    decimal Presupuesto,
    DateOnly FechaLimite,
    bool EsUrgente,
    IReadOnlyList<string> HabilidadesRequeridas,
    string NombreCliente,
    string Estado,
    DateTime CreadoEn);
