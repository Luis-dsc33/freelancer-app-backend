namespace Usuarios.Domain.Entities;

public class PerfilEstudiante
{
    public static readonly string[] CamposObligatorios = { "carrera", "habilidades", "descripcion" };

    public Guid Id { get; set; }
    public Guid UsuarioId { get; set; }
    public Usuario Usuario { get; set; } = default!;
    public string Carrera { get; set; } = string.Empty;
    public List<string> Habilidades { get; set; } = new();
    public ICollection<EstudianteSkill> SkillsCatalogo { get; private set; } = new List<EstudianteSkill>();
    public string Descripcion { get; set; } = string.Empty;

    public void ActualizarSkillsCatalogo(IEnumerable<EstudianteSkill> nuevasSkills)
    {
        SkillsCatalogo.Clear();
        foreach (var skill in nuevasSkills)
        {
            SkillsCatalogo.Add(skill);
        }

        if (Habilidades.Count == 0 && SkillsCatalogo.Count > 0)
        {
            Habilidades = SkillsCatalogo
                .Select(s => s.NombreSkill)
                .Where(n => !string.IsNullOrWhiteSpace(n))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();
        }
    }

    public List<string> ObtenerCamposFaltantes()
    {
        var faltantes = new List<string>();
        if (string.IsNullOrWhiteSpace(Carrera)) faltantes.Add("carrera");
        var tieneHabilidades = (Habilidades is not null && Habilidades.Any(h => !string.IsNullOrWhiteSpace(h)))
            || (SkillsCatalogo is not null && SkillsCatalogo.Count > 0);
        if (!tieneHabilidades) faltantes.Add("habilidades");
        if (string.IsNullOrWhiteSpace(Descripcion)) faltantes.Add("descripcion");
        return faltantes;
    }

    public bool EstaCompleto() => ObtenerCamposFaltantes().Count == 0;
}