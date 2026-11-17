using Marketplace.Application.Interfaces;
using MediatR;

namespace Marketplace.Application.Queries.CatalogoSkills;

public sealed class ListarSkillsHandler(ISkillCatalogRepository repository)
    : IRequestHandler<ListarSkillsQuery, IReadOnlyList<SkillDto>>
{
    public async Task<IReadOnlyList<SkillDto>> Handle(ListarSkillsQuery request, CancellationToken cancellationToken) =>
        (await repository.ObtenerSkillsAsync(request.CategoriaId, cancellationToken))
            .Select(x => new SkillDto(x.Id, x.Nombre, x.Descripcion, x.CategoriaId, x.Categoria.Nombre))
            .ToList();
}
