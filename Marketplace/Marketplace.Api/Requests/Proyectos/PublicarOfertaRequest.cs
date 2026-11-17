namespace Marketplace.Api.Requests.Proyectos;

public sealed record PublicarOfertaRequest(
    string Titulo,
    string Descripcion,
    string Disciplina,
    decimal Presupuesto,
    DateOnly FechaLimite,
    bool EsUrgente = false,
    List<string>? HabilidadesRequeridas = null);
