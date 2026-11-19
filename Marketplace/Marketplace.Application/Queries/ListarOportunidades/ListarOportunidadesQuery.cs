using MediatR;

namespace Marketplace.Application.Queries.ListarOportunidades;

public sealed record ListarOportunidadesQuery(
    string? Busqueda = null,
    string? Disciplina = null,
    string? Modalidad = null,
    decimal? PresupuestoMin = null,
    decimal? PresupuestoMax = null,
    bool? SoloUrgentes = null,
    List<Guid>? SkillIds = null,
    string? Habilidad = null,
    int Page = 1,
    int PageSize = 20) : IRequest<IReadOnlyList<OportunidadProyectoDto>>;
