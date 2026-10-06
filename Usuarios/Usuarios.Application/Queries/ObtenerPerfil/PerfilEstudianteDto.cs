namespace Usuarios.Application.Queries.ObtenerPerfil;

public class PerfilEstudianteDto
{
    public Guid? Id { get; set; }
    public Guid UsuarioId { get; set; }
    public string Carrera { get; set; } = string.Empty;
    public List<string> Habilidades { get; set; } = new();
    public string Descripcion { get; set; } = string.Empty;
    public bool EsPerfilCompleto { get; set; }
    public List<string> CamposObligatorios { get; set; } = new();
    public List<string> CamposFaltantes { get; set; } = new();
}