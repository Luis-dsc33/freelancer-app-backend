namespace Usuarios.Application.Queries.ObtenerPerfil;

public sealed record EstudianteSkillDto(
    Guid SkillId,
    string NombreSkill,
    string? Categoria,
    string NivelDominio);

public sealed record EstudianteSkillInputDto(
    Guid SkillId,
    string? NombreSkill,
    string NivelDominio);

public class PerfilEstudianteDto
{
    public Guid? Id { get; set; }
    public Guid UsuarioId { get; set; }
    public string? Nombre { get; set; }
    public string Carrera { get; set; } = string.Empty;
    public List<string> Habilidades { get; set; } = new();
    public List<EstudianteSkillDto> SkillsCatalogo { get; set; } = new();
    public string Descripcion { get; set; } = string.Empty;
    public bool EsPerfilCompleto { get; set; }
    public List<string> CamposObligatorios { get; set; } = new();
    public List<string> CamposFaltantes { get; set; } = new();
}