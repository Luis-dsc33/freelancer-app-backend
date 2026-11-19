namespace Usuarios.Application.Interfaces;

public sealed record SkillCatalogoValidadaDto(
    Guid SkillId,
    string NombreSkill,
    string? Categoria);

public interface ICatalogoSkillsMarketplaceClient
{
    Task<IReadOnlyDictionary<Guid, SkillCatalogoValidadaDto>> ValidarYObtenerSkillsAsync(
        IReadOnlyCollection<Guid> skillIds,
        CancellationToken cancellationToken);
}
