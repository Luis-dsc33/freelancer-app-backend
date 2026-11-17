using MediatR;

namespace Marketplace.Application.Queries.CatalogoSkills;

public sealed record ListarSkillsQuery(Guid? CategoriaId = null) : IRequest<IReadOnlyList<SkillDto>>;
