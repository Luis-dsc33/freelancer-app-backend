using Marketplace.Application.Interfaces;
using MediatR;

namespace Marketplace.Application.Queries.CatalogoSkills;

public sealed class ListarSkillCategoriesHandler(ISkillCatalogRepository repository)
    : IRequestHandler<ListarSkillCategoriesQuery, IReadOnlyList<SkillCategoryDto>>
{
    public async Task<IReadOnlyList<SkillCategoryDto>> Handle(ListarSkillCategoriesQuery request, CancellationToken cancellationToken) =>
        (await repository.ObtenerCategoriasAsync(cancellationToken))
            .Select(x => new SkillCategoryDto(x.Id, x.Nombre, x.Descripcion, x.Activo))
            .ToList();
}
