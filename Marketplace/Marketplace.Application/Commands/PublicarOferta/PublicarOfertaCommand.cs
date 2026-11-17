using Marketplace.Application.Queries.ListarOportunidades;
using MediatR;

namespace Marketplace.Application.Commands.PublicarOferta;

public sealed record PublicarOfertaCommand(
    string Titulo,
    string Descripcion,
    string Disciplina,
    decimal Presupuesto,
    DateOnly FechaLimite,
    bool EsUrgente,
    List<string> HabilidadesRequeridas) : IRequest<OportunidadProyectoDto>;
