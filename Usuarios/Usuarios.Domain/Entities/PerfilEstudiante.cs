namespace Usuarios.Domain.Entities;

public class PerfilEstudiante
{
    public static readonly string[] CamposObligatorios = { "carrera", "habilidades", "descripcion" };

    public Guid Id { get; set; }
    public Guid UsuarioId { get; set; }
    public Usuario Usuario { get; set; } = default!;
    public string Carrera { get; set; } = string.Empty;
    public List<string> Habilidades { get; set; } = new();
    public string Descripcion { get; set; } = string.Empty;

    public List<string> ObtenerCamposFaltantes()
    {
        var faltantes = new List<string>();
        if (string.IsNullOrWhiteSpace(Carrera)) faltantes.Add("carrera");
        if (Habilidades is null || !Habilidades.Any(h => !string.IsNullOrWhiteSpace(h))) faltantes.Add("habilidades");
        if (string.IsNullOrWhiteSpace(Descripcion)) faltantes.Add("descripcion");
        return faltantes;
    }

    public bool EstaCompleto() => ObtenerCamposFaltantes().Count == 0;
}