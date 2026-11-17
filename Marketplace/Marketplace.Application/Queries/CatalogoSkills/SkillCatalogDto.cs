namespace Marketplace.Application.Queries.CatalogoSkills;

public sealed record SkillCategoryDto(Guid Id, string Nombre, string Descripcion);
public sealed record SkillDto(Guid Id, string Nombre, string Descripcion, Guid CategoriaId, string Categoria);
