namespace Marketplace.Application.Queries.ListarOportunidades;

/// <summary>
/// Contrato que consume el frontend (modelo OportunidadProyecto). Se serializa en camelCase.
/// </summary>
public sealed record OportunidadProyectoDto(
    Guid Id,
    string Titulo,
    string Descripcion,
    decimal Presupuesto,
    bool EsUrgente,
    IReadOnlyList<string> HabilidadesRequeridas,
    string NombreCliente,
    string Estado,
    DateTime CreadoEn);

