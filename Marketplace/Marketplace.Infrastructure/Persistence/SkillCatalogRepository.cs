using Marketplace.Application.Interfaces;
using Marketplace.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Infrastructure.Persistence;

public sealed class SkillCatalogRepository(MarketplaceDbContext context) : ISkillCatalogRepository
{
    public async Task<bool> ExistenActivasAsync(IEnumerable<Guid> skillIds, CancellationToken cancellationToken)
    {
        var ids = skillIds.Distinct().ToArray();
        return ids.Length > 0 && await context.Skills.CountAsync(x => ids.Contains(x.Id) && x.Activo, cancellationToken) == ids.Length;
    }

    public Task<IReadOnlyList<SkillCategory>> ObtenerCategoriasAsync(CancellationToken cancellationToken) =>
        context.SkillCategories.AsNoTracking().Where(x => x.Activo).OrderBy(x => x.Nombre).ToListAsync(cancellationToken)
            .ContinueWith(task => (IReadOnlyList<SkillCategory>)task.Result, cancellationToken);

    public Task<IReadOnlyList<Skill>> ObtenerSkillsAsync(Guid? categoriaId, CancellationToken cancellationToken) =>
        context.Skills.AsNoTracking().Include(x => x.Categoria).Where(x => x.Activo && (!categoriaId.HasValue || x.CategoriaId == categoriaId.Value)).OrderBy(x => x.Nombre).ToListAsync(cancellationToken)
            .ContinueWith(task => (IReadOnlyList<Skill>)task.Result, cancellationToken);
}
