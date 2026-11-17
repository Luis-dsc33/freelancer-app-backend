namespace Marketplace.Domain.Entities;

public class Skill
{
    public Guid Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string NombreNormalizado { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public Guid CategoriaId { get; set; }
    public bool Activo { get; set; } = true;
    public SkillCategory Categoria { get; set; } = default!;
    public List<ProyectoSkill> ProyectoSkills { get; set; } = new();
}
