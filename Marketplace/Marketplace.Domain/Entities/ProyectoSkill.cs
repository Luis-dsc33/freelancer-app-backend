using Marketplace.Domain.Enums;

namespace Marketplace.Domain.Entities;

public class ProyectoSkill
{
    public Guid ProyectoId { get; set; }
    public Guid SkillId { get; set; }
    public TipoRequisitoSkill TipoRequisito { get; set; }
    public Proyecto Proyecto { get; set; } = default!;
    public Skill Skill { get; set; } = default!;
}
