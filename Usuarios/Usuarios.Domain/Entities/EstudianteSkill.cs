namespace Usuarios.Domain.Entities;

public enum NivelDominioSkill
{
    Basico = 1,
    Intermedio = 2,
    Avanzado = 3
}

public class EstudianteSkill
{
    public Guid Id { get; private set; }
    public Guid PerfilEstudianteId { get; private set; }
    public Guid SkillId { get; private set; }
    public string NombreSkill { get; private set; } = string.Empty;
    public string? Categoria { get; private set; }
    public string NivelDominio { get; private set; } = "Intermedio";

    public PerfilEstudiante PerfilEstudiante { get; private set; } = default!;

    private EstudianteSkill() { }

    public EstudianteSkill(
        Guid perfilEstudianteId,
        Guid skillId,
        string nombreSkill,
        string? categoria,
        string nivelDominio)
    {
        Id = Guid.NewGuid();
        PerfilEstudianteId = perfilEstudianteId;
        SkillId = skillId;
        NombreSkill = nombreSkill;
        Categoria = categoria;
        NivelDominio = nivelDominio;
    }
}
