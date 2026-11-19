namespace Marketplace.Application.Queries.CatalogoSkills;

public sealed record SkillCategoryDto(Guid Id, string Nombre, string Descripcion, bool Activa = true);
public sealed record SkillDto(Guid Id, string Nombre, string Descripcion, Guid CategoriaId, string Categoria, bool Activa = true)
{
    public string CategoriaNombre => Categoria;
}
