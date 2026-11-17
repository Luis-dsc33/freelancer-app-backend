using MediatR;

namespace Marketplace.Application.Queries.CatalogoSkills;

public sealed record ListarSkillCategoriesQuery : IRequest<IReadOnlyList<SkillCategoryDto>>;
