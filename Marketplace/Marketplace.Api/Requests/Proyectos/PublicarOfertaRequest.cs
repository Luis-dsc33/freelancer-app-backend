namespace Marketplace.Api.Requests.Proyectos;

public sealed record PublicarOfertaRequest(
    string Titulo,
    string Descripcion,
    string Disciplina,
    decimal Presupuesto,
    DateOnly FechaLimite);
