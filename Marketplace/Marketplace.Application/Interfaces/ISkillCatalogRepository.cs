using Marketplace.Domain.Entities;

namespace Marketplace.Application.Interfaces;

public interface ISkillCatalogRepository
{
    Task<bool> ExistenActivasAsync(IEnumerable<Guid> skillIds, CancellationToken cancellationToken);
    Task<IReadOnlyList<SkillCategory>> ObtenerCategoriasAsync(CancellationToken cancellationToken);
    Task<IReadOnlyList<Skill>> ObtenerSkillsAsync(Guid? categoriaId, CancellationToken cancellationToken);
}
