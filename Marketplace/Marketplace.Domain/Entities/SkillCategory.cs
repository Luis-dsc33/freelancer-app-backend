namespace Marketplace.Domain.Entities;

public class SkillCategory
{
    public Guid Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public bool Activo { get; set; } = true;
    public List<Skill> Skills { get; set; } = new();
}
