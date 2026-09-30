namespace Usuarios.Domain.Entities;

public class PerfilEstudiante
{
    public Guid Id { get; set; }
    public Guid UsuarioId { get; set; }
    public Usuario Usuario { get; set; } = default!;
    public string Carrera { get; set; } = default!;
    public List<string> Habilidades { get; set; } = new();
    public string Descripcion { get; set; } = default!;
}
